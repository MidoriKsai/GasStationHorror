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

        [SerializeField]
        private Transform holdingPoint;

        [SerializeField]
        private PlayerMovement playerMovement;

        [SerializeField]
        private CameraLook cameraLook;

        public PlayerMovement PlayerMovement => playerMovement;

        public CameraLook CameraLook => cameraLook;

        public Transform PlayerTransform => playerTransform;

        public CanvasGroup FaderCanvasGroup => faderCanvasGroup;

        public Camera  MainCamera => mainCamera;

        public Transform GetHoldingPoint => holdingPoint;
    }
}
