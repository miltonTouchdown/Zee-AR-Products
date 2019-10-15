using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trophies.Rappi
{
    public class GameManager : MonoBehaviour
    {
        [Header("Data Configuration Game")]
        public GameConfigData configData;
        public int idSceneMainMenu;
        public int idSceneAR;

        [Header("Options Selected")]
        // Premio actual seleccionado
        public PrizeType currPrize = PrizeType.Gift_Box;
        public LevelType currDifficulty = LevelType.Medium;

        [Header("Game")]
        // Indica si el jugador ha ganado la partida
        public bool IsPaused = false;
        public bool? IsPlayerWinner = false;

        private ARRappiMenu _arRappiMenu;
        private MusicControl _musicControl;
        private LoaderScene _loaderScene;

        private GameState _currState = GameState.None;
        private GameState _lastState = GameState.None;
        public GameState CurrState
        {
            get
            {
                return _currState;
            }
            set
            {
                if (_currState == value)
                    return;

                _lastState = _currState;

                _currState = value;

                OnStateChange();
            }
        }

        private SequenceControl[] _sequenceControls = null;

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
        }

        void Awake()
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

        void Start()
        {
            _musicControl = FindObjectOfType<MusicControl>();
            _loaderScene = FindObjectOfType<LoaderScene>();

            _loaderScene.ShowLoadScreen(.5f, () =>
            {
            // Inicializar por primera vez main menu
            FindObjectOfType<RappiMainMenu>().InitMenu();
            });
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (_currState != GameState.None)
                {
                    // Cargar menu principal
                    LoadScene(idSceneMainMenu);
                }
                else
                {
                    Application.Quit();
                }
            }
        }

        public void InitGame()
        {
            // Buscar todas las secuencias en la escena
            _sequenceControls = FindObjectsOfType<SequenceControl>();
            _arRappiMenu = FindObjectOfType<ARRappiMenu>();

            CurrState = GameState.Searching_Target;
            _lastState = GameState.Waiting_Initialize;

            _musicControl.SetMusicVolume(1f);
        }

        public void StartGame()
        {
            CurrState = GameState.Starting;
        }

        /// <summary>
        /// Juego Finalizado
        /// </summary>
        public void FinishGame()
        {
            // Si es nulo entonces no se ha escogido ningun contenedor
            if (PepitoMinigameControl.Instance.IsWin == null)
            {
                return;
            }

            IsPlayerWinner = PepitoMinigameControl.Instance.IsWin;

            CurrState = GameState.Finishing;
        }

        public void PauseGame()
        {
            PauseGame(!IsPaused);
        }

        public void PauseGame(bool value)
        {
            IsPaused = value;

            CurrState = GameState.Searching_Target;

            if (_lastState == GameState.Playing)
                PepitoMinigameControl.Instance.Pause(IsPaused);

            if (IsPaused)
            {
                //CurrState = GameState.Searching_Target;

                //if (_lastState == GameState.Playing)
                //    PepitoMinigameControl.Instance.Pause(IsPaused);

                if (_lastState == GameState.Starting)
                {
                    SequenceControl currSeq = _sequenceControls.SingleOrDefault((s) => s.gameStateSequence == GameState.Starting);
                    currSeq.CancelSequence();
                }
            }
            else
            {
                //if (_lastState == GameState.Playing)
                //    PepitoMinigameControl.Instance.Pause(IsPaused);

                CurrState = _lastState;
            }
        }

        /// <summary>
        /// Cambiar estado del juego
        /// </summary>
        /// <param name="state">Index de GameState</param>
        public void ChangeState(int state)
        {
            ChangeState((GameState)state);
        }

        public void ChangeState(GameState state)
        {
            CurrState = state;
        }

        private void OnStateChange()
        {
            if (_currState == GameState.None)
                return;

            _arRappiMenu.SetActiveSearchTarget(_currState == GameState.Searching_Target);

            switch (_currState)
            {
                case GameState.Searching_Target:
                    {
                        // Interfaz buscando marcador
                        _arRappiMenu.SetActiveWaitInitGame(false);
                        break;
                    }
                case GameState.Waiting_Initialize:
                    {
                        // Mostrar interfaz:
                        //  + Boton "iniciar juego"

                        _arRappiMenu.SetActiveWaitInitGame(true);
                        break;
                    }
                case GameState.Starting:
                    {
                        _arRappiMenu.SetActiveWaitInitGame(false);

                        // Iniciar secuencia "Esconder mochila"
                        SequenceControl currSeq = _sequenceControls.SingleOrDefault((s) => s.gameStateSequence == CurrState);

                        if (currSeq == null)
                        {
                            CurrState = GameState.Playing;
                            return;
                        }

                        currSeq.StartSequence(() =>
                        {
                        // Secuencia finalizada
                        CurrState = GameState.Playing;
                        });

                        break;
                    }
                case GameState.Playing:
                    {
                        // El juego no se reinicia si el juego estuvo en pausa
                        // Esto se maneja desde el metodo PauseGame
                        if (_lastState == GameState.Searching_Target)
                        {
                            if (PepitoMinigameControl.Instance.IsFinishAllMoves())
                            {
                                // Activar menu en caso de que el juego haya finalizado los movimientos
                                _arRappiMenu.SetActiveSelectorContainer(true);
                            }

                            return;
                        }

                        // Iniciar juego "Revolver contenedores"
                        PepitoMinigameControl.Instance.InitGame();
                        break;
                    }
                case GameState.Finishing:
                    {
                        _arRappiMenu.SetActiveSelectorContainer(false);

                        // Indicar si ganó o perdió a traves de las secuencias
                        // + Iniciar secuencia "Levantar contenedor escogido"
                        //  ++ Correcto: 
                        //      --  Iniciar secuencia "Mostrar premio"
                        //      --  Interfaz al finalizar secuencia
                        //  ++ Incorrecto: 
                        //      --  Iniciar secuencia "Incorrecto"
                        //      --  Interfaz al finalizar secuencia

                        SequenceControl currSeq = _sequenceControls.SingleOrDefault((s) => s.gameStateSequence == CurrState);

                        if (currSeq == null)
                        {
                            CurrState = GameState.Game_Over;
                            return;
                        }

                        // No realizar nuevamente la secuencia si se ha pausado el juego.
                        if (_lastState == GameState.Searching_Target)
                        {
                            // Activar interfaz si la secuencia ha finalizado
                            if (currSeq.IsFinished)
                            {
                                // Activar interfaz en caso de finalizada la secuencia
                                _arRappiMenu.SetActiveMessage(true);
                                _arRappiMenu.SetActiveFinishGame(true);
                                _arRappiMenu.SetActiveCelebration((bool)IsPlayerWinner);
                            }

                            return;
                        }

                        currSeq.StartSequence(() =>
                        {
                        // Secuencia finalizada
                        //CurrState = GameState.Game_Over;
                        _arRappiMenu.SetActiveFinishGame(true);
                        });
                        break;
                    }
                case GameState.Game_Over:
                    {
                        // Indica que el juego ha sido finalizado
                        Debug.Log("Game Over");
                        _sequenceControls = null;
                        CurrState = GameState.None;

                        //LeanTween.delayedCall(waitTimeGameOver, () =>
                        //{
                        //    // Reiniciar escena
                        //});

                        break;
                    }
                case GameState.Paused:
                    {
                        // Detener juego o secuencia

                        break;
                    }
            }
        }

        public void LoadScene(int idScene)
        {
            CurrState = GameState.None;
            _lastState = GameState.None;

            _loaderScene.LoadScene(idScene);
        }

        public MusicControl GetMusicControl()
        {
            return _musicControl;
        }

        /// <summary>
        /// Obtener Informacion de los modelos a cargar (nombre modelo, nombre archivo) de acuerdo
        /// al tipo de premio.
        /// </summary>
        /// <param name="prizeType"></param>
        /// <returns></returns>
        public DataPrize GetDataPrize(PrizeType prizeType)
        {
            return configData.GetDataPrize(prizeType);
        }

        public DataPrize[] GetAllDataPrize()
        {
            return configData.GetAllDataPrizes();
        }

        public float[] GetVelocityShuffleGame()
        {
            return configData.GetVelocityShuffle();
        }

        public float GetValueLevelGame()
        {
            return configData.GetValueLevel(currDifficulty);
        }

        public string GetMessageFeedbackGameOver()
        {
            bool isWin = (bool)IsPlayerWinner;

            if (isWin)
                return configData.GetMessageGameOver(isWin) + " <b>" + GetDataPrize(currPrize).namePrize + "</b>";
            else
                return configData.GetMessageGameOver(isWin);
        }
    }

    public enum GameState
    {
        None,
        Searching_Target,   // Buscando marcador
        Waiting_Initialize, // Esperando iniciar juego
        Starting,           // Empezando (secuencia)
        Playing,            // Jugando
        Paused,             // Al perder el marcador
        Finishing,          // Finalizando (secuencia)
        Game_Over           // Juego terminado
    }
}