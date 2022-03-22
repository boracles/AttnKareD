using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KetosGames.SceneTransition; //scene transition
using BNG;
namespace Arrange {
public class Guide : MonoBehaviour {
    /**************************************************************************
    // Global Constant / Parameter Definition
    ***************************************************************************/
    public static  int      TOTAL_OBJECT    = 17;    //�� �����ؾ��� ���Ǽ�
    public static  float    TIMEOUT_ARRANGE = 180f;  //��û�� �ð����� 3��_Timer
    public static  float    TIMEOUT_SCENE   = 300f;  //Scene�� �ð����� 5��, ������������    
    public int              NEXT_SCENE      = 6;
    public enum STATE { 
        INTRO,         
        ARRANGE,         
        END,  
        NEXT 
     }
    public enum HUD_REPORT  {
        PLAYED_HOWTO,        
        PLAYED_WELLDONE,
        PLAYED_TIMEOUT,
        PLAYED_MOVING,
        NONE,
    }
    
    /**************************************************************************
    // Database structure Definition
    ***************************************************************************/

    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/
    public static STATE   m_eState;        
    Manager.ARRANGE       m_eGrabbedArrange; //���� ������ ����
    int     m_nTotArranged;    //����ġ�� ������ ����
    float   m_fTimeTaken;      //�����ϴµ� �ɸ��� �ð�
    bool    m_bTimeOutArrange; //��û�ҽð� �ʰ�
    bool    m_bTImeOutScene;   //Scene Play�ð� �ʰ�    
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

    //������ ������ ���� Grabbed�ÿ� üũ
    public void GrabArrangeable(GameObject fgo) {                
        m_Hud.ShowStarParticle(fgo.transform);
        Make_Arrange();
    }
    //������ ������ ���ڸ��� ��ġ��Ŵ from Target.cs
    public void SetArranged(GameObject fgo) {
        m_nTotArranged++;        
        m_Hud.PopUpCount(m_nTotArranged,true);
        m_Hud.ShowStarParticle(fgo.transform);
        if (m_nTotArranged >= TOTAL_OBJECT) m_Hud.PlayWellDone();        
    }
    //from Hud.cs
    public void HudReport(HUD_REPORT eHudReport) {
        switch(eHudReport) {
        case HUD_REPORT.PLAYED_HOWTO  : 
            m_Hud.DisplayLeftHint();
            m_Hud.PlayVideo(true);
            m_Hud.PlayDuck(true);  
            Make_Arrange();      
            break;
        //case HUD_REPORT.PLAYED_INTRO_CLEAN  : Make_Clean();     break;
        case HUD_REPORT.PLAYED_WELLDONE:Make_End();       break;
        case HUD_REPORT.PLAYED_TIMEOUT: Make_Next();      break;  // check more later
        case HUD_REPORT.PLAYED_MOVING : Make_Next();      break;
        }
    }
    
    //Right Grabber�� �ִ� GrabbablesInTrigger ��ũ��Ʈ enable�����ָ� ������ ���� �� �ֽ��ϴ�.    
    //Right Controller�� RightHandPointer gameobject enable �̶� RightHandPointer�� �ִ� LaserPonter.cs�� �۵��Ǿ� �������� ������ �ֽ��ϴ�    
    public GameObject m_goRightGrabber;  
    public GameObject   m_goRightHandPointer;
    void MakeGrabbable(bool bOn) {
        m_bGrabbable = bOn;
        m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled = m_bGrabbable;          
        m_goRightHandPointer.SetActive(m_bGrabbable);
        //Debug.Log("MakeGrabble="+m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled);
    }

    //OnGrab Eventó�� --> RightHand>Grabber�� Event�� �Ҵ�   
    Grabber  m_RightGrabber;
    public void OnGrabRight() { 
        m_nGrabCount++;        
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject;     
        if(grab.tag == Manager.saTag[(int)Manager.TAG.NECESSARY]) {            
            m_eGrabbedArrange = Manager.GetArrangeByName(grab.name); //ã������ ����
            Manager.CDB[(int)m_eGrabbedArrange].target.SetActive(true);
        }else m_nObstacleTouch++;
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck�� ������
            m_Hud.PlayDuck(true,HUD.DUCK_ACTION.GRABBED);
        }        
    }

    //OnGrabRelease Eventó�� --> RightHand>Grabber�� Event�� �Ҵ�   
    public void OnGrabRightRelease() {
        Target target = Manager.CDB[(int)m_eGrabbedArrange].target.GetComponent<Target>();
        target.DeActivate();
        m_eGrabbedArrange = Manager.ARRANGE.NONE;
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
    STATE oldState;
    void Update() {
        switch (m_eState){
        case STATE.INTRO  :         Run_Intro();          break;        
        case STATE.ARRANGE:         Run_Arrange();        break;        
        case STATE.END    :         Run_End();            break;
        case STATE.NEXT   :         Run_Next();           break;
        }        
        CalculateMoveDistance();
        if(m_eState != oldState) Debug.Log(m_eState);
        oldState = m_eState;
    }
    
    void Make_Intro() {                
        MakeGrabbable(false);
        m_Hud.PlayHowTo();
        m_eState = STATE.INTRO;        
    }
    void Run_Intro() { 
    }

    void Make_Arrange() {      
        MakeGrabbable(true);
        m_eState = STATE.ARRANGE;
    }

    void Run_Arrange() { 
        MeasureTime();
        CalculateLookTime();
        if(m_bTImeOutScene) Make_End();
    }

    void Make_End() {
        m_Hud.PlayDuck(false);
        m_Hud.PlayVideo(false);        
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
        if(m_fTimeTaken > TIMEOUT_ARRANGE) m_bTimeOutArrange = true;
        if(m_fTimeTaken > TIMEOUT_SCENE) m_bTImeOutScene = true;
        if(!m_bTimeOutArrange) m_Hud.DrawTimeTaken(TIMEOUT_ARRANGE - m_fTimeTaken); 
    }
    // ���� Scene���� �Ѿ�� �ڵ带 �߰��ϼ���
    void Load_Next_Scene()  {
        KetosGames.SceneTransition.SceneLoader.LoadScene(NEXT_SCENE);
        Debug.Log("�������� �ε��մϴ�");

    }
}
}
