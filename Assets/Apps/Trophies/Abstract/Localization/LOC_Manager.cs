using Abstract.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOC_Manager : MonoBehaviour {

    public LOC_Options locOptions;
    static LOC_Manager Instance;

    public SystemLanguage currLang;

    /// <summary>
    /// Dictionaries
    /// </summary>
    Dictionary<string, string> KeysText = new Dictionary<string, string>();
    Dictionary<string, Sprite> KeysSprite = new Dictionary<string, Sprite>();

    private void Awake()
    {
        return;
        if (Instance == null)
        {
            currLang = SystemLanguage.Unknown;
            Instance = this;
            InitialSetupLangs();
            SetCurrentLang();
        }
        else
        {

        }
        
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void InitialSetupLangs()
    {
        locOptions.Setup();
    }

    public void SetCurrentLang()
    {
        currLang = Application.systemLanguage;
        if (currLang == SystemLanguage.Unknown || !IsSupported(currLang))
        {
            currLang = locOptions.BaseLang;
        }
        SetupLang(currLang);
    }

    static bool IsSupported(SystemLanguage langToSetup)
    {
        foreach (SystemLanguage currLang in Instance.locOptions.SuportedLangs)
        {
            if (currLang == langToSetup) return true;
        }

        return false;
    }

    void SetupLang(SystemLanguage langToSetup)
    {
        List<string> errorList = new List<string>();
        Dictionary<string, LOC_GroupBase> langDict = new Dictionary<string, LOC_GroupBase>();
        foreach (KeyValuePair<string,LOC_GroupBase[]> currKey in locOptions.GroupDictionary)
        {
            bool langFound = false;
            foreach (LOC_GroupBase currGroupBase in currKey.Value)
            {
                if (currGroupBase.language == langToSetup)
                {
                    langDict.Add(currKey.Key, currGroupBase);
                    currGroupBase.Setup();
                    langFound = true;
                }
            }
            if (!langFound) errorList.Add("Group "+ currKey.Key + " don't have definition for " + langToSetup.ToString());
        }

        foreach (KeyValuePair<string,LOC_GroupBase> currGroup in langDict)
        {
            //Setup texts
            Dictionary<string, string> TextKeys = currGroup.Value.TextKeys;
            foreach (KeyValuePair<string,string> currKey in TextKeys)
            {
                if (currKey.Value.CompareTo("") == 0)
                {
                    errorList.Add("Key "+ currKey.Key+ " of group "+ currGroup.Key+ "not set");
                }
                else
                {
                    KeysText.Add(currGroup.Key + "/" + currKey.Key, currKey.Value);
                }
            }

            //Setup Sprites
            Dictionary<string, Sprite> SpriteKeys = currGroup.Value.SpriteKeys;
            foreach (KeyValuePair<string, Sprite> currKey in SpriteKeys)
            {
                
                if (currKey.Value == null)
                {
                    errorList.Add("Key " + currKey.Key + " of group " + currGroup.Key + "not set");
                }
                else
                {
                    KeysSprite.Add(currGroup.Key + "/" + currKey.Key, currKey.Value);
                }
            }

            if (errorList.Count > 0)
            {
                foreach (string error in errorList) Debug.Log(error);
            }
        }

    }

    public static string GetText(string Group, string data)
    {
        string key = Group + "/" + data;
        if (Instance.KeysText.ContainsKey(key)) return Instance.KeysText[key];

        Debug.Log("Key " + key + " Not found");
        return "KeyNotFound";
    }

    public static SystemLanguage GetCurrentLang()
    {
        return Instance.currLang;
    }

    

    
}
