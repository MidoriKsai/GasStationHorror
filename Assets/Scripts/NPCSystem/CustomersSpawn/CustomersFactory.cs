using UnityEngine;

public class CustomersFactory
{
    private int nextId = 1;
    
    public CustomerData CreateCustomer()
    {
        var newCustomer = new CustomerData();
        newCustomer.customerId = nextId;
        newCustomer.petrolPumpNumber = Random.Range(0, 4);
        newCustomer.literQuantity = Random.Range(20, 45);
        newCustomer.patrolId = Random.Range(0, 4);
        nextId++;
        return newCustomer;
    }
}
