using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.Trophies
{
    [CreateAssetMenu(fileName = "PubsData", menuName = "ClycMe/Pubs Data", order = 1)]
    public class PubDataContainer : ScriptableObject
    {


        public PubData[] pubs;
        public string ContestButtonURL;

        [Serializable]
        public class PubData
        {
            public string pubName;
            public PubLocalizedData[] pubLocalizedData;

            public bool noLocalizedPub;
            public SystemLanguage defaultLang = SystemLanguage.Unknown;

            public bool hasDefinedARScene;
            public int definedARScene;

            public bool hasDefinedPrizeScene;
            public int definedPrizeScene;

            public bool hidePubInList;

            public Sprite pubLogo;

            public bool definedConstestUrl;
            public string pubConstestButtonUrl;

            public GameObject[] trackersPrefab;
        }

        [Serializable]
        public class PubLocalizedData
        {
            public SystemLanguage language;
        }

        public PubData GetPub(string pubName)
        {
            PubData selectedPub = null;

            foreach (PubData currPub in pubs)
            {
                if (currPub.pubName.CompareTo(pubName) == 0)
                {
                    selectedPub = currPub;
                }
            }

            return selectedPub;
        }
    }
}