using Robust.Shared.Serialization;

namespace Content.Shared.Score;

    /// <summary>
    /// Contains loaded score list as tuple.
    /// </summary>
    [NetSerializable, Serializable]
    public sealed class LoadedScoreListEvent(List<(string, int, int)> scores) : EntityEventArgs
    {
        public readonly List<(string, int, int)> Scores = scores;
    }
