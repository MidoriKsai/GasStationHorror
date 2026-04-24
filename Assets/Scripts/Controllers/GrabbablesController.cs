using Core.Interfaces;
using System.Collections.Generic;
using Services.Interfaces;

namespace Controllers
{
    public class GrabbablesController : IController
    {
        private List<Grabbable> grabbables;
        private readonly IInventoryService inventoryService;
        private readonly InputHandler inputHandler;

        public GrabbablesController(
            List<Grabbable> grabbables,
            IInventoryService inventoryService,
            InputHandler inputHandler)
        {
            this.grabbables = grabbables;
            this.inventoryService = inventoryService;
            this.inputHandler = inputHandler;

            this.inputHandler.ItemDropActionTriggered += OnItemDropActionTriggered;

            SubscribeOnEvents(grabbables);
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
            inventoryService.RemoveItem();
        }

        private void OnTryGrabbedEvent(Grabbable grabbable)
        {
            if (inventoryService.IsInventoryEmpty())
            {
                inventoryService.AddItem(grabbable);
                grabbable.Grab();
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

        public void SubscribeAtRuntime(Grabbable grabbable)
        {
            grabbable.TryGrabbedEvent += OnTryGrabbedEvent;
        }
    }
}
