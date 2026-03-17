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

        private CustomerController customerController;
        private ScenarioController scenarioController;

        protected override void Initialize(ServiceContainer serviceContainer)
        {
            var playerService = serviceContainer.Resolve<IPlayerService>();
            playerService.Initialize(playerDataHandler);

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