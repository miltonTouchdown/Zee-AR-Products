namespace Trophies.Maptek
{
    using UnityEngine;
    using ZeeAR.Visualization;

    public class ARManager : MonoBehaviour
    {
        public TrophiesImageTarget activeTracker;

        private ZeeARData.ARData currProductData;
        private ARMenu _ARMenu;

        private static ARManager _instance;
        public static ARManager Instance
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
                Destroy(this);
            }

            currProductData = ZeeAR.Visualization.AppManager.Instance.GetProductSelected();//AppManager.Instance.GetExpoSelected();

            LoadSceneTrackers();
        }

        void Start()
        {
            _ARMenu = FindObjectOfType<ARMenu>();

            // Cambiar feedback interfaz
            _ARMenu.SetStateTarget(ARMenu.StateTracking.NOT_FOUND);
            _ARMenu.SetActiveSearchTarget(true);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GoBack();
            }
        }

        public void GoBack()
        {
            ZeeAR.Visualization.AppManager.Instance.LoadMainMenu();
        }

        void LoadSceneTrackers()
        {
            foreach (GameObject currPrefab in currProductData.trackersPrefab)
            {
                Instantiate(currPrefab);
            }
        }

        /// <summary>
        /// Manejador que modifica al momento de indentificar un modelo.
        /// </summary>
        /// <param name="target"></param>
        public void AddTargetReference(TrophiesImageTarget target)
        {
            // Cambiar feedback interfaz
            _ARMenu.SetStateTarget(ARMenu.StateTracking.FOUND);
            _ARMenu.SetActiveSearchTarget(false);
            _ARMenu.SetInteractableBttnRotation(true);

            if (target == activeTracker)
            {
                //_ARMenu.SetInteractableBttnsTracket(target.GetComponent<ModelARControl>().modelsTarget);
                return;
            }

            activeTracker = target;

            // Agregar botones
            _ARMenu.FillBttnsTracket(target.GetComponent<ModelARControl>());

        }

        /// <summary>
        /// Manejador que modifica al momento de estar al momento de perder el marcador
        /// </summary>
        public void WaitTarget()
        {
            // Cambiar feedback interfaz
            _ARMenu.SetStateTarget(ARMenu.StateTracking.NOT_FOUND);
            _ARMenu.SetActiveSearchTarget(false);
            _ARMenu.SetInteractableBttnRotation(true);

        }

        /// <summary>
        /// Manejador que modifica al perder un marcador
        /// </summary>
        /// <param name="target"></param>
        public void OnTargetLost(TrophiesImageTarget target)
        {
            // Desactivar botones
            //_ARMenu.SetInteractableBttnsTracket(false);

            // Cambiar feedback interfaz
            _ARMenu.SetStateTarget(ARMenu.StateTracking.SEARCHING);
            _ARMenu.SetActiveSearchTarget(true);
            _ARMenu.SetInteractableBttnRotation(false);
        }

        public void ChangeModelTarget(ModelARControl mARControl)
        {
            //_ARMenu.SetInteractableBttnsTracket(mARControl.modelsTarget);
        }
    }
}