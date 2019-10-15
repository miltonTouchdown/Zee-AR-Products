namespace ZeeAR.Visualization
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class AppManager : MonoBehaviour
    {
        [Header("Global UI")]
        public int indexSceneMainMenu;
        public int indexSceneAR;

        [Header("Global UI")]
        public GameObject loadingScreen;

        [Header("AR")]
        public ZeeARData ARData;
        private ZeeARData.ARData productSelected;

        //private UIMainMenu _UIMainMenu = null;
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
            _popUp = GetComponentInChildren<PopUp>();
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
                //_UIMainMenu = FindObjectOfType<UIMainMenu>();
                loadingScreen.SetActive(false);
            });
        }

        /// <summary>
        /// Cargar escena AR
        /// </summary>
        public void LoadSceneAR(int idProduct)
        {
            SelectProductById(idProduct);

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

        #region AR
        public ZeeARData.ARData GetProductSelected()
        {
            ZeeARData.ARData productData = productSelected;

            if (productData == null || productData.NameProduct == null || productData.NameProduct.CompareTo("") == 0)
            {
                return null;
            }
            else
            {
                return productData;
            }
        }

        public void SelectProductByName(string expoName)
        {
            ZeeARData.ARData product = ARData.GetProduct(expoName);

            if (product != null)
            {
                productSelected = product;
            }
        }

        public void SelectProductById(int idExpo)
        {
            ZeeARData.ARData product = ARData.GetProductById(idExpo);

            if (product != null)
            {
                productSelected = product;
            }
        }
        #endregion

        public delegate void OnFinishCallback();
        public static event OnFinishCallback onFinish;
    }
}