using System.Collections.Generic;
using Player;
using UnityEngine;
using Controllers;
using Services.Interfaces;
using Core;
using Core.Interfaces;

namespace Components
{
    public class GrabbablesComponent : MonoBehaviour, IComponent<GrabbablesController>
    {
        [SerializeField]
        private List<Grabbable> grabbablesList = new();

        private IPlayerService playerService;
        private IInventoryService inventoryService;
        private InputHandler inputHandler;

        public void Initialize(ServiceContainer serviceContainer, InputHandler inputHandler)
        {
            playerService = serviceContainer.Resolve<IPlayerService>();
            inventoryService = serviceContainer.Resolve<IInventoryService>();
            this.inputHandler = inputHandler;

            foreach (var grabbable in grabbablesList)
            {
                Transform holdingPoint = playerService.GetGrabbablesHoldingPoint();
                grabbable.Initialize(holdingPoint);
            }
        }

        public GrabbablesController CreateController()
        {
            return new GrabbablesController(grabbablesList, inventoryService, inputHandler);
        }
    }
}