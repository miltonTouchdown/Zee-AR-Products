namespace Trophies.Maptek
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PerformanceChecker : MonoBehaviour
    {
        public string titleAlert = "Alerta bajo rendimiento";
        public string bodyAlert = "El dispositivo no cumple las especificaciones para un buen rendimiento";

        // Indica la cantidad de veces que se muestra la alerta
        public int countShowAlert = 1;

        // Indica si se puede mostrar la alerta
        private bool _canShowAlert = false;

        // Tiempo de espera para empezar a mostrar la alerta. 
        // Inicialmente siempre se empieza con bajo FPS por lo que se debe esperar hasta que se estabilice.
        private float _timeActiveAlert = 5f;

        public string formatedString = "{value} FPS";
        public Text txtFps;

        public float updateRateSeconds = 4.0F;

        public float minimumFPS = 30.0f;
        int frameCount = 0;
        float dt = 0.0F;
        float fps = 0.0F;

        private ZeeAR.Visualization.PopUp _popUp;

        void Start()
        {
            _popUp = FindObjectOfType<ZeeAR.Visualization.PopUp>();

            LeanTween.delayedCall(_timeActiveAlert, () => { _canShowAlert = true; });
            //debug.text = "Device Model: " + SystemInfo.deviceModel + " - Graphic: " + SystemInfo.graphicsDeviceType.ToString();      
        }

        void Update()
        {
            CountFPS();
        }

        private void CountFPS()
        {
            frameCount++;
            dt += Time.unscaledDeltaTime;
            if (dt > 1.0 / updateRateSeconds)
            {
                fps = frameCount / dt;
                frameCount = 0;
                dt -= 1.0F / updateRateSeconds;
            }
            //txtFps.text = formatedString.Replace("{value}", System.Math.Round(fps, 1).ToString("0.0"));

            //Debug.Log("FPS: " + System.Math.Round(fps, 1));

            if (System.Math.Round(fps, 1) < minimumFPS)
            {
                ShowAlert();
            }
        }

        private void ShowAlert()
        {
            if (countShowAlert <= 0 || !_canShowAlert)
                return;

            countShowAlert--;

            _canShowAlert = !(countShowAlert <= 0);

            _popUp.Show(titleAlert, bodyAlert);
        }
    }
}