using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KetosGames.SceneTransition; //scene transition
using BNG;
namespace CleanUp {
public class Guide : MonoBehaviour {
    /**************************************************************************
    // Global Constant / Parameter Definition
    ***************************************************************************/

    public static  float    TIMEOUT_ARRANGE = 120f; //��û�� �ð����� 2��_Timer
    public static  float    TIMEOUT_SCENE   = 300f; //Scene�� �ð����� 5��, ������������    
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
 
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/
    public static STATE   m_eState;        
    public static Arrange.ARRANGE       m_eGrabbedArrange;
    public static Trash.TRASH           m_eGrabbedTrash;    //��������     
    public static Books.BOOKS           m_eGrabbedBooks;    //��������     
    bool    m_bGrabbable;      //Player�� ���� �� �ִ��� ����
        
    //Data for Evaluation
    bool    m_bTimeOutArrange; //��û�ҽð� �ʰ� ���� / �� 2����
    bool    m_bTImeOutScene;   //Scene Play�ð� �ʰ�����  / �� 5����     
    float   m_fTimeTaken;      //�����ϴµ� �ɸ��� �ð� : �������, ����ڰ� �ߴ�, Timeout�Ǿ� �ߴܵǵ� 

    float   m_fTimeLookValid;  //Player�� �ʿ��Ѱ��� ���� �ð� 
    float   m_fTimeLookVideo;  //Player�� ���ʿ��� ������ ���� �ð�
    float   m_fTimeLookInvalid;//Player�� ���ʿ��Ѱ��� ���� �ð�  
    //int     m_nHearingReplay;  //Player�� �ٽõ�� ���Ƚ�� : ���Ƚ�� : SpeechBubles ó��?-- ���Ȳ���� Ȱ��ȭ?
    
    float   m_fMoveDistance;   //Player ���̵��Ÿ� 
    int     m_nObstacleTouch;  //Player�� ���� ��ü�� �ǵ� Ƚ��        
    int     m_nFinBtnDown;     //Player�� Fin Button Ŭ��Ƚ��        
    /**************************************************************************
    // Method Start
    ***************************************************************************/    

    //������ ������ ���� Grabbed�ÿ� üũ
    public void GrabArrangeable(GameObject fgo) {                
        m_Hud.ShowStarParticle(fgo.transform);
        Make_Arrange();
    }

    //ó�� ������ ������ ��� ��ƼŬ ����-called by Arragne.cs     
    public void SetFirstArranged(Arrange.ARRANGE arranged, Transform dst) {   
    Debug.Log("First Arrange="+arranged);
        m_Hud.PopUpCount(Arrange.TOTAL_CLEANED,true);
        m_Hud.ShowStarParticle(dst);        
        GetArrangeStr();  //������ ���� ���ڿ� ����       
        m_Hud.NoteUpdateArrange(arrangedStr,arrangeableStr);        
    }
    public void TrashCleaned(Trash.TRASH Cleaned, Transform dst){
            Debug.Log("Cleaned" + Cleaned);
            m_Hud.PopUpCountTrash(Trash.TOTAL_POSITIONED, true);
            m_Hud.ShowStarParticle(dst);
            //GetArrangeStr();  //������ ���� ���ڿ� ����       
            //m_Hud.NoteUpdateArrange(arrangedStr, arrangeableStr);
    }
    public void BookPositioned(Books.BOOKS Cleaned, Transform dst)
        {
            Debug.Log("Positioned" + Cleaned);
            m_Hud.PopUpCountBooks(Books.TOTAL_POSITIONED, true);
            m_Hud.ShowStarParticle(dst);
            //GetArrangeStr();  //������ ���� ���ڿ� ����       
            //m_Hud.NoteUpdateArrange(arrangedStr, arrangeableStr);
    }

        // �Ź� �����ɶ����� ȣ��-called by Arragne.cs     
        public void SetPositioned(){    //(Arrange.ARRANGE arranged) {                        
        //Debug.Log("Set Postioned ="+ arranged + " Tot_Postioned= "+Arrange.TOTAL_POSITIONED + " Tot_Arrange= "+Arrange.TOTAL_ARRANGE);
        if (Trash.TOTAL_POSITIONED >= Trash.TOTAL_TRASH && Books.TOTAL_POSITIONED >= Books.TOTAL_BOOK) m_Hud.PlayWellDone();        
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
    public GameObject m_goRightHandPointer;
    void MakeGrabbable(bool bOn) {
        m_bGrabbable = bOn;
        m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled = m_bGrabbable;          
        m_goRightHandPointer.SetActive(m_bGrabbable);
        //Debug.Log("MakeGrabble="+m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled);
    }

    //OnGrab Eventó�� --> RightHand>Grabber�� Event�� �Ҵ�   
    Grabber  m_RightGrabber;
    public void OnGrabRight() {         
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject; 
        Arrange arrange = grab.GetComponent<Arrange>();
        Trash trash = grab.GetComponent<Trash>();
        Books books = grab.GetComponent<Books>();
            //Debug.Log(arrange);
            //Debug.Log(trash);
        if(arrange) {
            arrange.OnGrab();
            m_eGrabbedArrange = arrange.m_eArrange;
        }
        if(trash){
            trash.OnGrab();
            m_eGrabbedTrash = trash.m_eTrash;
        }
        if(books){
            books.OnGrab();
            m_eGrabbedBooks = books.m_eBooks;
        }
        if (grab.tag != Manager.saTag[(int)Manager.TAG.NECESSARY])m_nObstacleTouch++; 
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck�� ������
            m_Hud.PlayDuck(true,HUD.DUCK_ACTION.GRABBED);
        }        
    }

    //OnGrabRelease Eventó�� --> RightHand>Grabber�� Event�� �Ҵ�   
    public void OnGrabRightRelease() {
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject;
        Arrange arrange = grab.GetComponent<Arrange>();
        Trash trash = grab.GetComponent<Trash>();
        Books books = grab.GetComponent<Books>();
        if (arrange){
            arrange.OnGrabRelease();
            m_eGrabbedArrange = Arrange.ARRANGE.NONE;
        }
        if (trash){
            trash.OnGrabRelease();
            m_eGrabbedTrash = Trash.TRASH.NONE;
        }
        if (books){
            books.OnGrabRelease();
            m_eGrabbedBooks = Books.BOOKS.NONE;
        }
        
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
            Make_End();
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
            else if (hit.transform.gameObject.tag == Manager.saTag[(int)Manager.TAG.UNNECESSARY]) 
                    m_fTimeLookVideo += Time.deltaTime;
            else m_fTimeLookInvalid += Time.deltaTime;
        } 
        else  m_fTimeLookInvalid += Time.deltaTime;                
    }

    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    public HUD    m_Hud;
    public GameObject goArranges, goTrashes;
    public GameObject goBooks;
    Arrange[] m_aArranges; //������ ������ ���� ����
    Trash[] m_aTrashes; //������ ������ ���� ����
    Books[] m_aBooks; //������ ������ ���� ����

        // Start is called before the first frame update
        void Start() {        
        m_RightGrabber  = m_goRightGrabber.GetComponent<Grabber>();
        m_aArranges     = goArranges.GetComponentsInChildren<Arrange>(true);
        m_aTrashes      = goTrashes.GetComponentsInChildren<Trash>(true);
        m_aBooks        = goBooks.GetComponentsInChildren<Books>(true);
            Make_Intro();         
    }

    // Update is called once per frame
    STATE oldState;
    public TextMeshPro debugMod;
    public bool debugModOnoff = true;
    void Update() {
        if (debugModOnoff) {
                debugMod.text = ""
        + "numberOfTrash=" + Trash.TOTAL_TRASH + "\r\n" //Constant - �濡 �����ϴ� ������ ��
            + "CleanedTrash= " + Trash.TOTAL_POSITIONED + "\r\n" //Constant - ġ�� ������ ����
            + "TrashgrabCount= " + Trash.TOTAL_GRAB_COUNT + "\r\n" //Player�� �����⸦ ���� �� Ƚ��
            + "TrashgrabTime= " + Trash.TOTAL_GRAB_TIME + "\r\n" //Player�� �����⸦ ���� �� �ð�
            + "numberOfTrash=" + Books.TOTAL_BOOK + "\r\n" //Constant - �濡 �����ϴ� å ��
            + "CleanedTrash= " + Books.TOTAL_POSITIONED + "\r\n" //Constant - ������ å ����
            + "TrashgrabCount= " + Books.TOTAL_GRAB_COUNT + "\r\n" //Player�� å�� ���� �� Ƚ��
            + "TrashgrabTime= " + Books.TOTAL_GRAB_TIME + "\r\n" //Player�� å�� ���� �� �ð�

            + "m_bTimeOutArrange= " + m_bTimeOutArrange + "\r\n"       //��û�ҽð� �ʰ� ���� / �� 2����
            + "m_bTImeOutScene= " + m_bTImeOutScene + "\r\n"        //Scene Play�ð� �ʰ�����  / �� 5����         
            + "m_fTimeTaken= " + m_fTimeTaken + "\r\n"        //�����ϴµ� �ɸ��� �ð� : �������, �����        
            + "m_fTimeLookValid= " + m_fTimeLookValid + "\r\n"        //Player�� �ʿ��Ѱ��� ���� �ð� 
            + "m_fTimeLookVideo= " + m_fTimeLookVideo+ "\r\n"        //Player�� ���ʿ��Ѱ��� ���� �ð�             
            + "m_fTimeLookInvalid= " + m_fTimeLookInvalid + "\r\n"        //Player�� ���ʿ��Ѱ��� ���� �ð�             
            + "m_fMoveDistance= " + m_fMoveDistance + "\r\n"        //Player ���̵��Ÿ� 
            + "m_nObstacleTouch= " + m_nObstacleTouch + "\r\n"        //Player�� ���� ��ü�� �ǵ� Ƚ��        
            + "m_nFinBtnDown= " + m_nFinBtnDown + "\r\n";       //Player�� Fin Button Ŭ��Ƚ��
            }
            
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
        ReportData();
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

    public float[] m_dataReportFloat = new float[10];// = new float[10];

    void ReportData() {

            //Summary ����Ÿ ó��
            m_dataReportFloat[0] = m_fMoveDistance; // �� �̵� �Ÿ�
            m_dataReportFloat[1] = Trash.TOTAL_TRASH- Trash.TOTAL_POSITIONED; // ���� ������ ��
            m_dataReportFloat[2] = Books.TOTAL_BOOK - Books.TOTAL_POSITIONED; // ���� å ��
            m_dataReportFloat[3] = Trash.TOTAL_GRAB_COUNT + Books.TOTAL_GRAB_COUNT; //Player�� �ʿ��� ������ ���� �� Ƚ��
            m_dataReportFloat[4] = Trash.TOTAL_GRAB_TIME + Books.TOTAL_GRAB_TIME; //Player�� �ʿ��� ������ ���� �� �ð�
            m_dataReportFloat[5] = m_fTimeLookVideo; //��̷ο� ���� ��û�ð�
            m_dataReportFloat[6] = m_nObstacleTouch; // ���ع�ü(����)�� ���� �� Ƚ��
            m_dataReportFloat[7] = m_nFinBtnDown; //Player�� Fin Button Ŭ�� Ƚ��   
            m_dataReportFloat[8] = 0;
            m_dataReportFloat[9] = 0;
            
            
            string eval = ""
            + "numberOfTrash=" + Trash.TOTAL_TRASH + "\r\n" //Constant - �濡 �����ϴ� ������ ��
            + "CleanedTrash= " + Trash.TOTAL_POSITIONED + "\r\n" //Constant - ġ�� ������ ����
            + "TrashgrabCount= " + Trash.TOTAL_GRAB_COUNT + "\r\n" //Player�� �����⸦ ���� �� Ƚ��
            + "TrashgrabTime= " + Trash.TOTAL_GRAB_TIME + "\r\n" //Player�� �����⸦ ���� �� �ð�
            + "numberOfTrash=" + Books.TOTAL_BOOK + "\r\n" //Constant - �濡 �����ϴ� å ��
            + "CleanedTrash= " + Books.TOTAL_POSITIONED + "\r\n" //Constant - ������ å ����
            + "TrashgrabCount= " + Books.TOTAL_GRAB_COUNT + "\r\n" //Player�� å�� ���� �� Ƚ��
            + "TrashgrabTime= " + Books.TOTAL_GRAB_TIME + "\r\n" //Player�� å�� ���� �� �ð�

            + "m_bTimeOutArrange= " + m_bTimeOutArrange + "\r\n"       //��û�ҽð� �ʰ� ���� / �� 2����
            + "m_bTImeOutScene= " + m_bTImeOutScene + "\r\n"        //Scene Play�ð� �ʰ�����  / �� 5����         
            + "m_fTimeTaken= " + m_fTimeTaken + "\r\n"        //�����ϴµ� �ɸ��� �ð� : �������, �����        
            + "m_fTimeLookValid= " + m_fTimeLookValid + "\r\n"        //Player�� �ʿ��Ѱ��� ���� �ð� 
            + "m_fTimeLookVideo= " + m_fTimeLookVideo + "\r\n"        //Player�� ���ʿ��Ѱ��� ���� �ð�             
            + "m_fTimeLookInvalid= " + m_fTimeLookInvalid + "\r\n"        //Player�� ���ʿ��Ѱ��� ���� �ð�             
            + "m_fMoveDistance= " + m_fMoveDistance + "\r\n"        //Player ���̵��Ÿ� 
            + "m_nObstacleTouch= " + m_nObstacleTouch + "\r\n"        //Player�� ���� ��ü�� �ǵ� Ƚ��        
            + "m_nFinBtnDown= " + m_nFinBtnDown + "\r\n";       //Player�� Fin Button Ŭ��Ƚ��        
            /*
            string eval = ""
            +"m_nTotArrangeable= "  + Arrange.TOTAL_ARRANGE      +"\r\n" //Constant - ������ ������ �ִ� ���Ǽ�
            +"m_nTotCleanable= "    + Arrange.TOTAL_TOCLEAN      +"\r\n" //Constant - ó������ ����������, ������ ���Ǽ�, �������� ���ڸ��� �ִ� ������ �ִ� ����
            +"m_nTotCleaned= "      + Arrange.TOTAL_CLEANED      +"\r\n" //�����ҹ����� ����ġ�� ������ ����
            +"m_nTotPositoned= "    + Arrange.TOTAL_POSITIONED   +"\r\n" //��ü ������ ������ �ִ� ������ ���������� ���ڸ��� �ִ� ���Ǽ�        
            +"m_nTotGrabCount= "    + Arrange.TOTAL_GRAB_COUNT   +"\r\n" //Player�� ������ ���� �� Ƚ��
            +"m_nTotGrabTime= "     + Arrange.TOTAL_GRAB_TIME    +"\r\n" //Player�� ������ ���� �� �ð�
            +"m_nTotPosiCount= "    + Arrange.TOTAL_POSI_COUNT   +"\r\n" //Player�� ������ ���ڸ��� ���� ���� Ƚ��
            +"m_bTimeOutArrange= "  + m_bTimeOutArrange    +"\r\n"       //��û�ҽð� �ʰ� ���� / �� 2����
            +"m_bTImeOutScene= "    + m_bTImeOutScene     +"\r\n"        //Scene Play�ð� �ʰ�����  / �� 5����         
            +"m_fTimeTaken= "       + m_fTimeTaken        +"\r\n"        //�����ϴµ� �ɸ��� �ð� : �������, �����        
            +"m_fTimeLookValid= "   + m_fTimeLookValid    +"\r\n"        //Player�� �ʿ��Ѱ��� ���� �ð� 
            +"m_fTimeLookInvalid= " + m_fTimeLookInvalid  +"\r\n"        //Player�� ���ʿ��Ѱ��� ���� �ð�             
            +"m_fMoveDistance= "    + m_fMoveDistance     +"\r\n"        //Player ���̵��Ÿ� 
            +"m_nObstacleTouch= "   + m_nObstacleTouch    +"\r\n"        //Player�� ���� ��ü�� �ǵ� Ƚ��        
            +"m_nFinBtnDown= "      + m_nFinBtnDown       +"\r\n";       //Player�� Fin Button Ŭ��Ƚ��        

            Debug.Log(eval);   // to console
            */
            Util.ELOG(eval);   // to logfile .//Eval.txt
                 
         //�� ���Ǻ� ����(Arrange.CDB ����Ÿ ó��        
        for(int i=0; i < Arrange.TOTAL_ARRANGE; i++) {           
            eval = "" 
               +"eArrange= "    + Arrange.CDB[i].eArrange    + ", "     //����
               +"Object_name= " + Arrange.CDB[i].Object_name + ", "     //GameObject Name 
               +"bCleanable= "  + Arrange.CDB[i].bCleanable  + ", "     //û�Ҵ�󿩺�
               +"bPositioned= " + Arrange.CDB[i].bPositioned + ", "     //����ġ����
               +"nGrabCount= "  + Arrange.CDB[i].nGrabCount  + ", "     //Player�� �� ������ ���� Ƚ��
               +"fGrabTime= "   + Arrange.CDB[i].fGrabTime   + ", "     //Player�� �� ������ ���� �ð�
               +"nPosiCount = " + Arrange.CDB[i].nPosiCount  + ", "     //�ڱ���ġ�� �� Ƚ��
               +"kor_name= "    + Arrange.CDB[i].kor_name    + ", "     //�ѱ۸�
               +"\r\n";
            //Debug.Log(eval);   // to console
            //Util.ELOG(eval);   // to logfile .//Eval.txt
         }
    }

    /*****************************************************************************
    // Helper & Utility
    ******************************************************************************/
    void MeasureTime() {
        m_fTimeTaken += Time.deltaTime; 
        if(m_fTimeTaken > TIMEOUT_ARRANGE) m_bTimeOutArrange = true;
        if(m_fTimeTaken > TIMEOUT_SCENE)  m_bTImeOutScene = true;
        if(!m_bTimeOutArrange) m_Hud.DrawTimeTaken(TIMEOUT_ARRANGE - m_fTimeTaken); 
    }
    // ���� Scene���� �Ѿ�� �ڵ带 �߰��ϼ���
    void Load_Next_Scene()  {
        KetosGames.SceneTransition.SceneLoader.LoadScene(NEXT_SCENE);
        Debug.Log("�������� �ε��մϴ�");
    }
    //������ ����, ������ ���� ���� ���ڿ� ó�� -for hud
    string arrangedStr, arrangeableStr;
    void GetArrangeStr() {
        arrangedStr = arrangeableStr = "";
        foreach(Arrange arrange in m_aArranges) {
            if (arrange.m_bCleanable) {
                string name = Arrange.CDB[(int)arrange.m_eArrange].kor_name;
                if(arrange.m_bPositioned) arrangedStr += name+",";
                else arrangeableStr += name+",";
            }            
        }
    }    
}
}

