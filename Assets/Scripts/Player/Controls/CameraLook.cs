using DG.Tweening;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private float lookSensitivity;

    private Transform playerTransform;
    private InputHandler _inputHandler;
    private Transform _cameraTransform;
    private float _cameraPitch;

    private bool _isLookEnabled = true;

    private void Awake()
    {
        if (playerTransform == null)
            playerTransform = transform;
    }

    public void Initialize(InputHandler inputHandler, Camera playerCamera)
    {
        _inputHandler = inputHandler;
        _cameraTransform = playerCamera.transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cameraPitch = 0f;
        lookSensitivity = StaticSettings.Sensetivity * 200f;
        Debug.Log($"SENSETIVITY SET TO {lookSensitivity}");
    }

    void Update()
    {
        if (_isLookEnabled)
        {
            HandleMouseLook();
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = _inputHandler.rotationX * lookSensitivity * Time.deltaTime;
        float mouseY = _inputHandler.rotationY * lookSensitivity * Time.deltaTime;

        _cameraPitch -= mouseY;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);

        _cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);

        playerTransform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Enables or disables the camera look functionality.
    /// </summary>
    public void SetLookEnabled(bool isEnabled)
    {
        _isLookEnabled = isEnabled;
    }

    /// <summary>
    /// Smoothly rotates the camera to look at a specific target.
    /// </summary>
    /// <param name="target">The transform of the target to look at.</param>
    public void SnapToTarget(Transform target)
    {
        // --- Player Rotation (Yaw) ---
        var playerDirection = target.position - playerTransform.position;
        playerDirection.y = 0; // We only want to rotate on the Y-axis
        var playerTargetRotation = Quaternion.LookRotation(playerDirection);
        playerTransform.DORotateQuaternion(playerTargetRotation, 0.5f);

        // --- Camera Rotation (Pitch) ---
        var cameraDirection = target.position - _cameraTransform.position;
        var cameraTargetRotation = Quaternion.LookRotation(cameraDirection);

        // Extract the pitch and update our internal state
        _cameraPitch = cameraTargetRotation.eulerAngles.x;

        // Clamp the pitch to prevent looking too far up or down
        if (_cameraPitch > 180) _cameraPitch -= 360;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);

        // Apply the pitch rotation locally to the camera
        _cameraTransform.DOLocalRotate(new Vector3(_cameraPitch, 0, 0), 0.5f);
    }
}
