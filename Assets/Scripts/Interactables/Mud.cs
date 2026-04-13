using UnityEngine;
using Interactables.Interface;

public class Mud : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject ground;
    [SerializeField] private int NeededWipeCount;
    private int wipeCount;

    public void Interact()
    {
        wipeCount++;

        if (wipeCount == NeededWipeCount)
            Destroy(gameObject);
    }

    public bool CanInteract()
        => true;
}
