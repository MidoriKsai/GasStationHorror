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

    public void Interact()
    {
        TryGrab();
    }

    public void TryGrab()
    {
        TryGrabbedEvent?.Invoke(this);
    }

    public void Grab(Transform holdingPoint)
    {
        collider.enabled = false;

        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.isKinematic = true;

        transform.SetParent(holdingPoint);

        ResetLocalRotation();
    }
    
    public void ResetLocalRotation()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public bool CanInteract()
        => true;

    public void Drop(bool needToAddForce)
    {
        Vector3 throwDirection = Camera.main.transform.forward;
        // NOTE: Помещение в контейнер с объектами ломает направление выкидывания Grabbable-объекта по взгляду игрока
        transform.SetParent(null);

        rigidbody.isKinematic = false;
        rigidbody.linearVelocity = Vector3.zero;
        collider.enabled = true;

        if (needToAddForce)
        {
            rigidbody.AddForce(throwDirection * 5f, ForceMode.Impulse);
        }
    }

    public void DropCarefully()
    {
        transform.SetParent(null);
        DisableRagdoll();
    }

    public void DisableRagdoll()
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        rigidbody.isKinematic = true;
        collider.enabled = true;
    }
}
