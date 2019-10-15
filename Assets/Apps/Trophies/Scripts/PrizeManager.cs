using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trophies.Trophies
{
    public class PrizeManager : MonoBehaviour
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
    }
}