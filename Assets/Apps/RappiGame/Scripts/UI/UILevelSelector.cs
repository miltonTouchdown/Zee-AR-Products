using UnityEngine;
using UnityEngine.UI;

namespace Trophies.Rappi
{
    public class UILevelSelector : MonoBehaviour
    {
        // Indican el nivel. La primera posicion indica menor nivel. Ultima posicion indica mayor nivel
        public Image[] lvlIndicators;
        public Color colorLevel = Color.yellow;
        public Sprite starEnabled, starDisabled;

        // Indica se utilizara imagen o color para resaltar la seleccion de nivel
        public bool useColor = false;

        void Start()
        {
            ChangeColorIndicators((int)GameManager.Instance.currDifficulty);
        }

        /// <summary>
        /// Cambiar nivel del juego. El nivel siempre va en aumento.
        /// Al llegar al nivel maximo se reinicia.
        /// </summary>
        public void SetLevel()
        {
            int maxLevel = LevelType.GetNames(typeof(LevelType)).Length;

            int nextLevel = (int)GameManager.Instance.currDifficulty + 1;

            if (nextLevel >= maxLevel)
            {
                nextLevel = 0;
            }

            GameManager.Instance.currDifficulty = (LevelType)nextLevel;

            ChangeColorIndicators(nextLevel);
        }

        public void SetLevel(int id)
        {
            GameManager.Instance.currDifficulty = (LevelType)id;

            ChangeColorIndicators(id);
        }

        private void ChangeColorIndicators(int lvl)
        {
            for (int i = 0; i < lvlIndicators.Length; i++)
            {
                if (useColor)
                    lvlIndicators[i].color = (i <= lvl) ? colorLevel : Color.white;
                else
                    lvlIndicators[i].overrideSprite = (i <= lvl) ? starEnabled : starDisabled;
            }
        }
    }
}