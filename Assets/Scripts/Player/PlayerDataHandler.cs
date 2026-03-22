using UnityEngine;

namespace Player
{
    public class PlayerDataHandler : MonoBehaviour
    {
        [field: SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private CanvasGroup faderCanvasGroup;
        
        [SerializeField]
        private Camera mainCamera;

        public Transform PlayerTransform => playerTransform;

        public CanvasGroup FaderCanvasGroup => faderCanvasGroup;
        
        public Camera  MainCamera => mainCamera;
    }
}