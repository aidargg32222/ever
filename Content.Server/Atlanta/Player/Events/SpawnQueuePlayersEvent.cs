using Robust.Shared.Prototypes;

namespace Content.Server.Atlanta.Player.Events;

/// <summary>
/// Try to spawn players.
/// </summary>
public sealed class SpawnQueuePlayersEvent(EntProtoId playerProto, EntProtoId spawnPointProto)
{
    public readonly EntProtoId PlayerProtoId = playerProto;
    public readonly EntProtoId SpawnPointProtoId = spawnPointProto;
}
