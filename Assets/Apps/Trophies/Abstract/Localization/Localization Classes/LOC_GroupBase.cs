using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abstract.Localization
{
    [SerializeField]
    public class LOC_GroupBase
    {
        public SystemLanguage language;

        public Dictionary<string, string> TextKeys = new Dictionary<string, string>();
        public Dictionary<string, Sprite> SpriteKeys = new Dictionary<string, Sprite>();

        public virtual void Setup()
        {

        }

        protected void AddKey(string keyName, string keyData)
        {

            if (!TextKeys.ContainsKey(keyName))
            {
                TextKeys.Add(keyName, keyData);
            }
            else
            {
                Debug.Log("Key on dictionary "+keyName + "|" + keyData);
            }
        }

        protected void AddKey(string keyName, Sprite keyData)
        {
            SpriteKeys.Add(keyName, keyData);
        }


    }
}
