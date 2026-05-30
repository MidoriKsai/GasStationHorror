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
        [SerializeField] private AudioClip grabSound;

        private IPlayerService playerService;
        private IInventoryService inventoryService;
        private ISoundService soundService;
        private InputHandler inputHandler;

        private GrabbablesController grabbablesController;
        public IInventoryService GetInventoryService() => inventoryService;

        public void Initialize(ServiceContainer serviceContainer, InputHandler inputHandler)
        {
            playerService = serviceContainer.Resolve<IPlayerService>();
            inventoryService = serviceContainer.Resolve<IInventoryService>();
            soundService = serviceContainer.Resolve<ISoundService>();
            this.inputHandler = inputHandler;
        }

        public void RegisterNewGrabbable(Grabbable grabbable)
        {
            grabbablesController.RegisterNewGrabbable(grabbable);
        }

        public GrabbablesController CreateController()
        {
            grabbablesController = new GrabbablesController(grabbablesList, inventoryService, 
            soundService, playerService, inputHandler, grabSound);
            return grabbablesController;
        }
    }
}