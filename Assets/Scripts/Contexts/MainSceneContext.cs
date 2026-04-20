using Components;
using Controllers;
using Core;
using Cysharp.Threading.Tasks.Triggers;
using Player;
using Services.Interfaces;
using UnityEngine;

namespace Contexts
{
    public class MainSceneContext : BaseContext
    {
        [SerializeField]
        private InputHandler inputHandler;

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
        
        [SerializeField] private SmartTerminalComponent smartTerminalComponent;

        [SerializeField]
        private PointsHandler pointsHandler;
        
        [SerializeField] private CashRegisterInteractable cashRegister;
        

        

        private DialogueSystemController dialogueSystemController;
        private CustomerController customerController;
        private ScenarioController scenarioController;
        private GrabbablesController grabbablesController;
        private SmartTerminalController smartTerminalController;

        protected override void Initialize(ServiceContainer serviceContainer)
        {
            var playerService = serviceContainer.Resolve<IPlayerService>();
            playerService.Initialize(playerDataHandler);
            
            var inventory = serviceContainer.Resolve<IInventoryService>();
            cashRegister.Initialize(inventory);

            dialogueSystemComponent.Initialize(serviceContainer);
            dialogueSystemController = dialogueSystemComponent.CreateController();
            serviceContainer.Register(dialogueSystemController);

            grabbablesComponent.Initialize(serviceContainer, inputHandler);
            grabbablesController = grabbablesComponent.CreateController();
            
            scenarioComponent.Initialize(serviceContainer);


            customersComponent.Initialize(pointsHandler);
            customerController = customersComponent.CreateController();
            serviceContainer.Register(customerController);
            
            smartTerminalComponent.Initialize(serviceContainer);
            smartTerminalController = smartTerminalComponent.CreateController();
            serviceContainer.Register(smartTerminalController);

            scenarioComponent.Initialize(serviceContainer);
            scenarioController = scenarioComponent.CreateController();

            scenarioController.StartScenario();
        }

        protected override void Deinitialize()
        {
            scenarioController.Dispose();
            customerController.Dispose();
            dialogueSystemController.Dispose();
            grabbablesController.Dispose();
            smartTerminalController.Dispose();
        }
    }
}