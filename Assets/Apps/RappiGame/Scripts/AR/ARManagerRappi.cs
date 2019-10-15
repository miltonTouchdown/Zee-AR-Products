using System;
using System.Collections;
using UnityEngine;

namespace Trophies.Rappi
{
    public class ARManagerRappi : MonoBehaviour
    {
        public enum StateTracking
        {
            FOUND,
            LOST,
            SEARCHING
        }

        public StateTracking currStateTracking = StateTracking.SEARCHING;

        private static ARManagerRappi _instance;
        public static ARManagerRappi Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

#if UNITY_EDITOR
        public string testPubName;
#endif

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }

            /*
            currPubData = GeneralManager.GetPubSelected();

    #if UNITY_EDITOR
            if(currPubData == null) currPubData = GeneralManager.SelectPub(testPubName);
    #endif
            LoadSceneTrackers();
            */
        }

        void Start()
        {
            GameManager.Instance.InitGame();
        }

        public void AddTargetReference()
        {
            GameManager.Instance.PauseGame(false);
        }

        public void OnTargetLost()
        {
            GameManager.Instance.PauseGame(true);
        }
    }
}