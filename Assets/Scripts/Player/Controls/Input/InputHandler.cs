using UnityEngine;

class InputHandler : MonoBehaviour
{
    private KeyboardInput keyboardInput;
    private MouseInput mouseInput;

    public float rotationX => mouseInput.GetRotationX;
    public float rotationY => mouseInput.GetRotationY;

    public float horizontalInput => keyboardInput.GetHorizontalInput;
    public float verticalInput => keyboardInput.GetVerticalInput;


    void Start()
    {
        keyboardInput = gameObject.AddComponent<KeyboardInput>();
        mouseInput = gameObject.AddComponent<MouseInput>();

        Debug.Log("KeyboardInput and MouseInput was instantiated!");
    }
}
