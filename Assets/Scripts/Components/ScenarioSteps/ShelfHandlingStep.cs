using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Components.ScenarioSteps;
using Services.Interfaces;

public class ShelfHandlingStep : BaseStep
{
    [SerializeField] private ShelfContainerComponent shelfContainer;
    public override void Initialize(ServiceContainer serviceContainer)
    {
        shelfContainer.InitializeShelfs(
            serviceContainer.Resolve<IPlayerService>(),
            serviceContainer.Resolve<IInventoryService>()
        );
    }

    public override async UniTask PerformStepAsync(CancellationToken ct)
    {
        Debug.Log("SHELF HANDLING STEP STARTED");
        shelfContainer.SetupShelfs(ct);
        await UniTask.WaitUntil(CheckShelfsDone, cancellationToken: ct); 
        Debug.Log("SHELF HANDLING STEP FINISHED");
    }

    private bool CheckShelfsDone()
        => shelfContainer.RemainingShelfs <= 0;
}
