namespace ZeeAR.Visualization
{
    using TMPro;
    using Trophies.Maptek;
    using UnityEngine;
    using Vuforia;

    public class QuestionControl : MonoBehaviour
    {
        public TextMeshProUGUI Question;

        public float WaitShow = 1f;
        public float ScaleShow = 1f;

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
            LeanTween.scale(this.gameObject, Vector3.zero, .1f);
        }
    }
}