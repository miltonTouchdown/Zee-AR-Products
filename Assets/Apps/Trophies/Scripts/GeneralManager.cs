using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.Trophies
{
    public class GeneralManager : MonoBehaviour
    {

        public LOC_Manager localization;
        public PubDataContainer pubData;

        public static GeneralManager Instance;

        PubDataContainer.PubData pubSelected;

        public int sceneARIndex;
        public int sceneMenuIndex;
        public int scenePrizeIndex;

        public string savedPubString;

        public static int SceneARIndex { get { return Instance.sceneARIndex; } }
        public static int SceneMenuIndex { get { return Instance.sceneMenuIndex; } }
        public static int ScenePrizeIndex { get { return Instance.scenePrizeIndex; } }

        private void Awake()
        {
            pubSelected = null;

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                string pubSaved = PlayerPrefs.GetString(savedPubString);

            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static PubDataContainer.PubData[] GetPubList()
        {
            return Instance.pubData.pubs;
        }

        public static PubDataContainer.PubData GetPubSelected()
        {
            PubDataContainer.PubData pubData = Instance.pubSelected;
            if (pubData == null || pubData.pubName == null || pubData.pubName.CompareTo("") == 0)
            {
                //Debug.Log("null data");
                return null;
            }
            else
            {
                return pubData;
            }
        }

        public static PubDataContainer.PubData SelectPub(string pubName)
        {
            PubDataContainer.PubData pubData = Instance.pubData.GetPub(pubName);
            if (pubData != null)
            {
                PlayerPrefs.SetString(Instance.savedPubString, pubData.pubName);
                Instance.pubSelected = pubData;
            }
            return pubData;
        }


    }
}