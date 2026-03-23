using UnityEngine;
using Interactables.Interface;

public class Grabbable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private Collider collider;

    private bool grabbed = false;
    private Transform holdingPoint;
    private Transform interactablesContainer;

    public void Initialize(Transform holdingPoint, Transform interactablesContainer)
    {
        this.holdingPoint = holdingPoint;
        this.interactablesContainer = interactablesContainer;
    }

    public void Interact()
    {
        if (!grabbed)
            Grab();
        else
            Drop();

        grabbed = !grabbed;
    }

    public void Grab()
    {
        Debug.Log("Object grabbed!");

        collider.enabled = false;
        rigidbody.isKinematic = true;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.linearVelocity = Vector3.zero;

        transform.SetParent(holdingPoint);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public bool CanInteract()
        => true;

    public void Drop()
    {
        Debug.Log("Object dropped!");

        transform.SetParent(interactablesContainer);

        rigidbody.isKinematic = false;
        collider.enabled = true;

        Vector3 throwForce = transform.parent.forward;
        rigidbody.AddRelativeForce(throwForce * 50f, ForceMode.Impulse);
    }
}