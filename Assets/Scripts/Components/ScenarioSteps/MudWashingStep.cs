using Components.ScenarioSteps;
using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MudWashingStep : BaseStep
{
    [SerializeField] private MudContainerComponent mudContainer;
    public override void Initialize(ServiceContainer serviceContainer)
    {
        Debug.Log("1. INITIALIZE DONE");
    }

    public override async UniTask PerformStepAsync(CancellationToken ct)
    {
        // await UniTask.SwitchToMainThread(ct);

        Debug.Log("2. STEP EXECUTION STARTED");

        await UniTask.WaitUntil(CheckContainerEmpty, cancellationToken: ct); 

        Debug.Log("3. STEP FINISHED");

    }

    private bool CheckContainerEmpty()
        => mudContainer.mudCountInContainer <= 0;
}
