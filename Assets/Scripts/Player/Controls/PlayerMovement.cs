using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float MovementSpeed;
    private InputHandler inputHandler;
    private Vector3 moveDirection;
    
    private bool _isMovementEnabled = true;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
    }

    void Update()
    {
        if (!_isMovementEnabled)
        {
            moveDirection = Vector3.zero;
            return;
        }
        
        float horizontal = inputHandler.horizontalInput;
        float vertical = inputHandler.verticalInput;

        moveDirection = transform.forward * vertical + transform.right * horizontal;
    }

    void FixedUpdate()
    {
        playerRigidbody.linearVelocity = moveDirection * MovementSpeed;
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _isMovementEnabled = isEnabled;

        if (!isEnabled)
        {
            moveDirection = Vector3.zero;
            playerRigidbody.linearVelocity = new Vector3(0f, playerRigidbody.linearVelocity.y, 0f);
        }
    }
}
