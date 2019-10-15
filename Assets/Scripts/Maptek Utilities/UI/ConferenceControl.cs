using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trophies.Maptek
{
    public class ConferenceControl : MonoBehaviour
    {
        public bool isLoadConference = false;

        // Informacion de exposiciones
        public List<Exposition> arrExposition;

        public Exposition currExposition = null;

        private static ConferenceControl _instace;
        public static ConferenceControl Instance
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
            // Obtener Likes de usuario

        }

        /// <summary>
        /// Llenar arreglo de exposiciones
        /// </summary>
        public void FillExpositionsInformation(List<Exposition> expositions)
        {
            // LLenar arreglo obtenido desde webservice
            arrExposition = expositions;

            isLoadConference = true;
        }

        /// <summary>
        /// Obtener charlas de un dia en especifico
        /// </summary>
        /// <param name="day">entero que indica dia de la charla</param>
        /// <returns>Retorna un arreglo ordena por fechas de las charlas del dia especifico</returns>
        public Exposition[] GetExpositionsByDay(int day)
        {
            List<Exposition> e = arrExposition.Where((exp) => (int)exp.date.Day == day).ToList();

            return e.OrderByDescending((d) => d.date).Reverse().ToArray();
        }

        /// <summary>
        /// Cambiar likes en las charlas
        /// </summary>
        /// <param name="indexLikes">Arreglo de id de las charlas con likes</param>
        public void SetLikesExposition(List<int> indexLikes)
        {
            for (int i = 0; i < arrExposition.Count; i++)
            {
                arrExposition[i].isLiked = indexLikes.Any((id) => id == arrExposition[i].id);
            }
        }

        public void setLikeExposition(Webservice.OnResponseCallback response = null)
        {
            Exposition expo = arrExposition.Single((ex) => ex.id == currExposition.id);

            User u = new User();
            u.id = AppManager.Instance.currUser.id;
            u.email = AppManager.Instance.currUser.email;
            u.idLikeExpositions = AppManager.Instance.currUser.idLikeExpositions;

            bool exists = u.idLikeExpositions.Any((l) => l == expo.id);

            if (exists)
            {
                // Eliminar like
                u.idLikeExpositions.Remove(expo.id);

                Webservice.Instance.deleteLike(expo.id, (s, m) =>
                {
                    if (s)
                    {
                        expo.isLiked = !exists;

                        AppManager.Instance.currUser = u;
                    }
                    else
                    {
                    // Problemas con el servidor
                }

                    if (response != null)
                        response(s, m);
                });
            }
            else
            {
                // Agregar like
                u.idLikeExpositions.Add(expo.id);

                Webservice.Instance.addLike(expo.id, (s, m) =>
                {
                    if (s)
                    {
                        expo.isLiked = !exists;

                        AppManager.Instance.currUser = u;
                    }
                    else
                    {
                    // Problemas con el servidor
                }

                    if (response != null)
                        response(s, m);
                });
            }
        }

        public void GetTextureExpositor(Action<Texture2D> action)
        {
            Webservice.Instance.getTextureExpositor(currExposition.url_photo_expositor, action);
        }
    }
}