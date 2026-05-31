using UnityEngine;

public class CustomerSpawner
{
    private readonly CustomerRandomizatorManager _randomizatorManager;

    public CustomerSpawner(
        CustomerRandomizatorManager randomizatorManager)
    {
        _randomizatorManager = randomizatorManager;
    }

    public Customer Spawn(
        Transform spawnPoint,
        CustomerData customerData)
    {
        CustomerRandomData randomData =
            _randomizatorManager.GetRandomCustomerData();

        if (randomData == null)
            return null;

        var customer = Object.Instantiate(
            randomData.customerPrefab,
            spawnPoint.position,
            spawnPoint.rotation);

        _randomizatorManager.ApplyRandomMaterial(
            customer,
            randomData);

        customer.Init(customerData);

        return customer;
    }

    public Customer SpawnOnRandom(
        Transform[] spawnPoints,
        CustomerData customerData)
    {
        int random = Random.Range(0, spawnPoints.Length);

        Transform spawnPoint = spawnPoints[random];

        return Spawn(spawnPoint, customerData);
    }
}