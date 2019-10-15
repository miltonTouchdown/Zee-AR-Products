using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abstract.Localization
{
    [Serializable]
    public class LOC_GroupMarkers : LOC_GroupBase
    {
        public MexicoGuatemalaBigDisplay mexicoGuatemalaBigDisplay;
        public MexicoGuatemalaTinyDisplay mexicoGuatemalaTinyDisplay;


        public override void Setup()
        {
            AddKey("MexicoGuatemalaBigDisplay/SocialCauseDescription", mexicoGuatemalaBigDisplay.SocialCauseDescription);
            AddKey("MexicoGuatemalaBigDisplay/Title", mexicoGuatemalaBigDisplay.Title);
            AddKey("MexicoGuatemalaBigDisplay/DonationGoalText", mexicoGuatemalaBigDisplay.DonationGoalText);
            AddKey("MexicoGuatemalaBigDisplay/DonationGoalNumber", mexicoGuatemalaBigDisplay.DonationGoalNumber);
            AddKey("MexicoGuatemalaBigDisplay/MoreInfo", mexicoGuatemalaBigDisplay.MoreInfo);

            AddKey("MexicoGuatemalaTinyDisplay/SocialCauseDescription1", mexicoGuatemalaTinyDisplay.SocialCauseDescription1);
            AddKey("MexicoGuatemalaTinyDisplay/SocialCauseDescription2", mexicoGuatemalaTinyDisplay.SocialCauseDescription2);
            AddKey("MexicoGuatemalaTinyDisplay/Title", mexicoGuatemalaTinyDisplay.Title);
            AddKey("MexicoGuatemalaTinyDisplay/DonationGoalText", mexicoGuatemalaTinyDisplay.DonationGoalText);
            AddKey("MexicoGuatemalaTinyDisplay/DonationGoalNumber", mexicoGuatemalaTinyDisplay.DonationGoalNumber);
            AddKey("MexicoGuatemalaTinyDisplay/MoreInfo", mexicoGuatemalaTinyDisplay.MoreInfo);

        }


        [Serializable]
        public class MexicoGuatemalaBigDisplay
        {
            [TextArea]
            public string SocialCauseDescription;

            [TextArea]
            public string Title;

            [TextArea]
            public string DonationGoalText;

            [TextArea]
            public string DonationGoalNumber;

            [TextArea]
            public string MoreInfo;
        }

        [Serializable]
        public class MexicoGuatemalaTinyDisplay
        {
            [TextArea]
            public string SocialCauseDescription1;

            [TextArea]
            public string SocialCauseDescription2;

            [TextArea]
            public string Title;

            [TextArea]
            public string DonationGoalText;

            [TextArea]
            public string DonationGoalNumber;

            [TextArea]
            public string MoreInfo;
        }
    }

}
