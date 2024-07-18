using Content.Server.Atlanta.GameTicking.Rules;

namespace Content.Server.Atlanta.Waves.Events;

/// <summary>
/// Pauses waves.
/// </summary>
public sealed class PauseWavesEvent;

/// <summary>
/// Unpauses waves.
/// </summary>
public sealed class UnpauseWavesEvent;

/// <summary>
/// Delays waves by <para>delay</para>.
/// </summary>
/// <param name="delay">Delaying time</param>
public sealed class DelayWaveEvent(TimeSpan delay)
{
    public readonly TimeSpan Delay = delay;
}

/// <summary>
/// Speedups waves.
/// </summary>
/// <param name="speedup"></param>
public sealed class SpeedupWaveEvent(TimeSpan speedup)
{
    public readonly TimeSpan SpeedUp = speedup;
}

/// <summary>
/// Change difficulty by value. <para>difficulty </para> may be negative value, if player leaves for example
/// </summary>
/// <param name="difficulty"></param>
public sealed class ChangeDifficultyEvent(float difficulty)
{
    public readonly float Difficulty = difficulty;
}

/// <summary>
/// Report to <see cref="EnemyWavesRuleSystem"/> about enemy death.
/// </summary>
/// <param name="enemy"></param>
public sealed class EnemyDeadReportEvent(EntityUid enemy, TimeSpan removingTime)
{
    public readonly EntityUid Enemy = enemy;
    public readonly TimeSpan RemovingTime = removingTime;
}
