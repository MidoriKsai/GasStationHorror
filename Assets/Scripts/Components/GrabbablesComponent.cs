using System.Collections.Generic;
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

        private GrabbablesController grabbablesController;
        public IInventoryService GetInventoryService() => inventoryService;

        public void Initialize(ServiceContainer serviceContainer, InputHandler inputHandler)
        {
            playerService = serviceContainer.Resolve<IPlayerService>();
            inventoryService = serviceContainer.Resolve<IInventoryService>();
            this.inputHandler = inputHandler;
        }

        public void RegisterNewGrabbable(Grabbable grabbable)
        {
            grabbablesController.RegisterNewGrabbable(grabbable);
        }

        public GrabbablesController CreateController()
        {
            grabbablesController = new GrabbablesController(grabbablesList, inventoryService, playerService, inputHandler);
            return grabbablesController;
        }
    }
}