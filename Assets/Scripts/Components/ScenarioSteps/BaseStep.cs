using System;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public abstract class BaseStep : MonoBehaviour
    {
        public event Action<BaseStep> StepCompleted;

        public virtual void BeginStep()
        {
            OnBeginStep();
        }

        protected virtual void OnBeginStep()
        {
            StepCompleted?.Invoke(this);
        }
    }
}