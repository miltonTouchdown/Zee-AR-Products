using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trophies.Rappi
{
    public class UIPrizeElement : MonoBehaviour
    {
        public TextMeshProUGUI namePrize;
        public Image imagePrize;
        public Button button;

        public PrizeType prizeType { private set; get; }

        public void SetInfo(DataPrize dp)
        {
            namePrize.text = dp.namePrize;
            imagePrize.overrideSprite = dp.iconPrize;
            prizeType = dp.prizeType;

            RappiMainMenu rm = FindObjectOfType<RappiMainMenu>();

            button.onClick.AddListener(() => { rm.OnSelectPrize(this); rm.SetActivePanelListPrize(false); });
        }
    }
}