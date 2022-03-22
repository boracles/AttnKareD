using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;//for Outlinable
using BNG;
namespace CleanUp {
public class Arrange : MonoBehaviour {

    public enum ARRANGE  { 
        //BOX, LAMP, GAME_PAD, GLOBE,  MOUTH_WASH, 
        TOY_HAMMER,  TOY_APPLE, TOY_CAR1, TOY_CAR2, TOY_GUN, 
        TOY_BALL, TOY_BUNNY, 
        BOOK1, BOOK2, BOOK3,  PANTS1 , PANTS2 , PANTS3 , NONE  
    }        
    
    //���������� �� ���ǿ� ���� �򰡰�� ����
    public struct ARRANGE_INFO   {
        public ARRANGE eArrange;         //����
        public string  Object_name;      //������ �;��� GameObject Name                   
        public bool    bCleanable;       //û�Ҵ�󿩺�
        public bool    bPositioned;      //����ġ����
        public int     nGrabCount;       //Player�� �� ������ ���� Ƚ��
        public float   fGrabTime;        //Player�� �� ������ ���� �ð�
        public int     nPosiCount;       //�ڱ���ġ�� �� Ƚ��
        public string  kor_name;         //�ѱ۸�
                                         
        public ARRANGE_INFO(ARRANGE eArrange, string Object_name, bool bCleanable , bool bPositioned, 
            int  nGrabCount , float fGrabTime , int  nPosiCount,string kor_name )  {         
            this.eArrange        =  eArrange;                              
            this.Object_name     =  Object_name;                        
            this.bCleanable      =  bCleanable ; 
            this.bPositioned     =  bPositioned;
            this.nGrabCount      =  nGrabCount ; 
            this.fGrabTime       =  fGrabTime  ;  
            this.nPosiCount      =  nPosiCount ; 
            this.kor_name        =  kor_name;
        }
    }
    // ���� �� �ִ� ���Ǽ� ; 13
    public static  int     TOTAL_ARRANGE    { get { return CDB.Length;}  }  
    // ������ ���Ǽ� : initial 8
    public static  int     TOTAL_TOCLEAN    { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) if(ainfo.bCleanable) nCount++; return nCount;  } }
    // ������ ���Ǽ�
    public static  int     TOTAL_CLEANED    { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) if(ainfo.bCleanable && ainfo.bPositioned) nCount++; return nCount;   } }
    //���� �� ��ġ�� �ִ� ���Ǽ� : initial 5
    public static  int     TOTAL_POSITIONED { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) if(ainfo.bPositioned) nCount++; return nCount;  } }
    // �� Grab Count
    public static  int     TOTAL_GRAB_COUNT { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) nCount+= ainfo.nGrabCount;  return nCount;  }  }
    // �� Grab Time
    public static  float   TOTAL_GRAB_TIME  { get { float time = 0; foreach(ARRANGE_INFO ainfo in CDB) time += ainfo.fGrabTime;  return time;  }  }
    // �� �ڱ���ġ�� ��Ƚ��
    public static  float   TOTAL_POSI_COUNT { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) nCount += ainfo.nPosiCount;  return nCount;  }  }

    public static ARRANGE_INFO[] CDB = new ARRANGE_INFO[]  {        
		//new ARRANGE_INFO(ARRANGE.BOX,         "Box",       false, false, 0, 0.0f, 0, "�ڽ�"   ),             
        //new ARRANGE_INFO(ARRANGE.LAMP,        "Lamp",      false, false, 0, 0.0f, 0, "����"   ),                     
        //new ARRANGE_INFO(ARRANGE.GAME_PAD,    "GamePad",   false, false, 0, 0.0f, 0, "�����е�" ),    
        //new ARRANGE_INFO(ARRANGE.GLOBE,       "Globe",     false, false, 0, 0.0f, 0, "������"  ),             
        //new ARRANGE_INFO(ARRANGE.MOUTH_WASH,  "MouthWash", false, false, 0, 0.0f, 0, "�μ�"   ),             
  		
        new ARRANGE_INFO(ARRANGE.TOY_HAMMER,  "ToyHammer", false, false, 0, 0.0f, 0, "�͸�ġ"  ),                     
        new ARRANGE_INFO(ARRANGE.TOY_APPLE,   "ToyApple",  false, false, 0, 0.0f, 0, "�峭�����"   ),             
        new ARRANGE_INFO(ARRANGE.TOY_CAR1,    "ToyCar1",   false, false, 0, 0.0f, 0, "�峭����" ),                     
        new ARRANGE_INFO(ARRANGE.TOY_CAR2,    "ToyCar2",   false, false, 0, 0.0f, 0, "�ڵ���2" ),                     
        new ARRANGE_INFO(ARRANGE.TOY_GUN,     "ToyGun",    false, false, 0, 0.0f, 0, "�峭������" ),                     
        new ARRANGE_INFO(ARRANGE.TOY_BALL,    "ToyBall",   false, false, 0, 0.0f, 0, "��"     ),                     
        new ARRANGE_INFO(ARRANGE.TOY_BUNNY,   "ToyBunny",  false, false, 0, 0.0f, 0, "�䳢"   ),   
        
        new ARRANGE_INFO(ARRANGE.BOOK1,       "Book1",     false, false, 0, 0.0f, 0, "���å1"    ),             
        new ARRANGE_INFO(ARRANGE.BOOK2,       "Book2",     false, false, 0, 0.0f, 0, "���å"    ),             
        new ARRANGE_INFO(ARRANGE.BOOK3,       "Book3",     false, false, 0, 0.0f, 0, "���å2"    ),                     
        new ARRANGE_INFO(ARRANGE.PANTS1,      "Pants1",    false, false, 0, 0.0f, 0, "����1"  ),             
        new ARRANGE_INFO(ARRANGE.PANTS2,      "Pants2",    false, false, 0, 0.0f, 0, "����2"  ),             
        new ARRANGE_INFO(ARRANGE.PANTS3,      "Pants3",    false, false, 0, 0.0f, 0, "����ݹ���"  ),             
    };    
  
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/  
    // Public... Assigned in Editor.
    // Editor���� ������ ���� �켱�̹Ƿ� ���⼭�� ����Ʈ�� false�� �ݴϴ�.
    public ARRANGE    m_eArrange       = ARRANGE.NONE; //Editor���� Kind�� �Ҵ��� �ݴϴ�
    public bool       m_bCleanable     = false;       //û���� �����������
    public bool       m_bPositioned    = false;       //���ڸ��� ��ġ����    
    public bool       m_bDroppable     = false;       //Positon�ڿ� Drop ����
    public bool       m_bNearCatch     = false;       //���ٳ��� ��� ��ó�� ���� �νĿ���
    public float      m_fAccept_radius = 0.2f;        //�ݶ��̴��ܿ� �ݰ�ȿ� ������ ���� ���������� ����
    public bool       m_bGrabbed;                     //���� ��������
    //Private
    Rigidbody         m_RigidBody;
    
    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //Grabbed �Ǿ��� �� Action�� �����մϴ� called by Guide
    public void OnGrab() {        
        m_bGrabbed   = true;
        SetRigidBody(true);     //droppable ������ Rigidbocy�� disable�Ǿ� �����Ƿ� ������ ������ Rigidbody Ȱ��ȭ
        m_Target.gameObject.SetActive(true);  //���ٳ��� ��� Target�� �����ְ�
        CDB[(int)m_eArrange].nGrabCount++;
        
    }
    //Release �Ǿ��� �� Action�� �������մϴ� called by Guide
    public void OnGrabRelease() {
        m_bGrabbed = false;
        if (m_Target.gameObject.activeSelf) m_Target.DeActivate();
    }

    void DropAt(Transform tf){
        transform.position      = tf.position;
        transform.eulerAngles   = tf.eulerAngles;
    }
    
    public void SetRigidBody(bool bOn){
        m_RigidBody.mass        = (bOn ? oMass        : 0);
        m_RigidBody.drag        = (bOn ? oDrag        : 0);
        m_RigidBody.angularDrag = (bOn ? oAngularDrag : 0); 
        m_RigidBody.useGravity  = (bOn ? oUseGravity  : false); 
        m_RigidBody.constraints = (bOn ? oConstraints : RigidbodyConstraints.FreezeAll);
    }

    // ����ġ�� ���ٳ�
    // ���� ��������� �ƴѵ� �ٽ� ���� ���°��, ��������� ���� ���°�� �����Ұ�
    void SetPositoned()  {                 
        //����ġ�� Target�� ��ġ�� �Ӵϴ�
        if(!m_bDroppable) SetRigidBody(false); 
        DropAt(m_Target.transform);
        StartCoroutine("AdhereTarget");
        Outlinable outline = GetComponent<Outlinable>();    //Ouline�� �����ͼ�
        StartCoroutine(OulineBlinkOut(outline));            //����� ������ ����              
    }

    // �����Ұ����� ������ valid for only "not droppable"
    void Detach() {
        CDB[(int)m_eArrange].bPositioned = m_bPositioned = false;    //CDB����
    }

    // ��������� ��� ��ġ�� �ٲ�Ƿ� ���������� ��ٷȴٰ� �ٽ� Target��ġ�� ��
    IEnumerator AdhereTarget()  {
        while(Guide.m_eGrabbedArrange != ARRANGE.NONE) yield return new WaitForSeconds(0.1f);
        DropAt(m_Target.transform);
        //������ ������ ��� ó�� ������ ���϶� ��ƼŬ ���� 
        if(!CDB[(int)m_eArrange].bPositioned && m_bCleanable) {  
            CDB[(int)m_eArrange].bPositioned = m_bPositioned = true; 
            m_Guide.SetFirstArranged(m_eArrange, m_Target.transform);  //�����Ѱ��� Guide���� �˷���  
        }
        CDB[(int)m_eArrange].bPositioned = m_bPositioned = true;
        m_Guide.SetPositioned(); // (m_eArrange);  
        CDB[(int)m_eArrange].nPosiCount++; // Eval            
    }

    //���������� Outline ����� ������ ����
    IEnumerator OulineBlinkOut(Outlinable outline) {
        if(!outline) yield break;
        outline.OutlineParameters.Color = HUD.COLOR_OUTLINE_WHITE; //������� ����
        yield return new WaitForSeconds(1f);
        for(int i=0; i < 6; i++) {
            outline.enabled = !outline.enabled;
            yield return new WaitForSeconds(0.3f);
        }
        outline.enabled = false;
    }

    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    Guide            m_Guide;   
    Target           m_Target;
    
    // Start is called before the first frame update
    //Original RigidBody Properties Backup
    float oMass, oDrag, oAngularDrag;  bool  oUseGravity ;  RigidbodyConstraints oConstraints;

    void Start() { 
        m_Guide       = GameObject.Find("Guide").GetComponent<Guide>();
        m_Target      = GameObject.Find("/Objects/Targets/"+CDB[(int)m_eArrange].Object_name).GetComponent<Target>();
        m_RigidBody   = GetComponent<Rigidbody>();
        oMass         = m_RigidBody.mass;
        oDrag         = m_RigidBody.drag;
        oAngularDrag  = m_RigidBody.angularDrag; 
        oUseGravity   = m_RigidBody.useGravity; 
        oConstraints  = m_RigidBody.constraints;
        
        //CDB�� �����Ϳ��� �����Ȱ� ����
        CDB[(int)m_eArrange].bCleanable  =  m_bCleanable;
        CDB[(int)m_eArrange].bPositioned =  m_bPositioned; 

        //���� ���ڸ��� �ִ� �����̸� ��ġ ����
        if(m_bPositioned) {
            DropAt(m_Target.transform);
            if(!m_bDroppable) SetRigidBody(false); 
        }  
    }

    // Update is called once per frame        
    void Update() {        
        if(m_bGrabbed) CDB[(int)m_eArrange].fGrabTime += Time.deltaTime; //EVAL
        //�����ٳ��� ��ġ�� �Ÿ��� ����� �Ӵϴ�.
        float distance = Vector3.Distance(transform.position, m_Target.transform.position);
        if(distance < m_fAccept_radius) { //�ݰ�ȿ� �ְ�
            if (!m_bPositioned && m_bNearCatch)   SetPositoned(); //���������� �ȵȻ����̸鼭 NearCatch�� �����Ǿ� ������ ����ó��
        } else{ //�ݰ�ۿ� �ְ�
            if ( !m_bDroppable && m_bPositioned)   Detach(); //�����Ȼ��¿��� Droppable�� �ƴѰ��� �־����� Detachó��
        }
    }
    
    // Ʈ����(Target)�� ���˵Ǹ� ���ٳ��������� ó���մϴ�.
    void OnTriggerEnter(Collider other) {        
        //�����ȵȻ��¿��� TriggerEnter�� Attach
        if(!m_bPositioned && other.gameObject.name == gameObject.name) {                                    
            SetPositoned();//������ ������ ó��
        }
    }    
}
}
