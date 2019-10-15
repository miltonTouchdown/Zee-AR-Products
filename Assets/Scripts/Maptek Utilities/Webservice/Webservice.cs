using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Trophies.Maptek
{
    public class Webservice : MonoBehaviour
    {
        public string URL_API;
        public string url_user_api;
        public string url_exposition_api;

        private static Webservice _instace;
        public static Webservice Instance
        {
            get
            {
                return _instace;
            }
            set
            {
                if (_instace == null)
                {
                    _instace = value;
                }
            }
        }

        void Awake()
        {
            if (_instace == null)
            {
                _instace = this;
            }
            else
            {
                Destroy(this);
            }
        }

        void Start()
        {

        }

        public void registerUser(string email, OnResponseCallback response = null)
        {
            StartCoroutine(Register(email, response));
        }

        IEnumerator Register(string email, OnResponseCallback response = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);

            string url = URL_API + url_user_api + "register.php";

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                string message = "";

                if (www.responseCode < 400)
                    message = "";
                else if (www.responseCode >= 400 && www.responseCode < 500)
                    message = "El usuario ingresado ya existe";
                else
                    message = "Problemas en el servidor. Intente nuevamente";

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);

                    if (response != null)
                        response(false, message);
                }
                else
                {
                    //Debug.Log(www.downloadHandler.text);

                    FillUserData(www.downloadHandler.text);

                    if (response != null)
                        response(true, message);
                }
            }
        }

        public void getUserData(string email, OnResponseCallback response = null)
        {
            StartCoroutine(GetUser(email, response));
        }

        IEnumerator GetUser(string email, OnResponseCallback response = null)
        {
            string uri = URL_API + url_user_api + "getUser.php?email=" + email;

            string message = "";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.responseCode < 400)
                    message = "";
                else if (webRequest.responseCode >= 400 && webRequest.responseCode < 500)
                    message = "El usuario no existe";

                if (webRequest.isNetworkError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                    if (response != null)
                        response(false, message);
                }
                else
                {
                    //Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                    FillUserData(webRequest.downloadHandler.text);

                    if (response != null)
                        response(true, message);
                }
            }
        }

        public void deleteLike(int idExpo, OnResponseCallback response = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("user_id", AppManager.Instance.currUser.id);
            form.AddField("expo_id", idExpo);

            string url = URL_API + url_user_api + "deleteLike.php";

            StartCoroutine(PostLibrary(form, url, response));
        }

        public void addLike(int idExpo, OnResponseCallback response = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("user_id", AppManager.Instance.currUser.id);
            form.AddField("expo_id", idExpo);

            string url = URL_API + url_user_api + "addLike.php";

            StartCoroutine(PostLibrary(form, url, response));
        }

        IEnumerator PostLibrary(WWWForm form, string url, OnResponseCallback response = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                string message = "";

                if (www.responseCode < 400)
                    message = "";
                else if (www.responseCode >= 400 && www.responseCode < 500)
                    message = "El usuario ingresado ya existe";
                else
                    message = "Problemas en el servidor. Intente nuevamente";

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);

                    if (response != null)
                        response(false, message);
                }
                else
                {

                    if (response != null)
                        response(true, message);
                }
            }
        }

        public void getConferenceData(OnResponseCallback response = null)
        {
            StartCoroutine(GetExpositions(response));
        }

        IEnumerator GetExpositions(OnResponseCallback response = null)
        {
            string uri = URL_API + url_exposition_api + "getExpositions.php";

            string message = "";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.responseCode < 400)
                    message = "";
                else if (webRequest.responseCode >= 400 && webRequest.responseCode < 500)
                    message = "Charlas no encontradas";

                if (webRequest.isNetworkError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                    if (response != null)
                        response(false, message);
                }
                else
                {
                    FillConferenceData(webRequest.downloadHandler.text, response);
                }
            }
        }

        public void getTextureExpositor(string url, Action<Texture2D> response = null)
        {
            // TODO id de la charla
            StartCoroutine(GetTexture(url, response));
        }

        IEnumerator GetTexture(string url, Action<Texture2D> response = null)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                    if (response != null)
                        response(null);
                }
                else
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);

                    if (response != null)
                        response(texture);
                }
            }
        }

        /// <summary>
        /// Llenar datos del usuario
        /// </summary>
        private void FillUserData(string data)
        {
            var n = JSON.Parse(data);

            User u = new User();

            List<int> likes = new List<int>();

            for (int i = 0; i < n["message"]["library"].Count; i++)
            {
                likes.Add(n["message"]["library"][i]["id"].AsInt);
            }

            u.id = n["message"]["id"].AsInt;
            u.email = n["message"]["email"].Value;
            u.idLikeExpositions = likes;

            AppManager.Instance.currUser = u;
        }

        private void FillConferenceData(string data, OnResponseCallback response = null)
        {
            var n = JSON.Parse(data);
            //Debug.Log(n);
            List<Exposition> arrExpo = new List<Exposition>();

            for (int i = 0; i < n["message"].Count; i++)
            {
                Exposition e = new Exposition();

                e.id = n["message"][i]["id"].AsInt;
                e.date = DateTime.Parse(n["message"][i]["date"].Value);
                e.name_exposition = n["message"][i]["name_exposition"].Value;
                e.info_exposition = n["message"][i]["info_exposition"].Value;
                e.hour = n["message"][i]["hour"].Value;
                e.room = n["message"][i]["room"].Value;
                e.name_expositor = n["message"][i]["name_expositor"].Value;
                e.url_photo_expositor = n["message"][i]["photo_expositor"].Value;
                e.info_expositor = n["message"][i]["info_expositor"].Value;

                arrExpo.Add(e);
            }

            ConferenceControl.Instance.FillExpositionsInformation(arrExpo);

            if (response != null)
                response(true, "Datos cargado correctamente");
        }

        public delegate void OnResponseCallback(bool status, string message);
        public static event OnResponseCallback onResponse;
    }
}