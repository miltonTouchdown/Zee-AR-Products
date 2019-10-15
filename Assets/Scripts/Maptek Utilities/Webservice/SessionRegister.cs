using UnityEngine;

namespace Trophies.Maptek
{
    public class SessionRegister : MonoBehaviour
    {
        public bool deletePlayerPref = false;

        void Start()
        {
#if UNITY_EDITOR
            if (deletePlayerPref)
                PlayerPrefs.DeleteAll();
#endif
        }

        public void RegisterUser(string email, Webservice.OnResponseCallback onFinish = null)
        {
            Webservice.Instance.registerUser(email, (r, m) =>
            {
                if (r)
                {
                // Guardar datos en playerpref
                PlayerPrefs.SetString("Email", email);

                // Cambiar de escena
                AppManager.Instance.LoadMainMenu();
                }

                if (onFinish != null)
                    onFinish(r, m);
            });
        }

        public bool LoadUserData()
        {
            // Cargar datos del usuario si es que ya esta registrado
            if (!PlayerPrefs.HasKey("Email"))
                return false;

            string email = "";

            email = PlayerPrefs.GetString("Email");

            Webservice.Instance.getUserData(email, (r, m) =>
            {
                if (r)
                {
                // Cambiar de escena
                AppManager.Instance.LoadMainMenu();
                }
                else
                {
                // TODO carga nuevamente o iniciar offline
            }
            });

            return true;
        }
    }
}