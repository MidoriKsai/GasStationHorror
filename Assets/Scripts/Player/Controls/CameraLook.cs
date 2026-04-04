using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 2f;
    private InputHandler inputHandler;
    private Transform camera;
    private float xRotation = 0f;
    
    private bool _isLookEnabled = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        inputHandler = GetComponent<InputHandler>();
        camera = Camera.main.transform;
    }

    void Update()
    {
        if (!_isLookEnabled)
            return;
        
        float mouseX = inputHandler.rotationX * sensitivity * Time.deltaTime;
        float mouseY = inputHandler.rotationY * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        transform.Rotate(Vector3.up * mouseX);
    }
    
    public void SetLookEnabled(bool isEnabled)
    {
        _isLookEnabled = isEnabled;
    }

    public void SnapToTarget(Transform target)
    {
        Vector3 direction = target.position - camera.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 euler = lookRotation.eulerAngles;

        float yaw = euler.y;
        float pitch = euler.x;

        if (pitch > 180f)
            pitch -= 360f;

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        xRotation = pitch;

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
