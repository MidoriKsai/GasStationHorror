using UnityEngine;

namespace Components.ScenarioSteps
{
    public class CustomerStepHandler: MonoBehaviour
    {
        [SerializeField] 
        private CashTriggerZone cashTriggerZone;

        public CashTriggerZone CashTriggerZone => cashTriggerZone;
    }
}