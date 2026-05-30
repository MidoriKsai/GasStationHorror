using NPCSystem;
using UnityEngine;

[System.Serializable]
public class CarRandomData
{
    public Car carPrefab;
    public Material[] materials;
}

public class CarRandomizatorManager : MonoBehaviour
{
    [SerializeField] private CarRandomData[] cars;

    public CarRandomData GetRandomCarData()
    {
        if (cars == null || cars.Length == 0)
        {
            return null;
        }

        return cars[Random.Range(0, cars.Length)];
    }

    public Material GetRandomMaterial(CarRandomData data)
    {
        if (data.materials == null || data.materials.Length == 0)
        {
            return null;
        }

        return data.materials[Random.Range(0, data.materials.Length)];
    }

    public void ApplyRandomMaterial(Car car, CarRandomData data)
    {
        var mat = GetRandomMaterial(data);
        if (mat == null) return;

        var renderers = car.GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
            r.material = mat;
    }
}