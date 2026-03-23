using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float MovementSpeed;
    private InputHandler inputHandler;
    private Vector3 moveDirection;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
    }

    void Update()
    {
        float horizontal = inputHandler.horizontalInput;
        float vertical = inputHandler.verticalInput;

        moveDirection = transform.forward * vertical + transform.right * horizontal;
    }

    void FixedUpdate()
    {
        playerRigidbody.linearVelocity = moveDirection * MovementSpeed;
    }


}
