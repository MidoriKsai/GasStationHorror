using Components;
using Components.ScenarioSteps;
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

        [SerializeField]
        private DialogueSystemComponent dialogueSystemComponent;

        [SerializeField]
        private PointsHandler pointsHandler;

        private DialogueSystemController dialogueSystemController;
        private CustomerController customerController;
        private ScenarioController scenarioController;

        protected override void Initialize(ServiceContainer serviceContainer)
        {
            var playerService = serviceContainer.Resolve<IPlayerService>();
            playerService.Initialize(playerDataHandler);

            dialogueSystemComponent.Initialize(serviceContainer);
            dialogueSystemController = dialogueSystemComponent.CreateController();
            serviceContainer.Register(dialogueSystemController);

            customersComponent.Initialize(pointsHandler);
            customerController = customersComponent.CreateController();
            serviceContainer.Register(customerController);

            grabbablesComponent.Initialize(playerDataHandler);

            scenarioComponent.Initialize(serviceContainer);
            scenarioController = scenarioComponent.CreateController();

            scenarioController.StartScenario();
        }

        protected override void Deinitialize()
        {
            scenarioController.Dispose();
            customerController.Dispose();
            dialogueSystemController.Dispose();
        }
    }
}