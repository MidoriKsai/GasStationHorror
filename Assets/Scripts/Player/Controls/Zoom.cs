using UnityEngine;

public class Zoom : MonoBehaviour
{
    /*[SerializeField]*/ private float zoomFOV;
    /*[SerializeField]*/ private float zoomAnimationSpeed;
    private float defaultFOV;
    private Camera camera;

    private bool zoomed;

    public void Initialize(Camera playerCamera, float zoomFieldOfViewMultiplier, float zoomAnimationSpeed)
    {
        camera = playerCamera;
        this.zoomFOV = camera.fieldOfView * zoomFieldOfViewMultiplier;
        this.zoomAnimationSpeed = zoomAnimationSpeed;
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
