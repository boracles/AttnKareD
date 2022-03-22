// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved
using Tobii.G2OM;
using UnityEngine;

namespace Tobii.XR.Examples
{
    public class HighlightAtGaze_Schedule1 : MonoBehaviour, IGazeFocusable
    {
        public Transform MainManager; //ScheduleManager

        public void GazeFocusChanged(bool hasFocus)
        {
/*            if (hasFocus)
            {
                MainManager.GetComponent<ScheduleManager>().OnBoard = false;
            }
            else
            {
                MainManager.GetComponent<ScheduleManager>().OnBoard = true;
            }*/
        }
    }
}
