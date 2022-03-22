using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using BNG;


public class SceneChange_Circle : MonoBehaviour
{

    public PlayMakerFSM watchManager;
    public CollectData TimeStamp;
    PlayMakerFSM selfFSM;
    public bool watchBool;
    public bool stayBool=false;
    bool eventStart;

    // Start is called before the first frame update
    void Start()
    {
        if (!stayBool) //activated하자마자 일단 escape start 하나 찍고 시작
        {
            Debug.Log("OUT");
            TimeStamp.AddTimeStamp("ESCAPE START");
            stayBool = true;
        }
        eventStart = false;
        selfFSM = this.gameObject.GetComponent<PlayMakerFSM>();

    }

    // Update is called once per frame
    void Update()
    {
        watchBool = FsmVariables.GlobalVariables.GetFsmBool("b_panelWatch").Value;



    }

    private void LateUpdate()
    {
        /*
        if (!watchBool)
        {
            // b_changeScene = false;
            StillMoving();

        }
        */
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "HeadCollision")
        {
            if (stayBool) //if stay inside trigger
            {
                Debug.Log("IN");
                TimeStamp.AddTimeStamp("ESCAPE END");
                stayBool = false;
            }
            if (watchBool)
            {
                if(!eventStart)
                { StayStill(); }
            }
            if(!watchBool)
            {
                if (eventStart)
                {
                    

                    StillMoving();
                }
                    
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HeadCollision")
        {
            if (!stayBool) //if exit trigger
            {
                Debug.Log("OUT");
                TimeStamp.AddTimeStamp("ESCAPE START");
                stayBool = true;
            }
            StillMoving();
            
        }
    }

    public void StayStill()
    {
        /*
        if(stayBool == null)//first time entering scene collider, don't stamp
        {
            stayBool = false;
        }
        */
        
        selfFSM.SendEvent("StayStill");
        eventStart = true;

    }

    public void StillMoving()
    {
        /*
        if(stayBool==null) //right after scene collider is activated, don't stamp
        {
            return;

        }
        */
       
        selfFSM.SendEvent("StillMoving");
        eventStart = false;

    }
}
