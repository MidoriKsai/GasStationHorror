using System.Threading;
using Controllers;
using Core;
using Cysharp.Threading.Tasks;
using NPCSystem.Manager;
using Services.Interfaces;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public class ManagerStep : BaseStep
    {
        private DialogueSystemController _dialogue;
        private TipsSystem.TipController _tipController;

        [SerializeField] private string dialogueID;
        
        [SerializeField] private ManagerTriggerZone managerTriggerZone;
        [SerializeField] private Transform dialoguePoint;
        
        private IPlayerService _playerService;

        public override void Initialize(ServiceContainer container)
        {
            _dialogue = container.Resolve<DialogueSystemController>();
            _playerService = container.Resolve<IPlayerService>();
            _tipController = container.Resolve<TipsSystem.TipController>();
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            if (_dialogue == null)
            {
                return;
            }

            if (_playerService == null)
            {
                return;
            }

            if (managerTriggerZone == null)
            {
                return;
            }

            if (dialoguePoint == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(dialogueID))
            {
                return;
            }
            
            _tipController.ShowTipDelayed("shift_start", 10f, ct);
            

            await managerTriggerZone.WaitPlayerEnter();
            
            
            _tipController.CancelDelayedTip();
            _tipController.HideTip();


            _playerService.FocusPlayerToDialogue(dialoguePoint);

            await _dialogue.StartDialogueAsync(dialogueID, ct);

            _playerService.UnfocusPlayerFromDialogue();
        }
    }
}