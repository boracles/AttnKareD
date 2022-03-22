using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using NAudio.Lame;
using NAudio.Wave.WZT;
/*using System.Diagnostics;*/

public class AutoVoiceRecording : MonoBehaviour
{
    string FileName;
    string fWAV = ".wav"; // wav 확장자
    string fMP3 = ".mp3"; // mp3 확장자

    float timer = 0f;

    bool NowRecording = false;    

    AudioClip recording;
    AudioSource audioSource;
    float startRecordingTime;

    int MaxRecordingTime = 300;

    void Start()
    {
        FileName = SceneManager.GetActiveScene().buildIndex.ToString(); // SceneManager.GetActiveScene().buildIndex.ToString();
        audioSource = GetComponent<AudioSource>();

        StartRecording();
        NowRecording = true;
    }

    void Update()
    {
        /*if (NowRecording)
        {
            timer += Time.deltaTime;

            if (timer > MaxRecordingTime - 1)   //timer > 5
            {
                //5분이 되면 자동 종료
                StopRecordingNBehavior(); 
                // 이 함수가 호출되면서 CollectData의 데이터 기록 함수가 한번 더 호출됩니다. 300초 이상 진행하는 어린이들의 데이터가 
                // 한번 끊기게 되고, delimiter도 이중으로 기록되어 데이터 분석에 문제가 될 것 같습니다. 빠르게 수정해주시면 감사하겠습니다.
            }
        }*/
    }

    public void StartRecording()
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            Destroy(audioSource.clip);
        }

        startRecordingTime = Time.time;
        recording = Microphone.Start("", false, MaxRecordingTime, 44100);
    }

    public void StopRecordingNBehavior()     // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 이거 호출하면 종료 및 저장
    {
        /*string callingFuncName = new StackFrame(1).GetMethod().Name;
        UnityEngine.Debug.Log("called by: " + callingFuncName);*/
        NowRecording = false;
        transform.GetComponent<BNG.CollectData>().SaveBehaviorData();
        StartCoroutine(FinishAndMakeClip());
    }

    IEnumerator FinishAndMakeClip()
    {
        Microphone.End("");

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);

        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        recording = recordingNew;
        audioSource.clip = recording;

        //wav파일로 저장
        SavWav.Save(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV, recording);
        
        //저장된 wav파일 mp3로 변환
        WaveToMP3(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV,
        UserData.DataManager.GetInstance().FilePath_Folder + FileName + fMP3);

        //파일 저장 완료까지 대기
        yield return new WaitUntil(() => File.Exists(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fMP3));  

        File.Delete(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV);
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV 파일 MP3로 변환
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }
}
