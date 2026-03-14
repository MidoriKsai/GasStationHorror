using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;

        playerRigidbody.linearVelocity = moveDirection * 5f;
    }


}
