using UnityEngine;
using Interactables.Interface;
using Components;

public class GarbageDispenserBin : MonoBehaviour, IInteractable
{
    [SerializeField] private Grabbable dispensableGrabbable;
    [SerializeField] private Transform holdingPoint;
    [SerializeField] private GrabbablesComponent grabbablesComponent;

    public void Interact()
    {
        Grabbable dispensedGrabbable = Instantiate(dispensableGrabbable, transform.position, Quaternion.identity);

        grabbablesComponent.AddGrubbableAtRuntime(dispensedGrabbable);
        dispensedGrabbable.Interact();
    }

    public bool CanInteract()
        => true;
}
