using Content.Client.Overlays;
using Content.Client.UserInterface.Systems.DamageOverlays.Overlays;
using Content.Shared.Mobs.Components;
using Robust.Client.Graphics;
using Robust.Shared.Player;

namespace Content.Shared.Atlanta.Mist;

/// <summary>
/// This handles...
/// </summary>
public sealed class MistPlayerSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlayManager = default!;

    private MistPlayerOverlay _overlay = default!;

    public override void Initialize()
    {
        _overlay = new MistPlayerOverlay();

        SubscribeLocalEvent<MistPlayerComponent, LocalPlayerAttachedEvent>(OnPlayerAttach);
        SubscribeLocalEvent<MistPlayerComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);
    }

    private void OnPlayerAttach(Entity<MistPlayerComponent> _, ref LocalPlayerAttachedEvent ev)
    {
        if (!EntityManager.TryGetComponent<MobStateComponent>(ev.Entity, out var _))
            return;
        _overlayManager.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(Entity<MistPlayerComponent> _, ref LocalPlayerDetachedEvent ev)
    {
        _overlayManager.RemoveOverlay(_overlay);
    }
}
