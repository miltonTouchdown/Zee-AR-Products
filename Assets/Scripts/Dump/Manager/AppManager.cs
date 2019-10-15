using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trophies.PackagesApps
{
    public class AppManager : MonoBehaviour
    {
        public GameObject loadingScreen;

        private static AppManager _instance;
        public static AppManager Instance
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

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this);
        }

        public void LoadMainMenu()
        {
            loadingScreen.SetActive(true);

            if (Maptek.AppManager.Instance != null)
            {
                Destroy(Maptek.AppManager.Instance.gameObject);
            }

            if (Rappi.GameManager.Instance != null)
            {
                Destroy(Rappi.GameManager.Instance.gameObject);
            }

            if (Trophies.GeneralManager.Instance != null)
            {
                Destroy(Trophies.GeneralManager.Instance.gameObject);
            }

            StartCoroutine(LoadAsyncScene(0, ScreenOrientation.Portrait));
        }

        public void LoadScene(int index, ScreenOrientation orientation)
        {
            loadingScreen.SetActive(true);

            StartCoroutine(LoadAsyncScene(index, orientation));
        }

        IEnumerator LoadAsyncScene(int index, ScreenOrientation orientation)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

            Screen.orientation = orientation;

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Resources.UnloadUnusedAssets();
        
            loadingScreen.SetActive(false);
        }
    }
}