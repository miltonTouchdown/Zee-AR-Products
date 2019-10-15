using UnityEngine;

namespace Trophies.Rappi
{
    public class S_InitGame_1 : ElementSequence
    {
        [Header("Elements")]
        public GameObject[] containers;
        public GameObject bag;
        public Material matBag;
        public Animator animBag;
        public Transform contentSpline;

        [Header("General")]
        public float[] timeToNextAction;

        [Header("Action 1")]
        // Accion 1: Aparece Mochila rappi
        public float scaleBag = 1f;
        public float timeScale = .3f;

        [Header("Action 2")]
        // Accion 2: Se elevan los contenedores
        public float maxheightContainers = 2f;
        public float timeMovContainer = .3f;

        [Header("Action 3")]
        // Accion 3: Mochila rappi ingresa en el contenedor central y desaparece 
        public float timeHideBag = .5f;
        public AudioClip clipHideBag;

        [Header("Action 4")]
        // Accion 4: Bajan los contenedores a su posicion inicial
        public float initheightContainers = 0f;

        //----------------------------------------  

        private int _maxSequence = 4;
        private int _currSequence = 0;

        private LTSpline _splineBag;

        protected override void Start()
        {
            base.Start();

            _splineBag = GetSplineFromTranform(contentSpline);
            matBag.SetFloat("_Transparency", 1f);
            //animBag = bag.GetComponent<Animator>();
        }

        public override void StartElementAction(SequenceControl.OnFinishElementActionCallback onFinish = null)
        {
            base.StartElementAction(onFinish);

            Debug.Log("Inicio Secuencia 1");

            // Inicializar valores de contenedores y mochila

            animBag.SetFloat("direction", -1f);
            animBag.Play("openningBag", 0, 0f);

            matBag.SetFloat("_Transparency", 0f);

            bag.transform.localScale = Vector3.zero;

            // Ubicar mochila al inicio del spline
            bag.transform.position = contentSpline.GetChild(0).position;

            // Ubicar contenedores en posicion inicial
            foreach (GameObject go in containers)
            {
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, initheightContainers, go.transform.localPosition.z);
            }

            _currSequence = -1;

            initNextSequence();
        }

        private void initNextSequence()
        {
            if (_currSequence >= _maxSequence)
            {
                FinishElementAction();

                return;
            }

            _currSequence++;

            switch (_currSequence)
            {
                case 0:
                    {
                        // Aparece Mochila rappi
                        LeanTween.scale(bag, Vector3.one * 1.1f * scaleBag, timeScale).setOnComplete(() =>
                        {
                            LeanTween.scale(bag, Vector3.one * scaleBag, .1f);
                            LeanTween.delayedCall(timeToNextAction[_currSequence], () => { initNextSequence(); });

                        });
                        break;
                    }
                case 1:
                    {

                        int count = containers.Length;

                        // Se elevan los contenedores
                        for (int i = 0; i < containers.Length; i++)
                        {
                            LeanTween.moveLocal(containers[i],
                                new Vector3(containers[i].transform.localPosition.x, maxheightContainers, containers[i].transform.localPosition.z),
                                timeMovContainer).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
                            {
                                count--;

                                if (count <= 0)
                                {
                                    LeanTween.delayedCall(timeToNextAction[_currSequence], () => { initNextSequence(); });
                                }
                            });
                        }

                        break;
                    }
                case 2:
                    {
                        // Mochila rappi ingresa en el contenedor central y desaparece 

                        PlaySound(clipHideBag);

                        LeanTween.move(bag, _splineBag, timeHideBag).setEase(LeanTweenType.easeInSine).setOnComplete(() =>
                        {
                            LeanTween.delayedCall(timeToNextAction[_currSequence], () => { initNextSequence(); });
                        });

                        LeanTween.scale(bag, Vector3.zero, .5f).
                            setOnUpdate((float f) =>
                            {
                                matBag.SetFloat("_Transparency", f);
                            }).
                            setDelay(timeHideBag / 3);
                        break;
                    }
                case 3:
                    {
                        int count = containers.Length;

                        // Bajan los contenedores a su posicion inicial.
                        for (int i = 0; i < containers.Length; i++)
                        {
                            LeanTween.moveLocal(containers[i],
                                new Vector3(containers[i].transform.localPosition.x, initheightContainers, containers[i].transform.localPosition.z),
                                timeMovContainer).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
                                {
                                    count--;

                                    if (count <= 0)
                                    {
                                        LeanTween.delayedCall(timeToNextAction[_currSequence], () => { FinishElementAction(); });
                                    }
                                });
                        }
                        break;
                    }
            }
        }

        public override void FinishElementAction()
        {
            base.FinishElementAction();

            LeanTween.cancel(gameObject);
            _currSequence = _maxSequence;

            Debug.Log("Final Secuencia 1");
        }

        public override void CancelElementAction()
        {
            base.CancelElementAction();

            LeanTween.cancel(gameObject);
            _currSequence = _maxSequence;

            Debug.Log("Cancelada Secuencia 1");
        }
    }
}