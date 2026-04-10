using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using Services.Interfaces;
using UnityEngine;
using Utils;

namespace Services.Implementations
{
    public class PlayerService : IPlayerService
    {
        private PlayerDataHandler playerDataHandler;

        public void Initialize(PlayerDataHandler playerDataHandler)
        {
            this.playerDataHandler = playerDataHandler;
        }

        public Transform GetPlayerTransform()
        {
            return playerDataHandler.PlayerTransform;
        }

        public Transform GetGrabbablesHoldingPoint()
        {
            return playerDataHandler.GetHoldingPoint;
        }

        public async UniTask FadeInAsync(CancellationToken ct)
        {
            await playerDataHandler.FaderCanvasGroup.DOFade(1f, 0.5f).AwaitAsync(ct);
        }

        public async UniTask FadeOutAsync(CancellationToken ct)
        {
            await playerDataHandler.FaderCanvasGroup.DOFade(0f, 0.5f).AwaitAsync(ct);
        }

        public void FocusPlayerToDialogue(Transform dialogTarget)
        {
            playerDataHandler.PlayerMovement.SetMovementEnabled(false);
            playerDataHandler.CameraLook.SnapToTarget(dialogTarget);
            playerDataHandler.CameraLook.SetLookEnabled(false);
        }

        public void UnfocusPlayerFromDialogue()
        {
            playerDataHandler.PlayerMovement.SetMovementEnabled(true);
            playerDataHandler.CameraLook.SetLookEnabled(true);
        }
    }
}
