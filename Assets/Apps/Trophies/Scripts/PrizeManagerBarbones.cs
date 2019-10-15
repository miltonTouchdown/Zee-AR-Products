using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trophies.Trophies
{
    public class PrizeManagerBarbones : MonoBehaviour
    {

        public void GoMainMenu()
        {
            SceneManager.LoadScene(GeneralManager.SceneMenuIndex);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GoMainMenu();
            }
        }

        public void OpenBarbonesRules()
        {
            Application.OpenURL("https://docs.google.com/document/d/1AH61M9a3ppuSX-EWa_0PfKqbNc_5WZV9WBMhpVYIR1E/edit?usp=sharing");
        }

        public void OpenSevenSushiRules()
        {
            Application.OpenURL("https://docs.google.com/document/d/1j1T0_pizRd_blpupChKWKPVrcikMp7LdG6H0P_viaXE/edit?usp=sharing");
        }
    }
}