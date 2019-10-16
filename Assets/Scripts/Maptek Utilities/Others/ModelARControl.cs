using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Trophies.Maptek
{
    public class ModelARControl : MonoBehaviour
    {
        [SerializeField]
        public ModelTarget[] modelsTarget;
        public bool initModelsDeactivate = false;
        private Transform _modelsContent;

        public UnityEvent OnInitialization;

        private void Start()
        {
            _modelsContent = gameObject.transform.GetChild(0);
        }

        public void setActiveModel(int id)
        {
            ModelTarget target = modelsTarget.SingleOrDefault((mt) => mt.Id == id);

            setActiveModel(target);
        }

        /// <summary>
        /// Activa/Desactiva los modelos dependiendo de su naturaleza. 
        /// Si el modelo no es "overlap" entonces solo puede haber uno activado (del conjunto con 
        /// la misma opcion).
        /// </summary>
        /// <param name="target"></param>
        public void setActiveModel(ModelTarget target)
        {
            bool isActive;
            int id = target.Id;

            if (target == null)
                return;

            if (target.isOverlap)
            {
                isActive = !target.IsActive;
                target.Model.SetActive(isActive);
                target.IsActive = isActive;
                return;
            }

            // Activar/Desactivar modelos que no tienen overlap.
            // Si un modelo se activa, los demás se desactivan.
            // Si un modelo se desactiva, los demás modelos se desactivan.
            foreach (ModelTarget m in modelsTarget)
            {
                if (id == m.Id)
                    isActive = !m.IsActive;
                else
                    isActive = false;

                if (!m.isOverlap)
                {
                    SetActiveSingleModel(m, isActive);
                }
            }
        }

        /// <summary>
        /// Activar desactivar un modelo en especifico
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void SetActiveSingleModel(ModelTarget target, bool value)
        {
            target.Model.SetActive(value);
            target.IsActive = value;
        }

        /// <summary>
        /// Reiniciar posicion de los modelos
        /// </summary>
        public void ResetRotation()
        {
            // Reinicia los valores a cero.
            _modelsContent.localRotation = Quaternion.Euler(Vector3.zero);
        }

        /// <summary>
        /// Obtener modelos activos
        /// </summary>
        /// <returns>Arreglo de modelos activos</returns>
        public ModelTarget[] GetActiveTargets()
        {
            return modelsTarget.Where((ts) => ts.IsActive == true).ToArray();
        }

        /// <summary>
        /// Inicializa los valores de modelos
        /// </summary>
        public void Init()
        {
            ModelTarget[] arrTargetActive = GetActiveTargets();

            // Obtener un modelo activo del arreglo que no se con overlap para topografias
            // Se evita tener mas de un modelo activo.
            ModelTarget singleModel = arrTargetActive.FirstOrDefault((m) => m.isOverlap == false);
            int idModelActive = (singleModel != null) ? singleModel.Id : -1;

            // Activar/Desactivar modelos segun su configuracion inicial
            foreach (ModelTarget om in modelsTarget)
            {
                if (om.isOverlap)
                {
                    om.Model.SetActive(om.IsActive);
                }
                else
                {
                    // Activar solo un modelo si no tiene overlap
                    bool isActive = (idModelActive == om.Id);

                    om.Model.SetActive(isActive);
                }
            }

            OnInitialization.Invoke();
        }
    }
}