using Core.Interfaces;
using System.Collections.Generic;
using Services.Interfaces;
using UnityEngine;

namespace Controllers
{
    public class GrabbablesController : IController
    {
        private List<Grabbable> grabbables;
        private readonly IInventoryService inventoryService;
        private readonly IPlayerService playerService;
        private readonly InputHandler inputHandler;
        private readonly ISoundService soundService;
        private readonly AudioClip grabSound;

        public GrabbablesController(
            List<Grabbable> grabbables,
            IInventoryService inventoryService,
            ISoundService soundService,
            IPlayerService playerService,
            InputHandler inputHandler,
            AudioClip grabSound)
        {
            this.grabbables = grabbables;
            this.inventoryService = inventoryService;
            this.playerService = playerService;
            this.inputHandler = inputHandler;
            this.soundService = soundService;
            this.grabSound = grabSound;
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
                soundService.Play2DSound(grabSound, 0.2f);
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