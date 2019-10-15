using UnityEngine;
using UnityEngine.Events;

namespace Trophies.Maptek
{
    public class Tutorial : MonoBehaviour
    {
        public GameObject[] tutorialScreens;

        public bool IsActive = true;

        private int currIndex = 0;

        public UnityEvent onFinish;

        void Start()
        {
            // Detectar si el tutorial esta desactivado por el usuario
            if (PlayerPrefs.HasKey("isDeactive"))
            {
                // Si el valor es 0 entonces no se muestra el tutorial
                int isDeactive = PlayerPrefs.GetInt("isDeactive");

                if (isDeactive == 0)
                {
                    Close();

                    return;
                }
            }

            Open();
        }

        public void OnNextScreen()
        {
            if ((currIndex + 1) > (tutorialScreens.Length - 1))
            {
                Close();

                return;
            }

            currIndex++;

            // Activar siguiente pantalla
            tutorialScreens[currIndex].SetActive(true);

            // Desactivar pantalla anterior
            tutorialScreens[currIndex - 1].SetActive(false);
        }

        public void OnPreviousScreen()
        {
            if ((currIndex - 1) < 0)
            {
                return;
            }

            currIndex--;

            // Activar siguiente pantalla
            tutorialScreens[currIndex].SetActive(true);

            // Desactivar pantalla anterior
            tutorialScreens[currIndex + 1].SetActive(false);
        }

        public void Open()
        {
            currIndex = 0;
            IsActive = true;

            for (int i = 0; i < tutorialScreens.Length; i++)
            {
                tutorialScreens[i].SetActive(false);
            }

            tutorialScreens[currIndex].SetActive(true);
        }

        public void Close()
        {
            onFinish.Invoke();

            currIndex = 0;
            IsActive = false;

            for (int i = 0; i < tutorialScreens.Length; i++)
            {
                tutorialScreens[i].SetActive(false);
            }
        }

        /// <summary>
        /// No mostrar el tutorial en la aplicacion
        /// </summary>
        public void OnToggleTutorial(bool value)
        {
            int isDeactive = (value) ? 0 : 1;

            PlayerPrefs.SetInt("isDeactive", isDeactive);
        }
    }
}