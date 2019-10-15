using UnityEngine;

namespace Trophies.Rappi
{
    public class S_EndGame_Error : ElementSequence
    {
        [Header("Elements")]
        public GameObject[] containers;
        public GameObject bag;
        public Material matBag;
        public Animator animBag;

        [Header("General")]
        public float[] timeToNextAction;

        [Header("Action 1")]
        // Accion 2: Se elevan los contenedores
        public float maxheightContainers = 2f;
        public float timeMovContainer = .3f;
        public AudioClip clipFailContainer;

        [Header("Action 2")]
        // Accion 1: Aparece mochila Rappi
        public float scaleBag = 1f;
        public float timeShowBag = .3f;
        // Posicion final de la mochila en la que se muestra
        // el premio.
        public Transform pointPosBag;
        public AudioClip clipShowBag;

        //----------------------------------------

        private int _maxSequence = 2;
        private int _currSequence = 0;

        protected override void Start()
        {
            base.Start();
        }

        public override void StartElementAction(SequenceControl.OnFinishElementActionCallback onFinish = null)
        {
            base.StartElementAction(onFinish);

            Debug.Log("Inicio Secuencia 3");

            // No ejecutar si el jugador ha ganado la partida
            if ((bool)GameManager.Instance.IsPlayerWinner)
            {
                FinishElementAction();
                return;
            }

            animBag.SetFloat("direction", -1f);
            animBag.Play("openningBag", 0, 0f);
            matBag.SetFloat("_Transparency", 0f);
            bag.transform.localScale = Vector3.zero;

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
                        //Se elevan los contenedores
                        PlaySound(clipFailContainer);

                        int count = containers.Length;

                        for (int i = 0; i < containers.Length; i++)
                        {
                            LeanTween.moveLocal(containers[i],
                                new Vector3(containers[i].transform.localPosition.x, maxheightContainers, containers[i].transform.localPosition.z),
                                timeMovContainer).setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
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
                case 1:
                    {
                        // Aparece mochila Rappi

                        Container c = null;

                        for (int i = 0; i < containers.Length; i++)
                        {
                            if (containers[i].GetComponent<Container>().HasPrize)
                            {
                                c = containers[i].GetComponent<Container>();
                                break;
                            }
                        }

                        Transform initPoint = c.GetPositionPrize();

                        bag.transform.position = initPoint.position;

                        PlaySound(clipShowBag);

                        LeanTween.move(bag,
                             new Vector3(bag.transform.position.x, pointPosBag.position.y, bag.transform.position.z),
                             timeMovContainer).setEase(LeanTweenType.easeOutBounce).setOnComplete(() =>
                             {
                                 LeanTween.delayedCall(timeToNextAction[_currSequence], () =>
                                 {
                                     MusicControl mc = FindObjectOfType<MusicControl>();
                                     mc.SetMusicVolume(.35f);
                                     PlaySound(mc.clipDefeat);

                                     FinishElementAction();
                                 });
                             });

                        LeanTween.scale(bag, Vector3.one * 1.1f * scaleBag, timeShowBag / 2).setOnComplete(() =>
                        {
                            LeanTween.scale(bag, Vector3.one * scaleBag, .1f);
                        });

                        break;
                    }
            }
        }

        public override void FinishElementAction()
        {
            base.FinishElementAction();

            Debug.Log("Final Secuencia 3");
        }
    }
}