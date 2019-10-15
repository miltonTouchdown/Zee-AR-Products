using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trophies.Maptek
{
    public class ButtonTarget : MonoBehaviour
    {
        public int IdTarget;

        private Button bttnTarget;

        void Start()
        {
            bttnTarget = GetComponent<Button>();
        }

        public void setId(int id)
        {
            IdTarget = id;
        }

        /// <summary>
        /// Modificar la informacion y listener del boton
        /// </summary>
        /// <param name="mControl"></param>
        /// <param name="mTarget"></param>
        public void setListenerButton(ModelARControl mControl, ModelTarget mTarget)
        {
            if (bttnTarget == null)
                bttnTarget = GetComponent<Button>();

            setId(mTarget.Id);

            // Cambiar nombre boton
            bttnTarget.GetComponentInChildren<TextMeshProUGUI>().text = mTarget.Name;

            bttnTarget.onClick.AddListener(() => mControl.setActiveModel(mTarget.Id));
            bttnTarget.onClick.AddListener(() => ARManager.Instance.ChangeModelTarget(mControl));

            bool isInteractable = (mTarget.isOverlap) ? true : !mTarget.IsActive;
            bttnTarget.interactable = true;
        }

        public void setInteractable(bool value)
        {
            bttnTarget.interactable = value;
        }
    }
}