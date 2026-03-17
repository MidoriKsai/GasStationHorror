using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public abstract class BaseStep : MonoBehaviour
    {
        public abstract void Initialize(ServiceContainer serviceContainer);

        public virtual async UniTask PerformStepAsync(CancellationToken ct)
        {
        }
    }
}