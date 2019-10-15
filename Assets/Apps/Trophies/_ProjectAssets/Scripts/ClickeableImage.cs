using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Trophies.Trophies
{
    public class ClickeableImage : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        public EmptyEvent onClickImage;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClickImage.Invoke();
        }
    }

    [System.Serializable]
    public class EmptyEvent : UnityEvent { }
}