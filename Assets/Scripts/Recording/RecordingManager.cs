using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using NAudio.Lame;
using NAudio.Wave.WZT;

public class RecordingManager : MonoBehaviour
{
    string fWAV = ".wav"; // wav 확장자
    string fMP3 = ".mp3"; // mp3 확장자

    public bool OnBoard = false;

    public TextMeshProUGUI Txt_Status;

    public GameObject Btn_start;
    public GameObject Btn_stop;
    public GameObject Btn_replay;

    float timer = 0f;
    float timerMin = 0f;
    float timerSec = 0f;

    bool NowRecoding = false;

    string FileName;
    string FilePath;

    AudioClip recording;
    AudioSource audioSource;
    float startRecordingTime;

    int MaxRecordingTime = 180; // sec 3분
    int latency = 2; // sec 타이밍 맞추기 위함

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        FilePath = Application.streamingAssetsPath + "/Hippo/";
    }

    void Update()
    {
        if (NowRecoding)
        {
            //녹음 중 시간 보여주기용 + MAX 시간이 되면 자동 종료
            timer += Time.deltaTime;

            if (timer > 1f)     // 1sec
            {
                timer = 0f;
                timerSec += 1f;

                if (timerSec == 60f)    // 1min
                {
                    timerSec = 0f;
                    timerMin += 1f;

                    if (timerMin == (MaxRecordingTime / 60))     // MaxTime -> Finish
                    {
                        StopRecording();
                    }
                }
            }

            Txt_Status.text = "간단한 자기소개를 해주세요.\n녹음 중 (최대 3분)\n" + timerMin.ToString("00") + ":" + timerSec.ToString("00");
        }
        else
        {
            //녹음 종료 상태일때 타이머 초기화
            if (timer != 0f || timerMin != 0f || timerSec != 0f)
            {
                timer = 0f;
                timerMin = 0f;
                timerSec = 0f;
                Txt_Status.text = "";
            }
        }
    }

    public void StartRecording()
    {
        //녹음 시작 버튼 클릭 시, 실제 녹음 시작
        //기존 저장된 cilp 있으면 삭제
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            Destroy(audioSource.clip);
        }

        startRecordingTime = Time.time;
        recording = Microphone.Start("", false, (MaxRecordingTime + latency), 44100); // MAX + Latency 녹음 시작과 끝에 딜레이가 있기 때문에 추가 여유분 더해서 MAX로 세팅

        Btn_start.SetActive(false);
        Btn_replay.SetActive(false);

        Txt_Status.text = "준비";
        StartCoroutine(Recording());
    }

    IEnumerator Recording()
    {
        //녹음 시작 알림, 첫 시작 부분 짤릴 수 있으므로 딜레이 주고 시작
        yield return new WaitForSeconds(.5f);
        NowRecoding = true;

        yield return new WaitForSeconds(1f);
        Btn_stop.SetActive(true);
    }

    public void StopRecording()
    {
        //녹음 종료 버튼 클릭 시
        Txt_Status.text = "녹음 종료\n저장 중 잠시만 기다려 주세요.";

        NowRecoding = false;
        StartCoroutine(FinishAndMakeClip());
    }

    IEnumerator FinishAndMakeClip()
    {
        //녹음 종료 후 프로세스
        //바로 종료하면 마지막 소리가 짤릴 수 있으므로 딜레이 주고 종료
        yield return new WaitForSeconds(1f);

        Microphone.End("");

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);

        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        recording = recordingNew;
        audioSource.clip = recording;

        FileName = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
        //wav파일로 저장
        SavWav.Save(FilePath + FileName + fWAV, recording);
        //저장된 wav파일 mp3로 변환
        WaveToMP3(FilePath + FileName + fWAV, FilePath + FileName + fMP3);
        //파일 저장 완료까지 대기
        yield return new WaitUntil(() => File.Exists(FilePath + FileName + fMP3));

        Txt_Status.text = "저장 완료!\n'들어보기'로 녹음된 내용을 확인하세요.\n'녹음하기'를 누르면 다시 녹음할 수 있습니다.";

        Btn_start.SetActive(true);
        Btn_stop.SetActive(false);
        Btn_replay.SetActive(true);


        File.Delete(FilePath + FileName + fWAV);

        //전송 완료 후 필요시, 저장된 mp3 원본 삭제
        //File.Delete(FilePath + FileName + fMP3);
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV 파일 MP3로 변환
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }


    public void PlayRecorded()
    {
        //녹음된 것 듣기
        audioSource.Play();
    }















}
