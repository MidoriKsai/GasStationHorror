using System;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-1)]
    public abstract class BaseContext : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Deinitialize();
        }

        protected abstract void Initialize();

        protected abstract void Deinitialize();
    }
}