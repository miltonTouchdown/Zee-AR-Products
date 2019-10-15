using UnityEngine;

namespace Trophies.Maptek
{
    public class SpriteRendererLoopAnim : MonoBehaviour
    {
        public Sprite[] sprites;
        public float timeForFrame;
        float timer;
        int counter;
        SpriteRenderer image;
        // Use this for initialization
        private void Awake()
        {
            image = GetComponent<SpriteRenderer>();
            timer = timeForFrame;
            counter = Random.Range(0, sprites.Length - 1);
            image.sprite = sprites[counter];
        }
        void Start()
        {

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
                timer = timeForFrame;
            }
        }
    }
}