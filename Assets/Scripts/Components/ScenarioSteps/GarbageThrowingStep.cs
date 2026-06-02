using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Components.ScenarioSteps;
using TipsSystem;

public class GarbageThrowingStep : BaseStep
{
    [SerializeField]
    private GarbageContainerInteractor garbageContainerInteractor;

    [SerializeField]
    private UrnInteractable urnInteractable;

    private bool finished;

    private TipController _tipController;

    public override void Initialize(
        ServiceContainer serviceContainer)
    {
        garbageContainerInteractor.garbageThrown +=
            CheckGarbageInContainer;

        _tipController =
            serviceContainer.Resolve<TipController>();

        if (urnInteractable != null)
            urnInteractable.SetCanTakeGarbage(false);
    }

    public override async UniTask PerformStepAsync(
        CancellationToken ct)
    {
        finished = false;

        garbageContainerInteractor.EmptyContainer();

        if (urnInteractable != null)
            urnInteractable.SetCanTakeGarbage(true);

        _tipController.ShowObjective(
            "task_trash");

        await UniTask.WaitUntil(
            () => finished,
            cancellationToken: ct);

        if (urnInteractable != null)
            urnInteractable.SetCanTakeGarbage(false);
    }

    private void CheckGarbageInContainer()
    {
        finished = true;
    }
}