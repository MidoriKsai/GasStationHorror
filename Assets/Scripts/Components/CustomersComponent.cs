using Core.Interfaces;
using UnityEngine;

namespace Components
{
    public class CustomersComponent : MonoBehaviour, IComponent<CustomerController>
    {
        [Header("Randomizers")]
        [SerializeField] private CarRandomizatorManager carRandomizator;
        [SerializeField] private CustomerRandomizatorManager customerRandomizator;

        private PointsHandler pointsHandler;

        public void Initialize(PointsHandler pointsHandler)
        {
            this.pointsHandler = pointsHandler;
        }

        public CustomerController CreateController()
        {
            return new CustomerController(
                pointsHandler,
                customerRandomizator,
                carRandomizator
            );
        }
    }
}