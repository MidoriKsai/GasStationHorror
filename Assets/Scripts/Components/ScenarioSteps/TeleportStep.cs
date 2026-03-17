using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Services.Interfaces;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public class TeleportStep : BaseStep
    {
        [SerializeField]
        private Transform teleportPoint;

        private IPlayerService playerService;

        public override void Initialize(ServiceContainer serviceContainer)
        {
            playerService = serviceContainer.Resolve<IPlayerService>();
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            await playerService.FadeInAsync(ct);
            playerService.GetPlayerTransform().position = teleportPoint.position;
            await playerService.FadeOutAsync(ct);
        }
    }
}