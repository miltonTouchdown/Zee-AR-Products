using UnityEngine;
using UnityEngine.Networking;

namespace Trophies.Maptek
{
    public class SenderEmail : MonoBehaviour
    {
        public void SendEmail()
        {
            string email = "gamer.metalero@gmail.com";
            string subject = MyEscapeURL("wena xoro");
            string body = MyEscapeURL("My Body is ready\r\nQue pasa xorizo del puerto");

            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        public static void SendEmail(string emailto, string subjectto, string bodyto)
        {
            string subject = UnityWebRequest.EscapeURL(subjectto).Replace("+", "%20");
            string body = UnityWebRequest.EscapeURL(bodyto).Replace("+", "%20");

            Application.OpenURL("mailto:" + emailto + "?subject=" + subject + "&body=" + body);
        }

        string MyEscapeURL(string URL)
        {
            return UnityWebRequest.EscapeURL(URL).Replace("+", "%20");
        }
    }
}