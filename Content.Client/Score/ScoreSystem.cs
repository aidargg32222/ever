using System.Threading.Tasks;
using Content.Client.Lobby.UI;
using Content.Shared.Score;
using Robust.Client.UserInterface;

namespace Content.Client.Score;

/// <summary>
/// This handles...
/// </summary>
public sealed class ScoreSystem : EntitySystem
{
    /// <inheritdoc/>
    [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<LoadedScoreListEvent>(OnScoreList);
    }

    private void OnScoreList(LoadedScoreListEvent ev)
    {
        if (_userInterfaceManager.ActiveScreen is LobbyGui lobbyGui)
        {
            lobbyGui.UpdateScoreList(ev.Scores);
        }
    }

    public void LoadScoreboard()
    {
        var rev = new RequestScoreListEvent();
        RaiseNetworkEvent(rev);
    }
}
