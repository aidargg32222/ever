using Content.Server.SurveillanceCamera;
using Content.Shared.Backmen.StationAI;
using Content.Shared.Backmen.StationAI.Events;
using Robust.Server.GameObjects;

namespace Content.Server.Backmen.StationAI.Systems;

public sealed class AICameraList : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _userInterfaceSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AICameraListMessage>(HandleCameraListMessage);
    }

    private void HandleCameraListMessage(AICameraListMessage args)
    {
        var owner = GetEntity(args.Owner);
        // You need to be an AI to use this.
        if (!EntityManager.TryGetComponent<AIEyeComponent>(owner, out var _))
            return;
        var cameraList = new List<EntityUid>();
        var cameras = EntityManager.EntityQueryEnumerator<SurveillanceCameraComponent>();
        while (cameras.MoveNext(out var uid, out _))
        {
            cameraList.Add(uid);
        }

        if (_userInterfaceSystem.TryGetOpenUi(owner, args.UiKey, out var ui))
        {
            var state = new AIBoundUserInterfaceState(GetNetEntityList(cameraList));
            _userInterfaceSystem.SetUiState(owner, ui.UiKey, state);
        }
    }
}
