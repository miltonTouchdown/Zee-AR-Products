namespace ZeeAR.Visualization
{
    using TMPro;
    using Trophies.Maptek;
    using UnityEngine;
    using UnityEngine.UI;
    using Vuforia;

    public class QuestionControl : MonoBehaviour
    {
        public TextMeshProUGUI Question;
        public Button buttonYes;
        public Button buttonNot;

        public float WaitShow = 1f;
        public float ScaleShow = 1f;

        public float ScaleButtonSelected = 25f;

        [SerializeField]
        private bool _isShow = false;

        private void Start()
        {
            this.gameObject.transform.localScale = Vector3.zero;
        }

        public void Initialization()
        {
            _isShow = true;

            Question.text = ARManager.Instance.CurrProductData.Question;
        }

        public void Show()
        {
            Initialization();

            LeanTween.delayedCall(WaitShow, () => {

                Vector3 scale = new Vector3(ScaleShow, ScaleShow, ScaleShow);

                LeanTween.scale(this.gameObject, scale, .2f);
            });         
        }

        public void Hide(bool answer)
        {
            buttonNot.interactable = false;
            buttonYes.interactable = false;

            RectTransform buttonSelected = (!answer) ? buttonYes.GetComponent<RectTransform>() : buttonNot.GetComponent<RectTransform>();

            float initValue = buttonSelected.rect.width;
            float timeTransition = .5f;

            LeanTween.value(initValue, ScaleButtonSelected, timeTransition).setOnUpdate((float f) => 
            {
                buttonSelected.sizeDelta = new Vector2(f, f);
            }).setOnComplete(() => {

                buttonNot.GetComponentInChildren<TextMeshProUGUI>().text = (answer) ? "0%" : "100%";
                buttonYes.GetComponentInChildren<TextMeshProUGUI>().text = (answer) ? "100%" : "0%";
            });
        }

        private void OnDestroy()
        {
            LeanTween.cancel(this.gameObject);
        }
    }
}