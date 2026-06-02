using Components.ScenarioSteps;
using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using TipsSystem;

public class MudWashingStep : BaseStep
{
    [SerializeField]
    private MudContainerComponent mudContainer;

    private TipController _tipController;

    public override void Initialize(
        ServiceContainer serviceContainer)
    {
        _tipController =
            serviceContainer.Resolve<TipController>();
    }

    public override async UniTask PerformStepAsync(
        CancellationToken ct)
    {
        _tipController.ShowObjective(
            "task_wash_start");

        await UniTask.WaitUntil(
            CheckContainerEmpty,
            cancellationToken: ct);

        _tipController.ShowPopup(
            "mud_washing_complete");
    }

    private bool CheckContainerEmpty()
        => mudContainer.mudCountInContainer <= 0;
}