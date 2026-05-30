using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Components.ScenarioSteps;

public class ShelfFillingStep : BaseStep
{
    [SerializeField] private ShelfSlotsComponent shelfSlots;
    public override void Initialize(ServiceContainer serviceContainer)
    {
    }

    public override async UniTask PerformStepAsync(CancellationToken ct)
    {
        //await UniTask.SwitchToMainThread(ct);

        Debug.Log("2. STEP EXECUTION STARTED");

        await UniTask.WaitUntil(CheckContainerEmpty, cancellationToken: ct); 

        Debug.Log("3. STEP FINISHED");

    }

    private bool CheckContainerEmpty()
        => shelfSlots.RemainingEmptySlots <= 0;
}