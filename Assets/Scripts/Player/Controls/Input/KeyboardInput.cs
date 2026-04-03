using System;
using UnityEngine;

class KeyboardInput : MonoBehaviour
{
    public event Action ItemDropActionTriggered;

    private float horizontalInput;
    private float verticalInput;

    public float GetHorizontalInput => horizontalInput;
    public float GetVerticalInput => verticalInput;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.R))
        {
            ItemDropActionTriggered?.Invoke();
        }
    }
}