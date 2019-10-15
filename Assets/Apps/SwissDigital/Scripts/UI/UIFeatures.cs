using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trophies.SwissDigital
{
    public class UIFeatures : MonoBehaviour
    {
        public WindowMovement information;
        public WindowMovement sideMenu;
        public WindowMovement title;
        public WindowMovement modificationTexture;
        public WindowMovement selectorTexture;

        public RectTransform posSelectedModification;

        // El boton fake es permite observar el boton final en su totalidad
        public GameObject prefButtonSelector, prefFakeButtonSelector;

        public enum TypeFeature { Information, Modification }
        public Feature[] features;

        public bool IsActive = false;

        [System.Serializable]
        public class Feature
        {
            public string title;
            public int id;
            public string information;
            public int idFocusFeature;
            public bool isActive;
            public TypeFeature type;
            public TextureInformation[] texturesInformation;
        }

        [System.Serializable]
        public class TextureInformation
        {
            public string name;
            // id del objeto "Zone external bag" al cual se le cambia la textura
            public int id;
            public bool isActive;
            public Texture[] textures;
        }

        private ShaderBagControl m_shaderControl;
        private Feature m_currFeature = null;

        void Start()
        {
            m_shaderControl = FindObjectOfType<ShaderBagControl>();
        }

        public void ShowActiveMenu()
        {
            // TODO Mostrar menu activo al detectar el AR
        }

        public void ShowMenu()
        {
            IsActive = true;

            if (m_currFeature != null)
            {
                RestoreMenu();

                return;
            }

            // Cambiar textura boton modificadores
            UIButtonModification[] bttnsModifications = modificationTexture.transform.GetComponentsInChildren<UIButtonModification>();

            for (int i = 0; i < bttnsModifications.Length; i++)
            {
                // TODO Cambiar texto
                bttnsModifications[i].SetTexture(m_shaderControl.GetTextureZoneBag(bttnsModifications[i].idZoneModification));
            }

            m_shaderControl.setModeView(ModeBagView.Normal);

            information.setActiveWindow(false);
            title.setActiveWindow(false);
            modificationTexture.setActiveWindow(false);
            selectorTexture.setActiveWindow(false);

            LeanTween.delayedCall(.2f, () => { sideMenu.setActiveWindow(true); });
        }

        private void RestoreMenu()
        {
            ShowFeature(m_currFeature.id);

            if (m_currFeature.type == TypeFeature.Modification)
            {
                var ti = m_currFeature.texturesInformation.SingleOrDefault((t) => t.isActive == true);

                if (ti == null)
                {
                    return;
                }

                ShowSelector(ti.id);
            }
        }

        public void HideMenu()
        {
            sideMenu.setActiveWindow(false);
        }

        public void ShowFeature(int id)
        {
            var feat = features.FirstOrDefault((f) => f.id == id);

            if (feat == null)
            {
                ShowMenu();
                return;
            }

            m_currFeature = feat;

            // Esconder side menu
            HideMenu();

            // al finalizar movimiento boton, mostrar menu features
            title.GetComponentInChildren<Text>().text = feat.title;
            title.setActiveWindow(true);

            feat.isActive = true;

            if (feat.type == TypeFeature.Information)
            {
                // Informacion

                information.GetComponentInChildren<TextMeshProUGUI>().text = feat.information;

                information.setActiveWindow(true);

                // Cambiar de shader a focus
                // set focus
                m_shaderControl.setModeView(ModeBagView.Focus);
                m_shaderControl.setFocus(feat.idFocusFeature);
            }
            else
            {
                // Modificar texturas

                modificationTexture.setActiveWindow(true);

                // Cambiar textura de los botones de acuerdo al modelo
                // Mostrar texto de boton

            }
        }

        public void ShowSelector(int id)
        {
            // Mover botones modificadores a un costado
            LeanTween.moveLocal(modificationTexture.gameObject, posSelectedModification.localPosition, modificationTexture.time);

            // TODO fade out del texto 
            UIButtonModification[] bttnsModifications = modificationTexture.transform.GetComponentsInChildren<UIButtonModification>();

            for (int i = 0; i < bttnsModifications.Length; i++)
            {
                bttnsModifications[i].FadeText(true);
                bttnsModifications[i].GetButton().interactable = false;
            }

            Feature feat = features.FirstOrDefault((f) => f.isActive == true);

            // Obtener contenedor de los botones de texturas
            Transform listContent = selectorTexture.transform.GetChild(0);

            // eliminar hijos
            for (int i = 0; i < listContent.childCount; i++)
            {
                Destroy(listContent.GetChild(i).gameObject);
            }

            // Obtener textura e informacion para utilizarlas en el cambio de texturas del objeto
            TextureInformation ti = feat.texturesInformation.Single((tinfo) => tinfo.id == id);

            ti.isActive = true;

            foreach (Texture t in ti.textures)
            {
                // crear botones
                // agregar listener para cambiar texturas

                Button bttn = Instantiate(prefButtonSelector, listContent).GetComponent<Button>();

                bttn.onClick.AddListener(() => { m_shaderControl.setTexture(ti.id, (Texture2D)t); });

                // cambiar textura del boton
                bttn.transform.GetComponentInChildren<RawImage>().texture = t;
            }

            // Crear boton fake para poder visualizar el ultimo boton
            // este problema ocurre debido a los diferentes aspect ratios
            if (ti.textures.Length > 2)
            {
                Instantiate(prefFakeButtonSelector, listContent);
            }

            selectorTexture.setActiveWindow(true);
        }

        public void HideSelector()
        {
            // desactivar textura activada
            m_currFeature.texturesInformation.Single((ti) => ti.isActive == true).isActive = false;

            UIButtonModification[] bttnsModifications = modificationTexture.transform.GetComponentsInChildren<UIButtonModification>();

            for (int i = 0; i < bttnsModifications.Length; i++)
            {
                bttnsModifications[i].FadeText(false);
                bttnsModifications[i].SetTexture(m_shaderControl.GetTextureZoneBag(bttnsModifications[i].idZoneModification));
                bttnsModifications[i].GetButton().interactable = true;
            }

            selectorTexture.setActiveWindow(false);

            modificationTexture.setActiveWindow(true);
        }

        public void Back()
        {
            if (m_currFeature == null)
                return;

            if (selectorTexture.isActive)
            {
                HideSelector();
            }
            else
            {
                m_currFeature = null;

                // desactivar caracteristicas activas
                for (int i = 0; i < features.Length; i++)
                {
                    features[i].isActive = false;

                    if (features[i].type == TypeFeature.Modification)
                    {
                        for (int x = 0; x < features[i].texturesInformation.Length; x++)
                        {
                            features[i].texturesInformation[x].isActive = false;
                        }
                    }
                }

                ShowMenu();
            }
        }

        public void HideAll()
        {
            IsActive = false;

            // Esconder todos los menus
            information.setActiveWindow(false);
            sideMenu.setActiveWindow(false);
            title.setActiveWindow(false);
            modificationTexture.setActiveWindow(false);
            selectorTexture.setActiveWindow(false);
        }
    }
}