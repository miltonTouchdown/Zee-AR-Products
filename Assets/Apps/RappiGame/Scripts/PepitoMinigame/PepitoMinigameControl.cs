using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Trophies.Rappi
{
    public class PepitoMinigameControl : MonoBehaviour
    {
        public bool HasSound = true;

        // Contenedores que se moveran en el minijuego. 
        public Container[] arrContainers;

        // Posicion inicial de cada contenedor
        public Transform[] posContainers;

        // Movimientos. Indica los objetos relacionados cuando
        // se realiza un movimiento.
        public ConfigTypeMove[] configMoves;

        public float[] velSingleShuffle;
        public float difficulty = 1f;

        // Objetos relacionados a un tipo de movimiento
        [System.Serializable]
        public struct ConfigTypeMove
        {
            public Type_Move typeMove;
            public int posObj1;
            public int posObj2;
        }

        // Direccion del movimiento
        public enum Dir_Move
        {
            None,
            Left,
            Right
        }

        // direccion actual
        [SerializeField]
        private Dir_Move _currDirMove;

        /**
         * Tipo de movimiento
         * 
         * Solo se soportan 3 tipos de movimientos.
         * Existen 3 tipos de movimientos:
         * - Move_1: Intercambio entre posicion 1 y posicion 2
         * - Move_2: Intercambio entre posicion 1 y posicion 3
         * - Move_3: Intercambio entre posicion 2 y posicion 3
         */
        public enum Type_Move
        {
            None,
            Move_1,
            Move_2,
            Move_3
        }

        // Tipo movimiento actual
        [SerializeField]
        private Type_Move _currTypeMove;

        // Cantidad maxima de movimientos
        public int maxMoves = 10;

        // Cantidad de movimientos realizados.
        [SerializeField]
        private int _countMoves = 0;

        public float currVelocityShuffle = .5f;

        // Velocidad de movimiento y en que momento cambia

        // Movimiento asociado a diferentes rutas que recorren los
        // contenedores.
        [System.Serializable]
        public class pathContainer
        {
            public string name;
            public Type_Move typeMove;
            public Path[] arrPathMovement;

            [System.Serializable]
            public struct Path
            {
                public string namePath;
                public Transform[] arrPath;
            }
        }

        // Conjunto de caminos para los diferentes tipos de movimientos
        public pathContainer[] splinePaths;

        // Contador de contenedores que estan en movimiento. 
        private int _countContainersMoving = 0;

        public bool canSelectContainer = false;
        public bool canNextMove = false;
        public bool isShuffling = false;
        public bool AutoShuffle = true;

        // Indica si se ha ganado la partida
        public bool? IsWin = null;

        public UnityEvent OnInitMove;
        public UnityEvent OnFinishMove;
        public UnityEvent OnFinishAllMoves;

        public UnityEvent OnInitGame;
        public UnityEvent OnFinishGame;

        [System.Serializable]
        public class OnSelectorEvent : UnityEvent<int> { }

        public OnSelectorEvent OnSelectionContainerChange;

        void OnEnable()
        {
            // create the path
            //cr = new LTSpline(new Vector3[] { trans[0].position, trans[1].position, trans[2].position, trans[3].position, trans[4].position });
            // cr = new LTSpline( new Vector3[] {new Vector3(-1f,0f,0f), new Vector3(0f,0f,0f), new Vector3(4f,0f,0f), new Vector3(20f,0f,0f), new Vector3(30f,0f,0f)} );
        }

        private static PepitoMinigameControl _instance;
        public static PepitoMinigameControl Instance
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
        }

        void Start()
        {
            // Configurar velocidades y dificultad desde data config
            velSingleShuffle = GameManager.Instance.GetVelocityShuffleGame();
            difficulty = GameManager.Instance.GetValueLevelGame();

            maxMoves = velSingleShuffle.Length;
        }

        void Update()
        {
            if (!canSelectContainer)
                return;

            // Seleccionar contenedor
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began) && !EventSystem.current.IsPointerOverGameObject(0))
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(raycast, out hit))
                {
                    if (hit.collider.CompareTag("Container"))
                    {
                        Container cs = hit.collider.GetComponent<Container>();

                        // Seleccionar container
                        cs.OnSelectContainer();

                        // Posicion inicial se toma como identificador para desactivar los otros 
                        // contenedores.
                        int posContainerSelected = cs.initPos;

                        foreach (Container c in arrContainers)
                        {
                            if (c.initPos != posContainerSelected)
                                c.OnDeselectedContainer();
                        }

                        OnSelectionContainerChange.Invoke(posContainerSelected);
                    }
                }
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(raycast, out hit))
                {
                    if (hit.collider.CompareTag("Container"))
                    {
                        Debug.Log("Container " + hit.collider.name + " clicked");

                        Container cs = hit.collider.GetComponent<Container>();

                        // Seleccionar container
                        cs.OnSelectContainer();

                        // Posicion inicial se toma como identificador para desactivar los otros 
                        // contenedores.
                        int posContainerSelected = cs.initPos;

                        foreach (Container c in arrContainers)
                        {
                            if (c.initPos != posContainerSelected)
                                c.OnDeselectedContainer();
                        }

                        OnSelectionContainerChange.Invoke(posContainerSelected);
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Inicializar minijuego
        /// </summary>
        public void InitGame()
        {
            if (isShuffling)
                return;

            // Reiniciar posicion de contenedores
            for (int i = 0; i < posContainers.Length; i++)
            {
                // Obtener el orden de cada contenedor (pos init) y cambiar la posicion del objeto
                // de acuerdo a su posicion inicial.
                arrContainers[i].transform.position = posContainers[arrContainers[i].GetInitPos()].position;
            }

            // Ordenar arreglo de acuerdo a su posicion inicial
            arrContainers = arrContainers.OrderBy((c) => c.initPos).ToArray();

            _countMoves = 0;
            _currDirMove = Dir_Move.None;
            _currTypeMove = Type_Move.None;
            isShuffling = true;
            canNextMove = true;
            canSelectContainer = false;

            OnInitGame.Invoke();

            Invoke("Shuffle", .2f);
        }

        public void FinishGame()
        {

            // Indicar si se ha ganado el juego
            foreach (Container c in arrContainers)
            {
                if (c.IsSelected)
                {
                    IsWin = c.HasPrize;
                }
            }

            GameManager.Instance.FinishGame();

            OnFinishGame.Invoke();
        }

        private void FinishShuffle()
        {
            IsWin = null;
            isShuffling = false;

            // Activar seleccion de contenedores
            canSelectContainer = true;

            OnFinishAllMoves.Invoke();
        }

        /// <summary>
        /// Revolver contenedores
        /// </summary>
        public void Shuffle()
        {
            if (_countMoves >= maxMoves)
                return;

            if (!canNextMove)
                return;

            OnInitMove.Invoke();

            canNextMove = false;

            _countMoves++;

            // Obtener direccion movimiento
            int dirMove = UnityEngine.Random.Range(1, Enum.GetNames(typeof(Dir_Move)).Length);

            _currDirMove = (Dir_Move)dirMove;

            // Obtener tipo de movimiento
            int typeMove = (int)_currTypeMove;

            // Evitar repetir el movimiento anterior
            while (typeMove == (int)_currTypeMove)
            {
                typeMove = UnityEngine.Random.Range(1, Enum.GetNames(typeof(Type_Move)).Length);
            }

            _currTypeMove = (Type_Move)typeMove;

            // Revolver contenedores
            //Move_1: Intercambio entre posicion 1 y posicion 2
            //Move_2: Intercambio entre posicion 1 y posicion 3
            //Move_3: Intercambio entre posicion 2 y posicion 3
            MoveContainer(_currTypeMove, _currDirMove);
        }

        private void MoveContainer(Type_Move tMove, Dir_Move dMove)
        {
            Container temp1 = null, temp2 = null;

            ConfigTypeMove move = configMoves.Single((m) => m.typeMove == tMove);
            pathContainer pathsContainer = splinePaths.Single((s) => s.typeMove == tMove);

            temp1 = arrContainers[move.posObj1];
            temp2 = arrContainers[move.posObj2];

            pathContainer.Path path_1;
            pathContainer.Path path_2;

            // Obtener path de acuerdo a la direccion
            path_1 = pathsContainer.arrPathMovement.Single((p) => p.namePath == ((dMove == Dir_Move.Right) ? "Up" : "Down"));
            path_2 = pathsContainer.arrPathMovement.Single((p) => p.namePath == ((dMove == Dir_Move.Right) ? "Down" : "Up"));

            // Obtener el spline que recorrera el contenedor
            // El segundo container siempre tendra el path inverso
            LTSpline spline_1 = GetSplineFromTranform(path_1.arrPath, false);
            LTSpline spline_2 = GetSplineFromTranform(path_2.arrPath, true);

            // Mover contenedores

            // cantidad de contenedores que se moveran.
            _countContainersMoving = 2;

            // Calcular velocidad. Velocidad de cada movimiento obtenida desde el arreglo multiplicado
            // por la dificultad del juego.
            currVelocityShuffle = velSingleShuffle[_countMoves - 1] * difficulty;

            arrContainers[move.posObj1].Move(spline_1, currVelocityShuffle, () =>
            {
                _countContainersMoving--;

                if (_countContainersMoving <= 0)
                    FinishMove();
            });

            arrContainers[move.posObj2].Move(spline_2, currVelocityShuffle, () =>
            {
                _countContainersMoving--;

                if (_countContainersMoving <= 0)
                    FinishMove();
            });

            arrContainers[move.posObj1] = temp2;
            arrContainers[move.posObj2] = temp1;

            spline_1 = null;
            spline_2 = null;
            pathsContainer = null;
            temp1 = null;
            temp2 = null;
        }

        /// <summary>
        /// Finalizar movimiento
        /// </summary>
        private void FinishMove()
        {
            OnFinishMove.Invoke();

            if (_countMoves >= maxMoves)
            {
                FinishShuffle();
                return;
            }

            _countContainersMoving = 0;
            canNextMove = true;

            // Revolver automaticamente si tiene activado autoshuffle
            if (AutoShuffle)
            {
                Shuffle();
            }
        }

        public void Pause(bool value)
        {
            foreach (Container c in arrContainers)
            {
                c.PauseMove(value);
            }
        }

        public bool IsFinishAllMoves()
        {
            return (maxMoves <= _countMoves);
        }

        /// <summary>
        /// Obtener arreglo de vectores a partir de un arreglo de Transform
        /// </summary>
        /// <param name="ts">arreglo de Transform</param>
        /// <returns>Arreglo de Vector3</returns>
        private Vector3[] GetVector3fromTransform(Transform[] ts)
        {
            Vector3[] arrVectors = new Vector3[ts.Length + 2];

            for (int i = 0; i < ts.Length; i++)
            {
                arrVectors[i + 1] = ts[i].position;
            }

            // En Leantween la primera ultima posicion se utilizan como referencia y 
            // no para definir la ruta.
            arrVectors[0] = ts[0].position;
            arrVectors[arrVectors.Length - 1] = ts[ts.Length - 1].position;

            return arrVectors;
        }

        /// <summary>
        /// Obtener un Spline (LeanTween) a partir de un arreglo de Transforms
        /// </summary>
        /// <param name="ts">Arreglo de Transform</param>
        /// <param name="isReverse">Indica si la direccion del spline. Si es "false" entonces se invierte el arreglo de Transform</param>
        /// <returns>Retorna un LTSpline</returns>
        private LTSpline GetSplineFromTranform(Transform[] ts, bool isReverse)
        {
            Transform[] arrT = new Transform[ts.Length];

            Array.Copy(ts, arrT, ts.Length);

            if (isReverse)
                Array.Reverse(arrT);

            return new LTSpline(GetVector3fromTransform(arrT));
        }

        public GameObject GetContainerSelected()
        {
            // Indicar si se ha ganado el juego
            foreach (Container c in arrContainers)
            {
                if (c.IsSelected)
                {
                    return c.gameObject;
                }
            }

            return null;
        }

        void OnDrawGizmos()
        {
            // TODO : Mostrar ruta de cada movimiento

            //// Debug.Log("drwaing");
            //if (cr == null)
            //    OnEnable();
            //Gizmos.color = Color.red;
            //if (cr != null)
            //    cr.gizmoDraw(); // To Visualize the path, use this method
        }

        public delegate void OnFinishShuffleCallback();
        public static event OnFinishShuffleCallback onFinishShuffleCallback;
    }
}