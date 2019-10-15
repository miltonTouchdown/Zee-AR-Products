using UnityEngine;
using TMPro;

namespace Trophies.Maptek
{
    public class PopUp : MonoBehaviour
    {
        public GameObject background;
        public GameObject contentPopup;

        public TextMeshProUGUI title;
        public TextMeshProUGUI content;

        void Start()
        {
            Hide();
        }

        public void Show(string title = "", string content = "")
        {
            this.title.text = title;
            this.content.text = content;

            background.SetActive(true);
            LeanTween.scale(contentPopup, Vector3.one, .1f);
        }

        public void Hide()
        {
            background.SetActive(false);
            LeanTween.scale(contentPopup, Vector3.zero, .1f);
        }
    }
}