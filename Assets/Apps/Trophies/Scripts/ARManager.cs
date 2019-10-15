using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

namespace Trophies.Trophies
{
    public class ARManager : MonoBehaviour
    {

        PubDataContainer.PubData currPubData;

#if UNITY_EDITOR
        public string testPubName;
#endif

        private void Awake()
        {
            currPubData = GeneralManager.GetPubSelected();

#if UNITY_EDITOR
            if (currPubData == null) currPubData = GeneralManager.SelectPub(testPubName);
#endif
            LoadSceneTrackers();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GoBack();
            }
        }


        public void GoBack()
        {
            SceneManager.LoadScene(GeneralManager.SceneMenuIndex);
        }

        void LoadSceneTrackers()
        {
            foreach (GameObject currPrefab in currPubData.trackersPrefab)
            {
                Instantiate(currPrefab);
            }
        }
    }
}