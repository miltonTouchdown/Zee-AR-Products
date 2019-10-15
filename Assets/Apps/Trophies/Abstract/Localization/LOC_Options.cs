using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Abstract.Localization {

    [CreateAssetMenu(fileName = "Localization Options", menuName = "Abstract/Localization/Localization Options SO", order = 1)]
    [Serializable]
    public class LOC_Options : ScriptableObject
    {
        public Dictionary<string, LOC_GroupBase[]> GroupDictionary = new Dictionary<string, LOC_GroupBase[]>();

        public SystemLanguage[] SuportedLangs;
        public SystemLanguage BaseLang;

        public string keyGroupLocation_Menu;
        public LOC_GroupMenu[] GroupLocation_Menu;

        public string keyGroupLocation_Contests;
        public LOC_GroupContests[] GroupLocation_Contests;

        public string keyGroupLocation_Markers;
        public LOC_GroupMarkers[] GroupLocation_Markers;

        public void Setup()
        {
            GroupDictionary.Add(keyGroupLocation_Menu, GroupLocation_Menu);
            GroupDictionary.Add(keyGroupLocation_Contests, GroupLocation_Contests);
            GroupDictionary.Add(keyGroupLocation_Markers, GroupLocation_Markers);
        }
    }
}

