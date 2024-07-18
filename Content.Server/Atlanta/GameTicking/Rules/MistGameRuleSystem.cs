using Content.Server.Atlanta.GameTicking.Rules.Components;
using Content.Server.GameTicking.Rules;
using Content.Server.Mind;
using Content.Server.Shuttles.Systems;

namespace Content.Server.Atlanta.GameTicking.Rules;

/// <summary>
/// This handles...
/// </summary>
public sealed class MistGameRuleSystem :  GameRuleSystem<MistGameRuleComponent>
{
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly ILogManager _logManager = default!;

    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<MistGameRuleComponent, ComponentStartup>(OnRuleStartup);

        _sawmill = _logManager.GetSawmill("Mist Game Rule");
    }

    private void OnRuleStartup(Entity<MistGameRuleComponent> ent, ref ComponentStartup args)
    {
        EntityManager.System<ArrivalsSystem>().SetArrivals(false); // ensure that arrivals disabled
    }
}
