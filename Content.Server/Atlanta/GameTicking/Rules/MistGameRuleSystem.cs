using Content.Server.Atlanta.GameTicking.Rules.Components;
using Content.Server.GameTicking.Rules;
using Content.Server.Mind;

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
        _sawmill = _logManager.GetSawmill("Mist Game Rule");
    }
}
