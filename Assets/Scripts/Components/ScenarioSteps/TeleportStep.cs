using UnityEngine;

namespace Components.ScenarioSteps
{
    public class TeleportStep : BaseStep
    {
        [SerializeField]
        private Transform teleportPoint;

        [SerializeField]
        private Transform playerTransform;

        protected override void OnBeginStep()
        {
            // Screen fade in.
            playerTransform.position = teleportPoint.position;
            // Screen fade out.

            base.BeginStep();
        }
    }
}