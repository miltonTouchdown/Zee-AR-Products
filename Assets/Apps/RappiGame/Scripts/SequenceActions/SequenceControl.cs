using UnityEngine;
using UnityEngine.Events;

namespace Trophies.Rappi
{
    public class SequenceControl : MonoBehaviour
    {
        // Indica a que estado del juego pertenece
        public GameState gameStateSequence;

        public ElementSequence[] arrElementAction;

        public int currElementAction = 0;

        public bool IsFinished = false;

        private OnFinishSequenceCallback _onFinishSequence = null;

        public UnityEvent OnStartSequence;
        public UnityEvent OnFinishSequence;
        public UnityEvent OnCancelSequence;

        public void StartSequence(OnFinishSequenceCallback onFinish = null)
        {
            OnStartSequence.Invoke();

            _onFinishSequence = onFinish;

            currElementAction = 0;

            IsFinished = false;

            StartElementAction(currElementAction);
        }

        public void FinishSequence()
        {
            IsFinished = true;

            OnFinishSequence.Invoke();

            if (_onFinishSequence != null)
            {
                _onFinishSequence();
            }
        }

        public void CancelSequence()
        {
            OnCancelSequence.Invoke();

            foreach (ElementSequence es in arrElementAction)
            {
                es.CancelElementAction();
            }
        }

        /// <summary>
        /// Iniciar accion del elemento.
        /// </summary>
        /// <param name="posElement">posicion del elemento</param>
        private void StartElementAction(int posElement)
        {
            currElementAction = posElement;

            arrElementAction[posElement].StartElementAction(() =>
            {
                currElementAction++;

                OnFinishedAction();
            });
        }

        /// <summary>
        /// Iniciar siguiente accion o finalizar secuencia.
        /// </summary>
        private void OnFinishedAction()
        {
            // Finalizar secuencia si se han ejecutado todas las acciones.
            if (currElementAction >= arrElementAction.Length)
            {
                FinishSequence();

                return;
            }

            // Iniciar siguiente secuencia
            StartElementAction(currElementAction);
        }

        public delegate void OnFinishElementActionCallback();
        public static event OnFinishElementActionCallback onFinishAction;

        public delegate void OnFinishSequenceCallback();
        public static event OnFinishSequenceCallback onFinishSequence;
    }
}