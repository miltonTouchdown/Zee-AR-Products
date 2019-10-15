using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Trophies.Maptek
{
    public class UIRegister : MonoBehaviour
    {
        public TextMeshProUGUI feedback;
        public InputField inputField;

        private SessionRegister _sessionRegister;
        private PopUp _popUp;

        void Start()
        {
            _sessionRegister = FindObjectOfType<SessionRegister>();
            _popUp = FindObjectOfType<PopUp>();
        }

        public void OnRegister()
        {
            string email = inputField.text;

            // Revisar formato de email
            bool isMailValid = EmailValidator.validateEmail(email);

            if (!isMailValid)
            {
                feedback.text = "Correo con formato incorrecto";

                return;
            }

            // Activar pantalla de carga "registrando"

            // registrar
            _sessionRegister.RegisterUser(email, (s, m) =>
            {
            // Desactivar pantalla de carga "registrando"

            if (!s)
                {
                // Mostrar feedback al usuario
                feedback.text = m;
                }
            });
        }
    }
}