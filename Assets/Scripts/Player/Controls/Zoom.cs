using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private float zoomFOV;
    [SerializeField] private float zoomAnimationSpeed;
    private float defaultFOV;
    private Camera camera;

    private bool zoomed;

    void Start()
    {
        camera = transform.GetComponentInChildren<Camera>();
        zoomed = false;
        defaultFOV = camera.fieldOfView;
    }

    void Update()
    {
        if (MouseRightButtonPressed())
            zoomed = !zoomed;

        float targetFOV = zoomed ? zoomFOV : defaultFOV;

        camera.fieldOfView = Mathf.Lerp(
            camera.fieldOfView, 
            targetFOV, 
            zoomAnimationSpeed * Time.deltaTime
        );
    }

    private bool MouseRightButtonPressed()
        => Input.GetMouseButtonDown(1);
}
