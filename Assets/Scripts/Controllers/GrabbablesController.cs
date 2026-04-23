using Core.Interfaces;
using System.Collections.Generic;
using Services.Interfaces;

namespace Controllers
{
    public class GrabbablesController : IController
    {
        private List<Grabbable> grabbables;
        private readonly IInventoryService inventoryService;
        private readonly IPlayerService playerService;
        private readonly InputHandler inputHandler;

        public GrabbablesController(
            List<Grabbable> grabbables,
            IInventoryService inventoryService,
            IPlayerService playerService,
            InputHandler inputHandler)
        {
            this.grabbables = grabbables;
            this.inventoryService = inventoryService;
            this.playerService = playerService;
            this.inputHandler = inputHandler;

            this.inputHandler.ItemDropActionTriggered += OnItemDropActionTriggered;

            SubscribeOnEvents(grabbables);
        }

        public void RegisterNewGrabbable(Grabbable grabbable)
        {
            grabbables.Add(grabbable);
            grabbable.TryGrabbedEvent += OnTryGrabbedEvent;
        }

        private void SubscribeOnEvents(List<Grabbable> grabbables)
        {
            foreach (Grabbable grabbable in grabbables)
            {
                grabbable.TryGrabbedEvent += OnTryGrabbedEvent;
            }
        }

        private void OnItemDropActionTriggered()
        {
            inventoryService.RemoveItem(true);
        }

        private void OnTryGrabbedEvent(Grabbable grabbable)
        {
            if (inventoryService.IsInventoryEmpty())
            {
                inventoryService.AddItem(grabbable);
                grabbable.Grab(playerService.GetGrabbablesHoldingPoint());
            }
        }

        public void Dispose()
        {
            foreach(Grabbable grabbable in grabbables)
            {
                grabbable.TryGrabbedEvent -= OnTryGrabbedEvent;
            }

            inputHandler.ItemDropActionTriggered -= OnItemDropActionTriggered;
        }
    }
}