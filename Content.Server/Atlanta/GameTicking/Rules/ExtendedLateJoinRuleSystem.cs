using Content.Server.Atlanta.GameTicking.Rules.Components;
using Content.Server.Atlanta.Player.Events;
using Content.Server.Atlanta.Waves.Events;
using Content.Server.GameTicking.Rules;
using Content.Server.Mind;
using Content.Shared.Atlanta.Player.Events;
using Content.Shared.Mind;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Atlanta.GameTicking.Rules;

/// <summary>
/// Handles events and try to spawn new entities with player attaching.
/// Powerful for others rules when you need some more logic for player spawning or respawn.
/// </summary>
public sealed class ExtendedLateJoinRuleSystem :  GameRuleSystem<ExtendedLateJoinRuleComponent>
{
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        // queue
        SubscribeNetworkEvent<JoinLateGameQueueEvent>(OnJoinQueue);
        SubscribeNetworkEvent<LeaveLateGameQueueEvent>(OnLeaveQueue);
        // spawners
        SubscribeLocalEvent<ExtendedLateJoinPointRegisterEvent>(OnSpawnerRegister);
        // spawning
        SubscribeLocalEvent<SpawnQueuePlayersEvent>(OnSpawnPlayerQueue);
        SubscribeLocalEvent<ExtendedSpawnEntityEvent>(OnExtendedSpawnEntity);

        _sawmill = _logManager.GetSawmill("Extended Late Join Rule");
    }

    private void OnSpawnerRegister(ExtendedLateJoinPointRegisterEvent ev)
    {
        var queue = EntityQueryEnumerator<ExtendedLateJoinRuleComponent>();
        while (queue.MoveNext(out var _, out var rule))
        {
            if (!rule.SpawnPointsDictionary.TryGetValue(ev.ProtoId, out var value))
            {
                value = [];
                rule.SpawnPointsDictionary[ev.ProtoId] = value;
            }

            value.Add(ev.SpawnerEnt);
        }
    }

    private void OnSpawnPlayerQueue(SpawnQueuePlayersEvent ev)
    {
        _sawmill.Debug("Starts player spawning!");

        var queue = EntityQueryEnumerator<ExtendedLateJoinRuleComponent>();
        while (queue.MoveNext(out var _, out var rule))
        {
            if (rule.MindQueue.Count == 0)
            {
                _sawmill.Debug("Skip rule spawning because queue is empty.");
                continue;
            }
            foreach (var mindId in rule.MindQueue)
            {
                if (!TryComp<MindComponent>(mindId, out var mindComp))
                {
                    _sawmill.Debug($"Skip {mindId} mind because can't get mindComp");
                    continue;
                }

                if (!_mind.IsCharacterDeadIc(mindComp))
                {
                    _sawmill.Debug($"Skip {mindId} mind because player is not dead IC.");
                    continue;
                }

                if (!TryGetRandomSpawnerCoordinates(ev.SpawnPointProtoId, out var coords))
                    continue;

                var instance = Spawn(ev.PlayerProtoId, coords!.Value);
                _mind.ControlMob(mindId, instance);

                RaiseLocalEvent(new PlayerSpawnAfterEvent(instance));
            }
        }
    }

    private void OnLeaveQueue(LeaveLateGameQueueEvent ev, EntitySessionEventArgs args)
    {
        if (_mind.TryGetMind(args.SenderSession.UserId, out var mindId))
        {
            var queue = EntityQueryEnumerator<ExtendedLateJoinRuleComponent>();
            while (queue.MoveNext(out var _, out var rule))
            {
                rule.MindQueue.Remove(mindId.Value);
            }
        }
        else
        {
            _sawmill.Debug("Cancel leaving queue because mind was not found.");
            ev.Cancel();
        }
    }


    private void OnJoinQueue(JoinLateGameQueueEvent ev, EntitySessionEventArgs args)
    {
        if (_mind.TryGetMind(args.SenderSession.UserId, out var mindId, out var _))
        {
            var queue = EntityQueryEnumerator<ExtendedLateJoinRuleComponent>();
            while (queue.MoveNext(out var _, out var rule))
            {
                rule.MindQueue.Add(mindId.Value);
            }
        }
        else
        {

            _sawmill.Debug("Cancel joining queue because mind was not found.");
            ev.Cancel();
        }
    }

    #region NonPlayerEntity

    private void OnExtendedSpawnEntity(ref ExtendedSpawnEntityEvent ev)
    {
        if (TryGetRandomSpawnerCoordinates(ev.SpawnerProtoId, out var coords))
        {
            ev.Instance = Spawn(ev.EntityProtoId, coords!.Value);
        }
    }

    #endregion

    private bool TryGetRandomSpawnerCoordinates(EntProtoId pointProto, out MapCoordinates? coords)
    {
        var queue = EntityQueryEnumerator<ExtendedLateJoinRuleComponent>();
        while (queue.MoveNext(out var _, out var rule))
        {
            var spawner = _random.Pick(rule.SpawnPointsDictionary[pointProto]);
            coords = _transform.GetMapCoordinates(spawner);

            return true;
        }

        coords = null;

        _sawmill.Debug("Could not find any spawner. Maybe you forgot to place on a map?");
        return false;
    }
}
