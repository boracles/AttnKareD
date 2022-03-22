using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;
using DG.Tweening;
public class Hud_Tutorial : MonoBehaviour
{
    public bool bGuide = false;

    [SerializeField] List<TextMeshProUGUI> Text;
    [SerializeField] List<AudioClip> Voice;
    [SerializeField] AudioClip clipEffect;
    [SerializeField] private GameObject model_Oculus;
    [SerializeField] private Animator anim_LeftHand;
    [SerializeField] private Animator anim_RightHand;
    [SerializeField] private GameObject hand_Oculus;
    [SerializeField] private GameObject video_Trigger;
    [SerializeField] private GameObject BananaNPlate;
    [SerializeField] private GameObject ButtonBox;
    [SerializeField] private GameObject Arrow_Button;
    public bool m_bFin;
    AudioSource m_Audio;
    bool bCoroutine=true;
    Coroutine runningcoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        if(runningcoroutine!= null)
        {
            StopCoroutine(runningcoroutine);
        }
        runningcoroutine = StartCoroutine(coroutine);
    }
    public IEnumerator CanvasStart()
    {
        //     StartCoroutine(TextNSpeech(Voice[0], Text[0], Text[1]));
        yield return new WaitForSeconds(1f);
        SetAudio(Voice[15]);
        yield return new WaitUntil(() => !m_Audio.isPlaying);
        StartCoroutine(TextFade(Text[15]));
        yield return new WaitUntil(() => !bCoroutine);
        StartCoroutine(TextNSpeech(Voice[0], Text[0]));
        yield return new WaitUntil(() => !bCoroutine);
        model_Oculus.SetActive(true);
        StartCoroutine(TextNSpeech(Voice[1], Text[1]));
        yield return new WaitUntil(() => !bCoroutine);
        model_Oculus.SetActive(false);
        Text[2].DOFade(1, 0.7f);
        SetAudio(Voice[2]);
        hand_Oculus.SetActive(true);
        yield return new WaitForSeconds(8);
        StartCoroutine(TextFade(Text[2]));
        yield return new WaitUntil(() => !bCoroutine);
        hand_Oculus.SetActive(false);
        video_Trigger.SetActive(true);
        video_Trigger.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
        Text[3].DOFade(1, 0.7f);
        ActivateGrabHand();

    }
    public IEnumerator GrabBanana()
    {
        m_Audio.Stop();
        model_Oculus.SetActive(false);
        video_Trigger.SetActive(false);
        StartCoroutine(TextFade(Text[3]));
        yield return new WaitUntil(() => !bCoroutine);
        StartCoroutine(TextNSpeech(Voice[4], Text[4]));
        yield return new WaitUntil(() => !bCoroutine);
        StartCoroutine(TextNSpeech(Voice[5], Text[5]));
        yield return new WaitUntil(() => !bCoroutine);
        BananaNPlate.SetActive(true);
        SetAudio(clipEffect);
        BananaAppear();


    }

    public IEnumerator NoteCheck()
    {
        m_Audio.Stop();
        StartCoroutine(TextFade(Text[6]));
        yield return new WaitUntil(() => !bCoroutine);
        Debug.Log("WHY?");
        StartCoroutine(TextNSpeech(Voice[7], Text[7]));
        yield return new WaitUntil(() => !bCoroutine);
        Text[8].DOFade(1, 0.7f);
        SetAudio(Voice[8]);
        bGuide = true;


    }
    public IEnumerator PutBanana()
    {
        m_Audio.Stop();
        StartCoroutine(TextFade(Text[8]));
        yield return new WaitUntil(() => !bCoroutine);
        StartCoroutine(TextNSpeech(Voice[9], Text[9]));
        yield return new WaitUntil(() => !bCoroutine);
        Text[10].DOFade(1, 0.7f);
        SetAudio(Voice[10]);
        bGuide = true;
    }
    public IEnumerator PressButton()
    {
        m_Audio.Stop();
        StartCoroutine(TextFade(Text[10]));
        yield return new WaitUntil(() => !bCoroutine);
        StartCoroutine(TextNSpeech(Voice[4], Text[4]));
        yield return new WaitUntil(() => !bCoroutine);
        Text[11].DOFade(1, 0.7f);
        SetAudio(Voice[11]);
        ButtonBox.transform.DOMoveX(-0.058f, 1.5f);
        Arrow_Button.SetActive(true);
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(TextFade(Text[11]));
        Text[12].DOFade(1, 0.7f);
        SetAudio(Voice[12]);
        bGuide = true;
    }

    public IEnumerator AllFinished()
    {
        m_Audio.Stop();
        Arrow_Button.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        ButtonBox.transform.DOMoveX(-0.754f, 3f);
        StartCoroutine(TextFade(Text[12]));
        yield return new WaitUntil(() => !bCoroutine);
        StartCoroutine(TextNSpeech(Voice[4], Text[4]));
        yield return new WaitUntil(() => !bCoroutine);
        StartCoroutine(TextNSpeech(Voice[13], Text[13]));
        yield return new WaitUntil(() => !bCoroutine);
        Text[14].DOFade(1, 0.7f);
        SetAudio(Voice[14]);
        bGuide = true;
    }

    public IEnumerator NextScene()
    {
        Debug.Log("STOP COROUTINE");
        Text[14].text = "이동합니다";
        yield return new WaitForSeconds(2f);
        Text[14].text = "<b><size=.4>3</size></b>";
        yield return new WaitForSeconds(1f);
        Text[14].text = "<b><size=.4>2</size></b>";
        yield return new WaitForSeconds(1f);
        Text[14].text = "<b><size=.4>1</size></b>";
        bGuide = true;

    }
    public IEnumerator StopNextScene()
    {
        Debug.Log("START COROUTINE");
        StopCoroutine(NextScene());
        Text[14].text = "바닥에 있는 원 안에 서서 화면을 바라보세요.";
        yield return null;

    }
    void BananaAppear()
    {
        Text[6].DOFade(1, 0.7f);
        SetAudio(Voice[6]);
        bGuide = true;
    }
    void ActivateGrabHand()
    {
        model_Oculus.SetActive(true);
        SetAudio(Voice[3]);
        anim_LeftHand.enabled = true;
        anim_RightHand.enabled = true;
        bGuide = true;
    }
    public IEnumerator TextNSpeech(AudioClip clip, TextMeshProUGUI text)
    {
        bCoroutine = true;
        text.DOFade(1, 0.7f);
        SetAudio(clip);
        yield return new WaitUntil(() => !m_Audio.isPlaying);
        text.DOFade(0, 0.7f);
        yield return new WaitForSeconds(0.7f);
        bCoroutine = false;
    }
    public IEnumerator TextFade(TextMeshProUGUI text)
    {
        bCoroutine = true;
        text.DOFade(0, 0.7f);
        yield return new WaitForSeconds(0.7f);
        bCoroutine = false;
    }

    void SetAudio(AudioClip clip)
    {
        m_Audio.clip = clip;
        m_Audio.Play();
    }

}
