using Robust.Shared.Prototypes;

namespace Content.Shared.Atlanta.Supply.Prototypes;

/// <summary>
/// Used to define supply point content.
/// </summary>
[Prototype("supplyPointContent")]
public sealed partial class SupplyContentPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    /// <summary>
    /// List of entities that can be spawned.
    /// </summary>
    [DataField]
    public readonly List<EntProtoId> Content = [];
}
