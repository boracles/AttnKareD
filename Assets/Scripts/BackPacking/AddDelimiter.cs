using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class AddDelimiter : MonoBehaviour
{
    public bool IDLE;
    public bool DISTURB;
    public CollectData dataCollect;
    public void addIDLE(bool check)
    {
        IDLE = check;
        if(IDLE)
        {
            dataCollect.AddTimeStamp("IDLE START");

        }
        if(!IDLE)
        {
            dataCollect.AddTimeStamp("IDLE END");

        }

    }

    public void addDISTURB(bool check)
    {
        DISTURB = check;
        if(DISTURB)
        {
            dataCollect.AddTimeStamp("DISTURB START");

        }
        if(!DISTURB)
        {
            dataCollect.AddTimeStamp("DISTURB END");
        }
    }

    public void endEverything()
    {
        if(DISTURB)
        {
            DISTURB = false;
            addDISTURB(DISTURB);
        }
        if(IDLE)
        {
            IDLE = false;
            addIDLE(IDLE);
        }
    }
}
