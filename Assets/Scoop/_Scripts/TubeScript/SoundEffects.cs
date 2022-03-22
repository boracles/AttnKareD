using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [Header("Sound Effect Audio Clips")]
    [SerializeField] AudioClip wrongBall;
    [SerializeField] AudioClip correctBall;
    // Start is called before the first frame update
    public void WrongBall()
    {
        GetComponent<AudioSource>().clip = wrongBall;
        GetComponent<AudioSource>().Play();
    }
    public void CorrectBall()
    {
        GetComponent<AudioSource>().clip = correctBall;
        GetComponent<AudioSource>().Play();
    }
}
