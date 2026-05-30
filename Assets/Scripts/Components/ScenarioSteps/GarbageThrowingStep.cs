using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Components.ScenarioSteps;
using Services.Interfaces;
public class GarbageThrowingStep : BaseStep
{
    [SerializeField] GarbageContainerInteractor garbageContainerInteractor;
    private bool finished;

    public override void Initialize(ServiceContainer serviceContainer)
    {
        garbageContainerInteractor.garbageThrown += CheckGarbageInContainer;
    }

    public override async UniTask PerformStepAsync(CancellationToken ct)
    {
        garbageContainerInteractor.EmptyContainer();
        Debug.Log("Garbage step started");
        await UniTask.WaitUntil(() => finished == true, cancellationToken: ct); 
        Debug.Log("Garbage step ended");
    }

    private void CheckGarbageInContainer()
    {
        finished = true;
    }
}
