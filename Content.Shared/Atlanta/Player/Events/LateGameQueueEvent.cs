using Robust.Shared.Serialization;

namespace Content.Shared.Atlanta.Player.Events;

/// <summary>
/// Ensure on player mind and insert it to late join queue.
/// </summary>
[ByRefEvent]
[Serializable, NetSerializable]
public sealed class JoinLateGameQueueEvent : CancellableEntityEventArgs;

/// <summary>
/// Remove mind from queue.
/// </summary>
[ByRefEvent]
[Serializable, NetSerializable]
public sealed class LeaveLateGameQueueEvent : CancellableEntityEventArgs;
