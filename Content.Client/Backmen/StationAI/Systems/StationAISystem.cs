using Content.Client.Storage.Components;
using Content.Shared.Backmen.StationAI;
using Content.Shared.Backmen.StationAI.Events;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Overlays;
using Robust.Shared.Utility;

namespace Content.Client.Backmen.StationAI;

public sealed class StationAISystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<StationAIComponent, InteractionAttemptEvent>(CanInteraction);

        SubscribeLocalEvent<AIHealthOverlayEvent>(OnHealthOverlayEvent);
    }

    private void CanInteraction(Entity<StationAIComponent> ent, ref InteractionAttemptEvent args)
    {
        if (HasComp<StationAiDroneComponent>(ent))
            return;
        var core = ent;
        if (TryComp<AIEyeComponent>(ent, out var eye))
        {
            if (eye.AiCore == null)
            {
                args.Cancelled = true;
                return;
            }

            core = eye.AiCore.Value;
        }

        if (!core.Owner.Valid)
        {
            args.Cancelled = true;
            return;
        }

        if (args.Target != null && Transform(core).GridUid != Transform(args.Target.Value).GridUid)
        {
            args.Cancelled = true;
            return;
        }


        if (HasComp<ItemComponent>(args.Target))
        {
            args.Cancelled = true;
            return;
        }

        if (HasComp<EntityStorageComponent>(args.Target))
        {
            args.Cancelled = true;
            return;
        }
    }

    private void OnHealthOverlayEvent(AIHealthOverlayEvent args)
    {
        if (!HasComp<ShowHealthBarsComponent>(args.Performer))
        {

            var showHealthBarsComponent = new ShowHealthBarsComponent
            {
                DamageContainers = new List<string>() { "Biological", "HalfSpirit" },
                NetSyncEnabled = false
            };

            _entityManager.AddComponent(args.Performer, showHealthBarsComponent, true);
        }
        else
        {
            _entityManager.RemoveComponentDeferred<ShowHealthBarsComponent>(args.Performer);
        }

        args.Handled = true;
    }
}
