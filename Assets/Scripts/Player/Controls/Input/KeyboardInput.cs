using UnityEngine;

class KeyboardInput : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    public float GetHorizontalInput => horizontalInput;
    public float GetVerticalInput => verticalInput;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
}
