using UnityEngine;

class MouseInput : MonoBehaviour
{
    private float rotationX;
    private float rotationY;

    public float GetRotationX => rotationX;
    public float GetRotationY => rotationY;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX = mouseX;
        rotationY = mouseY;
    }
}
