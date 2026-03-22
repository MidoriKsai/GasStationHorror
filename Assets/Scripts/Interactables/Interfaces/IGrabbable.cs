using UnityEngine;

interface IGrabbable : IInteractable
{
    void Grab();
    void Drop();
}
