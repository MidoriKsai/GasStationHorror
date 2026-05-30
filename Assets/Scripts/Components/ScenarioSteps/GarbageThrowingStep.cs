using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Components.ScenarioSteps;
using Services.Interfaces;
public class GarbageThrowingStep : BaseStep
{
    [SerializeField] GarbageContainerInteractor garbageContainerInteractor;

    public override void Initialize(ServiceContainer serviceContainer)
    {
        
    }

    public override async UniTask PerformStepAsync(CancellationToken ct)
    {
        garbageContainerInteractor.EmptyContainer();
        await UniTask.WaitUntil(CheckGarbageInContainer, cancellationToken: ct); 
    }

    private bool CheckGarbageInContainer()
        => garbageContainerInteractor.isContainerFull;
}
