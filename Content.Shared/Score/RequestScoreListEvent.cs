using System.Threading.Tasks;
using Robust.Shared.Serialization;

namespace Content.Shared.Score;

/// <summary>
/// This uses to require score list for listing in lobby.
/// </summary>
[NetSerializable, Serializable]
public sealed class RequestScoreListEvent : EntityEventArgs
{
}
