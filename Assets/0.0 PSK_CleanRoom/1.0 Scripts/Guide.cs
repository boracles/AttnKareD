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
    public static  int      TOTAL_OBJECT    = 9;    //�� �����ؾ��� ���Ǽ�
    public static  float    TIMEOUT_CLEAN   = 120f; //��û�� �ð����� 2��_Timer
    public static  float    TIMEOUT_SCENE   = 240f; //Scene�� �ð����� 4��, ������������    
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
    int     m_nTotFound;       //ã������
    int     m_nTotCleaned;     //����ġ�� ������ ����
    float   m_fTimeTaken;      //ã��� �����ϱ⿡ �ɸ��� �ð�
    bool    m_bTimeOutClean;   //��û�ҽð� �ʰ�
    bool    m_bTImeOutScene;   //Scene Play�ð� �ʰ�
    bool    m_bFindable;       //Player�� ã�� �� �ִ��� ����
    bool    m_bGrabbable;      //Player�� ���� �� �ִ��� ����
    int     m_nFinBtnDown;     //Player�� Fin Button Ŭ��Ƚ��        
    
    //Additional Data for Evaluation
    float   m_fTimeLookValid;  //Player�� �ʿ��Ѱ��� ���� �ð� 
    float   m_fTimeLookInvalid;//Player�� ���ʿ��Ѱ��� ���� �ð�     
    int     m_nHearingReplay;  //Player�� �ٽõ�� ���Ƚ�� : ���Ƚ�� : SpeechBubles ó��?-- ���Ȳ���� Ȱ��ȭ?
    int     m_nGrabCount;      //Player�� ������ ���� Ƚ��
    float   m_fMoveDistance;   //Player ���̵��Ÿ� 
    int     m_nObstacleTouch;  //Player�� ���� ��ü�� �ǵ� Ƚ��        
    /**************************************************************************
    // Method Start
    ***************************************************************************/    

    //������ ������ ã�� from LaserPonter.cs
    public void SetFound(GameObject fgo) {
        m_nTotFound++;            
        m_Hud.PopUpCount(m_nTotFound);
        m_Hud.ShowStarParticle(fgo.transform);
        if(m_nTotFound>=TOTAL_OBJECT) m_Hud.PlayStartClean();        
    }
    //������ ������ ���ڸ��� ��ġ��Ŵ from Target.cs
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

    //�����������ͷ� ã�� �� �ְ� ���ݴϴ�
    // Right Controller�� RightHandPointer gameobject enable �̶� RightHandPointer�� �ִ� LaserPonter.cs�� �۵��մϴ�
    public GameObject   m_goRightHandPointer;
    void MakeFindable(bool bOn)  {
        m_bFindable = bOn;
        m_goRightHandPointer.SetActive(m_bFindable);
    }

    //������ ������ �ְ� ���ݴϴ�
    //Right Grabber�� �ִ� GrabbablesInTrigger ��ũ��Ʈ enable�����ָ� ���� �� �ֽ��ϴ�.
    public GameObject m_goRightGrabber;  
    void MakeGrabbable(bool bOn) {
        m_bGrabbable = bOn;
        m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled = m_bGrabbable;  
        //Debug.Log("MakeGrabble="+m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled);
    }

    //OnGrab Eventó�� --> RightHand>Grabber�� Event�� �Ҵ�   
    Grabber  m_RightGrabber;
    public void OnGrabRight() { 
        m_nGrabCount++;        
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject;            
        if(grab.tag != Manager.saTag[(int)Manager.TAG.NECESSARY]) m_nObstacleTouch++;
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck�� ������
            m_Hud.PlayDuck(true,HUD.DUCK_ACTION.GRABBED);
        }        
    }

    //OnGrabRelease Eventó�� --> RightHand>Grabber�� Event�� �Ҵ�   
    public void OnGrabRightRelease() {
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject;
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck�� ������
            m_Hud.PlayDuck(true);
        }        
    }

    //Fin Button ó��    
    public void OnFinButtonDown() {
        m_nFinBtnDown++;
        if(m_nFinBtnDown<2) m_Hud.PlayWarning();
        if(m_nFinBtnDown==2) {
            m_Hud.ShowMoving();
            Make_Next();
        }       
    }
    //�򰡰��� : ���̵��Ÿ��� ����մϴ�.
    public Transform  m_Character; //CenterEyeAnchor Transform�Ҵ�
    Vector3 m_v3OldPosCharacter;
    void CalculateMoveDistance() {
        m_fMoveDistance += Vector3.Distance(m_Character.position, m_v3OldPosCharacter);
        m_v3OldPosCharacter = m_Character.position;
    }    

    //�ʿ��� ���� ���½ð� : ã��� �����ð��߿� necessary �̿� ���½ð��� 1���̻����� üũ    
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
        //�������� �ؾ��� ���۾����� �߰��Ͻʽÿ�

        //�������°� TIMEOUT_SCENE���¿��� �Ѿ���� �ƽ�����...ǥ��
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
    // ���� Scene���� �Ѿ�� �ڵ带 �߰��ϼ���
    void Load_Next_Scene()  {
        KetosGames.SceneTransition.SceneLoader.LoadScene(NEXT_SCENE);
        Debug.Log("�������� �ε��մϴ�");

    }
}
}
