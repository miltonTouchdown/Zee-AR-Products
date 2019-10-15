using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abstract.Localization {
    [Serializable]
    public class LOC_GroupMenu : LOC_GroupBase
    {
        public string PubSelectionText;
        public string PubSelectionWindowHeaderText;
        public string CameraButtonARText;
        public string PrizeButtonText;
        public string ContestButtonText;
        public string BottomInstructionText;
        public string BubbleTrophiesLogoText;

        public override void Setup()
        {
            Debug.Log("GroupMenu");
            AddKey("PubSelectionText", PubSelectionText);
            AddKey("PubSelectionWindowHeaderText", PubSelectionWindowHeaderText);

            AddKey("CameraButtonARText", CameraButtonARText);
            AddKey("PrizeButtonText", PrizeButtonText);
            AddKey("ContestButtonText", ContestButtonText);

            AddKey("BottomInstructionText", BottomInstructionText);
            AddKey("BubbleTrophiesLogoText", BubbleTrophiesLogoText);

        }
    }
}

