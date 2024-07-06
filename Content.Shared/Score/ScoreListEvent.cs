using Robust.Shared.Serialization;

namespace Content.Shared.Score;

/// <summary>
/// This uses in lobby.
/// </summary>
[NetSerializable, Serializable]
public sealed class ScoreListEvent(List<(string, int, int)> scores) : EntityEventArgs
{
    public readonly List<(string, int, int)> Scores = scores;
}
