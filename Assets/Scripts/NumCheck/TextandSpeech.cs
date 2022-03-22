using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextandSpeech : MonoBehaviour
{
    /*Only for UI
    * 
    * Call
    * yield return StartCoroutine(narration.Introduction());
    * where you want to start your introduction
    */
    [Header("NARRATION")]
    public AudioClip[] arrNarr;
    AudioSource audioPlay;

    [Header("BOARD")]
    public TextMeshProUGUI txt_BoardTxt;
    [SerializeField]
    private TextAsset txta_board;
    public AudioClip[] arrBoardNarr;

    [HideInInspector]
    public bool coroutine;
    [Header("SPEECH BUBBLE")]
    public GameObject go_SpeechBubble; // canvas or gameobject(turned off when not speaking)
    [SerializeField]
    private TextAsset txta_speech; // speech
    List<string> list_txtSpeech;
    List<string> list_txtBoard;
    private TextMeshProUGUI txt_speech; //text in canvas
    [SerializeField]
    private char divider;
    public GameObject m_goFace;
    public GameObject m_goAutoSpeech;

    private void Start()
    {
        list_txtSpeech = TextToList(txta_speech);
        audioPlay = GetComponent<AudioSource>();
        txt_speech = go_SpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
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
    private void NarrPlay(AudioClip nowclip) //play voice narration
    {
        audioPlay.clip = nowclip;
        audioPlay.Play();
    }

    public IEnumerator CharacterSpeak(int start, int end) //read all introduction text
    {
        for (int i = start; i < end; i++)
        {
            go_SpeechBubble.SetActive(true);
            NarrPlay(arrNarr[i]);
            txt_speech.text = list_txtSpeech[i];
            yield return new WaitWhile(() => audioPlay.isPlaying);
            go_SpeechBubble.SetActive(false);
            yield return new WaitForSeconds(0.9f);
        }
        go_SpeechBubble.SetActive(false);
    }

    public IEnumerator BoardUI(int index) //only for specific comments(also different UI, this case on Board)
    {
        coroutine = true;
        string originalText = txt_BoardTxt.text;
        list_txtBoard = TextToList(txta_board);
        txt_BoardTxt.text = list_txtBoard[index];
        if (arrBoardNarr[index]) { NarrPlay(arrBoardNarr[index]); yield return new WaitWhile(() => audioPlay.isPlaying); }
        txt_BoardTxt.text = originalText;
        coroutine = false;

    }

    public void EndUI(string txt)
    {
        txt_BoardTxt.text = txt;
 
    }

    public void Bothered(bool active)
    {
        m_goFace.SetActive(active);
        m_goAutoSpeech.SetActive(active);
    }

}
