using UnityEngine;
using Core;

class Interaction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 10f;
    private IInteractable grabbableObject;

    void Update()
    {
        if (grabbableObject != null & Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("YOU HAVE GRABBED OBJECT AND TRIED TO THROW IT");
            grabbableObject.Interact();
            grabbableObject = null;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
            CheckInteractability();
    }

    void CheckInteractability()
    {
        Camera camera = transform.GetComponentInChildren<Camera>();

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactableObject))
            {
                interactableObject.Interact();
                grabbableObject = interactableObject;
            }
        }
    }
}
