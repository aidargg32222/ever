using Content.Shared.CCVar;
using Content.Shared.Chat.TypingIndicator;
using Robust.Client.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;

namespace Content.Client.Chat.TypingIndicator;

// Client-side typing system tracks user input in chat box
public sealed class TypingIndicatorSystem : SharedTypingIndicatorSystem
{
    [Dependency] private readonly IGameTiming _time = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private readonly TimeSpan _typingTimeout = TimeSpan.FromSeconds(2);
    private TimeSpan _lastTextChange;
    private bool _isClientTyping;
<<<<<<< HEAD
    private bool _isClientChatFocused; // Corvax-TypingIndicator
=======
    private bool _isClientChatFocused;
>>>>>>> upstream/master

    public override void Initialize()
    {
        base.Initialize();

        Subs.CVar(_cfg, CCVars.ChatShowTypingIndicator, OnShowTypingChanged);
    }

    public void ClientChangedChatText()
    {
        // don't update it if player don't want to show typing indicator
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        // client typed something - show typing indicator
<<<<<<< HEAD
        // Corvax-TypingIndicator-Start
        _isClientTyping = true;
        ClientUpdateTyping();
        // Corvax-TypingIndicator-End
=======
        _isClientTyping = true;
        ClientUpdateTyping();
>>>>>>> upstream/master
        _lastTextChange = _time.CurTime;
    }

    public void ClientSubmittedChatText()
    {
        // don't update it if player don't want to show typing
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        // client submitted text - hide typing indicator
<<<<<<< HEAD
        // Corvax-TypingIndicator-Start
        _isClientTyping = false;
        _isClientChatFocused = false;
        ClientUpdateTyping();
        // Corvax-TypingIndicator-End
=======
        _isClientTyping = false;
        ClientUpdateTyping();
    }

    public void ClientChangedChatFocus(bool isFocused)
    {
        // don't update it if player don't want to show typing
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        // client submitted text - hide typing indicator
        _isClientChatFocused = isFocused;
        ClientUpdateTyping();
>>>>>>> upstream/master
    }

    // Corvax-TypingIndicator-Start
    public void ClientChangedChatFocus(bool isFocused)
    {
        // don't update it if player don't want to show typing
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        // client submitted text - hide typing indicator
        _isClientChatFocused = isFocused;
        ClientUpdateTyping();
    }
    // Corvax-TypingIndicator-End

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_time.IsFirstTimePredicted)
            return;

        // check if client didn't changed chat text box for a long time
        if (_isClientTyping)
        {
            var dif = _time.CurTime - _lastTextChange;
            if (dif > _typingTimeout)
            {
<<<<<<< HEAD
                // client didn't typed anything for a long time - hide indicator
                // Corvax-TypingIndicator-Start
                _isClientTyping = false;
                ClientUpdateTyping();
                // Corvax-TypingIndicator-End
=======
                // client didn't typed anything for a long time - change indicator
                _isClientTyping = false;
                ClientUpdateTyping();
>>>>>>> upstream/master
            }
        }
    }

<<<<<<< HEAD
    private void ClientUpdateTyping() // Corvax-TypingIndicator
    {
        // Corvax-TypingIndicator-Start
        // if (_isClientTyping == isClientTyping)
        //     return;
        // Corvax-TypingIndicator-End

        // check if player controls any entity.
        if (_playerManager.LocalEntity == null)
            return;

        // Corvax-TypingIndicator-Start
        // _isClientTyping = isClientTyping;
        var state = TypingIndicatorState.None;
        if (_isClientChatFocused)
            state = _isClientTyping ? TypingIndicatorState.Typing : TypingIndicatorState.Idle;
        // Corvax-TypingIndicator-End
        // send a networked event to server
        RaisePredictiveEvent(new TypingChangedEvent(state)); // Corvax-TypingIndicator
=======
    private void ClientUpdateTyping()
    {
        // check if player controls any pawn
        if (_playerManager.LocalEntity == null)
            return;

        var state = TypingIndicatorState.None;
        if (_isClientChatFocused)
            state = _isClientTyping ? TypingIndicatorState.Typing : TypingIndicatorState.Idle;

        // send a networked event to server
        RaisePredictiveEvent(new TypingChangedEvent(state));
>>>>>>> upstream/master
    }

    private void OnShowTypingChanged(bool showTyping)
    {
        // hide typing indicator immediately if player don't want to show it anymore
        if (!showTyping)
        {
<<<<<<< HEAD
            // Corvax-TypingIndicator-Start
            _isClientTyping = false;
            ClientUpdateTyping();
            // Corvax-TypingIndicator-End
=======
            _isClientTyping = false;
            ClientUpdateTyping();
>>>>>>> upstream/master
        }
    }
}
