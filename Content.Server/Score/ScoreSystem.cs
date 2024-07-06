using Content.Server.Database;
using Content.Shared.Score;

namespace Content.Server.Score;

/// <summary>
/// This handles...
/// </summary>
public sealed class ScoreSystem : EntitySystem
{
    /// <inheritdoc/>
    [Dependency] private readonly IServerDbManager _db = default!;
    public override void Initialize()
    {
        SubscribeNetworkEvent<RequestScoreListEvent>(OnScoreListRequired);
    }

    private async void OnScoreListRequired(RequestScoreListEvent ev, EntitySessionEventArgs args)
    {
        var list = await _db.LoadPlayersScores();
        var rev = new ScoreListEvent(list);
        RaiseNetworkEvent(rev, args.SenderSession);
    }
}
