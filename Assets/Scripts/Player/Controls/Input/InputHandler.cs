using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event Action ItemDropActionTriggered;

    private KeyboardInput keyboardInput;
    private MouseInput mouseInput;

    public float rotationX => mouseInput.GetRotationX;
    public float rotationY => mouseInput.GetRotationY;

    public float horizontalInput => keyboardInput.GetHorizontalInput;
    public float verticalInput => keyboardInput.GetVerticalInput;

    private void Awake()
    {
        keyboardInput = gameObject.AddComponent<KeyboardInput>();
        mouseInput = gameObject.AddComponent<MouseInput>();

        keyboardInput.ItemDropActionTriggered += OnItemDropActionTriggered;
    }

    private void OnItemDropActionTriggered()
    {
        ItemDropActionTriggered?.Invoke();
    }

    private void OnDestroy()
    {
        keyboardInput.ItemDropActionTriggered -= OnItemDropActionTriggered;
    }
}