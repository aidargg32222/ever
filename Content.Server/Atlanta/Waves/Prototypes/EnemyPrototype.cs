using Robust.Shared.Prototypes;

namespace Content.Server.Atlanta.Waves;

/// <summary>
/// This is a prototype for enemy setup. <see cref="EnemyStackPrototype"/>
/// </summary>
[Prototype("wavesEnemy")]
public sealed partial class EnemyPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    [DataField(required: true)]
    public EntProtoId ProtoId = default!;

    [DataField]
    public float Difficulty = 1;
}
