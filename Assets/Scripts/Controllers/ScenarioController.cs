using System.Collections.Generic;
using Components.ScenarioSteps;
using Core.Interfaces;

namespace Controllers
{
    public class ScenarioController : IController
    {
        private readonly List<BaseStep> scenarioSteps;

        private int currentStepIndex;

        public ScenarioController(List<BaseStep> scenarioSteps)
        {
            this.scenarioSteps = scenarioSteps;
        }

        public void StartScenario()
        {
            StartNextStep();
        }

        private void StartNextStep()
        {
            scenarioSteps[currentStepIndex].StepCompleted += OnStepCompleted;
            scenarioSteps[currentStepIndex].BeginStep();
            currentStepIndex++;
        }

        private void OnStepCompleted(BaseStep baseStep)
        {
            baseStep.StepCompleted -= OnStepCompleted;
            StartNextStep();
        }

        public void Dispose()
        {
        }
    }
}