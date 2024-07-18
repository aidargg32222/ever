using Content.Server.Atlanta.GameTicking.Rules.Components;
using Content.Server.Atlanta.Player;
using Content.Server.Atlanta.Player.Events;
using Content.Server.GameTicking.Rules;
using Content.Server.KillTracking;
using Content.Server.Mind;
using Content.Server.Shuttles.Systems;
using Content.Shared.Atlanta.Mist;
using Robust.Shared.Timing;

namespace Content.Server.Atlanta.GameTicking.Rules;

/// <summary>
/// This handles...
/// </summary>
public sealed class MistGameRuleSystem :  GameRuleSystem<MistGameRuleComponent>
{
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        // rule
        SubscribeLocalEvent<MistGameRuleComponent, ComponentStartup>(OnRuleStartup);

        // players
        SubscribeLocalEvent<PlayerSpawnAfterEvent>(OnAfterPlayerSpawner);
        SubscribeLocalEvent<KillReportedEvent>(OnKillReported);

        _sawmill = _logManager.GetSawmill("Mist Game Rule");
    }

    private void OnKillReported(KillReportedEvent ev)
    {
        var player = ev.Entity;
        _sawmill.Debug($"New kill report from {player}.");

        var query = EntityQueryEnumerator<MistGameRuleComponent>();
        while (query.MoveNext(out var _, out var rule))
        {
            rule.AlivePlayers.Remove(player);

            var lifetime = EnsureComp<LifeTrackerComponent>(player);

            if (!lifetime.IsDead)
            {
                _sawmill.Error("Life tracker component didn't catch KillReport before game rule!");
                lifetime.IsDead = true;
                lifetime.DeathTime = _timing.CurTime;
            }

            string characterName;

            try
            {
                characterName = rule.Minds.Find(e => e.Item1 == player).Item2;
            }
            catch (ArgumentNullException)
            {
                characterName = "[lost name]";
            }

            rule.PlayersGrave.Add(GenerateGraveMessage(ev, lifetime, characterName));
        }
    }

    private void OnRuleStartup(Entity<MistGameRuleComponent> ent, ref ComponentStartup args)
    {
        EntityManager.System<ArrivalsSystem>().SetArrivals(false); // ensure that arrivals disabled
    }

    private void OnAfterPlayerSpawner(PlayerSpawnAfterEvent ev)
    {
        var query = EntityQueryEnumerator<MistGameRuleComponent>();
        while (query.MoveNext(out var _, out var rule))
        {
            _sawmill.Debug("New player was spawned! Start initializing...");

            if (_mind.TryGetMind(ev.PlayerEntity, out var mindId, out var _))
            {
                var characterName = MetaData(ev.PlayerEntity).EntityName;
                rule.Minds.Add((mindId, characterName));
            }

            EnsureComp<MistPlayerComponent>(ev.PlayerEntity);
            EnsureComp<KillTrackerComponent>(ev.PlayerEntity);
            EnsureComp<LifeTrackerComponent>(ev.PlayerEntity);

            rule.AlivePlayers.Add(ev.PlayerEntity);

            _sawmill.Debug("New player was successfully initialized!");
        }
    }

    private string GenerateGraveMessage(KillReportedEvent ev, LifeTrackerComponent lifeTracker, string characterName)
    {
        // TODO: move to localization
        var primary = $"{characterName} - from {lifeTracker.StartupTime} to {lifeTracker.DeathTime} / {lifeTracker.Lifetime}";

        if (ev.Primary is KillEnvironmentSource)
        {
            primary = $"{primary}\n\tYou couldn't stand the environment";
        }
        else if (ev.Primary is KillPlayerSource playerSource)
        {
            if (ev.Suicide)
            {
                primary = "\n\tWe remember you, the mate...";
            }
            else
            {
                primary = $"{primary}\n\tMade a mistake with the choice of a friend.";
                if (_mind.TryGetMind(playerSource.PlayerId, out var mind))
                {
                    primary = $"{primary}\n\tTHANK YOU, MOTHERFUCKER {mind.Value.Comp.CharacterName ?? "murderer"}!";
                }
            }
        }
        else if (ev.Primary is KillNpcSource npcSource)
        {
            primary = $"\n\tFound your death in fight with {MetaData(npcSource.NpcEnt).EntityName}.";
        }
        else
        {
            primary = "\n\tNobody knows, hot them death. Developer too.";
        }

        return primary;
    }
}
