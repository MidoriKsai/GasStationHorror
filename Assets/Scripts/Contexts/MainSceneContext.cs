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
        private GrabbablesComponent grabbablesComponent;

        private CustomerController customerController;
        private ScenarioController scenarioController;
        private GrabbablesController grabbablesController;

        protected override void Initialize(ServiceContainer serviceContainer)
        {
            var playerService = serviceContainer.Resolve<IPlayerService>();
            playerService.Initialize(playerDataHandler);

            scenarioComponent.Initialize(serviceContainer);
            scenarioController = scenarioComponent.CreateController();
            customerController = customersComponent.CreateController();
            grabbablesController = grabbablesComponent.CreateController();
            grabbablesComponent.Initialize(playerDataHandler);

            scenarioController.StartScenario();
        }

        protected override void Deinitialize()
        {
            customerController.Dispose();
        }
    }
}
