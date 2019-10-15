namespace ZeeAR.Visualization
{
    using System;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ZeeARData")]
    public class ZeeARData : ScriptableObject
    {
        public ARData[] products;

        [Serializable]
        public class ARData
        {
            public string NameProduct;
            public int IdProduct;

            public GameObject[] trackersPrefab;
        }

        public ARData GetProduct(string productName)
        {
            ARData selectedProduct = null;

            foreach (ARData currProduct in products)
            {
                if (currProduct.NameProduct.CompareTo(productName) == 0)
                {
                    selectedProduct = currProduct;
                }
            }

            return selectedProduct;
        }

        public ARData GetProductById(int id)
        {
            return products.Single((e) => e.IdProduct == id);
        }

        public bool HasProduct(int id)
        {
            return products.Any((e) => e.IdProduct == id);
        }
    }
}