using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] private Camera mainCamera;
    [Header("Movement parameter")]
    [SerializeField] private float movementSpeed;
    [Header("Camera parameters")]
    [SerializeField] [Range(0.01f, 1f)] private float zoomFieldOfViewMultiplier;
    [SerializeField] private float zoomAnimationSpeed;
    [SerializeField] private float CameraJitterFrequency;
    [SerializeField] private float CameraJitterAmplitude;
    private Rigidbody playerRigidbody;
    private InputHandler inputHandler;
    private PlayerMovement playerMovement;
    private CameraLook cameraLook;
    private Zoom zoom;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
    }

    void Start()
    {
        playerMovement = gameObject.AddComponent<PlayerMovement>();
        playerMovement.Initialize(
            playerRigidbody, 
            movementSpeed, 
            mainCamera, 
            inputHandler, 
            CameraJitterFrequency, 
            CameraJitterAmplitude
        );

        cameraLook = gameObject.AddComponent<CameraLook>();
        cameraLook.Initialize(inputHandler, mainCamera);

        zoom = gameObject.AddComponent<Zoom>();
        zoom.Initialize(mainCamera, zoomFieldOfViewMultiplier, zoomAnimationSpeed);
    }

    public void StopControlsAndFocusToTarget(Transform target)
    {
        playerMovement.SetMovementEnabled(false);
        cameraLook.SnapToTarget(target);
        cameraLook.SetLookEnabled(false);
    }

    public void EnableControls()
    {
        playerMovement.SetMovementEnabled(true);
        cameraLook.SetLookEnabled(true);
    }
}
