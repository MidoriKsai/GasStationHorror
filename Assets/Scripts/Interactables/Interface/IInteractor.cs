using UnityEngine;
using Interactables.Interface;

namespace Interactables.Interface
{
    public interface IInteractor
    {
        void PerformInteraction(IInteractable interactable);
    }
}