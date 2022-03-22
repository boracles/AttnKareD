using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KetosGames.SceneTransition; //scene transition
using BNG;
namespace CleanRoom {
public class Guide : MonoBehaviour
{
    /**************************************************************************
    // Global Constant / Parameter Definition
    ***************************************************************************/
    public static  int      TOTAL_OBJECT    = 9;    //총 정리해야할 물건수
    public static  float    TIMEOUT_CLEAN   = 120f; //방청소 시간제한 2분_Timer
    public static  float    TIMEOUT_SCENE   = 240f; //Scene의 시간제한 4분, 다음게임으로    
    public int              NEXT_SCENE      = 6;
    public enum STATE { 
        INTRO, 
        FIND, 
        CLEAN,         
        END,  
        NEXT 
     }
    public enum HUD_REPORT  {
        PLAYED_INTRO_HOWTO,
        PLAYED_INTRO_CLEAN,
        PLAYED_INTRO_WELLDONE,
        PLAYED_INTRO_TIMEOUT,
        PLAYED_INTRO_MOVING,
        NONE,
    }
    
    /**************************************************************************
    // Database structure Definition
    ***************************************************************************/

    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/
    public static STATE   m_eState;    
    int     m_nTotFound;       //찾은갯수
    int     m_nTotCleaned;     //제위치에 정리된 갯수
    float   m_fTimeTaken;      //찾기와 정리하기에 걸리는 시간
    bool    m_bTimeOutClean;   //방청소시간 초과
    bool    m_bTImeOutScene;   //Scene Play시간 초과
    bool    m_bFindable;       //Player가 찾을 수 있는지 여부
    bool    m_bGrabbable;      //Player가 잡을 수 있는지 여부
    int     m_nFinBtnDown;     //Player가 Fin Button 클릿횟수        
    
    //Additional Data for Evaluation
    float   m_fTimeLookValid;  //Player가 필요한곳을 보는 시간 
    float   m_fTimeLookInvalid;//Player가 불필요한곳을 보는 시간     
    int     m_nHearingReplay;  //Player가 다시듣기 재생횟수 : 재생횟수 : SpeechBubles 처리?-- 어떤상황에서 활성화?
    int     m_nGrabCount;      //Player가 물건을 잡은 횟수
    float   m_fMoveDistance;   //Player 총이동거리 
    int     m_nObstacleTouch;  //Player가 방해 물체를 건든 횟수        
    /**************************************************************************
    // Method Start
    ***************************************************************************/    

    //정리할 물건을 찾음 from LaserPonter.cs
    public void SetFound(GameObject fgo) {
        m_nTotFound++;            
        m_Hud.PopUpCount(m_nTotFound);
        m_Hud.ShowStarParticle(fgo.transform);
        if(m_nTotFound>=TOTAL_OBJECT) m_Hud.PlayStartClean();        
    }
    //정리할 물건을 제자리에 위치시킴 from Target.cs
    public void SetCleaned(GameObject fgo) {
        m_nTotCleaned++;        
        m_Hud.PopUpCount(m_nTotCleaned,true);
        m_Hud.ShowStarParticle(fgo.transform);
        if (m_nTotCleaned >= TOTAL_OBJECT) m_Hud.PlayWellDone();
    }
    //from Hud.cs
    public void HudReport(HUD_REPORT eHudReport) {
        switch(eHudReport) {
        case HUD_REPORT.PLAYED_INTRO_HOWTO  : Make_Find();      break;
        case HUD_REPORT.PLAYED_INTRO_CLEAN  : Make_Clean();     break;
        case HUD_REPORT.PLAYED_INTRO_WELLDONE:Make_End();       break;
        case HUD_REPORT.PLAYED_INTRO_TIMEOUT: Make_Next();      break;  // check more later
        case HUD_REPORT.PLAYED_INTRO_MOVING : Make_Next();      break;
        }
    }

    //레이져포인터로 찾을 수 있게 해줍니다
    // Right Controller의 RightHandPointer gameobject enable 이때 RightHandPointer에 있는 LaserPonter.cs가 작동합니다
    public GameObject   m_goRightHandPointer;
    void MakeFindable(bool bOn)  {
        m_bFindable = bOn;
        m_goRightHandPointer.SetActive(m_bFindable);
    }

    //손으로 잡을수 있게 해줍니다
    //Right Grabber에 있는 GrabbablesInTrigger 스크립트 enable시켜주면 잡을 수 있습니다.
    public GameObject m_goRightGrabber;  
    void MakeGrabbable(bool bOn) {
        m_bGrabbable = bOn;
        m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled = m_bGrabbable;  
        //Debug.Log("MakeGrabble="+m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled);
    }

    //OnGrab Event처리 --> RightHand>Grabber에 Event에 할당   
    Grabber  m_RightGrabber;
    public void OnGrabRight() { 
        m_nGrabCount++;        
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject;            
        if(grab.tag != Manager.saTag[(int)Manager.TAG.NECESSARY]) m_nObstacleTouch++;
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck을 잡으면
            m_Hud.PlayDuck(true,HUD.DUCK_ACTION.GRABBED);
        }        
    }

    //OnGrabRelease Event처리 --> RightHand>Grabber에 Event에 할당   
    public void OnGrabRightRelease() {
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject;
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck을 놓으면
            m_Hud.PlayDuck(true);
        }        
    }

    //Fin Button 처리    
    public void OnFinButtonDown() {
        m_nFinBtnDown++;
        if(m_nFinBtnDown<2) m_Hud.PlayWarning();
        if(m_nFinBtnDown==2) {
            m_Hud.ShowMoving();
            Make_Next();
        }       
    }
    //평가관련 : 총이동거리를 계산합니다.
    public Transform  m_Character; //CenterEyeAnchor Transform할당
    Vector3 m_v3OldPosCharacter;
    void CalculateMoveDistance() {
        m_fMoveDistance += Vector3.Distance(m_Character.position, m_v3OldPosCharacter);
        m_v3OldPosCharacter = m_Character.position;
    }    

    //필요한 곳을 보는시간 : 찾기와 정리시간중에 necessary 이외 보는시간이 1초이상경과시 체크    
    void CalculateLookTime() {
         RaycastHit hit;
        if (Physics.Raycast(m_Character.position, m_Character.forward, out hit))  {
            if(hit.transform.gameObject.tag == Manager.saTag[(int)Manager.TAG.NECESSARY]) 
                m_fTimeLookValid += Time.deltaTime;
            else m_fTimeLookInvalid += Time.deltaTime;
        } else  m_fTimeLookInvalid += Time.deltaTime;                
    }

    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    public HUD    m_Hud;
    
    // Start is called before the first frame update
    void Start() {        
        m_RightGrabber = m_goRightGrabber.GetComponent<Grabber>();
        Make_Intro();         
    }

    // Update is called once per frame
    void Update() {
        switch (m_eState){
        case STATE.INTRO  :         Run_Intro();          break;
        case STATE.FIND   :         Run_Find();           break;
        case STATE.CLEAN  :         Run_Clean();          break;        
        case STATE.END    :         Run_End();            break;
        case STATE.NEXT   :         Run_Next();           break;
        }        
        CalculateMoveDistance();
    }
    
    void Make_Intro() {        
        MakeFindable(false);
        MakeGrabbable(false);
        m_Hud.PlayHowTo();
        m_eState = STATE.INTRO;        
    }
    void Run_Intro() { 
    }

    void Make_Find() {
        m_Hud.DisplayLeftHint();
        m_Hud.PlayVideo(true);
        m_Hud.PlayDuck(true);
        MakeFindable(true);
        m_eState = STATE.FIND;
    }
    void Run_Find() { 
        MeasureTime();
        CalculateLookTime();
        if(m_bTImeOutScene) Make_End();
    }

    void Make_Clean() {      
        m_Hud.ShowTarget(true);
        MakeGrabbable(true);
        m_eState = STATE.CLEAN;
    }

    void Run_Clean() { 
        MeasureTime();      
        CalculateLookTime();
        if(m_bTImeOutScene) Make_End();
    }            

    void Make_End() {
        m_Hud.PlayDuck(false);
        m_Hud.PlayVideo(false);
        MakeFindable(false);
        MakeGrabbable(false);
        //마지막에 해야할 평가작업등을 추가하십시요

        //이전상태가 TIMEOUT_SCENE상태에서 넘어오면 아쉬지만...표시
        if(m_bTImeOutScene) {
            m_Hud.PlayTimeOut();
        } else m_Hud.PlayMoving();                       

        m_eState = STATE.END;
    }
    void Run_End() { 

    }

    void Make_Next() {      
        Load_Next_Scene();
        m_eState = STATE.NEXT;
    }
    void Run_Next() { }


    /*****************************************************************************
    // Helper & Utility
    ******************************************************************************/
    void MeasureTime() {
        m_fTimeTaken += Time.deltaTime; 
        if(m_fTimeTaken > TIMEOUT_CLEAN) m_bTimeOutClean = true;
        if(m_fTimeTaken > TIMEOUT_SCENE) m_bTImeOutScene = true;
        if(!m_bTimeOutClean) m_Hud.DrawTimeTaken(TIMEOUT_CLEAN - m_fTimeTaken); 
    }
    // 다음 Scene으로 넘어가는 코드를 추가하세요
    void Load_Next_Scene()  {
        KetosGames.SceneTransition.SceneLoader.LoadScene(NEXT_SCENE);
        Debug.Log("다음신을 로드합니다");

    }
}
}
