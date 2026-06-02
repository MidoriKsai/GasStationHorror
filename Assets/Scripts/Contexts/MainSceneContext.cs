using Components;
using Controllers;
using Core;
using Cysharp.Threading.Tasks.Triggers;
using Player;
using Services.Interfaces;
using TipsSystem;
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
        
        [SerializeField]
        private CookingSystemComponent cookingSystemComponent;
        
        [SerializeField] private SmartTerminalComponent smartTerminalComponent;
    
        [SerializeField] private TipComponent tipComponent;
        
        [SerializeField] private InteractablesTipsComponent interactablesTipsComponent;
        
        [SerializeField]
        private PointsHandler pointsHandler;

        [SerializeField]
        private SoundsComponent soundsComponent;

        private DialogueSystemController dialogueSystemController;
        private CustomerController customerController;
        private ScenarioController scenarioController;
        private GrabbablesController grabbablesController;
        private SmartTerminalController smartTerminalController;
        private TipController tipController;

        protected override void Initialize(ServiceContainer serviceContainer)
        {
            var playerService = serviceContainer.Resolve<IPlayerService>();
            playerService.Initialize(playerDataHandler);
            
            var inventory = serviceContainer.Resolve<IInventoryService>();

            var soundService = serviceContainer.Resolve<ISoundService>();
            soundService.Initialize(soundsComponent.TwoDAudioSource, soundsComponent.ThreeDAudioSources);

            dialogueSystemComponent.Initialize(serviceContainer);
            dialogueSystemController = dialogueSystemComponent.CreateController();
            serviceContainer.Register(dialogueSystemController);
            
            tipController = tipComponent.CreateController();
            serviceContainer.Register(tipController);

            grabbablesComponent.Initialize(serviceContainer, inputHandler);
            grabbablesController = grabbablesComponent.CreateController();
            
            scenarioComponent.Initialize(serviceContainer);
            
            interactablesTipsComponent.Initialize(serviceContainer, tipController);

            customersComponent.Initialize(pointsHandler);
            customerController = customersComponent.CreateController();
            serviceContainer.Register(customerController);
            
            smartTerminalComponent.Initialize(serviceContainer);
            smartTerminalController = smartTerminalComponent.CreateController();
            serviceContainer.Register(smartTerminalController);
            
            cookingSystemComponent.Initialize(serviceContainer);

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
            tipController?.Dispose();
        }
    }
}