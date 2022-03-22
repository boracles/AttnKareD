using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAudio : MonoBehaviour
{
    [SerializeField] AudioClip nextStage;
    
    public void NextStage()
    {
        GetComponent<AudioSource>().clip = nextStage;
        GetComponent<AudioSource>().Play();
    }
}
