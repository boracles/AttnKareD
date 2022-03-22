using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    public GameObject timer;

    [SerializeField] AudioClip stage1FinishAudio;
    [SerializeField] AudioClip stage2FinishAudio;
    [SerializeField] AudioClip stage3FinishAudio;

    public IEnumerator Stage1Finish()
    {
        if(timer != null)
        {
            while(timer.GetComponent<AudioSource>().isPlaying)
            {
                Debug.Log("Waiting for Audio to Finish...");
                yield return StartCoroutine(Wait());
            }
        }
        
        GetComponent<AudioSource>().clip = stage1FinishAudio;
        GetComponent<AudioSource>().Play();
    }

    public IEnumerator Stage2Finish()
    {
        if (timer != null)
        {
            while (timer.GetComponent<AudioSource>().isPlaying)
            {
                Debug.Log("Waiting for Audio to Finish...");
                yield return StartCoroutine(Wait());
            }
        }
        GetComponent<AudioSource>().clip = stage2FinishAudio;
        GetComponent<AudioSource>().Play();
    }

    public IEnumerator Stage3Finish()
    {
        if (timer != null)
        {
            while (timer.GetComponent<AudioSource>().isPlaying)
            {
                Debug.Log("Waiting for Audio to Finish...");
                yield return StartCoroutine(Wait());
            }
        }
        GetComponent<AudioSource>().clip = stage3FinishAudio;
        GetComponent<AudioSource>().Play();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
    }
}
