using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
// ��ü Manager
// ��Ű���,Read,Write,Parse
namespace Arrange {
public class Manager : MonoBehaviour{

    /**************************************************************************
    // Global Constant / Parameter Definition
    ***************************************************************************/
    public static string INSTALL_PATH   = ".\\";                    //���α׷� ��ġ��� - ������ġ    
    public static string BUILD_DATE = "Build Date: 20211210";		//��Ʈ�� ȭ�� ���峯¥ ǥ��
    public static byte   SW_VER = 0x01;                               //  S/W����
    public static bool   bDEBUG = false; //true;                       // ���߽� Debug ���

    /**************************************************************************
    // Database structure Definition
    ***************************************************************************/
    // GLOBAL VARIABLES
    public enum             TAG { NECESSARY, UNNECESSARY };
    public static string [] saTag = { "Necessary", "Unnecessary"};

    /**************************************************************************
    // Arrange Object Info
    ***************************************************************************/
    public enum ARRANGE  { BOX, LAMP, GAME_PAD, GLOBE,  HAMMER, MOUTH_WASH, APPLE, TOY_CAR, 
            BOOK1, BOOK2, BOOK3,  TRASH1, TRASH2, TRASH3, CLOTH1,CLOTH2,CLOTH3,NONE  }
    public static string[] saCleanK =  {"�ڽ�", "����",  "�����е�", "������", "�и�ġ", "�μ�", "���", "�峭���ڵ���",
            "å1", "å2", "å3", "������1", "������2", "������3", "��1","��2","��3","" };
    public static string[] saKindE =  {"Box",  "Lamp", "GamePad","Globe","Hammer", "MouthWash", "Apple", "ToyCar",
            "Book1","Book2","Book3", "Trash1","Trash2","Trash3","Cloth1","Cloth2","Cloth3","" };
    public struct ARRANGE_INFO   {
        public ARRANGE eArrange;         //����        
        public bool    bArranged;           //������
        public string  src_Object_name;  //������ �;��� GameObject Name    
        public string  dst_Object_name;  //my GameObject Name
        public GameObject target;        // ������� ������ ���ٳ��� ��ġ
        public float   accept_radius;    //�ݶ��̴��ܿ� �ݰ�ȿ� ������ ���� ���������� ����
                                         
        public ARRANGE_INFO(ARRANGE eArrange,bool bArranged, string src_Object_name,string dst_Object_name, GameObject target,float  accept_radius) {         
            this.eArrange        =  eArrange;                   
            this.bArranged       =  bArranged;
            this.src_Object_name =  src_Object_name;
            this.dst_Object_name =  dst_Object_name;
            this.target          =  target;
            this.accept_radius   =  accept_radius;
        }
    }
    //����Ÿ�� ������ Hash�� Map�� ����ؾ������� �����Ƿ� �ϴ� �迭���
    public static ARRANGE_INFO[] CDB = new ARRANGE_INFO[]  {        
		new ARRANGE_INFO(ARRANGE.BOX,         false,  "Box",           "Box",        null, 0.5f),             
        new ARRANGE_INFO(ARRANGE.LAMP,        false,  "Lamp",          "Lamp",       null, 0.3f),                     
        new ARRANGE_INFO(ARRANGE.GAME_PAD,    false,  "GamePad",       "GamePad",    null, 0.3f),    
        new ARRANGE_INFO(ARRANGE.GLOBE,       false,  "Globe",         "Globe",      null, 0.5f),             
  		new ARRANGE_INFO(ARRANGE.HAMMER,      false,  "Hammer",        "Hammer",     null, 0.3f),             
        new ARRANGE_INFO(ARRANGE.MOUTH_WASH,  false,  "MouthWash",     "MouthWash",  null, 0.2f),             
        new ARRANGE_INFO(ARRANGE.APPLE,       false,  "Apple",         "Apple",      null, 0.2f),             
        new ARRANGE_INFO(ARRANGE.TOY_CAR,     false,  "ToyCar",        "ToyCar",     null, 0.3f),                     
        new ARRANGE_INFO(ARRANGE.BOOK1,       false,  "Book1",         "Book1",      null, 0.2f),             
        new ARRANGE_INFO(ARRANGE.BOOK2,       false,  "Book2",         "Book2",      null, 0.2f),             
        new ARRANGE_INFO(ARRANGE.BOOK3,       false,  "Book3",         "Book3",      null, 0.2f),             
        new ARRANGE_INFO(ARRANGE.TRASH1,      false,  "Trash1",        "Trash1",     null, 0.5f),             
        new ARRANGE_INFO(ARRANGE.TRASH2,      false,  "Trash2",        "Trash2",     null, 0.5f),             
        new ARRANGE_INFO(ARRANGE.TRASH3,      false,  "Trash3",        "Trash3",     null, 0.5f),             
        new ARRANGE_INFO(ARRANGE.CLOTH1,      false,  "Cloth1",        "Cloth1",     null, 0.2f),             
        new ARRANGE_INFO(ARRANGE.CLOTH2,      false,  "Cloth2",        "Cloth2",     null, 0.2f),             
        new ARRANGE_INFO(ARRANGE.CLOTH3,      false,  "Cloth3",        "Cloth3",     null, 0.2f),             
    };    
    // �迭�� ����ϹǷ� ����Ž��  
    public static ARRANGE GetArrangeByName(string name) {
        for(int i= 0; i< CDB.Length; i++) { if(CDB[i].src_Object_name == name) return CDB[i].eArrange;  }
        return ARRANGE.NONE;
    }    
    public void ResetCDB()  { for(int i =0; i< CDB.Length; i++)     { CDB[i].bArranged = false;  }    } 
    public GameObject[]  m_goTargets; //���⿡ Target Gameobject �Ҵ�
    public void InitCDB() { for(int i =0; i< CDB.Length; i++)     {  CDB[i].target = m_goTargets[i];  }   }
    /**************************************************************************
    // Disturbing Object Info
    ***************************************************************************/
    public enum DISTURB                  { DUCK }
    public static string[] saDisturbK =  { "����"  };
    public static string[] saDisturbE =  { "duck" };
    public struct DISTURB_INFO   {
        public DISTURB eDisturb;         //����        
        public string  name;             //GameObject Name    
        
        public DISTURB_INFO(DISTURB eDisturb,string name) {         
            this.eDisturb        =  eDisturb          ;       
            this.name            =  name;            
        }    
    }
    public static DISTURB_INFO[] DDB = new DISTURB_INFO[]  {
		new DISTURB_INFO(DISTURB.DUCK,      "duck"),                      
    };    
    public static DISTURB GetDisturbByName(string name) {
        for(int i= 0; i< DDB.Length; i++) {
            if(DDB[i].name == name) return DDB[i].eDisturb;                            
        }
        return DISTURB.DUCK;
    }
    /******************************************************/
    /*        Member Variable
    /******************************************************/
    
    /**************************************************************************
    // Method Start
    ***************************************************************************/

    public void Exit()    { Application.Quit(); }
    
    //�� �ʱ�ȭ
    void SetDefault() {    }


    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    public HUD      m_Hud;        
    public Guide    m_Guide;    
    
    //0.1sec�ֱ�� �������� Task�� ����ϼ���
    void Do01SecTask()  {
        ProcessUserAction();
    }

    //0.5sec�ֱ�� �������� Task�� ����ϼ���
    void Do05SecTask() {

    }

    //1sec�ֱ�� �������� Task�� ����ϼ���
    void Do1SecTask()   {
      
    }

    //����� Action�� ó���մϴ�
    void ProcessUserAction() {
    }

    // Start is called before the first frame update
    void Start()   {        
        //HMI���� App������ FrameRate�� 60�̻��ʿ����
        //Canvas UI�� Expensive�� ���ҽ���, Frame Rate�� ����Ʈ�εθ�(60~)�׷��� ������ CPU�� 30~50%������)
        Application.targetFrameRate = 30; //30���� �����ϸ� Serial Queue�� ����� ���� ����
        InitCDB();
        SetDefault();
        CommandLine();      // ����ȯ�� �Ķ���� ó��
        ReadSetting();
    }

    // Update is called once per frame
    float next01SecUpdate = 0;
    float next05SecUpdate = 0;
    float next1SecUpdate  = 0;

    // Update is called once per frame
    void Update()  {
        if (Time.time > next01SecUpdate) { Do01SecTask(); next01SecUpdate = Time.time + 0.1f; } //�ð�����
        if (Time.time > next05SecUpdate) { Do05SecTask(); next05SecUpdate = Time.time + 0.5f; } //�ð�����
        if (Time.time > next1SecUpdate) { Do1SecTask(); next1SecUpdate = Time.time + 1.0f; } //�ð�����
    }

    /*****************************************************************************
    //����� Command Parameter ó��
    ******************************************************************************/
    // ����� ����� ���
    public enum RUN_DEBUG_MODE {         
        SERVICE,    // �� ����ȯ��
        DEBUG,      // ����׸�� => ���Ϸ� �α׸� ����մϴ�.(DT, OT�� Ȱ��ȭ �Ͻʽÿ�
    };
    public static RUN_DEBUG_MODE m_eRunDebugMode = RUN_DEBUG_MODE.SERVICE;     //Command line arguments, Debug�ô� �α����� ����

    // Title Bar ���ֱ� Comman line�� -popupwindow �߰� 
    // ����ȯ�� ó��: default : aagwId=simulator1, debugMode=service userInput=network -popupwindow
    void CommandLine()   {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)        {
            if (args[i] == "-service") { m_eRunDebugMode = RUN_DEBUG_MODE.SERVICE; }
            if (args[i] == "-debug") { m_eRunDebugMode = RUN_DEBUG_MODE.DEBUG; }
        }
        //������ �ػ� ���� 
        //Screen.SetResolution(1920, 1080, false);
        //Util.SetPosition(0, 0);	 // ���� ����� �»�ܿ� ��ġ �ϵ���.	
    }
    void OnDestroy()   {
        WriteSetting();     
    }
    /**************************************************************************
    // Save & Load Simuator��������
    ***************************************************************************/    
	const string SETTING_FILE = "Setting.json";
    public struct SETTING {
        public string portName;
        public int    baud;
        public bool   bShowTime;
        public bool   bCapture;
        public bool   bCRLF;
        public SETTING(string portName, int baud, bool bShowTime, bool bCapture, bool bCRLF) {
            this.portName   = portName; 
            this.baud       = baud; 
            this.bShowTime  = bShowTime;
            this.bCapture   = bCapture; 
            this.bCRLF      = bCRLF;
        }
    }      
    public static SETTING  SET  = new  SETTING("COM3",9600,false,false,true);

	int ReadSetting(){        
        string json; 
        if(!Util.FileRead(SETTING_FILE, out json, INSTALL_PATH)) return 1; // try to the behavior for this id
        if(json == null || json == string.Empty) return 2; // no data found for this behavior
        try { 
            //������� ���� Json State������ �����Ƿ� �Ľ��մϴ�.
            var N = JSON.Parse(json);
            if(N.IsNull) {  Util.TLOG("Json Null"); return 3;  }                
            SET.portName    = N["portName"];
            SET.baud        = N["baud"].AsInt;
            SET.bShowTime   = N["bShowTime"].AsBool;           
            SET.bCapture    = N["bCapture"].AsBool;    
            SET.bCRLF       = N["bCRLF"].AsBool;

            //Debug.Log("portNmae="+ SET.portName+" baud= "+ SET.baud+" bShowTome= "+ SET.bShowTime
            //    +" bCapture= "+ SET.bCapture+" bCRLF= "+ SET.bCRLF);
        }
        catch (Exception e) { 
            Util.TLOG(e.ToString());
            return 4;
        }
       return 0; 
	}

    int WriteSetting(){
        string json = "{";
            json+="\"portName\":"+"\""      + SET.portName+"\""+ ",";
            json+="\"baud\":"+"\""          + SET.baud+"\""+ ",";
            json+="\"bShowTime\":"+"\""     + SET.bShowTime+"\""+ ",";            
            json+="\"bCapture\":"+"\""      + SET.bCapture+"\""+ ",";
            json+="\"bCRLF\":"+"\""         + SET.bCRLF+"\""+ ",";                       
            json+="}";
        Util.FileSave(SETTING_FILE,json, INSTALL_PATH);
        return 0; 
	}
}   
}
