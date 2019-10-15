using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trophies.Rappi
{
    public class ARRappiMenu : MonoBehaviour
    {
        [Header("Waiting Initialize")]
        // Interfaz iniciar partida
        public WindowMovement startGameContent;

        [Header("Selection Container")]
        // interfaz de seleccion de contenedor
        public WindowMovement selectorContainer;
        public TextMeshProUGUI textMessage;

        [Header("Finish Game")]
        // interfaz de seleccion de contenedor
        public WindowMovement finishGameContent;

        [Header("Celebration")]
        // Particulas de celebracion
        public ParticleSystem particleCelebration;

        [Header("Message Feedback")]
        // Interfaz mensaje de feedback
        public WindowMovement feedbackMessage;

        [Header("UI Sound")]
        public Button bttnSound;
        public GameObject imageNoSound;

        public MusicControl _musicControl;

        [Header("Feedback Tracking")]
        public RectTransform imgSearchTarget;
        public float timeAlphaTransition = .8f;

        void Start()
        {
            _musicControl = GameManager.Instance.GetMusicControl();
            imageNoSound.SetActive(!_musicControl.hasAudio);

            particleCelebration.Stop(true);

            SetActiveSearchTarget(true);
        }

        public void StartGame()
        {
            GameManager.Instance.StartGame();
        }

        public void RestartGame()
        {
            GameManager.Instance.LoadScene(GameManager.Instance.idSceneAR);
        }

        public void GoToMainMenu()
        {
            GameManager.Instance.LoadScene(GameManager.Instance.idSceneMainMenu);
        }

        /// <summary>
        /// Mostrar/Ocultar interfaz para iniciar juego
        /// </summary>
        public void SetActiveWaitInitGame(bool value)
        {
            // Evita activar interfaz cuando se esta en pausa
            if (GameManager.Instance.IsPaused && value)
                return;

            startGameContent.setActiveWindow(value);
        }

        /// <summary>
        /// Mostrar/Ocultar interfaz de seleccion contenedor
        /// </summary>
        public void SetActiveSelectorContainer(bool value)
        {
            // Evita activar interfaz cuando se esta en pausa
            if (GameManager.Instance.IsPaused && value)
                return;

            selectorContainer.setActiveWindow(value);
        }

        /// <summary>
        /// Mostrar/Ocultar interfaz final juego
        /// </summary>
        public void SetActiveFinishGame(bool value)
        {
            // Evita activar interfaz cuando se esta en pausa
            if (GameManager.Instance.IsPaused && value)
                return;

            finishGameContent.setActiveWindow(value);
        }

        public void SetActiveMessage(bool value)
        {
            // Evita activar interfaz cuando se esta en pausa
            if (GameManager.Instance.IsPaused && value)
                return;

            if (value)
            {
                feedbackMessage.setActiveWindow(true);
                textMessage.text = GameManager.Instance.GetMessageFeedbackGameOver();
            }
            else
            {
                feedbackMessage.setActiveWindow(false);
                textMessage.text = "";
            }
        }

        /// <summary>
        /// Activar/Desactivar celebracion
        /// </summary>
        /// <param name="value"></param>
        public void SetActiveCelebration(bool value)
        {
            // Evita activar interfaz cuando se esta en pausa
            if (GameManager.Instance.IsPaused && value)
                return;

            if (value)
                particleCelebration.Play(true);
            else
                particleCelebration.Stop(true);
        }

        /// <summary>
        /// Mostrar interfaz de asombro
        /// </summary>
        /// <param name="time"> Tiempo que se muestra el efecto</param>
        public void ActiveAmaze(float time)
        {
            // Evita activar interfaz cuando se esta en pausa
            if (GameManager.Instance.IsPaused)
                return;

        }

        /// <summary>
        /// Activar/Desactivar imagen que indica la busqueda de un marcador
        /// </summary>
        /// <param name="value"></param>
        public void SetActiveSearchTarget(bool value)
        {
            imgSearchTarget.gameObject.SetActive(value);

            if (value)
            {
                // Desactivar las interfaces activas
                SetActiveCelebration(false);
                SetActiveMessage(false);
                SetActiveSelectorContainer(false);
                SetActiveWaitInitGame(false);
                SetActiveFinishGame(false);

                // Reiniciar valores
                Image imgSearching = imgSearchTarget.GetComponent<Image>();
                imgSearching.color = new Color(imgSearching.color.r, imgSearching.color.g, imgSearching.color.b, 1f);
                //Text textChild = imgSearchTarget.GetComponentInChildren<Text>();
                //textChild.color = new Color(textChild.color.r, textChild.color.g, textChild.color.b, 1f);

                // Activar interfaz
                //Image imgSearching = imgSearchTarget.GetComponent<Image>();
                Debug.Log("SET ALPHA: " + imgSearching.color);

                LeanTween.alpha(imgSearchTarget, .2f, timeAlphaTransition).setLoopPingPong();
            }
            else
            {
                LeanTween.cancel(imgSearchTarget);
            }
        }

        public void SetAudio()
        {
            bool HasAudio = !_musicControl.hasAudio;

            _musicControl.setAudio(HasAudio);

            // Activar/desactivar imagen como feedback
            imageNoSound.SetActive(!HasAudio);
        }
    }
}