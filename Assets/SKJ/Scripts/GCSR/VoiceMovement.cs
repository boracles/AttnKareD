using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceMovement : MonoBehaviour
{

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private void Start()
    {        
        actions.Add("������.", Forword);
        actions.Add("�ڷ�.", Back);
        actions.Add("��������", Left);
        actions.Add("����������", Right);
        
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        //keywordRecognizer.OnPhraseRecognized += RecognizedSpeedch;
        keywordRecognizer.Start();
    }

    public void VoiceSpeech(string ResWords)
    {
        Debug.Log(ResWords);
        actions[ResWords].Invoke();
    }      

    private void Forword()
    {
        transform.Translate(0, 0, 1);
    }

    private void Back()
    {
        transform.Translate(0, 0, -1);
    }

    private void Left()
    {
        transform.Translate(-1, 0, 0);
    }

    private void Right()
    {
        transform.Translate(1, 0, 0);
    }
}
