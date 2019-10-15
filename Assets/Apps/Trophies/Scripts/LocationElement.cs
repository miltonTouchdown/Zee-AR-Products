using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trophies.Trophies
{
    public class LocationElement : MonoBehaviour
    {

        public Button buttonBhvr;
        public string pubName;
        MainMenu mainMenu;

        public Image logoRef;
        public TextMeshProUGUI nameRef;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Setup(string locationName, Sprite locationLogo, MainMenu manager)
        {
            Enable();
            buttonBhvr.onClick = null;
            buttonBhvr.onClick = new Button.ButtonClickedEvent();

            pubName = locationName;
            nameRef.text = pubName;
            logoRef.sprite = locationLogo;
            mainMenu = manager;

            buttonBhvr.onClick.AddListener(onClick);

        }

        public void onClick()
        {
            mainMenu.ChangePub(pubName);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}