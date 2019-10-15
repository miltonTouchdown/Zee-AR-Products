using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Trophies.Trophies
{
    public class TargetInformation : MonoBehaviour
    {
        //Titulo, foto principal, descripcion, monto de fundraising, impacto esperado
        public SpriteRenderer SpriteRenderer;
        public TextMeshPro Title;
        public TextMeshPro Description;
        public TextMeshPro Amount;
        public TextMeshPro Impact;

        public Texture2D testTExture;

        // Start is called before the first frame update
        void Start()
        {
            //SetInformation(testTExture, "Titulon", "Descripcion de la causa", "un chillion 11110000", "impacto cero");
        }

        public void SetInformation(Texture2D photo, string title, string description, string amount, string impact)
        {
            SpriteRenderer.sprite = ConvertToSprite(photo);
            Title.text = title;
            Description.text = description;
            Amount.text = amount;
            Impact.text = impact;
        }

        public Sprite ConvertToSprite(Texture2D texture)
        {
            Texture2D textureResized = Resize(texture, 512, 342);
            texture = null;

            return Sprite.Create(textureResized, new Rect(0, 0, textureResized.width, textureResized.height), Vector2.zero);
        }

        public Texture2D Resize(Texture2D source, int newWidth, int newHeight)
        {
            source.filterMode = FilterMode.Point;
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Texture2D nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newWidth), 0, 0);
            nTex.Apply();
            RenderTexture.active = null;
            return nTex;

        }
    }
}