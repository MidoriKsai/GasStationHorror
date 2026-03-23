using UnityEngine;

namespace Interactables
{
    public class DoorTriggerZone : MonoBehaviour
    {
        [SerializeField] private DoorInteractable doorInteractable;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (!other.TryGetComponent<Customer>(out var customer))
                return;
            
            Debug.Log("Customer enter door trigger");
            
            if (customer == null)
                return;

            doorInteractable.RegisterUser(customer);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (!other.TryGetComponent<Customer>(out var customer))
                return;

            if (customer == null)
                return;
            Debug.Log("Customer exited door trigger");
            doorInteractable.UnregisterUser(customer);
        }
    }
}