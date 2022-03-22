using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud_Paddle : MonoBehaviour
{
    public Text txtButton;
    public Canvas canvasDistance;
    public TextMeshProUGUI txtDISTANCE;
    public TextMeshProUGUI txtERROR;
    public Canvas canvasFinish;
    public Text TimeText;
    TextMeshProUGUI txtFinish;
    public AudioClip[] clipNarration;
    /*
     * [0] : Guide
     * [1] : Time Limit
     * [2] : Time Out
     */
    public AudioClip[] clipEffect;
    /*
     * [0] : CountDown
     * [1] : ButtonClick
     * [2] : Success
     * [3] : Fail
     * [4] : Stage UP
     * [5] : YE(ALL DONE)
     */
    AudioSource m_audioEffect;
    AudioSource m_audioNarration;
    AudioSource m_audioBGM;
    int m_nDISTANCE  = 0;
    int m_nPERCENT;
    public bool bCoroutine;
    // Start is called before the first frame update
    [SerializeField]
    public bool bTimeStart = false;
    public float m_fTime;
    TimeSpan m_TimeSpan;

    void Awake()
    {
        var audioSources = transform.GetComponents<AudioSource>();
        m_audioEffect = audioSources[0];
        m_audioNarration = audioSources[1];
        m_audioBGM = audioSources[2];
    }
    void Start()
    {
        m_audioBGM.clip = clipNarration[3];
    }

    // Update is called once per frame
    void Update()
    {
        if (bTimeStart)
        {
            m_fTime -= Time.deltaTime;
            m_TimeSpan = TimeSpan.FromSeconds(m_fTime);
            TimeText.text = m_TimeSpan.ToString(@"mm\:ss");
            if (m_fTime < 0) bTimeStart = false;
        }
    }

    public void SetDistance(int nStage)
    { 
        m_nPERCENT = Manager_Paddle.SDB[nStage].intPercent;
        m_nDISTANCE += m_nPERCENT;
        txtDISTANCE.text = m_nDISTANCE.ToString();
    }

    public void AudioController(string text)
    {
        switch (text)
            {
                case "guide"      : PlayNarration(clipNarration[0], false); break;
                case "button"     : StartCoroutine(TextSpeechWarning(null, clipEffect[1])); break;
                case "correct"    : StartCoroutine(TextSpeechWarning("잘했어!", clipEffect[2])); break;
                case "wrong order": StartCoroutine(TextSpeechWarning("친구 방향에 맞춰 돌려야 해", clipEffect[3])); break;
                case "wrong speed": StartCoroutine(TextSpeechWarning("친구 페달의 속도에 맞춰 돌려줘", clipEffect[3])); break;
                case "time limit" : PlayNarration(clipNarration[1], false); break;
                case "time over"  : PlayNarration(clipNarration[2], false); break;
                case "stage"      : StartCoroutine(TextSpeechWarning("속도와 방향이 바뀌었어!", clipEffect[4])); break;
                case "complete"   : StartCoroutine(TextSpeechWarning("정말 잘했어!", clipEffect[5])); break;
            }
    }
   
    public void BGMplay(bool play)
    {
        if (play) m_audioBGM.Play();
        if (!play) m_audioBGM.Pause();
    }
    public IEnumerator CountDown() {
        bCoroutine = true;
        m_audioNarration.Stop();
        m_audioEffect.clip = clipEffect[0];
        m_audioEffect.Play();
        txtButton.text = "3";
        yield return new WaitForSeconds(.9f);
        m_audioEffect.Play();
        txtButton.text = "2";
        yield return new WaitForSeconds(.9f);
        m_audioEffect.Play();
        txtButton.text = "1";
        yield return new WaitForSeconds(.9f);
        txtButton.transform.parent.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        canvasDistance.gameObject.SetActive(true);
        bCoroutine = false;


    }
    IEnumerator TextSpeechWarning(string text,AudioClip audioClip )
    {
        bCoroutine = true;
        if (text!=null) txtERROR.text = text;
        Debug.Log(text);
        m_audioEffect.clip = audioClip;
        m_audioEffect.Play();
        if (audioClip.length < 1f) yield return new WaitForSeconds(2.0f);
        else yield return new WaitForSeconds(audioClip.length);
        txtERROR.text = "";
        bCoroutine = false;
    }

    public void PlayNarration(AudioClip audioClip, bool loop)
    {
        bCoroutine = true;
        m_audioNarration.clip = audioClip;
        m_audioNarration.Play();
        m_audioNarration.loop = loop;
        bCoroutine = false;

    }

    public IEnumerator NextScene()
    {
        bCoroutine = true;
        canvasFinish.gameObject.SetActive(true);
        canvasDistance.gameObject.SetActive(false);
        txtFinish= canvasFinish.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        txtFinish.text = "이동합니다";
        yield return new WaitForSeconds(0.9f);
        txtFinish.text = "3";
        yield return new WaitForSeconds(0.9f);
        txtFinish.text = "2";
        yield return new WaitForSeconds(0.9f);
        txtFinish.text = "1";
        yield return new WaitForSeconds(0.5f);
        bCoroutine = false;
    }
}
