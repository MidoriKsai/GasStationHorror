using UnityEngine;

[System.Serializable]
public class CustomerRandomData
{
    public Customer customerPrefab;
    public Material[] materials;
}

public class CustomerRandomizatorManager : MonoBehaviour
{
    [SerializeField] private CustomerRandomData man;
    [SerializeField] private CustomerRandomData woman;

    public CustomerRandomData GetRandomCustomerData()
    {
        bool isMan = Random.Range(0, 2) == 0;
        return isMan ? man : woman;
    }

    public Material GetRandomMaterial(CustomerRandomData data)
    {
        if (data.materials == null || data.materials.Length == 0)
        {
            return null;
        }

        return data.materials[Random.Range(0, data.materials.Length)];
    }

    public void ApplyRandomMaterial(Customer customer, CustomerRandomData data)
    {
        var mat = GetRandomMaterial(data);
        if (mat == null) return;

        var renderers = customer.GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
            r.material = mat;
    }
}