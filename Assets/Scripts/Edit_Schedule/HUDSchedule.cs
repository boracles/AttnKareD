using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Scheduler;
using UnityEngine.EventSystems;

public class HUDSchedule : MonoBehaviour
{
    private enum Voice {HowTo, Start, HalfInfo, WellDone}

    public DictionaryScript dicScript01;
    public DictionaryScript dicScript02;
    [SerializeField] DictionaryScriptableObject dicData01;
    [SerializeField] DictionaryScriptableObject dicData02;

    [SerializeField] private ScheduleManager1 schManager;
    /*************************************************************************
    //처음 안내문구 음성과 문구을 전시합니다
    *************************************************************************/
    [SerializeField] private AudioSource audSIntro; //안내 오디오 소스
    [SerializeField] private AudioClip[] audCIntro; // 안내 음성 클립
    [SerializeField] private DOTweenAnimation[] dotAnim; // 텍스트 애니메이션

    [SerializeField] private Dictionary<string, float> txtNTimingDic01;
    [SerializeField] private Dictionary<string, float> txtNTimingDic02;
    
    public Canvas infoCanvas;

    [SerializeField] private TextMeshProUGUI howToTMP;
    [SerializeField] private string[] howToScriptTxt;

    private bool _isFade;

    private void Awake()
    {
        txtNTimingDic01 = dicScript01.TxtDictionary;
        for (int i = 0; i < Mathf.Min(dicData01.Keys.Count, dicData01.Values.Count); i++)
        {
            txtNTimingDic01.Add(dicData01.Keys[i], dicData01.Values[i]);
        }
        
        txtNTimingDic02 = dicScript02.TxtDictionary;
        for (int i = 0; i < Mathf.Min(dicData02.Keys.Count, dicData02.Values.Count); i++)
        {
            txtNTimingDic02.Add(dicData02.Keys[i], dicData02.Values[i]);
        }
    }

    private void Start()
    {
        _isFade = false;
        HowToPlay();
    }
    
    public void HowToPlay()
    {
        //StartCoroutine(HowToPlayVoiceText());
        StartCoroutine(HowToPlaySetUiTxt());
    }

    
    private IEnumerator HowToPlaySetUiTxt()
    {
        yield return new WaitForSeconds(2f);
        
        foreach (var item in dicScript01.TxtDictionary)
        {
            var index = 0;
            if (!_isFade)
            {
                howToTMP.SetText(item.Key);
                FadeInCanvas(infoCanvas, 1f); // Info Canvas fade In
                _isFade = true;
                index = dicScript01.TxtDictionary.Values.ToList().IndexOf(item.Value);
                Debug.Log(index + " 번째값");
                Debug.Log(item.Value);
                yield return new WaitForSeconds(item.Value);
                continue;
            }

            index = dicScript01.TxtDictionary.Values.ToList().IndexOf(item.Value);
            Debug.Log(index + " 번째값");
            Debug.Log(item.Value);
            howToTMP.SetText(item.Key);
            yield return new WaitForSeconds(item.Value);
        }

        _isFade = false;
        FadeOutCanvas(infoCanvas, 1f);
        yield return new WaitForSeconds(1f);
        //schManager.subUi.gameObject.SetActive(true);
        schManager.VisibleStartBtn(true);
    }

    public IEnumerator HalfInfoSetUiTxt()
    {
        yield return new WaitForSeconds(2f);
        
        foreach (var item in dicScript02.TxtDictionary)
        {
            var index = 0;
            if (!_isFade)
            {
                howToTMP.SetText(item.Key);
                FadeInCanvas(infoCanvas, 1f); // Info Canvas fade In
                _isFade = true;
                index = dicScript02.TxtDictionary.Values.ToList().IndexOf(item.Value);
                Debug.Log(index + " 번째값");
                Debug.Log(item.Value);
                yield return new WaitForSeconds(item.Value);
                continue;
            }

            index = dicScript02.TxtDictionary.Values.ToList().IndexOf(item.Value);
            Debug.Log(index + " 번째값");
            Debug.Log(item.Value);
            howToTMP.SetText(item.Key);
            yield return new WaitForSeconds(item.Value);
        }

        _isFade = false;
        FadeOutCanvas(infoCanvas, 1f);
        yield return new WaitForSeconds(1f);
        //schManager.subUi.gameObject.SetActive(true);
        schManager.VisibleStartBtn(true);
    }
    
    private IEnumerator HowToPlayVoiceText()
    {
        yield return new WaitForSeconds(3f);
        
        var wait = PlaySound(audSIntro, audCIntro[(int)Voice.HowTo]);
        yield return new WaitForSeconds(wait);
        foreach (var aC in audCIntro)
        {
            yield return new WaitForSeconds(1f);
            wait = PlaySound(audSIntro, aC);
            
            dotAnim[Array.IndexOf(audCIntro, aC)].DOPlay(); // Start Text Anim
            yield return new WaitForSeconds(wait);
        }

        FadeOutCanvas(infoCanvas, 1f); // Info Canvas fade Out
        yield return new WaitForSeconds(2f);
    }

    /*************************************************************************
    //Canvas Fade
    *************************************************************************/
    private void FadeInCanvas(Canvas canvas, float time)
    {
        if(!canvas) return;  // check valid canvas ?
        StartCoroutine(AnimAlpha(canvas, time,true));
    }

    private void FadeOutCanvas(Canvas canvas, float time)
    {
        if(!canvas) return;  // check valid canvas ?
        StartCoroutine(AnimAlpha(canvas, time,false));
    }
    
    private IEnumerator AnimAlpha(Canvas canvas,float time, bool bIn)
    {
        var cg = canvas.GetComponent<CanvasGroup>();
        cg.alpha = bIn ? 0 : 1f;
        var loop = (int)(time/0.05f);
        var fadeStep = 1f / loop;
        for(var i=0; i< loop; i++) 
        {
            yield return new WaitForSeconds(0.05f);
            cg.alpha += bIn ? fadeStep : (-1f)*fadeStep;
        }
    }

    /*************************************************************************
    //Control Sound Clip
    *************************************************************************/
    private float PlaySound(AudioSource aSource, AudioClip aClip)
    {
        if(!aSource || !aClip) return 0f;        
        aSource.clip  = aClip;
        aSource.Play();
        return aClip.length;  
    }

}
