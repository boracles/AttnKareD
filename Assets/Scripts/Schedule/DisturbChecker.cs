using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbChecker : MonoBehaviour
{
    public Transform BehaviorMG;
    Vector3 OriginPos;

    private void Start()
    {
        OriginPos = this.transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Grabber")
        {
            Debug.Log("Grabber_IN");
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("DISTURB START");
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
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("DISTURB END");
        }
    }
}
