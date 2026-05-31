using NPCSystem;
using UnityEngine;

public class CarSpawner
{
    private readonly CarRandomizatorManager _randomizatorManager;

    public CarSpawner(CarRandomizatorManager randomizatorManager)
    {
        _randomizatorManager = randomizatorManager;
    }

    public Car Spawn(Transform spawnPoint)
    {
        CarRandomData randomCarData =
            _randomizatorManager.GetRandomCarData();

        if (randomCarData == null)
            return null;

        var car = Object.Instantiate(
            randomCarData.carPrefab,
            spawnPoint.position,
            spawnPoint.rotation);

        _randomizatorManager.ApplyRandomMaterial(
            car,
            randomCarData);
        

        return car;
    }
}