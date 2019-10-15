using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trophies.Maptek
{
    public class AppManager : MonoBehaviour
    {
        [Header("Global UI")]
        public int indexSceneMainMenu;
        public int indexSceneAR;
        public int indexSceneRegister;

        [Header("Global UI")]
        public GameObject loadingScreen;

        [Header("User")]
        public User currUser = null;

        [Header("Email data")]
        public string emailTo;
        public string subject;
        public string body;

        [Header("AR")]
        public ExpoDataContainer expoData;
        private ExpoDataContainer.ExpoData expoSelected;

        private UIMainMenu _UIMainMenu = null;
        private SessionRegister _sessionRegister = null;
        private PopUp _popUp = null;

        private static AppManager _instace;
        public static AppManager Instance
        {
            get
            {
                return _instace;
            }
            set
            {
                if (_instace == null)
                {
                    _instace = value;
                }
            }
        }

        void Awake()
        {
            if (_instace == null)
            {
                _instace = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            _sessionRegister = FindObjectOfType<SessionRegister>();
            _popUp = GetComponentInChildren<PopUp>();

            if (!_sessionRegister.LoadUserData())
            {
                loadingScreen.SetActive(false);
            }
        }

        /// <summary>
        /// Cargar escene Menu principal
        /// </summary>
        public void LoadMainMenu()
        {
            loadingScreen.SetActive(true);

            // Cargar escena main menu
            LoadScene(indexSceneMainMenu, () =>
            {
                _UIMainMenu = FindObjectOfType<UIMainMenu>();

                if (ConferenceControl.Instance.isLoadConference)
                {
                // Cargar charlas
                _UIMainMenu.initMainMenu();

                // Cargar charla en caso de estar iniciada
                if (ConferenceControl.Instance.currExposition.isOpen)
                        _UIMainMenu.ShowCharlaInformation();

                    LeanTween.delayedCall(.3f, () =>
                    {
                        loadingScreen.SetActive(false);
                    });
                }
                else
                {
                // Descargar conferencias si es que no estan descargadas

                Webservice.Instance.getConferenceData((s, m) =>
                    {
                        if (s)
                        {
                            ConferenceControl.Instance.SetLikesExposition(currUser.idLikeExpositions);

                            _UIMainMenu.initMainMenu();

                            LeanTween.delayedCall(.3f, () =>
                            {
                                loadingScreen.SetActive(false);
                            });
                        }
                    });
                }
            });
        }

        /// <summary>
        /// Cargar escena AR
        /// </summary>
        public void LoadSceneAR()
        {
            if (!HasExpoAR(ConferenceControl.Instance.currExposition.id))
                return;

            SelectExpoById(ConferenceControl.Instance.currExposition.id);

            loadingScreen.SetActive(true);

            // Cargar escena AR
            LoadScene(indexSceneAR, () =>
            {
                LeanTween.delayedCall(.3f, () =>
                {
                    loadingScreen.SetActive(false);
                });
            });
        }

        public void LoadRegisterMenu()
        {
            LoadScene(indexSceneRegister, () =>
            {
                _sessionRegister = FindObjectOfType<SessionRegister>();

                if (!_sessionRegister.LoadUserData())
                {
                    loadingScreen.SetActive(false);
                }
            });
        }

        #region Load_Scene
        public void LoadScene(int index, OnFinishCallback onFinish = null)
        {
            //loadingScreen.SetActive(true);

            StartCoroutine(LoadAsyncScene(index, onFinish));
        }

        IEnumerator LoadAsyncScene(int index, OnFinishCallback onFinish = null)
        {

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            //loadingScreen.SetActive(false);

            if (onFinish != null)
                onFinish();
        }
        #endregion

        #region Email_Sender
        public void SendEmail()
        {
            body = body.Replace("@NameExposition", "'" + ConferenceControl.Instance.currExposition.name_exposition + "'");

            SenderEmail.SendEmail(emailTo, subject, body);
        }
        #endregion

        #region AR
        public ExpoDataContainer.ExpoData GetExpoSelected()
        {
            ExpoDataContainer.ExpoData expoData = expoSelected;

            if (expoData == null || expoData.expoName == null || expoData.expoName.CompareTo("") == 0)
            {
                return null;
            }
            else
            {
                return expoData;
            }
        }

        public void SelectExpoByName(string expoName)
        {
            ExpoDataContainer.ExpoData expo = expoData.GetExpo(expoName);

            if (expo != null)
            {
                expoSelected = expo;
            }
        }

        public void SelectExpoById(int idExpo)
        {
            ExpoDataContainer.ExpoData expo = expoData.GetExpoById(idExpo);

            if (expo != null)
            {
                expoSelected = expo;
            }
        }

        /// <summary>
        /// Indica si la charla tiene realidad aumentada
        /// </summary>
        /// <param name="idExpo">id de la charla</param>
        /// <returns></returns>
        public bool HasExpoAR(int idExpo)
        {
            return expoData.HasExpo(idExpo);
        }
        #endregion

        public delegate void OnFinishCallback();
        public static event OnFinishCallback onFinish;
    }
}