using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Trophies.Trophies
{
    public class MainMenu : MonoBehaviour
    {
        bool isPubSelectionMenuOpen;
        public GameObject PubSelectionMenu;
        public LocationsMenu locationMenuRef;
        public Image selectedPubImage;
        public TextMeshProUGUI selectedPubText;

        private void Awake()
        {
            EnablePubSelectionWindow(false);
        }

        private void Start()
        {
            SetPubSelection();
            BuildPubWindow();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    Application.Quit();
                }
                else
                {
                    GoMainMenu();
                }

            }
        }

        public void EnablePubSelectionWindow(bool enable)
        {
            PubSelectionMenu.SetActive(enable);
            isPubSelectionMenuOpen = enable;
        }

        public void OpenPubSelection()
        {
            EnablePubSelectionWindow(true);
        }

        public void ClosePubSelection()
        {
            EnablePubSelectionWindow(false);
        }

        public void CameraMode()
        {
            PubDataContainer.PubData pubData = GeneralManager.GetPubSelected();
            if (pubData != null && pubData.hasDefinedARScene)
            {
                Debug.Log("scene index pubdata: " + pubData.definedARScene);
                SceneManager.LoadScene(pubData.definedARScene);
            }
            else if (pubData != null && !pubData.hasDefinedARScene)
            {
                Debug.Log("scene index general: " + GeneralManager.SceneARIndex);
                SceneManager.LoadScene(GeneralManager.SceneARIndex);
            }
        }

        public void MainMenu1()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void OpenCLYCme()
        {
            PubDataContainer.PubData pubData = GeneralManager.GetPubSelected();


            if (pubData != null && pubData.definedConstestUrl && pubData.pubConstestButtonUrl.CompareTo("") != 0)
            {
                Debug.Log(pubData.pubConstestButtonUrl);
                Application.OpenURL(pubData.pubConstestButtonUrl);
            }
            else if (pubData != null && (!pubData.definedConstestUrl || pubData.pubConstestButtonUrl.CompareTo("") == 0))
            {
                Debug.Log(GeneralManager.Instance.pubData.ContestButtonURL);
                Application.OpenURL(GeneralManager.Instance.pubData.ContestButtonURL);
            }
        }

        public void GoMainMenu()
        {
            SceneManager.LoadScene(7);
        }

        public void GoConcursos()
        {
            PubDataContainer.PubData pubData = GeneralManager.GetPubSelected();

            if (pubData != null && pubData.hasDefinedPrizeScene)
            {
                SceneManager.LoadScene(pubData.definedPrizeScene);
            }
            else if (pubData != null && !pubData.hasDefinedARScene)
            {
                SceneManager.LoadScene(GeneralManager.ScenePrizeIndex);
            }
        }

        public void OpenTargetTest()
        {
            Application.OpenURL("https://drive.google.com/file/d/169OuBiuEGeWg4eyw-LIXpWZ3gy5aHybh/view?usp=sharing");
        }

        public void OpenFeedbackPoll()
        {
            Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSfhv994fZK0d5Fe5hXMi_3sMG09R6XpklAKeGc4RzQK5C953A/viewform?usp=sf_link");
        }

        public void ChangePub(string pubName)
        {
            SetPubSelection(pubName);

            if (isPubSelectionMenuOpen)
            {
                ClosePubSelection();
            }

        }

        void SetPubSelection(string pubName = "")
        {
            PubDataContainer.PubData pubData = GeneralManager.GetPubSelected();

            // on Enter scene first time
            if (pubName.CompareTo("") == 0 && pubData == null)
            {

            } // on Set for first time or next times
            else if (
                (pubName.CompareTo("") != 0 && pubData == null) ||
                (pubName.CompareTo("") != 0 && pubData != null)
                )
            {
                pubData = GeneralManager.SelectPub(pubName);
                if (pubData != null)
                {
                    selectedPubText.text = pubData.pubName;
                    selectedPubImage.sprite = pubData.pubLogo;
                }
                else
                {
                    Debug.Log("Error pub not found " + pubName);
                }
            } // on Set from saved data
            else if (pubName.CompareTo("") == 0 && pubData != null)
            {
                Debug.Log("Load from saved data");
                if (pubData != null)
                {
                    selectedPubText.text = pubData.pubName;
                    selectedPubImage.sprite = pubData.pubLogo;
                }
                else
                {
                    Debug.Log("Error pub not found");
                }
            }

        }

        void BuildPubWindow()
        {
            PubDataContainer.PubData[] pubsData = GeneralManager.GetPubList();

            List<string> pubsNames = new List<string>();
            List<Sprite> pubSprites = new List<Sprite>();
            int index = 0;
            foreach (PubDataContainer.PubData currPub in pubsData)
            {
                //Debug.Log(currPub.pubName);
                if (currPub.hidePubInList) continue;
                pubsNames.Add(currPub.pubName);
                pubSprites.Add(currPub.pubLogo);
                //Debug.Log(currPub.pubName);
                index++;
            }

            locationMenuRef.SetLocations(pubsNames.ToArray(), pubSprites.ToArray(), this);

        }
    }
}