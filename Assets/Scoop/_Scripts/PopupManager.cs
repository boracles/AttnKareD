using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] public GameObject shovelGrab;
    [SerializeField] public GameObject ballPool;
    [SerializeField] public GameObject colorGuide;
    [SerializeField] public GameObject numberGuide;

    [Header("For Some Necessary Variables")]
    [SerializeField] GameObject audioTrigger;

    bool introShown = false;

    // Start is called before the first frame update
    void Start()
    {
        shovelGrab.SetActive(false);
        ballPool.SetActive(false);
        colorGuide.SetActive(false);
        numberGuide.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioTrigger.GetComponent<AudioSource>().isPlaying && !introShown)
        {
            StartCoroutine(IntroMessage());
            introShown = true;            
        }
    }

    IEnumerator IntroMessage()
    {
        shovelGrab.SetActive(true);

        yield return new WaitForSeconds(10f);

        shovelGrab.SetActive(false);

        yield return new WaitForSeconds(7f);

        ballPool.SetActive(true);

        yield return new WaitForSeconds(10f);

        ballPool.SetActive(false);
    }

    public IEnumerator ShowMessage(GameObject message, float seconds = 7f)
    {
        if(message == colorGuide && numberGuide.activeSelf)
        {
            numberGuide.SetActive(false);
        }
        else if (message == numberGuide && colorGuide.activeSelf)
        {
            colorGuide.SetActive(false);
        }

        message.SetActive(true);

        yield return new WaitForSeconds(seconds);

        message.SetActive(false);
    }

    
}
