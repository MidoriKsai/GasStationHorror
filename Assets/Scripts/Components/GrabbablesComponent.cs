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
        private Transform holdingPoint;
        private GrabbablesController grabbablesController;

        public IInventoryService GetInventoryService() => inventoryService;

        public void Initialize(ServiceContainer serviceContainer, InputHandler inputHandler)
        {
            playerService = serviceContainer.Resolve<IPlayerService>();
            inventoryService = serviceContainer.Resolve<IInventoryService>();
            this.inputHandler = inputHandler;

            holdingPoint = playerService.GetGrabbablesHoldingPoint();

            foreach (var grabbable in grabbablesList)
            {
                grabbable.Initialize(holdingPoint);
            }
        }

        public void AddGrubbableAtRuntime(Grabbable grabbable)
        {
            grabbable.Initialize(holdingPoint);
            grabbablesList.Add(grabbable);
            grabbablesController.SubscribeAtRuntime(grabbable);
        }

        public GrabbablesController CreateController()
        {
            grabbablesController = new GrabbablesController(grabbablesList, inventoryService, inputHandler);
            return grabbablesController;
        }
    }
}
