using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Components.ScenarioSteps;
using Services.Interfaces;
using TipsSystem;

public class ShelfHandlingStep : BaseStep
{
    [SerializeField] private ShelfContainerComponent shelfContainer;

    private TipController _tipController;

    public override void Initialize(ServiceContainer serviceContainer)
    {
        shelfContainer.InitializeShelfs(
            serviceContainer.Resolve<IPlayerService>(),
            serviceContainer.Resolve<IInventoryService>(),
            serviceContainer.Resolve<TipController>()
        );

        _tipController =
            serviceContainer.Resolve<TipController>();
    }

    public override async UniTask PerformStepAsync(
        CancellationToken ct)
    {
        Debug.Log("SHELF HANDLING STEP STARTED");

        _tipController?.ShowObjective(
            "task_fill_shelf_start");

        shelfContainer.SetupShelfs(ct);

        await UniTask.WaitUntil(
            CheckShelfsDone,
            cancellationToken: ct);
        
        _tipController?.HideObjective();

        Debug.Log("SHELF HANDLING STEP FINISHED");
    }

    private bool CheckShelfsDone()
        => shelfContainer.RemainingShelfs <= 0;
}