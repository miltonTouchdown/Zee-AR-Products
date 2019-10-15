using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Trophies.Trophies
{
    public class SpriteLoopAnim : MonoBehaviour
    {
        public Sprite[] sprites;
        public float[] timeForFrame;
        float timer;
        int counter;
        Image image;
        // Use this for initialization
        void Start()
        {
            image = GetComponent<Image>();
            timer = timeForFrame[0];
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                counter++;
                counter %= sprites.Length;
                image.sprite = sprites[counter];
                timer = timeForFrame[counter];
            }
        }
    }
}