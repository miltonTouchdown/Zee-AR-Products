using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trophies.Maptek
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("General")]
        public Color colorMaptek;

        [Header("Main Menu")]
        public GameObject prefCharlaGloblInfo;
        public Transform contentInfo;
        public TextMeshProUGUI title;

        public Button bttnDayOne;
        public Button bttnDayTwo;
        public Button bttnDayThree;

        [Header("Charla")]
        public WindowMovement viewCharla;
        public ScrollRect scrollRect;
        public TextMeshProUGUI titleCharla;
        public TextMeshProUGUI aboutCharla;

        public Button bttnLike;
        public Image imgLike;

        public Button bttnAR;

        [Header("Expositor")]
        public WindowMovement viewExpositor;

        public TextMeshProUGUI aboutExpositor;
        public RawImage rawimgExpositor;
        public Texture2D profileDefault;

        [Header("Email Notification")]
        public WindowMovement viewEmailNotification;

        private PopUp _popUp;

        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Back();
            }
        }

        public void initMainMenu()
        {
            _popUp = FindObjectOfType<PopUp>();

            // Cambiar color del boton del dia y mostrar charlas

            int today = DateTime.Now.Day;

            Button bttnToday = null;

            bttnDayOne.transform.Find("bar").GetComponent<Image>().color = colorMaptek;
            bttnDayTwo.transform.Find("bar").GetComponent<Image>().color = colorMaptek;
            bttnDayThree.transform.Find("bar").GetComponent<Image>().color = colorMaptek;

            if (today <= 20)
            {
                bttnToday = bttnDayOne;
            }
            if (today == 21)
            {
                bttnToday = bttnDayTwo;
            }
            if (today >= 22)
            {
                bttnToday = bttnDayThree;
            }

            bttnToday.transform.Find("bar").GetComponent<Image>().color = Color.white;
            bttnToday.GetComponent<Image>().color = colorMaptek;
            bttnToday.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

            // Cargar charlas del dia
            if (ConferenceControl.Instance.currExposition.isOpen)
            {
                Exposition e = ConferenceControl.Instance.currExposition;
                Button b = null;

                if (e.date.Day <= 20)
                    b = bttnDayOne; ;

                if (e.date.Day == 21)
                    b = bttnDayOne;

                if (e.date.Day >= 22)
                    b = bttnDayOne;

                b.onClick.Invoke();
                b.Select();
            }
            else
            {
                bttnToday.onClick.Invoke();
                bttnToday.Select();
            }
        }

        public void ShowCharlasByDay(int day)
        {
            Exposition[] expos = ConferenceControl.Instance.GetExpositionsByDay(day);

            // Eliminar informacion que se muestra
            for (int i = contentInfo.childCount - 1; i >= 0; i--)
            {
                Destroy(contentInfo.GetChild(i).gameObject);
            }

            // Crear botones con informacion
            foreach (Exposition e in expos)
            {
                Button bttn = Instantiate(prefCharlaGloblInfo, contentInfo).GetComponent<Button>();

                // Para diferenciar una charla de una que no es verificar si tiene informacion.
                // Si no tiene informacion entonces no es una charla
                if (!string.IsNullOrEmpty(e.info_exposition))
                {
                    // Es una charla

                    // listener a botones para cambiar de vista
                    bttn.onClick.AddListener(() =>
                    {
                        ConferenceControl.Instance.currExposition = e;
                        ConferenceControl.Instance.currExposition.isOpen = true;

                        ShowCharlaInformation();
                    });

                    // Activar imagen flecha
                    bttn.transform.Find("arrow").gameObject.SetActive(true);

                    // Cambiar texto informacion
                    bttn.GetComponentInChildren<TextMeshProUGUI>().text = getFormatStringInfo(e);
                }
                else
                {
                    // No es charla. Solo informativo.

                    bttn.onClick.RemoveAllListeners();

                    bttn.transform.Find("arrow").gameObject.SetActive(false);

                    // Cambiar texto informacion
                    bttn.GetComponentInChildren<TextMeshProUGUI>().text = getFormatStringInfo(e);
                }
            }

            Canvas.ForceUpdateCanvases();

            scrollRect.verticalNormalizedPosition = 1f;
        }

        public void ShowCharlaInformation()
        {
            bool hasAR = AppManager.Instance.HasExpoAR(ConferenceControl.Instance.currExposition.id);

            // Configurar boton AR
            bttnAR.onClick.RemoveAllListeners();
            bttnAR.interactable = hasAR;
            // Cambiar color de icono y letras si esta desactivado
            bttnAR.transform.Find("icon").GetComponent<Image>().color = (hasAR) ? colorMaptek : Color.gray;
            bttnAR.GetComponentInChildren<TextMeshProUGUI>().color = (hasAR) ? colorMaptek : Color.gray;

            if (hasAR)
                bttnAR.onClick.AddListener(() => { AppManager.Instance.LoadSceneAR(); });

            viewCharla.setActiveWindow(true);

            imgLike.color = (ConferenceControl.Instance.currExposition.isLiked) ? colorMaptek : Color.white;

            titleCharla.text = getFormatStringInfo(ConferenceControl.Instance.currExposition);
            aboutCharla.text = ConferenceControl.Instance.currExposition.info_exposition;
        }

        public void setLikeCharla()
        {
            // Guardar color
            Color temp = (ConferenceControl.Instance.currExposition.isLiked) ? colorMaptek : Color.white;

            // Cambiar color inmediatamente. Si existe alguna falla en el servidor se regresa a su color anterior
            imgLike.color = (!ConferenceControl.Instance.currExposition.isLiked) ? colorMaptek : Color.white;

            ConferenceControl.Instance.setLikeExposition((s, m) =>
            {
                if (s)
                {
                    if (ConferenceControl.Instance.currExposition.isLiked)
                    {
                    //imgLike.color = colorMaptek;
                    ShowEmailNotification();
                    }
                    else
                    {
                    //imgLike.color = Color.white;
                    HideEmailNotification();
                    }
                }
                else
                {
                // Problemas del servidor
                _popUp.Show("Ups!", "Hay problemas con el servidor. Intente de nuevo");
                    imgLike.color = temp;
                }
            });
        }

        public void ShowExpositorInformation()
        {
            viewExpositor.setActiveWindow(true);

            aboutExpositor.text = ConferenceControl.Instance.currExposition.info_expositor;

            // Obtener imagen de perfil del expositor
            if (ConferenceControl.Instance.currExposition.photo_expositor != null)
            {
                rawimgExpositor.texture = (Texture2D)ConferenceControl.Instance.currExposition.photo_expositor;
            }
            else
            {
                ConferenceControl.Instance.GetTextureExpositor((t) =>
                {
                    if (t != null)
                    {
                        rawimgExpositor.texture = t;
                    }
                    else
                    {
                        rawimgExpositor.texture = profileDefault;
                    }
                });
            }
        }

        public void Back()
        {
            if (viewExpositor.isActive)
            {
                viewExpositor.setActiveWindow(false);

                return;
            }

            if (viewCharla.isActive)
            {
                viewCharla.setActiveWindow(false);
                HideEmailNotification();

                rawimgExpositor.texture = profileDefault;

                ConferenceControl.Instance.currExposition.isOpen = false;
                ConferenceControl.Instance.currExposition = null;

                return;
            }

            Application.Quit();
        }

        private string getFormatStringInfo(Exposition expo)
        {
            //string textFormat = "<size=80%>@NameExposition\n\n<size=60%>@Hour\n<size=60%><color=#3fcf4e>Room @Room</color>\n<size=100%><b>@NameExpositor";
            // TODO Cambiar formato
            string textFormat = "<line-height=70%><size=90%>@NameExposition\n\n<line-height=50%><size=80%><color=#3fcf4e>@Hour\n<size=60%>Room @Room</color>\n\n<line-height=100%><size=100%><b>@NameExpositor";

            textFormat = textFormat.Replace("@NameExposition", expo.name_exposition);
            textFormat = textFormat.Replace("@Hour", expo.hour);
            textFormat = textFormat.Replace("@Room", expo.room);
            textFormat = textFormat.Replace("@NameExpositor", expo.name_expositor);

            if (String.IsNullOrEmpty(expo.room))
            {
                textFormat = textFormat.Replace("Room", "");
            }

            return textFormat;
        }

        public void ShowEmailNotification()
        {
            viewEmailNotification.setActiveWindow(true);
        }

        public void HideEmailNotification()
        {
            viewEmailNotification.setActiveWindow(false);
        }

        public void OnClickEmailNotification()
        {
            if (!viewEmailNotification.isActive || !ConferenceControl.Instance.currExposition.isOpen)
                return;

            HideEmailNotification();

            AppManager.Instance.SendEmail();
        }
    }
}