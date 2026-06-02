using System.Collections.Generic;
using Components;
using Cysharp.Threading.Tasks;
using Interactables.Products;
using UnityEngine;

public class CustomerProductsHandler : MonoBehaviour
{
    [SerializeField] private ProductsDataHandler productsHandler;
    [SerializeField] private PointsHandler pointsHandler;
    [SerializeField] private GrabbablesComponent grabbablesComponent;

    private readonly List<Grabbable> _customerProducts = new();
    private readonly List<Grabbable> _scannedProducts = new();

    private UniTaskCompletionSource _tcs;

    public void StartProducts()
    {
        _tcs = new UniTaskCompletionSource();

        int count = Random.Range(2, 6);
        var prefabs = productsHandler.ProductPrefabs;

        for (int i = 0; i < count; i++)
        {
            var prefab = prefabs[Random.Range(0, prefabs.Count)];
            var point = pointsHandler.ProductsSpawnPoints[i];

            var product = Instantiate(prefab, point.position, point.rotation);

            grabbablesComponent.RegisterNewGrabbable(product);

            _customerProducts.Add(product);
        }
    }

    public bool TryScan(Grabbable grabbable)
    {
        if (grabbable == null)
        {
            return false;
        }

        if (!_customerProducts.Contains(grabbable))
        {
            return false;
        }

        _customerProducts.Remove(grabbable);
        _scannedProducts.Add(grabbable);

        MoveToScannedPoint(grabbable);

        if (_customerProducts.Count == 0)
        {
            _tcs?.TrySetResult();
        }

        return true;
    }

    public UniTask WaitAllScanned()
    {
        return _tcs.Task;
    }

    private void MoveToScannedPoint(Grabbable product)
    {
        int index = _scannedProducts.Count - 1;

        var point = pointsHandler.ScannedProductPoints[index];

        product.transform.SetParent(null);

        product.transform.position = point.position;
        product.transform.rotation = point.rotation;
    }

    public void ClearProducts()
    {
        foreach (var product in _customerProducts)
        {
            if (product != null)
            {
                Destroy(product.gameObject);
            }
        }

        foreach (var product in _scannedProducts)
        {
            if (product != null)
            {
                Destroy(product.gameObject);
            }
        }

        _customerProducts.Clear();
        _scannedProducts.Clear();
    }
}