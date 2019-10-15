using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trophies.Rappi
{
    public class LoaderScene : MonoBehaviour
    {

        [Header("Options Loading")]
        public GameObject loadingUI;
        // Indica el tiempo de espera despues de terminar de cargar una escena
        public float delayTimeLoading;

        void Start()
        {

        }

        public void ShowLoadScreen(float time, OnFinishLoad onFinish = null)
        {
            loadingUI.SetActive(true);

            LeanTween.delayedCall(time, () =>
            {
                loadingUI.SetActive(false);

                if (onFinish != null)
                {
                    onFinish();
                }
            });
        }

        public void LoadScene(int id)
        {
            StartCoroutine(LoadAsyncScene(id));
        }

        IEnumerator LoadAsyncScene(int id)
        {
            loadingUI.SetActive(true);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            LeanTween.delayedCall(delayTimeLoading, () =>
            {
                loadingUI.SetActive(false);
            });
        }

        public delegate void OnFinishLoad();
        public static event OnFinishLoad onFinishLoad;

    }
}