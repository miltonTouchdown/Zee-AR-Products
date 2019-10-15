using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LOC_Localizable_TextMeshPro : MonoBehaviour {
    public string localizationGroup;
    public string localizationTag;

    public TextMeshPro textmeshProRef;

	// Use this for initialization
	void Start () {
        string localizedText = LOC_Manager.GetText(localizationGroup, localizationTag);
        if (localizedText.CompareTo("") != 0)
        {
            textmeshProRef.text = localizedText;
        }
        

    }
}
