using UnityEngine;
using Interactables.Interface;
using System;

public class Grabbable : MonoBehaviour, IInteractable
{
    public event Action<Grabbable> TryGrabbedEvent;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private Collider collider;

    private bool grabbed = false;
    private Transform holdingPoint;

    public void Initialize(Transform holdingPoint)
    {
        this.holdingPoint = holdingPoint;
    }

    public void Interact()
    {
        TryGrab();
    }

    public void TryGrab()
    {
        TryGrabbedEvent?.Invoke(this);
    }

    public void Grab()
    {
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
        Vector3 throwDirection = Camera.main.transform.forward;
        // NOTE: Помещение в контейнер с объектами ломает направление выкидывания Grabbable-объекта по взгляду игрока
        transform.SetParent(null);

        rigidbody.isKinematic = false;
        rigidbody.linearVelocity = Vector3.zero;
        collider.enabled = true;

        rigidbody.AddForce(throwDirection * 10f, ForceMode.Impulse);
    }
}