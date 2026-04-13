using System.Collections.Generic;
using UnityEngine;

namespace Interactables.Products
{
    public class ProductsDataHandler: MonoBehaviour
    {
        [SerializeField] private List<Grabbable> productPrefabs;

        public List<Grabbable> ProductPrefabs => productPrefabs;
    }
}