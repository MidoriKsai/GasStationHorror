using Components;
using Controllers;
using Core;
using Player;
using Services.Interfaces;
using UnityEngine;

namespace Contexts
{
    public class MainSceneContext : BaseContext
    {
        [SerializeField]
        private PlayerDataHandler playerDataHandler;

        [SerializeField]
        private ScenarioComponent scenarioComponent;

        [SerializeField]
        private CustomersComponent customersComponent;

        [SerializeField]
        private PlayerComponent playerComponent;

        [SerializeField]
        private GrabbablesComponent grabbablesComponent;

        private CustomerController customerController;
        private ScenarioController scenarioController;
        private GrabbablesController grabbablesController;

        protected override void Initialize(ServiceContainer serviceContainer)
        {
            var playerService = serviceContainer.Resolve<IPlayerService>();
            playerService.Initialize(playerDataHandler);
            playerComponent.SetServiceContainer(serviceContainer);

            grabbablesController = grabbablesComponent.CreateController();
            grabbablesComponent.Initialize(serviceContainer);


            IInventoryService inventoryService = serviceContainer.Resolve<IInventoryService>();
            inventoryService.Initialize(grabbablesController);

            scenarioComponent.Initialize(serviceContainer);
            scenarioController = scenarioComponent.CreateController();
            customerController = customersComponent.CreateController();


            scenarioController.StartScenario();
        }

        protected override void Deinitialize()
        {
            customerController.Dispose();
        }
    }
}
