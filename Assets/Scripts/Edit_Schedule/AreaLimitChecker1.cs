using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLimitChecker1 : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip Return;

    public Transform BehaviorMG;

    bool Checker = false;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        Checker = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && Checker)
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE END");
            Checker = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player" && !Checker)
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE START");
            Checker = true;
            audioSource.clip = Return;
            audioSource.Play();
        }
    }
}
