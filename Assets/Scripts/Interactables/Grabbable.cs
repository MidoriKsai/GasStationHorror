using UnityEngine;
using Core;
using Services;

class Grabbable : MonoBehaviour, IGrabbable
{
    // [SerializeField] Transform playerTransform;
    private bool grabbed = false;
    // private InputHandler playerInputHandler;
    private Rigidbody rigidbody;
    private Transform holdingPoint;

    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();
        holdingPoint = GameObject.FindGameObjectWithTag("Player").transform.Find("HoldingPoint").transform;

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


    public void Drop()
    {
        Debug.Log("Object dropped!");


        rigidbody.isKinematic = false;

        Vector3 throwForce = transform.parent.forward;
        transform.SetParent(null);
        rigidbody.AddForce(throwForce * 5000f, ForceMode.Force);
    }
}
