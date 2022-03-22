using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;//for Outlinable
using BNG;
namespace CleanUp {
public class Trash : MonoBehaviour {

        //������ �߰� �� �߰��ؾߵǴ� �κ�
    public enum TRASH  { 
        TRASH1, 
        TRASH2,
        TRASH3,
        TRASH4,
        TRASH5, TRASH6, TRASH7, TRASH8, TRASH9,
        //TRASH10,
        NONE
        }        
    
    //���������� �� ���ǿ� ���� �򰡰�� ����
    public struct TRASH_INFO   {
        public TRASH eTrash;         // ����
        //public string  Object_name;      //������ �;��� GameObject Name                   
        //public bool    bCleanable;       //û�Ҵ�󿩺�
        public bool    tPositioned;      //����ġ����
        public int     nGrabCount;       //Player�� �� ������ ���� Ƚ��
        public float   fGrabTime;        //Player�� �� ������ ���� �ð�
        //public int     nPosiCount;       //�ڱ���ġ�� �� Ƚ��
        public string  kor_name;         //�ѱ۸�
                                         
        public TRASH_INFO(TRASH eTrash, bool tPositioned, int  nGrabCount , float fGrabTime , string kor_name )  {         
            this.eTrash        =  eTrash;                              
            //this.Object_name     =  Object_name;                        
            //this.bCleanable      =  bCleanable ; 
            this.tPositioned     =  tPositioned;
            this.nGrabCount      =  nGrabCount ; 
            this.fGrabTime       =  fGrabTime  ;  
            //this.nPosiCount      =  nPosiCount ; 
            this.kor_name        =  kor_name;
        }
    }
    // ���� �� �ִ� ���Ǽ� ; 13
    public static  int     TOTAL_TRASH    { get { return CDB.Length;}  }  
    // ������ ���Ǽ� : initial 8
    //public static  int     TOTAL_TOCLEAN    { get { int nCount = 0; foreach(TRASH_INFO ainfo in CDB) if(ainfo.bCleanable) nCount++; return nCount;  } }
    // ������ ���Ǽ�
    //public static  int     TOTAL_CLEANED    { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) if(ainfo.bCleanable && ainfo.bPositioned) nCount++; return nCount;   } }
    //���� �� ��ġ�� �ִ� ���Ǽ� : initial 5
    public static  int     TOTAL_POSITIONED { get { int nCount = 0; foreach(TRASH_INFO ainfo in CDB) if(ainfo.tPositioned) nCount++; return nCount;  } }
    // �� Grab Count
    public static  int     TOTAL_GRAB_COUNT { get { int nCount = 0; foreach(TRASH_INFO ainfo in CDB) nCount+= ainfo.nGrabCount;  return nCount;  }  }
    // �� Grab Time
    public static  float   TOTAL_GRAB_TIME  { get { float time = 0; foreach(TRASH_INFO ainfo in CDB) time += ainfo.fGrabTime;  return time;  }  }
    // �� �ڱ���ġ�� ��Ƚ��
    //public static  float   TOTAL_POSI_COUNT { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) nCount += ainfo.nPosiCount;  return nCount;  }  }


    //������ �߰� �� �߰��ؾߵǴ� �κ�
    public static TRASH_INFO[] CDB = new TRASH_INFO[]  {        
		//new ARRANGE_INFO(ARRANGE.BOX,         "Box",       false, false, 0, 0.0f, 0, "�ڽ�"   ),             
        //new ARRANGE_INFO(ARRANGE.LAMP,        "Lamp",      false, false, 0, 0.0f, 0, "����"   ),                     
        //new ARRANGE_INFO(ARRANGE.GAME_PAD,    "GamePad",   false, false, 0, 0.0f, 0, "�����е�" ),    
        //new ARRANGE_INFO(ARRANGE.GLOBE,       "Globe",     false, false, 0, 0.0f, 0, "������"  ),             
        //new ARRANGE_INFO(ARRANGE.MOUTH_WASH,  "MouthWash", false, false, 0, 0.0f, 0, "�μ�"   ),             
  		
        new TRASH_INFO(TRASH.TRASH1, false, 0, 0.0f, "������1"),
        new TRASH_INFO(TRASH.TRASH2, false, 0, 0.0f, "������2"),
        new TRASH_INFO(TRASH.TRASH3, false, 0, 0.0f, "������3"),
        new TRASH_INFO(TRASH.TRASH4, false, 0, 0.0f, "������4"),
        new TRASH_INFO(TRASH.TRASH5, false, 0, 0.0f, "������5"),
        new TRASH_INFO(TRASH.TRASH6, false, 0, 0.0f, "������6"),
        new TRASH_INFO(TRASH.TRASH7, false, 0, 0.0f, "������7"),
        new TRASH_INFO(TRASH.TRASH8, false, 0, 0.0f, "������8"),
        new TRASH_INFO(TRASH.TRASH9, false, 0, 0.0f, "������9")
    };    
  
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/  
    // Public... Assigned in Editor.
    // Editor���� ������ ���� �켱�̹Ƿ� ���⼭�� ����Ʈ�� false�� �ݴϴ�.
    public TRASH      m_eTrash       = TRASH.NONE; //Editor���� Kind�� �Ҵ��� �ݴϴ�
    //public bool       m_bCleanable     = false;       //û���� �����������
    public bool       m_bPositioned    = false;       //���ڸ��� ��ġ����    
    //public bool       m_bDroppable     = false;       //Positon�ڿ� Drop ����
    //public bool       m_bNearCatch     = false;       //���ٳ��� ��� ��ó�� ���� �νĿ���
    //public float      m_fAccept_radius = 0.2f;        //�ݶ��̴��ܿ� �ݰ�ȿ� ������ ���� ���������� ����
    public bool       m_bGrabbed;                     //���� ��������
    //Private
    Rigidbody         m_RigidBody;
    
    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //Grabbed �Ǿ��� �� Action�� �����մϴ� called by Guide
    public void OnGrab() {        
        m_bGrabbed   = true;
        //SetRigidBody(true);     //droppable ������ Rigidbocy�� disable�Ǿ� �����Ƿ� ������ ������ Rigidbody Ȱ��ȭ
        //m_Target.gameObject.SetActive(true);  //���ٳ��� ��� Target�� �����ְ�
        CDB[(int)m_eTrash].nGrabCount++;
        
    }
    //Release �Ǿ��� �� Action�� �����մϴ� called by Guide
    public void OnGrabRelease() {
        m_bGrabbed = false;
        //if (m_Target.gameObject.activeSelf) m_Target.DeActivate();
    }

    void DropAt(Transform tf){
        transform.position      = tf.position;
        transform.eulerAngles   = tf.eulerAngles;
    }
    
    public void SetRigidBody(bool bOn){
        //false�� �־��� �� ����
        m_RigidBody.mass        = (bOn ? oMass        : 0);
        m_RigidBody.drag        = (bOn ? oDrag        : 0);
        m_RigidBody.angularDrag = (bOn ? oAngularDrag : 0); 
        m_RigidBody.useGravity  = (bOn ? oUseGravity  : true);
        m_RigidBody.constraints = (bOn ? oConstraints : RigidbodyConstraints.FreezeAll);
        gameObject.layer = 0;
        //grabbable ����
     }
    public void SetRigidBody2(bool bOn)
    {
            //false�� �־��� �� ����
         m_RigidBody.mass = (bOn ? oMass : 1);
         m_RigidBody.drag = (bOn ? oDrag : 0);
         m_RigidBody.angularDrag = (bOn ? oAngularDrag : 0);
         m_RigidBody.useGravity = (bOn ? oUseGravity : true);
         m_RigidBody.constraints = (bOn ? oConstraints : RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
         gameObject.layer = 0;
            //grabbable ����
    }

        // ����ġ�� ���ٳ�
        // ���� ��������� �ƴѵ� �ٽ� ���� ���°��, ��������� ���� ���°�� �����Ұ�
        void SetPositonedTrash()  {
        m_Guide.TrashCleaned(m_eTrash, m_TrashParticle.transform);
        //����ġ�� Target�� ��ġ�� �Ӵϴ�
        //if(!m_bDroppable) SetRigidBody(false); 
        //DropAt(m_Target.transform);
        //StartCoroutine("AdhereTarget");
        //Outlinable outline = GetComponent<Outlinable>();    //Ouline�� �����ͼ�
        //StartCoroutine(OulineBlinkOut(outline));            //����� ������ ����              
        }

    // �����Ұ����� ������ valid for only "not droppable"
    void Detach() {
        CDB[(int)m_eTrash].tPositioned = m_bPositioned = false;    //CDB����
    }

    // ��������� ��� ��ġ�� �ٲ�Ƿ� ���������� ��ٷȴٰ� �ٽ� Target��ġ�� ��
    /*
    IEnumerator AdhereTarget()  {
        while(Guide.m_eGrabbedArrange != TRASH.NONE) yield return new WaitForSeconds(0.1f);
        DropAt(m_Target.transform);
        //������ ������ ��� ó�� ������ ���϶� ��ƼŬ ���� 
        if(!CDB[(int)m_eTrash].bPositioned && m_bCleanable) {  
            CDB[(int)m_eTrash].bPositioned = m_bPositioned = true; 
            m_Guide.SetFirstArranged(m_eTrash, m_Target.transform);  //�����Ѱ��� Guide���� �˷���  
        }
        CDB[(int)m_eTrash].bPositioned = m_bPositioned = true; 
        m_Guide.SetPositioned(m_eTrash);  
        CDB[(int)m_eTrash].nPosiCount++; // Eval            
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
    */
    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    Guide            m_Guide;
        //Target           m_Target;
    GameObject m_TrashParticle;
    
    
    // Start is called before the first frame update
    //Original RigidBody Properties Backup
    float oMass, oDrag, oAngularDrag;  bool  oUseGravity ;  RigidbodyConstraints oConstraints;

    void Start() { 
        m_Guide       = GameObject.Find("Guide").GetComponent<Guide>();
        m_TrashParticle = GameObject.Find("TrashParticleTransform");
        //m_Target      = GameObject.Find("/Objects/Targets/"+CDB[(int)m_eTrash].Object_name).GetComponent<Target>();
        m_RigidBody   = GetComponent<Rigidbody>();
        oMass         = m_RigidBody.mass;
        oDrag         = m_RigidBody.drag;
        oAngularDrag  = m_RigidBody.angularDrag; 
        oUseGravity   = m_RigidBody.useGravity; 
        oConstraints  = m_RigidBody.constraints;
        
        //CDB�� �����Ϳ��� �����Ȱ� ����
        //CDB[(int)m_eTrash].bCleanable  =  m_bCleanable;
        //CDB[(int)m_eTrash].tPositioned =  m_bPositioned; 

        //���� ���ڸ��� �ִ� �����̸� ��ġ ����
        //if(m_bPositioned) {
            //DropAt(m_Target.transform);
            //if(!m_bDroppable) SetRigidBody(false); 
        //}  
    }

    // Update is called once per frame        
    void Update() {        
        if(m_bGrabbed) CDB[(int)m_eTrash].fGrabTime += Time.deltaTime; //EVAL
        //�����ٳ��� ��ġ�� �Ÿ��� ����� �Ӵϴ�.
        //float distance = Vector3.Distance(transform.position, m_Target.transform.position);
        /*
        if(distance < m_fAccept_radius) { //�ݰ�ȿ� �ְ�
            if (!m_bPositioned && m_bNearCatch)   SetPositoned(); //���������� �ȵȻ����̸鼭 NearCatch�� �����Ǿ� ������ ����ó��
        } else{ //�ݰ�ۿ� �ְ�
            if ( !m_bDroppable && m_bPositioned)   Detach(); //�����Ȼ��¿��� Droppable�� �ƴѰ��� �־����� Detachó��
        }
        */
    }
    
    // Ʈ����(Target)�� ���˵Ǹ� ���ٳ��������� ó���մϴ�.
    void OnTriggerEnter(Collider other) {        
        //�����ȵȻ��¿��� TriggerEnter�� Attach
        if(other.gameObject.tag == "Surface") {
            CDB[(int)m_eTrash].tPositioned = m_bPositioned = true;
            SetRigidBody2(false);
            gameObject.layer = 0;
            SetPositonedTrash();//������ ������ ó��
            m_Guide.SetPositioned();
            }
    }    
}
}
