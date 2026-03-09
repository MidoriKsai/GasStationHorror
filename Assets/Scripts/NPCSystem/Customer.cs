using UnityEngine;

public class Customer : MonoBehaviour
{
    private CustomerData _customerData;

    public void Init(CustomerData customerData)
    {
        _customerData = customerData;
        Debug.Log(_customerData.customerId);
    }
}
