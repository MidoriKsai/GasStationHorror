using UnityEngine;
using Core;
using Services;
using Interactables.Interface;

class Grabbable : MonoBehaviour, IGrabbable
{
    // [SerializeField] Transform playerTransform;
    private bool grabbed = false;
    // private InputHandler playerInputHandler;
    private Rigidbody rigidbody;
    private Transform holdingPoint;
    private Transform InteractablesObject;

    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();
        holdingPoint = GameObject.FindGameObjectWithTag("Player").transform.Find("HoldingPoint").transform;
        InteractablesObject = transform.parent.gameObject.transform;

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

        rigidbody.isKinematic = false;
        Vector3 throwForce = transform.parent.forward;
        transform.SetParent(InteractablesObject);
        rigidbody.AddForce(throwForce * 50f, ForceMode.Force);
    }
}
