using UnityEngine;
using System.Collections.Generic;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private List<GameObject> canvases;

    private bool isRunning;
    private Dictionary<GameObject, bool> canvasStates = new Dictionary<GameObject, bool>();

    private bool savedCursorVisible;
    private CursorLockMode savedCursorLockState;

    void Start()
    {
        inputHandler.PauseEnterActionTriggered += OnPauseEnterActionTriggered;
        isRunning = true;
        TogglePauseOverlay(false);
    }

    void OnPauseEnterActionTriggered()
    {
        isRunning = !isRunning;
        Time.timeScale = isRunning ? 1f : 0f;
        TogglePauseOverlay(!isRunning);
        ToggleUI(isRunning);

        if (isRunning)
        {
            RestoreCursorState();
        }
        else
        {
            SaveAndEnableCursor();
        }
    }

    void ToggleUI(bool enabled)
    {
        if (!enabled)
        {
            canvasStates.Clear();
            foreach (GameObject canvas in canvases)
            {
                if (canvas != null)
                {
                    canvasStates[canvas] = canvas.activeSelf;
                    canvas.SetActive(false);
                }
            }
        }
        else
        {
            foreach (GameObject canvas in canvases)
            {
                if (canvas != null && canvasStates.TryGetValue(canvas, out bool wasActive))
                {
                    canvas.SetActive(wasActive);
                }
            }
        }
    }

    void TogglePauseOverlay(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    void SaveAndEnableCursor()
    {
        savedCursorVisible = Cursor.visible;
        savedCursorLockState = CursorLockMode.None;

        savedCursorLockState = Cursor.lockState; 

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void RestoreCursorState()
    {
        Cursor.visible = savedCursorVisible;
        Cursor.lockState = savedCursorLockState;
    }
}
