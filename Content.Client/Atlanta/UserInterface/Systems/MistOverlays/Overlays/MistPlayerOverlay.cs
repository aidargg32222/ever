using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client.UserInterface.Systems.DamageOverlays.Overlays;

public sealed class MistPlayerOverlay : Overlay
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public float MistLevel = 0.8f;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private readonly ShaderInstance _mistShader;

    public MistPlayerOverlay()
    {
        IoCManager.InjectDependencies(this);

        _mistShader = _prototypeManager.Index<ShaderPrototype>("GradientCircleMask").InstanceUnique();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (!_entityManager.TryGetComponent(_playerManager.LocalEntity, out EyeComponent? eyeComp))
            return;

        if (args.Viewport.Eye != eyeComp.Eye)
            return;

        var viewport = args.WorldAABB;
        var handle = args.WorldHandle;
        var distance = args.ViewportBounds.Width;

        var time = (float)_timing.RealTime.TotalSeconds;
        var lastFrameTime = (float)_timing.FrameTime.TotalSeconds;

        var pulseRate = 3f;
        var adjustedTime = time * pulseRate;
        float outerMaxLevel = 2.0f * distance;
        float outerMinLevel = 0.8f * distance;
        float innerMaxLevel = 0.6f * distance;
        float innerMinLevel = 0.2f * distance;

        var outerRadius = outerMaxLevel - MistLevel * (outerMaxLevel - outerMinLevel);
        var innerRadius = innerMaxLevel - MistLevel * (innerMaxLevel - innerMinLevel);

        var pulse = MathF.Max(0f, MathF.Sin(adjustedTime));

        _mistShader.SetParameter("time", pulse);
        _mistShader.SetParameter("color", new Vector3(1f, 1f, 1f));
        _mistShader.SetParameter("darknessAlphaOuter", 0.15f);

        _mistShader.SetParameter("outerCircleRadius", outerRadius);
        _mistShader.SetParameter("outerCircleMaxRadius", outerRadius + 0.2f * distance);
        _mistShader.SetParameter("innerCircleRadius", innerRadius);
        _mistShader.SetParameter("innerCircleMaxRadius", innerRadius + 0.02f * distance);
        handle.UseShader(_mistShader);
        handle.DrawRect(viewport, Color.White);
    }
}
