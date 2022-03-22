using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using NAudio.Lame;
using NAudio.Wave.WZT;

public class AutoVoiceRecording_Ending : MonoBehaviour
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


        
    }

    void Update()
    {
        
    }

    public void StartRecording()
    { 
        NowRecording = true;

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
        if(UserData.DataManager.GetInstance().FilePath_Folder!= null)
        {
            SavWav.Save(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV, recording);
            //저장된 wav파일 mp3로 변환
            WaveToMP3(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV,
           UserData.DataManager.GetInstance().FilePath_Folder + FileName + fMP3);

            //파일 저장 완료까지 대기
            yield return new WaitUntil(() => File.Exists(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fMP3));

            File.Delete(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV);

        }
        if(UserData.DataManager.GetInstance().FilePath_Folder == null)
        {
            //wav파일로 저장
            //    SavWav.Save(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV, recording);
            // 왕왕 중요하다 여기 수정해뒀으니까 꼭 다시 읽어봐라
            SavWav.Save(@"C:\Desktop\" + "sound" + fWAV, recording);
            //저장된 wav파일 mp3로 변환
            WaveToMP3(@"C:\Desktop\" + "sound" + fWAV,
           @"C:\Desktop\" + "sound" + fMP3);

            //파일 저장 완료까지 대기
            yield return new WaitUntil(() => File.Exists(@"C:\Desktop\" + "sound" + fMP3));

            File.Delete(@"C:\Desktop\" + "sound" + fWAV);

        }
       
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV 파일 MP3로 변환
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }
}
