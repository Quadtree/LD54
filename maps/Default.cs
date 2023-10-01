using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;

public class Default : Control
{
    MatchState MS = new MatchState();

    Spell SelectedSpell;

    public static Spell[] PossibleSpells = new Spell[]{
        new FlameWave(),
        new BurningBolt(),
        new Nullify(),
        new Counterspell(),
        new Shield(),
        new Energize(),
        new Feedback(),
    };

    public enum SpellEnum
    {
        FlameWave,
        BurningBolt,
        Nullify,
        Counterspell,
        Shield,
        Energize,
        Feedback,
    }

    public static string[] SPELL_TEXTURES = new string[]{
        "res://textures/flame_lance.png",
        "res://textures/fire_particle.png",
        "",
        "res://textures/counterspell.png",
        "",
        "",
        "res://textures/feedback.png",
    };

    CombatantStatus[] CS;

    IEnumerator<bool> Operation;

    BasicAI AI = new BasicAI();

    Tuple<int, Spell>[] AvailableSpells = Array.Empty<Tuple<int, Spell>>();

    public bool IsCurrentlyPlayersTurn => Loser == null && ((MS.CurrentTurn == 0 && MS.CurrentPhase == MatchState.Phase.Main) || (MS.CurrentTurn == 1 && MS.CurrentPhase == MatchState.Phase.Reaction));

    public Sprite SpellInFlight;
    public int SpellInFlightTargetId;

    public List<Tuple<int, int, string, string>> SpellInFlightQueue = new List<Tuple<int, int, string, string>>();

    public static int CurrentLevel = 2;

    int? Loser;
    string DefeatTextHeadline = "";
    string DefeatTextBody = "";

    string QueuedAnimation;

    Dictionary<Spell, float> SpellCardHeights = new Dictionary<Spell, float>();

    public override void _Ready()
    {
        this.FindChildByName<Control>("Modal").FindChildByType<Button>().Connect("pressed", this, nameof(OnModalProceedPressed));
        this.FindChildByName<Button>("TryNotCastingSpellButton").Connect("pressed", this, nameof(OnTryNotCastingSpellButton));
        this.FindChildByName<Button>("CancelNotCastingSpellButton").Connect("pressed", this, nameof(OnCancelNotCastingSpellButton));
        this.FindChildByName<Button>("RestartLevelButton").Connect("pressed", this, nameof(OnRestartLevelButton));
        this.FindChildByName<Button>("EndTurnButton").Connect("pressed", this, nameof(OnEndTurnButton));

        var c1 = this.FindChildByName<CombatantStatus>("CombatantStatus");
        var c2 = this.FindChildByName<CombatantStatus>("CombatantStatus2");
        c1.Src = () => MS.Combatants[0];
        c2.Src = () => MS.Combatants[1];

        CS = new CombatantStatus[] { c1, c2 };

        c1.CellClickedListeners.Add(v2 =>
        {
            if (IsCurrentlyPlayersTurn)
            {
                var oo = MS.TryCastSpell(SelectedSpell, 0, 1, v2);
                if (oo == null)
                {
                    Util.SpawnOneShotSound(GD.Load<AudioStream>("res://sounds/place_rune.wav"), this);
                }
                else
                {
                    GD.Print(oo);
                }
            }
        });

        //c2.CellClickedListeners.Add(v2 => { if (IsCurrentlyPlayersTurn) MS.TryCastSpell(SelectedSpell, 0, 1, v2); });

        MS.ChangeListeners.Add(ComputeAvailableSpells);
        MS.SpellCastListeners.Add((from, to, spell) =>
        {
            for (var i = 0; i < PossibleSpells.Length; ++i) if (PossibleSpells[i] == spell && SPELL_TEXTURES[i] != "") AddSpellInFlight(from, to, SPELL_TEXTURES[i], spell.SoundEffect);
        });

        LoadLevel($"res://levels/Level{CurrentLevel}.tscn");

        MS.StartGame();
        ComputeAvailableSpells();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (GetTree().Root.FindChildByName<AudioStreamPlayer>("BGMPlayer") == null && !OS.IsDebugBuild())
        {
            var bgmPlayer = new AudioStreamPlayer();
            bgmPlayer.Stream = GD.Load<AudioStream>("res://music/bgm.ogg");
            bgmPlayer.Name = "BGMPlayer";
            bgmPlayer.VolumeDb = -8;
            GetTree().Root.AddChild(bgmPlayer);
            bgmPlayer.Play();
        }

        if (Input.IsActionJustPressed("toggle_music"))
        {
            GetTree().Root.FindChildByName<AudioStreamPlayer>("BGMPlayer").Playing = !GetTree().Root.FindChildByName<AudioStreamPlayer>("BGMPlayer").Playing;
        }

        if (Operation != null)
        {
            if (!Operation.MoveNext())
            {
                GD.Print("AI reports complete!");
                Operation = null;
            }
        }

        if (Operation == null && MS.CurrentTurn == 1 && MS.CurrentPhase == MatchState.Phase.Main)
        {
            Operation = AI.RunMainTurn(MS.CurrentTurn, MS).GetEnumerator();
        }

        if (Operation == null && MS.CurrentTurn == 0 && MS.CurrentPhase == MatchState.Phase.Reaction)
        {
            Operation = AI.RunReactionTurn(MS.CurrentTurn, MS).GetEnumerator();
        }

        if (MS.CurrentTurn == 0 && MS.CurrentPhase == MatchState.Phase.Main) this.FindChildByName<Label>("TurnStatusLabel").Text = "Your main phase!";
        if (MS.CurrentTurn == 1 && MS.CurrentPhase == MatchState.Phase.Main) this.FindChildByName<Label>("TurnStatusLabel").Text = "Opponent main phase!";
        if (MS.CurrentTurn == 0 && MS.CurrentPhase == MatchState.Phase.Reaction) this.FindChildByName<Label>("TurnStatusLabel").Text = "Opponent's chance to react!";
        if (MS.CurrentTurn == 1 && MS.CurrentPhase == MatchState.Phase.Reaction) this.FindChildByName<Label>("TurnStatusLabel").Text = "Your chance to react!";

        this.FindChildByName<Label>("IncomingSpellsLabel").Text = $"Incoming: {string.Join(", ", MS.PendingSpells.Select(it => $"{it.Item1.Name}>{it.Item2}"))}";

        this.FindChildByName<Label>("AvailableSpellsLabel").Text = $"Available: {string.Join(", ", AvailableSpells.Select(it => $"{it.Item2.Name} ({it.Item1})"))}";

        if (Loser == null)
        {
            for (var i = 0; i < 2; ++i)
            {
                if (MS.Combatants[i].HP <= 0)
                {
                    Loser = i;
                    QueuedAnimation = "Death";
                    if (i == 0) DefeatTextBody = "Better luck next time!";
                    if (i == 1) DefeatTextBody = "Another victory in the Spellfire Tournament!";
                    //CS[i].FindChildByType<AnimationPlayer>().Play("Death");

                    break;
                }
            }

            if (MS.CurrentPhase == MatchState.Phase.Main && MS.SpellsCastSoFarThisTurn == 0 && AvailableSpells.Length == 0)
            {
                Loser = MS.CurrentTurn;

                if (Loser == 0) DefeatTextBody = "Looks like you shouldn't have skipped the class on Rune Grid Overloads in Magic 104.";
                if (Loser == 1) DefeatTextBody = "So that's what happens when you overload your rune grid...";

                QueuedAnimation = "Explode";
                //CS[MS.CurrentTurn].FindChildByType<AnimationPlayer>().Play("Explode");
            }
        }

        if (Loser != null && QueuedAnimation != null && SpellInFlight == null && SpellInFlightQueue.Count == 0)
        {
            if (QueuedAnimation == "Explode") Util.SpawnOneShotSound(GD.Load<AudioStream>("res://sounds/explode.wav"), this);
            CS[Loser.Value].FindChildByType<AnimationPlayer>().Play(QueuedAnimation);
            QueuedAnimation = null;
        }

        if (Loser is int loserNotNull && CS[loserNotNull].FindChildByType<AnimationPlayer>().IsPlaying() == false && SpellInFlight == null && SpellInFlightQueue.Count == 0)
        {
            if (DefeatTextHeadline == "")
            {
                if (Loser == 0) DefeatTextHeadline = "Defeat!";
                else DefeatTextHeadline = "Victory!";
            }

            PopModal(DefeatTextHeadline, DefeatTextBody, () =>
            {
                if (Loser == 0)
                {
                    GetTree().ChangeScene("res://maps/Default.tscn");
                }
                else
                {
                    CurrentLevel++;
                    if (CurrentLevel < 7)
                        GetTree().ChangeScene("res://maps/Default.tscn");
                    else
                        GetTree().ChangeScene("res://maps/VictoryScreen.tscn");
                }
            });
        }

        if (SpellInFlight != null)
        {
            var toPos = CS[SpellInFlightTargetId].FindChildByName<Sprite>("Sprite2").GlobalPosition;
            if (toPos.DistanceSquaredTo(SpellInFlight.GlobalPosition) < 60 * 60)
            {
                SpellInFlight.GetParent().RemoveChild(SpellInFlight);
                SpellInFlight = null;
            }
            else
                SpellInFlight.GlobalPosition += (toPos - SpellInFlight.GlobalPosition).Normalized() * 2000 * delta;
        }
        else
        {
            if (SpellInFlightQueue.Any())
            {
                DoAddSpellInFlight(SpellInFlightQueue[0].Item1, SpellInFlightQueue[0].Item2, SpellInFlightQueue[0].Item3, SpellInFlightQueue[0].Item4);
                SpellInFlightQueue.RemoveAt(0);
            }
        }

        // stop the player from doing anything if it's not their turn
        if (!IsCurrentlyPlayersTurn) return;

        for (var i = 0; i < 10; ++i)
        {
            if (Input.IsActionJustPressed($"select_spell_{i}"))
            {
                SelectedSpell = PossibleSpells[i];
                GD.Print($"Spell {SelectedSpell.Name} selected");
            }
        }

        if (OS.IsDebugBuild() && Input.IsActionJustPressed("test"))
        {
            //this.FindChildrenByType<AnimationPlayer>().Last().Play("Explode");
            //AddSpellInFlight(0, 1, "res://textures/feedback.png");
        }

        if (OS.IsDebugBuild() && Input.IsActionJustPressed("cheat_deal_damage"))
        {
            MS.Combatants[1].TakeDamage(5);
        }

        if (OS.IsDebugBuild() && Input.IsActionJustPressed("cheat_self_damage"))
        {
            MS.Combatants[0].TakeDamage(5);
        }

        if (OS.IsDebugBuild() && Input.IsActionJustPressed("cheat_free_sp"))
        {
            MS.Combatants[0].SP += 10;
        }

        if (Input.IsActionJustPressed("end_turn"))
        {
            PlayerInitatedEndTurn();
        }

        for (var i = 0; i < 1; ++i)
        {
            CS[i].Grid1.HoveredCellsSource = () => Array.Empty<Tuple<IntVec2, Grid1.HoverType>>();

            var curHover = CS[i].Grid1.CurrentHover;

            if (SelectedSpell != null && curHover is IntVec2 curHoverNotNull)
            {
                //GD.Print("CUR HOVER! " + string.Join(" ", SelectedSpell.Footprint.Select(it => Tuple.Create(it + curHoverNotNull, Grid1.HoverType.SpellBase))));
                CS[i].Grid1.HoveredCellsSource = () => SelectedSpell.Footprint.Select(it => Tuple.Create(it + curHoverNotNull, Grid1.HoverType.SpellBase)).ToArray();

                if (SelectedSpell is Counterspell)
                {
                    CS[1].Grid1.HoveredCellsSource = () => SelectedSpell?.Footprint?.Select(it => Tuple.Create(it + curHoverNotNull, Grid1.HoverType.SpellBase))?.ToArray() ?? Array.Empty<Tuple<IntVec2, Grid1.HoverType>>();
                }
            }
        }

        var rot = -2f * AvailableSpells.Length;

        foreach (var sc in this.FindChildrenByType<SpellCard>())
        {
            if (!SpellCardHeights.ContainsKey(sc.Spell)) SpellCardHeights[sc.Spell] = 0;

            float dest = 0;

            if (sc.IsMouseHovering)
            {
                dest = -30;
                if (Input.IsMouseButtonPressed((int)ButtonList.Left)) SelectedSpell = sc.Spell;
            }

            if (SelectedSpell == sc.Spell) dest = -110;

            SpellCardHeights[sc.Spell] += (dest - SpellCardHeights[sc.Spell]) * 4 * delta;

            //GD.Print(SpellCardHeights[sc.Spell]);

            sc.RectPosition = new Vector2(sc.RectPosition.x, SpellCardHeights[sc.Spell]);
            sc.RectRotation = rot;

            rot += 5f;
        }

        if (Input.IsMouseButtonPressed((int)ButtonList.Left)) this.FindChildByName<Label>("StartOfLevelTextLabel").Visible = false;

        // automatic end turn!
        if (MS.CurrentPhase == MatchState.Phase.Reaction && MS.CurrentTurn == 1 && AvailableSpells.Length == 0) PlayerInitatedEndTurn();
    }

    public void ComputeAvailableSpells()
    {
        var availSpells = new List<Tuple<int, Spell>>();
        int idx = 0;

        foreach (var spell in PossibleSpells)
        {
            if (MS.KnownSpells.Contains((SpellEnum)idx))
            {
                if (spell.IsReaction == (MS.CurrentPhase == MatchState.Phase.Reaction))
                {
                    var comb = MS.CurrentPhase == MatchState.Phase.Reaction ? MS.Combatants[1 - MS.CurrentTurn] : MS.Combatants[MS.CurrentTurn];
                    var enemy = MS.CurrentPhase != MatchState.Phase.Reaction ? MS.Combatants[1 - MS.CurrentTurn] : MS.Combatants[MS.CurrentTurn];

                    if (spell.IsValidForCaster(comb))
                    {
                        var isValid = false;

                        for (var x = 0; x < comb.Grid.Width && !isValid; ++x)
                        {
                            for (var y = 0; y < comb.Grid.Height && !isValid; ++y)
                            {
                                if (spell.IsValidAtPoint(new IntVec2(x, y), comb.Grid, enemy.Grid))
                                {
                                    isValid = true;
                                }
                            }
                        }

                        if (isValid) availSpells.Add(Tuple.Create(idx, spell));
                    }
                }
            }

            ++idx;
        }

        AvailableSpells = availSpells.ToArray();

        if (this.FindChildByName<HBoxContainer>("SpellCardTray") is var sct)
        {
            sct.ClearChildren();

            var first = true;

            {
                var spacer1 = new Control();
                spacer1.SizeFlagsHorizontal = (int)Control.SizeFlags.Expand;
                sct.AddChild(spacer1);
            }

            foreach (var it in AvailableSpells)
            {
                var card = GD.Load<PackedScene>("res://ui/SpellCard.tscn").Instance<SpellCard>();
                card.Spell = it.Item2;
                sct.AddChild(card);
            }

            {
                var spacer1 = new Control();
                spacer1.SizeFlagsHorizontal = (int)Control.SizeFlags.Expand;
                sct.AddChild(spacer1);
            }
        }
    }

    public void PlayerInitatedEndTurn()
    {
        if (MS.SpellsCastSoFarThisTurn == 0)
        {
            this.FindChildByName<Control>("NotCastingSpellModal").Visible = true;
        }
        else
        {
            SelectedSpell = null;
            MS.EndTurn();
        }
    }

    public void LoadLevel(string resName)
    {
        GD.Print($"Loading {resName}");
        var level = GD.Load<PackedScene>(resName).Instance<BaseLevel>();
        var idx = 0;

        foreach (var it in level.FindChildrenByType<TileMap>())
        {
            for (var x = 0; x < MS.Combatants[idx].Grid.Width; ++x)
            {
                for (var y = 0; y < MS.Combatants[idx].Grid.Height; ++y)
                {
                    if (it.GetCell(x, y) != TileMap.InvalidCell)
                    {
                        GD.Print($"{x},{y} is filled on {idx}");
                        MS.Combatants[idx].Grid.CellsAvailable[x, y] = true;
                    }
                }
            }

            ++idx;
        }

        MS.KnownSpells = level.KnownSpells;
        MS.Combatants[0].HP = level.PlayerHP;
        MS.Combatants[1].HP = level.EnemyHP;
        MS.AIMinSPToCast = level.AIMinSPToCast;
        MS.EnemyAvailableSpells = level.EnemySpells.Select(it => PossibleSpells[(int)it]).ToArray();

        this.FindChildByName<Label>("StartOfLevelTextLabel").Text = level.StartOfLevelText;

        CS[0].FindChildByName<Sprite>("Sprite2").Modulate = Colors.SandyBrown;
        CS[1].FindChildByName<Sprite>("Sprite2").Modulate = level.EnemyMageRobeColor;
    }

    public void AddSpellInFlight(int fromId, int toId, string texture, string soundEffect)
    {
        SpellInFlightQueue.Add(Tuple.Create(fromId, toId, texture, soundEffect));
    }

    public void DoAddSpellInFlight(int fromId, int toId, string texture, string soundEffect)
    {
        var fromPos = CS[fromId].FindChildByName<Sprite>("Sprite2").GlobalPosition;
        var toPos = CS[toId].FindChildByName<Sprite>("Sprite2").GlobalPosition;

        SpellInFlight = new Sprite();
        AddChild(SpellInFlight);
        SpellInFlight.Scale = new Vector2(8, 8);
        SpellInFlight.GlobalPosition = fromPos;
        SpellInFlight.LookAt(toPos);
        SpellInFlight.Texture = GD.Load<Texture>(texture);
        SpellInFlightTargetId = toId;

        Util.SpawnOneShotSound(GD.Load<AudioStream>(soundEffect), this);
    }

    Action ModalProceedPressed;

    public void PopModal(string headline, string body, Action proceed)
    {
        var modal = this.FindChildByName<Control>("Modal");
        modal.Visible = true;
        modal.FindChildByName<Label>("Headline").Text = headline;
        modal.FindChildByName<Label>("Body").Text = body;
        ModalProceedPressed = proceed;
    }

    void OnModalProceedPressed()
    {
        ModalProceedPressed?.Invoke();
    }

    void OnTryNotCastingSpellButton()
    {
        this.FindChildByName<Control>("NotCastingSpellModal").Visible = false;
        Loser = 0;
        CS[0].FindChildByType<AnimationPlayer>().Play("Dropped");
        DefeatTextHeadline = "Orange Card";
        DefeatTextBody = "Well that clarifies things.";
    }

    void OnCancelNotCastingSpellButton()
    {
        this.FindChildByName<Control>("NotCastingSpellModal").Visible = false;
    }

    void OnRestartLevelButton()
    {
        GetTree().ChangeScene("res://maps/Default.tscn");
    }

    void OnEndTurnButton()
    {
        PlayerInitatedEndTurn();
    }
}
