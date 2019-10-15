namespace ZeeAR.Visualization
{
    using UnityEngine;

    public class SelectorSceneButton : MonoBehaviour
    {
        public int IdProduct;

        public void LoadScene()
        {
            AppManager.Instance.LoadSceneAR(IdProduct);
        }

        public void LoadMainMenu()
        {
            AppManager.Instance.LoadMainMenu();
        }
    }
}