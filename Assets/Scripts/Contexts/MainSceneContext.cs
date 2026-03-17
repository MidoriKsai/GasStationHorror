using Components;
using Controllers;
using Core;
using UnityEngine;

namespace Contexts
{
    public class MainSceneContext : BaseContext
    {
        [SerializeField]
        private ScenarioComponent scenarioComponent;

        [SerializeField]
        private CustomersComponent customersComponent;

        private CustomerController customerController;
        private ScenarioController scenarioController;

        protected override void Initialize()
        {
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