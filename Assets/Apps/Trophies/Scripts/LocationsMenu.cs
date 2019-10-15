using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.Trophies
{
    public class LocationsMenu : MonoBehaviour
    {

        public LocationElement[] locationElements;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetLocations(string[] locationNames, Sprite[] locationLogos, MainMenu mainMenuRef)
        {
            if (locationNames.Length == locationLogos.Length)
            {
                if (locationNames.Length <= locationElements.Length)
                {
                    int index = 0;

                    foreach (string locationName in locationNames)
                    {
                        locationElements[index].Setup(locationNames[index], locationLogos[index], mainMenuRef);
                        index++;
                    }

                    if (index < locationElements.Length - 1)
                    {
                        for (int subIndex = index; subIndex < locationElements.Length; subIndex++)
                        {
                            locationElements[subIndex].Disable();
                        }
                    }
                }
                else
                {
                    Debug.Log("Error: to many elements");
                }
            }
            else
            {
                Debug.Log("Error: Diferent lengths on location data");
            }
        }
    }
}