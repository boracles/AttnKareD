using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    [SerializeField] GameObject voices;
    [SerializeField] GameObject audioTrigger;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (voices.GetComponent<AudioSource>().isPlaying || audioTrigger.GetComponent<AudioSource>().isPlaying)
        {
            if (audioTrigger.GetComponent<AudioSource>().isPlaying || voices.GetComponent<AudioSource>().isPlaying)
            {
                StartCoroutine(DecVolume());
            }
        }
        else
        {
            StartCoroutine(IncVolume());
        }
    }

    IEnumerator IncVolume()
    {
        while (GetComponent<AudioSource>().volume < 0.4f)
        {
            GetComponent<AudioSource>().volume += 0.0003f;
            yield return new WaitForSeconds(0.001f);            
        }

        yield break;
    }

    IEnumerator DecVolume()
    {
        while (GetComponent<AudioSource>().volume > 0.1f)
        {
            GetComponent<AudioSource>().volume -= 0.0003f;
            yield return new WaitForSeconds(0.001f);            
        }

        yield break;
    }
}
