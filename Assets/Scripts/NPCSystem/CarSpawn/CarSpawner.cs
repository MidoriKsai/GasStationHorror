using NPCSystem;
using UnityEngine;

public class CarSpawner
{
    private readonly Car _carPrefab;

    public CarSpawner(Car carPrefab)
    {
        _carPrefab = carPrefab;
    }

    public Car Spawn(Transform spawnPoint)
    {
        var car = Object.Instantiate(_carPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Spawn Car");
        return car;
    }
}