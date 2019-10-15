using UnityEngine;

namespace Trophies.Maptek
{
    [System.Serializable]
    public class ModelTarget
    {
        public int Id;
        public string Name;
        public GameObject Model;
        public bool isOverlap;
        public bool IsActive;
    }
}