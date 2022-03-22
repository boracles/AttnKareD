using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMcontroller1 : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip Intro;
    public AudioClip BGM;
    public AudioClip TimeLimit;
    public AudioClip TimeOut;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        StartCoroutine(PlayIntro());
    }

    public void PlayBGMByTypes(string Type)
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        
        if (Type == "INTRO")
        {
            audioSource.clip = Intro;
            audioSource.loop = false;
        }
        else if (Type == "BGM")
        {
            audioSource.clip = BGM;
            audioSource.loop = true;
        }
        else if (Type == "LIMIT")
        {
            audioSource.clip = TimeLimit;
            audioSource.loop = false;
        }
        else if (Type == "OUT")
        {
            audioSource.clip = TimeOut;
            audioSource.loop = false;
        }

        audioSource.Play();
    }

    IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(2f);
        PlayBGMByTypes("INTRO");
    }
}
