using System.Collections.Generic;
using Components.ScenarioSteps;
using Controllers;
using Core.Interfaces;
using UnityEngine;

namespace Components
{
    public class ScenarioComponent : MonoBehaviour , IComponent<ScenarioController>
    {
        [SerializeField]
        private List<BaseStep> scenarioSteps = new();


        public ScenarioController CreateController()
        {
            return new ScenarioController(scenarioSteps);
        }
    }
}