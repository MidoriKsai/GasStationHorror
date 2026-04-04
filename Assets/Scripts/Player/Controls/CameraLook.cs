using DG.Tweening;
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
        var direction = target.position - camera.position;
        var targetRotation = Quaternion.LookRotation(direction);

        camera.DORotateQuaternion(targetRotation, 0.5f);
    }
}
