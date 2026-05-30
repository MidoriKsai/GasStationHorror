using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private Camera playerCamera;

    private InputHandler inputHandler;
    private Vector3 moveDirection;

    private bool _isMovementEnabled = true;

    private Vector3 cameraDefaultPosition;

    private float frequency = 15f;
    private float amplitude = 0.05f;

    private float localMovementTime;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        cameraDefaultPosition = playerCamera.transform.localPosition;
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

        Vector2 localMovement = new Vector2(horizontal, vertical).normalized;

        moveDirection =
            transform.right * localMovement.x +
            transform.forward * localMovement.y;
    }

    void FixedUpdate()
    {
        if (!_isMovementEnabled)
        {
            playerRigidbody.linearVelocity = Vector3.zero;

            localMovementTime = 0;

            DefaultCamera();

            return;
        }

        if (IsMoving())
        {
            ApplySineCosineForCamera();

            localMovementTime += Time.deltaTime;

            playerRigidbody.linearVelocity =
                moveDirection * MovementSpeed;
        }
        else
        {
            localMovementTime = 0;

            playerRigidbody.linearVelocity = Vector3.zero;

            DefaultCamera();
        }
    }

    private bool IsMoving()
    {
        return _isMovementEnabled &&
               (inputHandler.horizontalInput != 0 ||
                inputHandler.verticalInput != 0);
    }

    private void ApplySineCosineForCamera()
    {
        float xShift =
            Mathf.Cos(localMovementTime * frequency * 0.5f) * amplitude;

        float yShift =
            Mathf.Sin(-1 * localMovementTime * frequency) * amplitude;

        Vector3 newCameraPosition = new Vector3(
            cameraDefaultPosition.x + xShift,
            cameraDefaultPosition.y + yShift,
            cameraDefaultPosition.z
        );

        playerCamera.transform.localPosition = Vector3.Lerp(
            playerCamera.transform.localPosition,
            newCameraPosition,
            Time.deltaTime * 10f
        );
    }

    private void DefaultCamera()
    {
        playerCamera.transform.localPosition = Vector3.Lerp(
            playerCamera.transform.localPosition,
            cameraDefaultPosition,
            Time.deltaTime * 10f
        );
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _isMovementEnabled = isEnabled;

        if (!isEnabled)
        {
            moveDirection = Vector3.zero;

            playerRigidbody.linearVelocity = Vector3.zero;

            localMovementTime = 0;

            DefaultCamera();
        }
    }
}