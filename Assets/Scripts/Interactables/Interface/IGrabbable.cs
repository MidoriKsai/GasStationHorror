using UnityEngine;

namespace Interactables.Interface
{
    interface IGrabbable : IInteractable
    {
        void Grab();
        void Drop();
    }
}
