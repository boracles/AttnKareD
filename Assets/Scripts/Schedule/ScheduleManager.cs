using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KetosGames.SceneTransition;


public class ScheduleManager : MonoBehaviour
{
    public Transform Behavior;
    //Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("delimiter name");

    public Transform Intro;
    public Transform Schedule;
    public Transform Finish;
    public Transform WellDoneAndBye;
    public TextMeshProUGUI FinishCntDwn;

    public Transform BGM_Controller;

    public Text Result;

    public Transform Btn_Finish;

    public TextMeshProUGUI TextTitle;

    public Text ToShow;

    public Transform Slot;
    public Transform Grp;

    public Text TimerText;

    List<Transform> SlotList;
    List<Transform> GrpList;

    public float[] scene2arr;

    bool LeGogo = false;
    bool BeforeStart = false;
    bool FirstSelect = false;
    bool BeforeStartGuideCheck = false;

    bool CheckTimeLimit = false;
    bool CheckTimeOut = false;

    float Guide_Length = 25;

    int Timer_Min = 1;
    int Timer_Sec = 30;

    float TimeLimit = 90;              //시간 제한 사용 방향 기획 필요
    float TimeLimitForFinish = 120;      //강제종료시간
    float TotalElapsedTime = 0;         //수행한 시간 계산용
    float TotalElapsedTimeForCalc = 0;  //수행한 시간 보여주기용
    float TimerForBeforeStarted = 0;    //시작하기 누르기까지 시간
    float TimerForFirstSelect = 0;      //시작하기 누르고 첫 선택까지 시간

    //float TotalScenePlayingTime = 0;    //컨텐츠 시작부터 끝까지 총 시간 TimerForBeforeStarted + TotalElapsedTimeForCalc

    public int TotalMovingCnt = 0;      //이동 횟수
    public int ResetCnt = 0;            //초기화 누른 횟수
    int ClickNoCnt = 0;                 //아니오 누른 횟수

    int SkipYn = 0;

    //------------- SOUND
    AudioSource audioSource;
    public AudioClip Sound_Count;
    public AudioClip Sound_In;
    public AudioClip Sound_Click;
    public AudioClip Sound_Put;

    //------------- Manager
    public Transform setData_PlayerData;
    public Transform saveData_GameDataMG;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        BeforeStart = true;
        SlotList = new List<Transform>();
        GrpList = new List<Transform>();

        foreach (Transform s in Slot)
        {
            SlotList.Add(s);
        }

        foreach (Transform g in Grp)
        {
            GrpList.Add(g);
        }

        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("GUIDE START");
    }

    void Update()
    {
        if (LeGogo)
        {
            TotalElapsedTime += Time.deltaTime;

            if (TotalElapsedTime > 1)
            {
                TotalElapsedTime = 0;
                TotalElapsedTimeForCalc += 1;

                if (Timer_Min != 0 || Timer_Sec != 0)
                {
                    Timer_Sec -= 1;

                    if (Timer_Sec < 0 && Timer_Min > 0)
                    {
                        Timer_Sec = 59;
                        Timer_Min = 0;
                    }

                    string TextSec = "";

                    if (Timer_Sec < 10)
                    {
                        TextSec = "0" + Timer_Sec.ToString();
                    }
                    else
                    {
                        TextSec = Timer_Sec.ToString();
                    }


                    TimerText.text = "0" + Timer_Min.ToString() + ":" + TextSec;
                }

                if (!CheckTimeLimit && TotalElapsedTimeForCalc >= TimeLimit)
                {
                    //시간제한
                    CheckTimeLimit = true;
                    StartCoroutine(TimeLimitAndKeepGoing());
                }

                if (!CheckTimeOut && TotalElapsedTimeForCalc >= TimeLimitForFinish)
                {
                    //강제종료
                    CheckTimeOut = true;
                    StartCoroutine(TimeOutAndFinish());
                }
            }
        }

        if (BeforeStart)
        {
            TimerForBeforeStarted += Time.deltaTime;

            if (!BeforeStartGuideCheck && TimerForBeforeStarted > Guide_Length)
            {
                Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("GUIDE END");
                BeforeStartGuideCheck = true;
            }
        }

        if (FirstSelect)
        {
            TimerForFirstSelect += Time.deltaTime;
        }
    }

    IEnumerator TimeLimitAndKeepGoing()
    {
        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("TIME LIMIT");
        BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("LIMIT");

        Timer_Sec = 30;

        yield return new WaitForSeconds(6);

        BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");
    }

    IEnumerator TimeOutAndFinish()
    {
        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("TIME OUT");
        BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("OUT");

        yield return new WaitForSeconds(6);

        FinishPanel_Yes(true);
    }

    public void LockAllCollision(Transform obj)
    {
        foreach (Transform go in GrpList)
        {
            if (go != obj)
            {
                go.GetComponent<PlanCubeController>().enabled = false;
            }
        }
    }

    public void ReleaseAllCollision()
    {
        foreach (Transform go in GrpList)
        {
            go.GetComponent<PlanCubeController>().enabled = true;
        }
    }

    public void CheckMovingCnt()
    {
        TotalMovingCnt += 1;
        FirstSelect = false;
    }

    bool CheckEmptySlot()
    {
        bool check = false;

        foreach (Transform s in SlotList)
        {
            if (s.GetComponent<PlanSlotController>().passenger != null)
            {
                check = true;
            }
        }

        return check;
    }

    public void reSetAll()
    {
        PlaySoundByTypes("CLICK");

        if (CheckEmptySlot())
        {
            for (int s = 0; s < SlotList.Count; s++)
            {
                SlotList[s].GetComponent<PlanSlotController>().resetPlanSlot();
            }

            for (int g = 0; g < GrpList.Count; g++)
            {
                GrpList[g].GetComponent<PlanCubeController>().resetPlanCube();
            }

            Btn_Finish.gameObject.SetActive(false);
            ResetCnt += 1;
        }
    }

    public void CheckAllScheduleOnSlot()
    {
        bool AllDone = true;

        foreach (Transform aSlot in SlotList)
        {
            if (aSlot.GetComponent<PlanSlotController>().passenger == null)
            {
                AllDone = false;
            }
        }

        if (AllDone)
        {
            Btn_Finish.gameObject.SetActive(true);
        }
    }


    public void DoStartSchedule()
    {
        PlaySoundByTypes("CLICK");
        StartCoroutine(StartCntDown());
    }

    IEnumerator StartCntDown()
    {
        BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");

        TextTitle.text = "준비 ~";

        yield return new WaitForSeconds(1f);
        PlaySoundByTypes("CNT");
        TextTitle.text = "3";
        yield return new WaitForSeconds(1);
        PlaySoundByTypes("CNT");
        TextTitle.text = "2";
        yield return new WaitForSeconds(1);
        PlaySoundByTypes("CNT");
        TextTitle.text = "1";
        yield return new WaitForSeconds(1);
        TextTitle.text = "시작 !";
        yield return new WaitForSeconds(1f);

        Intro.gameObject.SetActive(false);
        Schedule.gameObject.SetActive(true);
        BeforeStart = false;
        LeGogo = true;
        FirstSelect = true;

        if (!BeforeStartGuideCheck)
        {
            Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("GUIDE SKIP");
        }

        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("MISSION START");
    }

    public void ShowFinishPanel()
    {
        PlaySoundByTypes("CLICK");
        Finish.gameObject.SetActive(true);
    }

    public void FinishPanel_No()
    {
        PlaySoundByTypes("CLICK");
        Finish.gameObject.SetActive(false);

        ClickNoCnt += 1;
    }

    public void FinishPanel_Yes(bool Skipped)
    {
        PlaySoundByTypes("CLICK");
        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("MISSION END");

        LeGogo = false;
        float PlanData = 0;

        Schedule.gameObject.SetActive(false);
        Finish.gameObject.SetActive(false);
        WellDoneAndBye.gameObject.SetActive(true);

        Behavior.GetComponent<AutoVoiceRecording>().StopRecordingNBehavior();

        if (Skipped)
        {
            SkipYn = 1;
            Intro.gameObject.SetActive(false);
            //Schedule.gameObject.SetActive(true);
        }
        else
        {
            SkipYn = 0;
            string MySchedule = "";
            string MyScheduleforJson = "";

            foreach (Transform plan_Slot in SlotList)
            {
                Transform plan_Box = plan_Slot.GetComponent<PlanSlotController>().passenger.transform;

                if (plan_Box != null)
                {
                    MySchedule += plan_Box.GetChild(0).GetComponent<Text>().text + " ";
                    MyScheduleforJson += plan_Box.GetChild(1).name;
                }
                else
                {
                    MySchedule += "0 ";
                    MyScheduleforJson += "0";
                }
            }

            PlanData = float.Parse(MyScheduleforJson, System.Globalization.CultureInfo.InvariantCulture);
        }


        /*
        Data_201 계획을 완료하는데 걸린 총 시간                               TotalElapsedTimeForCalc
        Data_202 계획을 얼마나 바꾸는지(이동)                                 TotalMovingCnt
        Data_203 계획 초기화(다시하기)를 누른 횟수                            ResetCnt
        Data_204 완료 결정을 못하고 번복한 횟수                               ClickNoCnt
        Data_205 완료된 계획 전송                                             PlanData
        Data_206 중도 포기(스킵)                                              SkipYn
        Data_207 시작버튼 누르기 까지 걸린 시간                               TimerForBeforeStarted
        Data_208 시작하기 누른 후 첫번째 계획 선택까지 걸린 시간              TimerForFirstSelect
        */


        // 흩어져 있는 데이터들을 배열에 넣어 전달할 준비
        scene2arr = new float[] { TotalElapsedTimeForCalc, TotalMovingCnt, ResetCnt, ClickNoCnt, PlanData, SkipYn, TimerForBeforeStarted, TimerForFirstSelect };        
        saveData_GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();
        StartCoroutine(GoToNextScene());
    }

    IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(2);

        FinishCntDwn.text = "3";
        yield return new WaitForSeconds(1);

        FinishCntDwn.text = "2";
        yield return new WaitForSeconds(1);

        FinishCntDwn.text = "1";
        yield return new WaitForSeconds(1);

        SceneLoader.LoadScene(3);
    }

    
    public void PlaySoundByTypes(string Type)
    {
        audioSource.clip = null;

        if (Type == "IN")
        {
            audioSource.clip = Sound_In;
        }
        else if (Type == "CLICK")
        {
            audioSource.clip = Sound_Click; 
        }
        else if (Type == "CNT")
        {
            audioSource.clip = Sound_Count;
        }
        else if (Type == "PUT")
        {
            audioSource.clip = Sound_Put;
        }

        audioSource.Play();
    }
}
