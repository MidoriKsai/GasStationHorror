using UnityEngine;

public class CustomersFactory
{
    private int nextId = 1;
    
    public CustomerData CreateCustomer()
    {
        var newCustomer = new CustomerData();
        newCustomer.customerId = nextId;
        nextId++;
        return newCustomer;
    }
}
