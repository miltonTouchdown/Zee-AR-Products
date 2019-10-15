using UnityEngine;

namespace Trophies.Rappi
{
    public class Prize : MonoBehaviour
    {
        public const string pathResources = "Prizes/";

        public PrizeType prizeType;

        public float scale = 1f;
        public Transform posCoin;
        private GameObject _coin;
        public Material matCoin;

        private GameObject currPrizeRotate = null;

        void Start()
        {
            prizeType = GameManager.Instance.currPrize;

        }

        public void CreateElementsPrize(OnFinishCallback onFinish = null)
        {
            prizeType = GameManager.Instance.currPrize;

            // Obtener nombre de los modelos a cargar
            DataPrize data = GameManager.Instance.GetDataPrize(prizeType);

            matCoin.SetFloat("Vector1_9AB0C6D0", 1f);

            foreach (string fileName in data.nameFile)
            {
                // Cargar modelos desde resources
                GameObject p = Instantiate(Resources.Load<GameObject>(pathResources + fileName));
                p.name = data.namePrize;

                if (fileName != "Coin")
                {
                    currPrizeRotate = p;
                    p.transform.SetParent(this.transform);
                }
                else
                {
                    p.transform.SetParent(posCoin);
                    _coin = p;
                }

                p.transform.localPosition = Vector3.zero;
                p.transform.localScale = new Vector3(scale, scale, scale);
            }

            if (onFinish != null)
                onFinish();
        }

        public void StartAnimCoin(float velCoin, float timeSpawnCoin, float velRotationCoin, Vector3 vCoinRotation)
        {
            if (posCoin.childCount <= 0)
                return;

            matCoin.SetFloat("Vector1_9AB0C6D0", 1f);
            _coin.transform.localPosition = Vector3.zero;

            _coin.GetComponent<RotationMovement>().StartRotation(velRotationCoin, vCoinRotation);

            Vector3 firstPoint = new Vector3(_coin.transform.localPosition.x, _coin.transform.localPosition.y + 2f, _coin.transform.localPosition.z);
            Vector3 lastPoint = new Vector3(firstPoint.x, firstPoint.y - .6f, firstPoint.z);

            // Escalar y fade al aparecer moneda
            LeanTween.scale(_coin, Vector3.one, velCoin / 4).
                        setOnUpdate((float f) =>
                        {
                            matCoin.SetFloat("Vector1_9AB0C6D0", 1 - f);
                        });

            // Movimiento hacia arriba
            LeanTween.moveLocal(_coin, firstPoint, velCoin).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
            {
            // Espera para el siguiente movimiento
            LeanTween.delayedCall(.2f, () =>
                {
                // Movimiento hacia abajo
                LeanTween.moveLocal(_coin, lastPoint, velCoin / 3).setOnComplete(() =>
                    {
                        matCoin.SetFloat("Vector1_9AB0C6D0", 1f);
                        LeanTween.delayedCall(timeSpawnCoin, () => { StartAnimCoin(velCoin, timeSpawnCoin, velRotationCoin, vCoinRotation); });
                    }).setOnUpdate((float f) =>
                    {
                        matCoin.SetFloat("Vector1_9AB0C6D0", f);
                    }); ;
                });
            });
        }

        public void StopAllAnimation()
        {
            StopRotation();

            // Animacion moneda
            if (posCoin.childCount > 0)
            {
                LeanTween.cancelAll();
            }
        }

        public void StartRotation(float vel, Vector3 vDir)
        {
            currPrizeRotate.GetComponent<RotationMovement>().StartRotation(vel, vDir);
        }

        public void StopRotation()
        {
            currPrizeRotate.GetComponent<RotationMovement>().StopRotation();
        }

        public delegate void OnFinishCallback();
        public static event OnFinishCallback onFinishCallback;
    }

    public enum PrizeType
    {
        None,
        Gift_Box,
        Credits     // Billete
    }
}