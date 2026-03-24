using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 2f;
    private InputHandler inputHandler;
    private Transform camera;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        inputHandler = GetComponent<InputHandler>();
        camera = Camera.main.transform;
    }

    void Update()
    {
        float mouseX = inputHandler.rotationX * sensitivity * Time.deltaTime;
        float mouseY = inputHandler.rotationY * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        transform.Rotate(Vector3.up * mouseX);
    }
}
