using UnityEngine;
using UnityEngine.Events;

namespace Trophies.Rappi
{
    public class S_EndGame_Correct : ElementSequence
    {
        [Header("Elements")]
        public GameObject[] containers;
        public GameObject bag;
        public Material matBag;
        public Animator animBag;

        [Header("General")]
        // Si el numero es negativo indica que la siguiente accion se ejecuta (en algun momento) al mismo
        // tiempo que la actual
        public float[] timeToNextAction;

        [Header("Action 1")]
        // Accion 1: Aparece mochila Rappi
        public float scaleBag = 1f;
        public float timeShowBag = .3f;
        public AudioClip clipShowBag;
        public UnityEvent OnFinishAction1;

        [Header("Action 2")]
        // Accion 2: Se elevan los contenedores
        public float maxheightContainers = 2f;
        public float timeMovContainer = .3f;

        [Header("Action 3")]
        // Accion 3: Mochila se posiciona al centro
        public float timetoPosBag;
        // Posicion final de la mochila en la que se muestra
        // el premio.
        public Transform pointPosBag;

        [Header("Action 4")]
        // Accion 4: Desaparecer contenedores y abrir mochila
        public float timeHideContainer = .5f;

        [Header("Action 5")]
        // Accion 5: Aparece premio girando desde el interior de la mochila
        public float timeShowPrize = .5f;
        public Transform posPrize;
        public Transform posBagPrize;
        public UnityEvent OnInitAction5;

        // Animaciones premio
        public float velRotationPrize = .8f;
        public float velRotationCoin = 10f;
        public float velCoin = .5f;
        public float timeSpawnCoin = .2f;

        //----------------------------------------
        private GameObject _prize = null;
        private Container _correctContainer = null;

        private int _maxSequence = 5;
        private int _currSequence = 0;

        protected override void Start()
        {
            base.Start();
        }

        public override void StartElementAction(SequenceControl.OnFinishElementActionCallback onFinish = null)
        {
            base.StartElementAction(onFinish);

            Debug.Log("Inicio Secuencia 2");

            // No ejecutar si el jugador ha perdido la partida
            if (!(bool)GameManager.Instance.IsPlayerWinner)
            {
                FinishElementAction();
                return;
            }

            animBag.SetFloat("direction", -1f);
            animBag.Play("openningBag", 0, 0f);
            matBag.SetFloat("_Transparency", 0f);
            bag.transform.localScale = Vector3.zero;

            _prize = null;

            _currSequence = -1;

            for (int i = 0; i < containers.Length; i++)
            {
                Container c = containers[i].GetComponent<Container>();

                if (c.HasPrize)
                {
                    _correctContainer = c;
                    break;
                }
            }

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
                        // Aparece mochila Rappi

                        // Obtener posicion en donde aparece la mochila. La posicion se encuentra en el contenedor 
                        // correcto.
                        Transform initPoint = _correctContainer.GetPositionPrize();

                        bag.transform.position = initPoint.position;

                        PlaySound(clipShowBag);

                        LeanTween.move(bag,
                                new Vector3(bag.transform.position.x, pointPosBag.position.y, bag.transform.position.z),
                                timeMovContainer).setEase(LeanTweenType.easeOutBounce).setOnComplete(() =>
                                {
                                    OnFinishAction1.Invoke();

                                    LeanTween.delayedCall(timeToNextAction[_currSequence], () => { initNextSequence(); });
                                });

                        LeanTween.scale(bag, Vector3.one * 1.1f * scaleBag, timeShowBag / 2).setOnComplete(() =>
                          {
                              LeanTween.scale(bag, Vector3.one * scaleBag, .1f);
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
                case 2:
                    {
                        // Mochila se posiciona al centro

                        float timeNextMove = timeToNextAction[_currSequence];

                        // Ejecutar la siguiente accion antes de finalizar la actual
                        if (timeNextMove < 0)
                            LeanTween.delayedCall(timetoPosBag + timeToNextAction[_currSequence], () => { initNextSequence(); });

                        LeanTween.move(bag, pointPosBag.position, timetoPosBag).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
                        {
                            if (timeNextMove >= 0)
                                LeanTween.delayedCall(timeToNextAction[_currSequence], () => { initNextSequence(); });
                        });

                        break;
                    }
                case 3:
                    {
                        // Desaparecer contenedores y abrir mochila

                        int count = containers.Length;

                        // Achicar contenedores
                        for (int i = 0; i < containers.Length; i++)
                        {
                            LeanTween.scale(containers[i], Vector3.zero, timeHideContainer).setEase(LeanTweenType.easeOutSine);
                        }

                        // Abrir mochila
                        animBag.SetFloat("direction", 1f);
                        animBag.Play("openningBag", 0, 0);

                        // Obtener largo de la animacion
                        AnimatorClipInfo[] m_CurrentClipInfo = animBag.GetCurrentAnimatorClipInfo(0);
                        float lenghtAnim = m_CurrentClipInfo[0].clip.length;

                        // Terminar accion
                        LeanTween.delayedCall(timeToNextAction[_currSequence] + lenghtAnim, () => { initNextSequence(); });

                        break;
                    }
                case 4:
                    {
                        // Aparece premio girando desde el interior de la mochila

                        OnInitAction5.Invoke();

                        MusicControl mc = FindObjectOfType<MusicControl>();
                        mc.SetMusicVolume(.35f);
                        PlaySound(mc.clipVictory);
                        // premio

                        // Crear premio y ubicarlo en el punto de inicio de la mochila
                        _prize = _correctContainer.CreatePrize(posBagPrize);

                        // Activar animacion de rotacion premio
                        Prize p = _prize.GetComponent<Prize>();
                        p.CreateElementsPrize(() =>
                        {
                            p.StartRotation(velRotationPrize, Vector3.up);
                        });


                        LeanTween.move(_prize,
                                posPrize.position,
                                timeShowPrize).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
                                {
                                    p.StartAnimCoin(velCoin, timeSpawnCoin, velRotationCoin, Vector3.up);
                                //LeanTween.delayedCall(timeToNextAction[_currSequence], () => { FinishElementAction(); });
                                FinishElementAction();
                                });

                        LeanTween.scale(_prize, Vector3.one, timeShowPrize / 1.5f);

                        break;
                    }
                    //case 5:
                    //    {
                    //        // Activar animaciones monedas

                    //        Prize p = _prize.GetComponent<Prize>();
                    //        p.StartAnimation(velRotationPrize, velCoin, timeSpawnCoin);

                    //        FinishElementAction();
                    //        break;
                    //    }
            }
        }

        public override void FinishElementAction()
        {
            base.FinishElementAction();

            Debug.Log("Final Secuencia 2");
        }
    }
}