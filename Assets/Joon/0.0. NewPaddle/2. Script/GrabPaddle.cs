using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPaddle : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool HOLDING;
    public Transform MyWheel;
    public Transform FriendWheel;

    
    public float fIdleTime;
    AddDelimiter delimiters;
    public float fDisturbTime;
    public float fDisturbCount;
    public float fIdleCount;
    bool nothing = true;
    bool disturbed = true;
    bool disturbT = false;
    bool idleT = false;

    Vector3 OriginPos;
    void Start()
    {
        delimiters = GetComponent<AddDelimiter>();
        OriginPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (MyWheel.childCount != 0) //idle end
        {
            HOLDING = true;
            if (!nothing) //Stop IDLE in data log
            {
                idleT = false;
                delimiters.addIDLE(nothing);
                nothing = true;
            }
        }
        if(MyWheel.childCount == 0) //idle start
        {
            HOLDING = false;
            if (nothing) //START IDLE in data log
            {
                idleT = true;
                fIdleCount++;
                Debug.Log("NOTHIG");
                delimiters.addIDLE(nothing);
                nothing = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (disturbT) fDisturbTime += Time.deltaTime;
        if (idleT) fIdleTime += Time.deltaTime; //내 페달이나 친구 페달을 잡고 있으면 시간체크 안함

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Grabber")
        {
            Debug.Log("Grabber_IN");
            if (disturbed)
            {
                disturbT = true;
                fDisturbCount++;
                delimiters.addDISTURB(disturbed);
                disturbed = idleT = false;
            }
        }
        else if (collision.tag == "Wall")
        {
            this.transform.position = OriginPos;
            this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Grabber")
        {
            Debug.Log("Grabber_OUT");
            if (!disturbed)
            {
                disturbT = false;
                Debug.Log("DISTURBED");
                delimiters.addDISTURB(disturbed);
                disturbed = idleT = true;
            }
        }
    }

    public void AllFinish()
    {
        disturbT = idleT = nothing = disturbed = false;
        delimiters.endEverything();
    }


}
