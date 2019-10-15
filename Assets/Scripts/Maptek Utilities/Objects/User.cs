using System.Collections.Generic;

namespace Trophies.Maptek
{
    [System.Serializable]
    public class User
    {
        public int id;
        public string email;
        public List<int> idLikeExpositions; // Los "me gusta" guardados del usuario
    }
}