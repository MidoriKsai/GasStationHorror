using System.Collections.Generic;
using System.Threading;
using Components.ScenarioSteps;
using Core;
using Core.Interfaces;
using Cysharp.Threading.Tasks;

namespace Controllers
{
    public class ScenarioController : IController
    {
        private readonly List<BaseStep> scenarioSteps;

        private int currentStepIndex;
        private CancellationTokenSource scenarioCts;

        public ScenarioController(List<BaseStep> scenarioSteps, ServiceContainer serviceContainer)
        {
            this.scenarioSteps = scenarioSteps;

            InitializeAllSteps(serviceContainer);
        }

        public void StartScenario()
        {
            scenarioCts?.Cancel();
            scenarioCts = new CancellationTokenSource();
            CompleteScenarioAsync(scenarioCts.Token).Forget();
        }

        private async UniTask CompleteScenarioAsync(CancellationToken ct)
        {
            foreach (var scenarioStep in scenarioSteps)
            {
                await scenarioStep.PerformStepAsync(ct);
            }
        }

        private void InitializeAllSteps(ServiceContainer serviceContainer)
        {
            foreach (var scenarioStep in scenarioSteps)
            {
                scenarioStep.Initialize(serviceContainer);
            }
        }

        public void Dispose()
        {
        }
    }
}