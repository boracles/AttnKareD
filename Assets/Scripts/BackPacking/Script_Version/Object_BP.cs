using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using EPOOutline;
using DG.Tweening;
using KetosGames.SceneTransition;
using UnityEngine.SceneManagement;

public class Object_BP : MonoBehaviour
{
    /*
     * 고학년 버전 필요한 오브젝트
     * 1단계
     * 연필 3개, 빨간볼펜, 초록 볼펜, 지우개 (필요 없는 학용품 : 검정볼펜, 파랑볼펜)
     * 필통 뚜껑
     * 
     * 2단계
     * 필통
     * 교과서 : 국어, 과학, 미술, 영어 (필요 없는 책 : 사회, 수학, 음악, 체육, 도덕)
     * 딱풀
     * 
     * 방해물
     *  장난감 기차, 장난감 차, 폴더, 물감, 노트
     */
    public enum OBJ_BP { DISTURB, PENCIL, PEN, ERASER, TXTBOOK, GLUE, PCAP, PCASE, CRAYON, WATERBOTTLE}
    public enum TAG_BP {NECESSARY, UNNECESSARY, NECESSARY_PENCIL, NECESSARY_BOOK}
    public enum KIND_BP { NONE, GREEN, RED, BLUE, PURLPLE, BLACK, KOREAN, SCIENCE, ART, ENGLISH, SOCIALS, MATH, MUSIC, GYM, ETHICS, SCHOOL, TOY }

    public enum GAZE_BP { MEMO, TV, TIMETABLE, NOTWATCHING }
    public enum CASE_BP { INCORRECT, CORRECT, COMPLETE, STAR, BEEP, APPEAR, PENCILCASE }
    public enum STATE { ENTER, EXIT }
    public struct BP_INFO
    {
        public OBJ_BP eObj;
        public KIND_BP eKind;
        public bool bCorrect;

        public BP_INFO(OBJ_BP eObj, KIND_BP eKind, bool bCorrect)
        {
            this.eObj = eObj;
            this.eKind = eKind;
            this.bCorrect = bCorrect;
        } 
    }
    public static BP_INFO[] BP1DB = new BP_INFO[]
    {
        
        new BP_INFO(OBJ_BP.PENCIL, KIND_BP.NONE, true ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.GREEN, true ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.RED, true ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.BLUE, false ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.PURLPLE, false ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.BLACK, false )


    };
    public static BP_INFO[] BP2DB = new BP_INFO[]
    {

        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.KOREAN, true ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.SCIENCE, true ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.ART, true ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.ENGLISH, true ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.SOCIALS, false ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.MATH, false ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.ETHICS, false ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.MUSIC, false ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.GYM, false ),
        new BP_INFO(OBJ_BP.GLUE, KIND_BP.NONE, true),
        new BP_INFO(OBJ_BP.PCASE, KIND_BP.NONE, true)

    };

    //PACK_DISTRACTION
    // Start is called before the first frame update
    public bool YOUNG;

    UI_BP Hud;
    BagPack_BP Bag;
    Pencilcase_BP Pencilcase;
    BagPack_BP_Young Bag_Young;
    GazedTime_BP GazeTime;
    GrabbedObject_BP Grabbed;
    MemoCheck_BP MemoCheck;
  //  Pencilcase_BP_Young Pencilcase;

    public InputBridge XrRig;
    public GameObject CenterEye;
    public GameObject prefabFadeIn;
    public GameObject prefabFadeOut;
    public Transform RightController;
    public static bool bGrabbed;
    public GameObject[] arrStage2;
    public GameObject VideoPlayer;
    public GameDataManager SaveData;
    //
    public float TimeLimit;
    public float TimeFin;
    //DATA
    public CollectData DataCollect;
    public AutoVoiceRecording BehaviorData;
    AddDelimiter Delimiter;
    bool m_bTotalTime = false; //총시간 확인
    public float m_fTotalTime;
    public bool m_bStageChangeTime = true; //1단계 완료 후 2단계 시작까지 걸리는 시간
    float m_fStageChangeTime;
    bool bTimeLimit;
    bool bTimeDone;

    GameObject m_goFade;
    GameObject m_tRightPointer;
    Grabber m_tGrabber;

    float data_501 = 0; //TOTAL TIME
    float data_502 = 0; //MEMO COUNT
    float data_503 = 0; //TIMETABLE GAZE TIME
    float data_504 = 0; // UNNECESSARY GRAB TIME
    float data_505 = 0; //WRONMG PUT COUNT
    float data_506 = 0;
    float data_507 = 0;// STAGE 1 -> STAGE 2 TIME
    float data_508 = 0; //DISTARCTED VIDEO TIME
    float data_509 = 0; //SKIP
    float data_510 = 0; //UNNECESSARY GRAB COUNT
    /*DATA NEEDED
     *  (501)
     *  (502)
     *  (503)
     * (504) //방해요소와 필요한 학용품 구분
     * (505)
     * STAGE 2 GRAB IN STAGE 1 (506) //없애기
     *  (507)
     *  (508)
     * SKIP(509)
     * UNNECESSARY GRAB COUNT(509)
     */


    public List<Grabbable> listGrabbable; //list of all grabbable;
    public int buildindex;

    void Start()
    {
        foreach (Grabbable grab in listGrabbable)
        {
            grab.enabled = false;
        }
 //       m_tPencilcase = GameObject.Find("Pencilcase_complete");
        Hud = GameObject.Find("UI").GetComponent<UI_BP>();
        Pencilcase = GameObject.Find("Pencilcase_Collider").GetComponent<Pencilcase_BP>();
        Grabbed = GameObject.FindObjectOfType<GrabbedObject_BP>();
        Bag_Young = GameObject.Find("Bag_Collider").GetComponent<BagPack_BP_Young>();
        Bag = GameObject.Find("Bag_Collider").GetComponent<BagPack_BP>();
        Delimiter = GameObject.Find("DataCheck_Manager").GetComponent<AddDelimiter>();
        GazeTime = GameObject.Find("HighlightAtGaze").GetComponent<GazedTime_BP>();
        MemoCheck = GameObject.FindObjectOfType<MemoCheck_BP>();
        m_tRightPointer = RightController.Find("RightHandPointer").gameObject;
        m_tGrabber = RightController.Find("Grabber").GetComponent<Grabber>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        m_tRightPointer.SetActive(false);
        m_tGrabber.enabled = false;
        m_goFade =  Instantiate(prefabFadeOut, CenterEye.transform.position, Quaternion.identity);
        m_goFade.transform.SetParent(CenterEye.transform);
        yield return new WaitForSeconds(1.2f);
        KetosGames.SceneTransition.SceneLoader.LoadScene(buildindex);
    }

    IEnumerator FadeIn()
    {
        m_goFade = Instantiate(prefabFadeIn, CenterEye.transform.position, Quaternion.identity);
        m_goFade.transform.SetParent(CenterEye.transform);
        Debug.Log("wait");
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(Hud.CanvasStart());
        DataCollect.AddTimeStamp("GUIDE START");
    }
    void Update()
    {
        if (XrRig.RightTrigger > 0.8f) bGrabbed = true;
        if (XrRig.RightTrigger < 0.2f) bGrabbed = false;
        if (m_bTotalTime)
        {
            m_fTotalTime += Time.deltaTime;
            if (m_fTotalTime >= TimeLimit & !bTimeLimit) { TimeCheck("TIME LIMIT"); bTimeLimit = true; }
            if (m_fTotalTime >= TimeFin & !bTimeDone) { TimeCheck("TIME OUT"); StartCoroutine(GameDone()); bTimeDone = true; }
        }
        if (m_bStageChangeTime) m_fStageChangeTime += Time.deltaTime;

    }

    public void Stage1()
    {
        DataCollect.AddTimeStamp("GUIDE END");
        DataCollect.AddTimeStamp("MISSION START");
        VideoPlayer.SetActive(true);
        Hud.ChangeMemo(1);
        m_bTotalTime = true;
        StartCoroutine(Hud.StageNotification(1));
        m_tRightPointer.SetActive(true);
        m_tGrabber.enabled = true;
        foreach (Grabbable grab in listGrabbable)
        {
            grab.enabled = true;
        }
    }
    public void Stage2()
    {
        m_bStageChangeTime = true;
        if (YOUNG) Bag_Young.bStage2 = true; 
        if(!YOUNG) Bag.bStage2 = true;
        Hud.ChangeMemo(2);
        StartCoroutine(Hud.StageNotification(2));
    }

    void TimeCheck(string strTime) //Add timestamp (TIME LIMIT, TIME OVER) & Show Warning
    {
        DataCollect.AddTimeStamp(strTime);
        StartCoroutine(Hud.TimeCheck(strTime));
    }

    void DataGather()
    {
        data_501 = m_fTotalTime;
        data_502 = MemoCheck.fNote;
        data_503 = GazeTime.m_fTimetable;
        data_504 = Grabbed.m_fbpUnpkT;
        if (!YOUNG)
        {
            data_505 = Bag.WrongPut + Pencilcase.WrongPut;
            data_506 = Bag.fStage1Try;
        }
        if (YOUNG)
        {
            data_505 = Bag_Young.WrongPut + Pencilcase.WrongPut;
            data_506 = Bag_Young.fStage1Try;
        }
       
        data_507 = m_fStageChangeTime;
        data_508 = GazeTime.m_fTV;
        //data_509 == skip
        data_510 = Grabbed.m_fbpUnpC;
    }
    public IEnumerator GameDone()
    {
        yield return new WaitUntil(() => Hud.bEndUI == true);
        yield return StartCoroutine(Hud.GameFinish());
        NextScene();
    }

    public void Skip()
    {
        data_509 = 1;
        NextScene();
    }
  
    public void NextScene()
    {
        Debug.Log("CALLED");
        m_bTotalTime = false;
        Delimiter.endEverything();
        DataCollect.AddTimeStamp("MISSION END");
        SaveData.SaveCurrentData();
        BehaviorData.StopRecordingNBehavior();
        StartCoroutine(FadeOut());
    }
   
}
