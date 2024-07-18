using Content.Shared.Wieldable;
using Robust.Shared.Prototypes;

namespace Content.Server.Atlanta.GameTicking.Rules.Components;

/// <summary>
/// Mist Game Rule.
/// </summary>
[RegisterComponent, Access(typeof(MistGameRuleSystem))]
public sealed partial class MistGameRuleComponent : Component
{
    public static readonly EntProtoId PlayerSpawner = "MistGamePlayerSpawnPoint";
    /// <summary>
    /// Contains all players minds.
    /// </summary>
    public readonly List<(EntityUid, string)> Minds = [];

    /// <summary>
    /// Contains currently alive players
    /// </summary>
    public readonly List<EntityUid> AlivePlayers = [];

    /// <summary>
    /// Contains records about player deaths. Uses in round-end manifest.
    /// </summary>
    public readonly List<string> PlayersGrave = [];
}
