using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using KetosGames.SceneTransition;
using EPOOutline;

public class Guide_Paddle : MonoBehaviour
{

    public static float TIMELIMIT_PADDLE = 150f;
    public static float TIMEOUT_PADDLE = 200f;

    public enum PADDLE_STATE
    {
        INTRO,
        START,
        STAGE,
        ALLDONE
    }

    public static PADDLE_STATE m_ePSTATE;
    public int intStage;
    public CollectData BehaviorData;
    public MoveVehicle CableCar;
    private int m_nComplete;
    private string m_strOrder;
    public GrabPaddle GrabPaddle;
    public Animation FriendAnimation;
    bool m_bStamp = true;
    float m_nWrongSpeed;
    float m_nWrongOrder;

    float m_fPrevTime = 0;
    float m_fStageTime = 0;
    float m_fTOTALTIME =0;
    float m_fSTARTTIME;
    float[] m_listSTAGE = new float[] { 0, 0, 0 };
    float[] m_listOrder = new float[] { 0, 0, 0 };
    float[] m_listSpeed = new float[] { 0, 0, 0 };
    List<PaddleCollider> m_listCOLLIDER;

    //DATA
    public GameDataManager DataManager;
    float data_401; //시작버튼 누르는데까지 걸린 시간
    float data_402;//완료까지 걸린 총 시간
    float data_403, data_404, data_405; //스테이지 별 걸린 시간
    float data_406, data_407, data_408; //스테이지별 협동을 지키지 않은 횟수
    float data_409, data_410, data_411; //스테이지별 방향을 맞추지 않은 횟수
    float data_412; //친구의 페달을 건드린 횟수
    float data_413; //아무 행동도 하지 않은 총시간
    float data_414; //중도포기
    float data_415; //친구 페달을 건드린 시간
    float data_416; //페달에서 손을 땐 횟수
    public float[] arrData;
    void TimeCheck_Stage()
    {
        m_fTOTALTIME += Time.deltaTime;
        if (m_fTOTALTIME > TIMELIMIT_PADDLE) Hud.AudioController("time limit");
        if (m_fTOTALTIME > TIMEOUT_PADDLE) Hud.AudioController("time over");
    }

    void TimeCheck_Start()
    {
        m_fSTARTTIME += Time.deltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_listCOLLIDER = new List<PaddleCollider>(FindObjectsOfType<PaddleCollider>());
        Hud = GameObject.Find("Hud_Paddle").GetComponent<Hud_Paddle>();
        Make_INTRO(); //시작하기까지 시간체크
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_ePSTATE)
        {
            case PADDLE_STATE.INTRO:     Run_INTRO(); break;
            case PADDLE_STATE.START:     Run_START(); break;
            case PADDLE_STATE.STAGE:     Run_STAGE(); break; 
            case PADDLE_STATE.ALLDONE:   break;
        }
    }
    void Make_INTRO()
    {
        foreach (PaddleCollider collider in m_listCOLLIDER) collider.GetComponent<Collider>().enabled = false;
        //+hud start voice and canvas
        Hud.AudioController("guide");
        BehaviorData.AddTimeStamp("GUIDE START");
        m_ePSTATE = PADDLE_STATE.INTRO;
    }

    void Run_INTRO()
    {
        TimeCheck_Start();
        if (!Hud.bCoroutine)
        {
            if (m_bStamp) { Debug.Log("STAMP");  BehaviorData.AddTimeStamp("GUIDE END"); m_bStamp = false; }
        }
    }

    public void Make_START()
    {
        if (Hud.bCoroutine)
        {
            BehaviorData.AddTimeStamp("GUIDE SKIP");
        }
        foreach (PaddleCollider collider in m_listCOLLIDER) collider.GetComponent<Collider>().enabled = true;
        CableCar.GetComponent<Outlinable>().enabled = true;
        StartCoroutine(Hud.CountDown());
        FriendAnimation.Play("Intro");
        m_ePSTATE = PADDLE_STATE.START;
        Hud.BGMplay(true);
        BehaviorData.AddTimeStamp("MISSION START");
        Hud.bTimeStart = true;
    }
    
    void Run_START()
    {
        if (!Hud.bCoroutine)
        {
            Hud.AudioController("bgm");
            AnimationStart();
            Hud.bCoroutine = true;
        }
        TimeCheck_Stage();
    }

    void Make_STAGE()
    {
        StageTimeAdd();
        AnimationStart();
        m_ePSTATE = PADDLE_STATE.STAGE;
    }
    void Run_STAGE()
    {
        TimeCheck_Stage();
    }
   
    void Make_ALLDONE()
    {
        StageTimeAdd();
        Debug.Log("COMPLETE");
        foreach (PaddleCollider collider in m_listCOLLIDER) collider.GetComponent<Collider>().enabled = false; //paddle component 내부의 collider를 빼 더이상 체크 X
        m_ePSTATE = PADDLE_STATE.ALLDONE;
        GameFinish();
    }

    void AnimationStart()
    {
        string animName = "Paddle" + intStage.ToString(); //Change Animation of Character
        FriendAnimation.Play(animName);
    }

    void StageTimeAdd()
    {
        m_fStageTime = m_fTOTALTIME - m_fPrevTime;
        m_fPrevTime = m_fTOTALTIME;
        m_fStageTime = m_listSTAGE[intStage];
      //  m_nWrongOrder = m_listOrder[intStage];
      //  m_nWrongSpeed= m_listSpeed[intStage];
      //  m_nWrongSpeed = m_nWrongOrder = 0;  //요부분 바꿈 바로바로 저장되게
    }
    void NEXTSTAGE()
    {
        Hud.AudioController("stage");
        Debug.Log("CHECK_STAGE");
        m_nComplete = 0;
        Debug.Log(intStage);
        if (intStage >= 2) { Make_ALLDONE(); return; }
        Manager_Paddle.intStage++;
        intStage = Manager_Paddle.intStage;
        Make_STAGE();
    }
    public void Check_Order() //write in hud and datacheck 
    {
        Hud.AudioController("wrong order");
        m_listOrder[intStage] += 1;
        Debug.Log("CHECK_ORDER");

    }

    public void Check_Speed() //write in hud and data check
    {
        Debug.Log("CHECK_SPEED");
        Hud.AudioController("wrong speed");
        m_listSpeed[intStage] += 1;
    }

    public void PaddleCheck(float time)
    {
        m_strOrder = Manager_Paddle.SDB[intStage].strHANDLE;
        Manager_Paddle.SDB[intStage].strHANDLE = null; //한바퀴 돌면 strHANDEL을 null 시킴
        if (m_strOrder != Manager_Paddle.SDB[intStage].strORDER) { Debug.Log(m_strOrder+ intStage); Check_Order(); return; }
        if (time > 0.4f || time < -0.4f) { Check_Speed(); return; }
        m_nComplete += 1;
        CableCar.PlusDistance();
        Hud.SetDistance(intStage);
        if (Manager_Paddle.SDB[intStage].intCount == m_nComplete)
        {
            Debug.Log(m_listOrder[intStage]);
            NEXTSTAGE();//다음 스테이지 넘어가기
            return;
        }
        Hud.AudioController("correct");
        Debug.Log("CHECK" + Manager_Paddle.SDB[intStage].fTime + "  ," + Manager_Paddle.SDB[intStage].intCount + " , " + m_nComplete);
    }

    void GameFinish() // 게임 끝나면 어떻게 할지 여기에 추가
    {
        BehaviorData.AddTimeStamp("MISSION END");
        Hud.AudioController("complete");
        GrabPaddle.AllFinish();
        CableCar.GameFinish();
        FriendAnimation.Play("Finish");
        Datacollect();
    }

    public void Skip()
    {
        data_413 = 1;
        GameFinish();
    }
  
    IEnumerator NextScene()
    {
        Hud.NextScene();
        Hud.bCoroutine = true;
        yield return new WaitUntil(() => Hud.bCoroutine == false);
        Debug.Log("FIN");
        KetosGames.SceneTransition.SceneLoader.LoadScene(7);

    }

    void Datacollect()
    {
        BehaviorData.GetComponent<AutoVoiceRecording>().StopRecordingNBehavior();

        data_401 = m_fSTARTTIME;
        data_402 = m_fTOTALTIME;
        data_403 = m_listSTAGE[0];
        data_404 = m_listSTAGE[1];
        data_405 = m_listSTAGE[2];
        data_406 = m_listSpeed[0];
        data_407 = m_listSpeed[1];
        data_408 = m_listSpeed[2];
        data_409 = m_listOrder[0];
        data_410 = m_listOrder[1];
        data_411 = m_listOrder[2];
        data_412 = GrabPaddle.fDisturbCount;
        data_413 = GrabPaddle.fIdleTime;
        data_415 = GrabPaddle.fDisturbTime;
        data_416 = GrabPaddle.fIdleCount;
        Debug.Log("MID");
        arrData = new float[] { data_401, data_402, data_403, data_404, data_405, data_406, data_407, data_408, data_409, data_410 , data_411 ,data_412 , data_413, data_414, data_415, data_416 };
        for(int i = 0; i < arrData.Length; i++)
        {
           Debug.Log(arrData[i]);
        }
        //    DataManager.SaveCurrentData();
        StartCoroutine(NextScene());
    }

    Hud_Paddle Hud;
}
