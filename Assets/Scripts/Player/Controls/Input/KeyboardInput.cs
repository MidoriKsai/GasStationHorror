using System;
using UnityEngine;

class KeyboardInput : MonoBehaviour
{
    public event Action ItemDropActionTriggered;
    public event Action PauseEnterActionTriggered;

    private float horizontalInput;
    private float verticalInput;

    public float GetHorizontalInput => horizontalInput;
    public float GetVerticalInput => verticalInput;

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.G))
        {
            ItemDropActionTriggered?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseEnterActionTriggered?.Invoke();
        }
    }
}
