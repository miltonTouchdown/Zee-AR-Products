using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.Trophies
{
    public class DiscoLightsController : MonoBehaviour
    {

        public SpriteRenderer[] DiscoLights;
        public SpriteRenderer MainDiscoImage;

        public Color[] ColorLights;

        public float MainDiscoColorChangeTime;
        float mainDiscoColorTimer;
        public float DiscoColorLightsChangeTime;
        float discoColorLightsTimer;

        // Use this for initialization
        void Start()
        {
            discoColorLightsTimer = DiscoColorLightsChangeTime;
            mainDiscoColorTimer = MainDiscoColorChangeTime;
            SetDiscoBackgroundColor();
            SetDiscoColorsLights();


        }

        // Update is called once per frame
        void Update()
        {
            mainDiscoColorTimer -= Time.deltaTime;
            discoColorLightsTimer -= Time.deltaTime;
            if (mainDiscoColorTimer <= 0)
            {
                mainDiscoColorTimer = MainDiscoColorChangeTime;
                SetDiscoBackgroundColor();
            }

            if (discoColorLightsTimer <= 0)
            {
                discoColorLightsTimer = DiscoColorLightsChangeTime;
                SetDiscoColorsLights();
            }


        }

        void SetDiscoBackgroundColor()
        {
            MainDiscoImage.color = ColorLights[Random.Range(0, ColorLights.Length - 1)];
        }


        void SetDiscoColorsLights()
        {
            List<SpriteRenderer> activeLights = new List<SpriteRenderer>();
            List<SpriteRenderer> inactiveLights = new List<SpriteRenderer>();

            foreach (SpriteRenderer discolight in DiscoLights)
            {
                if (discolight.gameObject.activeInHierarchy)
                {
                    activeLights.Add(discolight);
                    discolight.gameObject.SetActive(false);
                }
                else
                {
                    inactiveLights.Add(discolight);
                }
            }


            foreach (SpriteRenderer discolight in inactiveLights)
            {
                discolight.gameObject.SetActive(true);
                int indexColor = Random.Range(0, ColorLights.Length - 1);
                discolight.color = ColorLights[indexColor];
            }
        }
    }
}