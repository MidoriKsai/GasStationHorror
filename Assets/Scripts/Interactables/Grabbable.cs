using UnityEngine;
using Interactables.Interface;
using System;

public class Grabbable : MonoBehaviour, IInteractable
{
    public delegate void GrabbableEventHandler(Grabbable sender, EventArgs args);

    public event GrabbableEventHandler GrabbedEvent;
    public event GrabbableEventHandler DroppedEvent;

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

        GrabbedEvent?.Invoke(this, EventArgs.Empty);
    }

    public bool CanInteract()
        => true;

    public void Drop()
    {
        Vector3 throwDirection = Camera.main.transform.forward;
        // NOTE: Помещение в контейнер с объектами ломает направление выкидывания Grabbable-объекта по взгляду игрока
        transform.SetParent(null);

        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;
        collider.enabled = true;

        rigidbody.AddForce(throwDirection * 10f, ForceMode.Impulse);

        DroppedEvent?.Invoke(this, EventArgs.Empty);
    }
}
