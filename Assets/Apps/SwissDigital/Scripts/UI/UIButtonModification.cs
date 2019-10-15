using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trophies.SwissDigital
{
    public class UIButtonModification : MonoBehaviour
    {
        public int idZoneModification;

        public RawImage textureModification;
        public Text textMesh;

        //private Button button;

        void Start()
        {
        }

        public void SetText(string text)
        {
            textMesh.text = text;
        }

        public void SetTexture(Texture texture)
        {
            textureModification.texture = texture;
        }

        public Button GetButton()
        {
            return GetComponent<Button>();
        }

        public void FadeText(bool value)
        {
            float to = (value) ? 0f : 1f;
            LeanTween.textAlpha(textMesh.GetComponent<RectTransform>(), to, .3f);
        }
    }
}