using Robust.Shared.Prototypes;

namespace Content.Server.Atlanta.GameTicking.Rules.Components;

/// <summary>
/// For extended logic for late join.
/// </summary>
[RegisterComponent, Access(typeof(ExtendedLateJoinRuleSystem))]
public sealed partial class ExtendedLateJoinRuleComponent : Component
{
    /// <summary>
    /// Collects all minds ready to join.
    /// </summary>
    public readonly List<EntityUid> MindQueue = [];

    /// <summary>
    /// Collects all spawners.
    /// </summary>
    public readonly Dictionary<EntProtoId, List<EntityUid>> SpawnPointsDictionary = new();
}
