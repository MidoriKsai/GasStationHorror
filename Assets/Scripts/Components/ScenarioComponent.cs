using System.Collections.Generic;
using Components.ScenarioSteps;
using Controllers;
using Core;
using Core.Interfaces;
using UnityEngine;

namespace Components
{
    public class ScenarioComponent : MonoBehaviour , IComponent<ScenarioController>
    {
        [SerializeField]
        private List<BaseStep> scenarioSteps = new();

        private ServiceContainer serviceContainer;

        public ScenarioController CreateController()
        {
            return new ScenarioController(scenarioSteps, serviceContainer);
        }

        public void Initialize(ServiceContainer serviceContainer)
        {
            this.serviceContainer = serviceContainer;
        }
    }
}