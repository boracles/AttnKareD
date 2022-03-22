using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UserData;

public class SceneTransition_Tutorial : MonoBehaviour
{
    public Guide_Tutorial Guide;
    public CollectData TimeStamp;
    PlayMakerFSM selfFSM;
    public bool watchBool;
    public bool stayBool = false;
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
    public void OnTriggerEnter(Collider other)
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
                if (!eventStart)
                { StartCoroutine(StayStill()); }
            }
            if (!watchBool)
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

    IEnumerator StayStill()
    {
        eventStart = true;
        yield return new WaitForSeconds(1.5f);
        Guide.RunningCoroutione(Guide.SceneChange(true));

    }

    public void StillMoving()
    {
        Guide.RunningCoroutione(Guide.SceneChange(false));
        eventStart = false;

    }
}
