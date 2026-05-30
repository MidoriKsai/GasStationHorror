using System.Collections.Generic;
using UnityEngine;

public class CustomersFactory
{
    private int nextId = 1;
    private List<string> fuels = new List<string>
    {
        "АИ-92",
        "АИ-95",
        "АИ-100",
        "Дизель"
    };

    
    public CustomerData CreateCustomer()
    {
        var newCustomer = new CustomerData();
        newCustomer.customerId = nextId;
        newCustomer.petrolPumpNumber = Random.Range(0, 4);
        newCustomer.literQuantity = Random.Range(20, 45);
        newCustomer.patrolId = Random.Range(0, 4);
        newCustomer.fuelType = fuels[newCustomer.patrolId];
        newCustomer.frenchDogCount = Random.Range(0, 3);
        newCustomer.coffeeCount = Random.Range(0, 3);
        nextId++;
        return newCustomer;
    }
    
}
