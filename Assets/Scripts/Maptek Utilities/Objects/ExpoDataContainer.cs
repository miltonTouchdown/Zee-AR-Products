using System;
using System.Linq;
using UnityEngine;

namespace Trophies.Maptek
{
    [CreateAssetMenu(fileName = "ExpoData", menuName = "ClycMe/Expos Data", order = 1)]
    public class ExpoDataContainer : ScriptableObject
    {
        public ExpoData[] expos;

        [Serializable]
        public class ExpoData
        {
            public string expoName;
            public int idExpo;

            public bool hasDefinedARScene;
            public int definedARScene;

            public Sprite expoLogo;

            public GameObject[] trackersPrefab;
        }

        public ExpoData GetExpo(string pubName)
        {
            ExpoData selectedExpo = null;

            foreach (ExpoData currExpo in expos)
            {
                if (currExpo.expoName.CompareTo(pubName) == 0)
                {
                    selectedExpo = currExpo;
                }
            }

            return selectedExpo;
        }

        public ExpoData GetExpoById(int id)
        {
            return expos.Single((e) => e.idExpo == id);
        }

        public bool HasExpo(int id)
        {
            return expos.Any((e) => e.idExpo == id);
        }
    }
}