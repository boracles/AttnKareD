using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KetosGames.SceneTransition;
using HutongGames.PlayMaker;


public class PaddleManager : MonoBehaviour
{
    public PlayMakerFSM DataFsm;
    public Transform Behavior;
    public Transform BGM_Controller;

    public GameObject Canvas_Intro;
    public TextMeshProUGUI ShowIntroText;
    public GameObject Canvas_Display;
    public GameObject Canvas_Finish;
    public TextMeshProUGUI ShowCntDwnText;

    public TextMeshProUGUI ShowDisplayText;
    public Transform ShowPopUpText;

    public Transform Vehicle;

    public Transform Wheel_Parent_My;   //어떤 핸들을 잡았는지 식별용 - 내거
    public Transform Wheel_Parent_Bot;  //어떤 핸들을 잡았는지 식별용 - 친구거 실패

    public Transform AutoController;
    public Transform AutoGrabScript;      //시작 전에 잡는건 막아야할듯

    int StageLvl = 1;                   //스테이지 단계 1 ~ 3 속도가 점점 올라감
    
    float Failure_DistbT = 0;         //친구 손잡이를 잡고 있는 시간
    bool DisturbChecker = false;      //친구 방해 확인용
    int DisturbCnt = 0;               //친구 손잡이 잡은 횟수

    bool SceneStart = false;             //Scene 시작
    bool PaddleStart = false;           //시작
    float TimeElapsed = 0;
    int TimeElapsedShow = 0;

    float TimeElapsed_Timer = 0;
    int TimeElapsedShow_Timer = 0;

    bool MyPaddleOn = false;            //핸들을 잡았는지
    bool PaddleTimerSwitch = false;     //바르게 돌리고 있을때 시간 체크용
    float PaddleSpeedTimer = 0;         //핸들 속도 체크용
    int PaddleFailCnt = 0;              //친구와 속도 맞지 않은 횟수

    float DoNothingTimeElapsed = 0;             //안돌리는 시간
    bool DoNothingNow = true;             //안돌리는지 확인용

    int PaddleOutCnt = 0;               //핸들에서 손을 뗀 횟수
    bool CheckPaddleOutCnt = false;     //핸들에서 손을 뗀 횟수 - 1회용 bool

    bool GuideComplete = false;

    public Text TimerText;

    float Guide_Length = 29;

    int Timer_Min = 2;                  //2 분
    int Timer_Sec = 30;                 //30 초
    float TimeLimit = 150;              //시간 제한
    float TimeLimitForFinish = 180;     //강제종료시간

    bool CheckTimeLimit = false;
    bool CheckTimeOut = false;


    //------------- SOUND
    AudioSource audioSource;
    public AudioClip Sound_Count;
    public AudioClip Sound_Click;
    public AudioClip Sound_Success;
    public AudioClip Sound_Fail;
    public AudioClip Sound_StageUp;
    public AudioClip Sound_Finish;

    //------------- Manager
    public Transform setData_PlayerData;
    public Transform saveData_GameDataMG;


    public float Data_401 = 0;      //시작버튼 누르기 까지 걸린 시간
    public float Data_402 = 0;      //완료까지 걸린 총 시간
    public float Data_403 = 0;      //스테이지1 걸린 시간
    public float Data_404 = 0;      //스테이지2 걸린 시간
    public float Data_405 = 0;      //스테이지3 걸린 시간
    public float Data_406 = 0;      //스테이지1에서 협동을 지키지 않은 횟수
    public float Data_407 = 0;      //스테이지2에서 협동을 지키지 않은 횟수
    public float Data_408 = 0;      //스테이지3에서 협동을 지키지 않은 횟수
    public float Data_409 = 0;      //친구의 페달을 건드린 횟수
    public float Data_410 = 0;      //아무 행동도 하지 않은 총 시간
    public float Data_411 = 0;      //중도 포기(스킵)
    public float Data_412 = 0;      //친구 페달을 건드린 시간
    public float Data_413 = 0;      //페달에서 손을 뗀 횟수


    private void Start()
    {
        SceneStart = true;
        audioSource = transform.GetComponent<AudioSource>();
        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("GUIDE START");
    }

    void Update()
    {
        if (SceneStart)
        {
            TimeElapsed += Time.deltaTime;

            if (TimeElapsed > 1)
            {
                TimeElapsed = 0;
                TimeElapsedShow += 1;
            }


            if (!GuideComplete && TimeElapsedShow > Guide_Length)
            {
                Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("GUIDE END");
                GuideComplete = true;
            }


            if (PaddleStart)
            {
                TimeElapsed_Timer += Time.deltaTime;

                if (TimeElapsed_Timer > 1)
                {
                    TimeElapsed_Timer = 0;
                    TimeElapsedShow_Timer += 1;

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

                    if (!CheckTimeLimit && TimeElapsedShow_Timer >= TimeLimit)
                    {
                        //시간제한
                        CheckTimeLimit = true;
                        StartCoroutine(TimeLimitAndKeepGoing());
                    }

                    if (!CheckTimeOut && TimeElapsedShow_Timer >= TimeLimitForFinish)
                    {
                        //강제종료
                        CheckTimeOut = true;
                        StartCoroutine(TimeOutAndFinish());
                    }
                }

                if (Wheel_Parent_My.childCount != 0)
                {
                    MyPaddleOn = true;
                    CheckPaddleOutCnt = true;

/*                    if (DoNothingNow)
                    {
                        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("IDLE END");
                        DoNothingNow = false;
                    }*/
                }
                else
                {
                    MyPaddleOn = false;
                    PaddleTimerSwitch = false;
                    PaddleSpeedTimer = 0;
                }

                if (!MyPaddleOn && CheckPaddleOutCnt)
                {
                    PaddleOutCnt += 1;
                    CheckPaddleOutCnt = false;
                }

                if (Wheel_Parent_Bot.childCount != 0)
                {
                    //친구 핸들 잡고 있는 시간
                    Failure_DistbT += Time.deltaTime;

                    if (DisturbChecker)
                    {
                        //Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("DISTURB START");
                        DisturbChecker = false;
                        DisturbCnt += 1;
                    }
                }
                else
                {
                    if (!DisturbChecker)
                    {
                        //Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("DISTURB END");
                        DisturbChecker = true;
                    }
                }

                if (MyPaddleOn && PaddleTimerSwitch)
                {
                    //핸들 속도 계산용
                    PaddleSpeedTimer += Time.deltaTime;
                }


                if (!MyPaddleOn)
                {
                    DoNothingTimeElapsed += Time.deltaTime;

/*                    if (!DoNothingNow)
                    {
                        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("IDLE START");
                        DoNothingNow = true;
                    }*/
                }

                if (Vehicle.GetComponent<VehicleController>().Distance == 100)
                {
                    GameFinish(false);
                }
            }
        }
    }

    IEnumerator TimeLimitAndKeepGoing()
    {
        BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("LIMIT");
        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("TIME LIMIT");

        Timer_Sec = 30; //30

        yield return new WaitForSeconds(6.2f);

        BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");
    }

    IEnumerator TimeOutAndFinish()
    {
        BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("OUT");
        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("TIME OUT");

        yield return new WaitForSeconds(6);

        GameFinish(true);
    }

    public void DoStartPaddle()
    {
        Data_401 = TimeElapsedShow;
        PlaySoundByType("CLICK");

        if (!GuideComplete)
        {
            Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("GUIDE SKIP");
            GuideComplete = true;
        }

        StartCoroutine(StartPaddle());
    }

    IEnumerator StartPaddle()
    {
        BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");
        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("MISSION START");

        ShowIntroText.text = "준비 ~";
        yield return new WaitForSeconds(1);

        PlaySoundByType("CNT");
        ShowIntroText.text = "3";
        yield return new WaitForSeconds(1);

        PlaySoundByType("CNT");
        ShowIntroText.text = "2";
        yield return new WaitForSeconds(1);

        PlaySoundByType("CNT");
        ShowIntroText.text = "1";
        yield return new WaitForSeconds(1);

        ShowIntroText.text = "출발 !";
        yield return new WaitForSeconds(1);

        Canvas_Intro.SetActive(false);
        Canvas_Display.SetActive(true);

        PaddleStart = true;
        AutoGrabScript.GetComponent<BNG.Grabbable>().enabled = true;

        AutoController.GetComponent<AutoController>().SetPaddleAnimSpeed(StageLvl);
    }

    void ShowPopUpInfo(string info)
    {
        ShowPopUpText.GetComponent<PopUpController>().DoAvtivatePopUp(info);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (PaddleStart && col.collider.tag == "HANDLE_MY")
        {
            Debug.Log(StageLvl + "_" + PaddleSpeedTimer);

            MyPaddleSpeedForCal();
            PaddleTimerSwitch = false;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (PaddleStart && col.collider.tag == "HANDLE_MY")
        {
            PaddleTimerSwitch = true;
        }
    }

    void MyPaddleSpeedForCal()
    {
        if (StageLvl == 1)
        {
            if (PaddleSpeedTimer > 2.5f && PaddleSpeedTimer < 3.5f)
            {
                SuccessToGo();

                if (Vehicle.GetComponent<VehicleController>().Distance > 40)
                {
                    Data_403 = TimeElapsedShow - Data_401;
                    Data_406 = PaddleFailCnt;
                    PaddleFailCnt = 0;
                    StageLvl += 1;
                    ShowPopUpInfo("2 단계로 난이도가 변경됩니다.\n조금 더 빠르게 돌리세요 !");

                    PlaySoundByType("STAGE");
                    AutoController.GetComponent<AutoController>().SetPaddleAnimSpeed(StageLvl);
                }
            }
            else
            {
                FailToPause();
            }
        }
        else if (StageLvl == 2)
        {
            if (PaddleSpeedTimer > 1.5f && PaddleSpeedTimer < 2.5f)
            {
                SuccessToGo();

                if (Vehicle.GetComponent<VehicleController>().Distance > 70)
                {
                    Data_404 = TimeElapsedShow - Data_401 - Data_403;
                    Data_407 = PaddleFailCnt;
                    PaddleFailCnt = 0;
                    StageLvl += 1;
                    ShowPopUpInfo("3 단계로 난이도가 변경됩니다.\n빠르게 돌리세요 !");

                    PlaySoundByType("STAGE");
                    AutoController.GetComponent<AutoController>().SetPaddleAnimSpeed(StageLvl);
                }
            }
            else
            {
                FailToPause();
            }
        }
        else if (StageLvl == 3)
        {
            if (Vehicle.GetComponent<VehicleController>().Distance < 100)
            {
                if (PaddleSpeedTimer > 1f && PaddleSpeedTimer < 2f)
                {
                    SuccessToGo();
                }
                else
                {
                    FailToPause();
                }
            }
        }

        PaddleSpeedTimer = 0;
    }

    void SuccessToGo()
    {
        if (PaddleStart)
        {
            PlaySoundByType("SUCCESS");

            ShowPopUpInfo("성공 ~ !!");
            Vehicle.GetComponent<VehicleController>().PlusDistance();
        }
    }

    void FailToPause()
    {
        if (PaddleStart)
        {
            PlaySoundByType("FAIL");

            PaddleFailCnt += 1;
            ShowPopUpInfo("친구와 속도를 맞춰서 돌리세요");
        }
    }

    public void GameFinish(bool isSkipped)
    {
        PlaySoundByType("FIN");
        Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("MISSION END");
        StartCoroutine(FinishNSave());

        SceneStart = false;
        PaddleStart = false;

        transform.GetComponent<BoxCollider>().enabled = false;
        AutoController.GetComponent<AutoController>().GameFinish();
        Vehicle.GetComponent<VehicleController>().GameFinish();

        Data_402 = TimeElapsedShow;
        Data_405 = TimeElapsedShow - Data_401 - Data_403 - Data_404;
        Data_408 = PaddleFailCnt;
        Data_409 = DisturbCnt;
        Data_410 = DoNothingTimeElapsed;
        Data_412 = Failure_DistbT;
        Data_413 = PaddleOutCnt;

        if (isSkipped)
        {
            Data_411 = 1;


            if (StageLvl == 1)
            {
                Data_404 = 0;
                Data_405 = 0;
            }
            else if (StageLvl == 2)
            {
                Data_405 = 0;
            }

            ShowPopUpInfo("수고했어요! 짝짝짝 ~");
        }
        else
        {
            Data_411 = 0;
            ShowPopUpInfo("정상에 도착했어요!\n잘했어요! 짝짝짝 ~");
        }

        DataFsm.SendEvent("GameClear");
        saveData_GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();

        StartCoroutine(GoToNextScene());
    }

    IEnumerator FinishNSave()
    {
        Behavior.GetComponent<AutoVoiceRecording>().StopRecordingNBehavior();
        yield return new WaitForSeconds(1);
    }

    IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(3);
        Canvas_Finish.SetActive(true);

        yield return new WaitForSeconds(1);
        ShowCntDwnText.text = "3";

        yield return new WaitForSeconds(1);
        ShowCntDwnText.text = "2";

        yield return new WaitForSeconds(1);
        ShowCntDwnText.text = "1";

        yield return new WaitForSeconds(1);

        SceneLoader.LoadScene(7);       /// -------------------------- 다음컨텐츠 번호 넣어야 함
    }

    void PlaySoundByType(string sType)
    {
        audioSource.clip = null;

        if (sType == "SUCCESS")
        {
            audioSource.clip = Sound_Success;
        }
        else if (sType == "CLICK")
        {
            audioSource.clip = Sound_Click;
        }
        else if (sType == "CNT")
        {
            audioSource.clip = Sound_Count;
        }
        else if (sType == "FAIL")
        {
            audioSource.clip = Sound_Fail;
        }
        else if (sType == "STAGE")
        {
            audioSource.clip = Sound_StageUp;
        }
        else if (sType == "FIN")
        {
            audioSource.clip = Sound_Finish;
        }

        audioSource.Play();
    }
}
