using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using BNG;

public class UI_BP : MonoBehaviour
{
    [Header("CANVAS")]
    public CanvasGroup Camera_Stage; // Stage_Canvas
    public CanvasGroup Board_Start; //Start_Canvas
    public Transform Bag_Wrong; //Wrong Books
    public Transform PencilCase_Wrong; //WrongPencil
    public Transform Cap_Find; //Cap Canvas
    public Transform Button_Skip; //Button_Canvas
    public Transform Board_Finish; //Finish_Canvas
    public CanvasGroup Camera_Time; //5min_Canvas
    public CanvasGroup Camera_Finish; //FinCavas
    public Text Time_Text; //TIMER
    [Header("TEXTASSET")]
    public char divider = '@';
    public TextAsset TEXT;
    List<string> list_txtIntro;
    [Header("MEMO")]
    public GameObject ParticleSystem;
    GameObject goParticle;
    public Image Memo;
    public Sprite[] StageMemo;
    [Header("AUDIO SOUNDS")]
    public AudioSource Audio_Narration;
    public AudioClip[] Clips_Narration;
    public AudioSource Audio_Effect;
    public AudioClip[] Clips_Effect;
    TextMeshProUGUI m_txtStartInfo;
    public bool bEndUI;
    public bool bTimeStart;
    public float m_fTime;
    TimeSpan m_TimeSpan;
    string line;
    bool timeChange = true;
    Object_BP Manager;
    MemoCheck_BP MemoCheck;
    private bool b_firstMEMO = false;

    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("GameFlow_Manager").GetComponent<Object_BP>();
        MemoCheck = GameObject.FindObjectOfType<MemoCheck_BP>();
        list_txtIntro = TextToList(TEXT);
        bEndUI = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (bTimeStart)
        {
            m_fTime -= Time.deltaTime;
            m_TimeSpan = TimeSpan.FromSeconds(m_fTime);
            Time_Text.text = m_TimeSpan.ToString(@"mm\:ss");
            if (m_fTime <= 60 && timeChange == true) { StartCoroutine(TimeChange()); timeChange = false; }
            if (m_fTime < 0) bTimeStart = false;
        }
        if (b_firstMEMO)
        {
            if(!MemoCheck.set) { StartCoroutine(MemoChecked()); b_firstMEMO = false; }
        }
    }
    private List<string> TextToList(TextAsset txta_speech) //change textasset to list of string using divider
    {
        var listToReturn = new List<string>();
        var arrayString = txta_speech.text.Split(divider); //can change divider
        foreach (var line in arrayString)
        {
            listToReturn.Add(line);
        }
        return listToReturn;
    }


        public IEnumerator CanvasStart()
    {
        yield return new WaitForSeconds(2.0f);
        var child = Board_Start.transform.GetChild(0);
        m_txtStartInfo = child.transform.Find("Info1").GetComponent<TextMeshProUGUI>();
        m_txtStartInfo.text = list_txtIntro[0];
        NarrationSound(0);
        b_firstMEMO = true;
        yield return new WaitUntil(() => !Audio_Narration.isPlaying);
    }

    IEnumerator MemoChecked()
    {
        Audio_Narration.Stop();
        MemoParticle();
        EffectSound("STAR");
        m_txtStartInfo.text = list_txtIntro[1];
        NarrationSound(1);
        yield return new WaitUntil(() => !Audio_Narration.isPlaying);
        Destroy(goParticle);
        m_txtStartInfo.text = list_txtIntro[2];
        NarrationSound(2);
        yield return new WaitUntil(() => !Audio_Narration.isPlaying);
        Board_Start.DOFade(0, 3);
        yield return new WaitUntil(() => Board_Start.alpha == 0);
        Manager.Stage1();
    }
   

    void MemoParticle()
    {
        goParticle = Instantiate(ParticleSystem, Memo.transform.parent.position, Quaternion.identity);
        goParticle.GetComponent<ParticleSystem>().Play();
    }
    void NarrationSound(int index)
    {
        Audio_Narration.clip = Clips_Narration[index];
        Audio_Narration.Play();
    }



    //==============When Wrong==================
   public IEnumerator WrongPencil() //Wrong in Pencilcase
    {
        PencilCase_Wrong.gameObject.SetActive(true);
        EffectSound("INCORRECT");
        yield return new WaitForSeconds(2.5f);
        PencilCase_Wrong.gameObject.SetActive(false);

    }

    public IEnumerator WrongBag(int index)
    {
        TextMeshProUGUI tmproText = Bag_Wrong.GetComponentInChildren<TextMeshProUGUI>();
        switch (index)
        {
            case 1: line = "연필은 필통에 넣는거 잊지 마!";  break; //pencil
            case 2: line = "시간표를 다시 확인해봐!";  break; //textbook
            case 3: line = "알림장을 다시 확인해봐!";  break; //memo
            case 4: line = "지금은 필통에 필기구를 넣어야 해"; break; //wrong stage
        }
        tmproText.text = line;
        Bag_Wrong.gameObject.SetActive(true);
        EffectSound("INCORRECT");
        yield return new WaitForSeconds(3.0f);
        Bag_Wrong.gameObject.SetActive(false);
    }

    public IEnumerator StageNotification(int stage)
    {
        TextMeshProUGUI text;
        var child = Camera_Stage.transform.GetChild(0);
        text = child.Find("Num").GetComponent<TextMeshProUGUI>();
        text.text = stage.ToString();
        Camera_Stage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Camera_Stage.DOFade(0, 1.5f);
        yield return new WaitUntil(() => Camera_Stage.alpha == 0);
        bTimeStart = true;
        Camera_Stage.gameObject.SetActive(false);
    }

    public void ChangeMemo(int index)
    {
        Memo.sprite = StageMemo[index];
    }

    public void EffectSound(string when)
    {
        switch(when)
        {
            case "INCORRECT": Audio_Effect.clip = Clips_Effect[0];  break; //INCORRECT
            case "CORRECT": Audio_Effect.clip = Clips_Effect[1];  break; //CORRECT
            case "APPEAR": Audio_Effect.clip = Clips_Effect[2]; break; //Appear
            case "COMPLETE": Audio_Effect.clip = Clips_Effect[3]; break; //Appear
            case "STAR": Audio_Effect.clip = Clips_Effect[4]; break;
            case "BEEP": Audio_Effect.clip = Clips_Effect[5]; break; //Appear
            case "PENCILCASE": Audio_Effect.clip = Clips_Effect[6]; break; //Appear
        }
        Audio_Effect.Play();
    }
    //===========Time Check=======

    public IEnumerator TimeChange()
    {
        Color originalColor = Time_Text.color;
        Time_Text.color = Color.red;
        for(int i =0; i < 4; i++)
        {
            Time_Text.enabled = false;
            EffectSound("BEEP");
            yield return new WaitForSeconds(0.5f);
            Time_Text.enabled = true;
            yield return new WaitForSeconds(0.5f);

        }
        Time_Text.color = originalColor;
        yield return null;
    }
    public IEnumerator TimeCheck(string strStamp)
    {
        bEndUI = false;
        TextMeshProUGUI tmproTime = Camera_Time.GetComponentInChildren<TextMeshProUGUI>();
        int index = 0;
        switch (strStamp)
        {
            case "TIME LIMIT": tmproTime.text = "조금만 서둘러볼까?"; index = 3; break;
            case "TIME OUT": tmproTime.text = "여기까지 해볼게!"; index = 4; break;
            default: break;
        }
        Camera_Time.DOFade(1, 0.8f);
        NarrationSound(index);
        yield return new WaitUntil(() => !Audio_Narration.isPlaying);
        Camera_Time.DOFade(0, 0.8f);
        yield return new WaitUntil(() => Camera_Time.alpha == 0);
        bEndUI = true;
    }

    public IEnumerator GameFinish()
    {
        GameObject Fin2 = Camera_Finish.transform.Find("Fin_2").gameObject;
        GameObject Fin1 = Camera_Finish.transform.Find("Fin_1").gameObject;
        Fin1.SetActive(false);
        Fin2.SetActive(true);
        Camera_Finish.DOFade(1, 0.4f);
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1.0f);
        
    }

    public void BagAllPacked()
    {
        Board_Finish.gameObject.SetActive(true);
        EffectSound("STAR");
    }
}
