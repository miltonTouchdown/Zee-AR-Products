using System;
using UnityEngine;

namespace Trophies.Maptek
{
    [System.Serializable]
    public class Exposition
    {
        public int id;
        [SerializeField]
        public DateTime date; // fecha y hora de la charla. Formato ejemplo "2019-07-27 04:35:52"
        public string name_exposition;
        public string info_exposition;
        public string hour; // Horario de la charla. Formato ejemplo "08:00 am - 10:00 am"
        public string room;
        public string name_expositor;
        public Texture photo_expositor = null;
        public string url_photo_expositor;
        public string info_expositor;

        public bool isLiked = false;
        public bool isOpen = false;
    }
}