using System.Linq;
using UnityEngine;

namespace Trophies.Rappi
{
    [CreateAssetMenu(fileName = "GameDataConfig")]
    public class GameConfigData : ScriptableObject
    {
        // Obtener/Configurar dificultades

        [System.Serializable]
        public struct level
        {
            public LevelType levelType;
            public float valueLvl;
        }

        [System.Serializable]
        public struct feedback
        {
            public bool IsWinner;
            public string Message;
        }

        [Header("Niveles del juego")]
        // Niveles disponibles
        public level[] levelsGame;

        [Header("Velocidades de los movimientos")]
        // Velocidades de cada movimiento
        public float[] velSingleShuffle;

        [Header("references Prize Resources")]
        // Premios disponibles
        public DataPrize[] dataPrize;

        [Header("Message feedback on finish game")]
        public feedback[] feedbackGameOver;

        /// <summary>
        /// Obtener valor multiplicador de un nivel.
        /// </summary>
        /// <param name="lvlType">Enum del nivel</param>
        /// <returns>Retorna flotante multiplicador del nivel</returns>
        public float GetValueLevel(LevelType lvlType)
        {
            return levelsGame.Single((l) => l.levelType == lvlType).valueLvl;
        }

        public DataPrize GetDataPrize(PrizeType prize)
        {
            return dataPrize.Single((p) => p.prizeType == prize);
        }

        public DataPrize[] GetAllDataPrizes()
        {
            return dataPrize;
        }

        public float[] GetVelocityShuffle()
        {
            return velSingleShuffle;
        }

        public string GetMessageGameOver(bool value)
        {
            return feedbackGameOver.Single((f) => f.IsWinner == value).Message;
        }
    }

    [System.Serializable]
    public struct DataPrize
    {
        public string namePrize;
        public string[] nameFile;
        public Sprite iconPrize;
        public PrizeType prizeType;
    }

    public enum LevelType
    {
        Low,
        Medium,
        Hard
    }
}