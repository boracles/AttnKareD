using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;//for Outlinable
using BNG;
namespace CleanUp {
public class Books : MonoBehaviour {
        //å �߰� �� �߰��ؾߵǴ� �κ�
    public enum BOOKS  { 
        BOOK1, BOOK2, BOOK3, BOOK4, BOOK5,
        
        //BOOKS3, BOOKS4, BOOKS5, BOOKS6, BOOKS7, BOOKS8, BOOKS9, BOOKS10,
        NONE
        }        
    
    //���������� �� ���ǿ� ���� �򰡰�� ����
    public struct BOOKS_INFO   {
        public BOOKS eBooks;         // ����
        //public string  Object_name;      //������ �;��� GameObject Name                   
        //public bool    bCleanable;       //û�Ҵ�󿩺�
        public bool    bPositioned;      //����ġ����
        public int     nGrabCount;       //Player�� �� ������ ���� Ƚ��
        public float   fGrabTime;        //Player�� �� ������ ���� �ð�
        public float   tAngle;
        //public int     nPosiCount;       //�ڱ���ġ�� �� Ƚ��
        public string  kor_name;         //�ѱ۸�
                                         
        public BOOKS_INFO(BOOKS eBooks, bool bPositioned, int  nGrabCount , float fGrabTime , float tAngle, string kor_name )  {         
            this.eBooks        =  eBooks;                              
            //this.Object_name     =  Object_name;                        
            //this.bCleanable      =  bCleanable ;
            this.bPositioned     =  bPositioned;
            this.nGrabCount      =  nGrabCount ;
            this.fGrabTime       =  fGrabTime  ;
            this.tAngle          =  tAngle;
            //this.nPosiCount      =  nPosiCount ; 
            this.kor_name        =  kor_name;
        }
    }
    // ���� �� �ִ� ���Ǽ� ; 13
    public static  int     TOTAL_BOOK    { get { return CDB.Length;}  }  
    // ������ ���Ǽ� : initial 8
    //public static  int     TOTAL_TOCLEAN    { get { int nCount = 0; foreach(BOOKS_INFO ainfo in CDB) if(ainfo.bCleanable) nCount++; return nCount;  } }
    // ������ ���Ǽ�
    //public static  int     TOTAL_CLEANED    { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) if(ainfo.bCleanable && ainfo.bPositioned) nCount++; return nCount;   } }
    //���� �� ��ġ�� �ִ� ���Ǽ� : initial 5
    public static  int     TOTAL_POSITIONED { get { int nCount = 0; foreach(BOOKS_INFO ainfo in CDB) if(ainfo.bPositioned) nCount++; return nCount;  } }
    // �� Grab Count
    public static  int     TOTAL_GRAB_COUNT { get { int nCount = 0; foreach(BOOKS_INFO ainfo in CDB) nCount+= ainfo.nGrabCount;  return nCount;  }  }
    // �� Grab Time
    public static  float   TOTAL_GRAB_TIME  { get { float time = 0; foreach(BOOKS_INFO ainfo in CDB) time += ainfo.fGrabTime;  return time;  }  }
        // �� �ڱ���ġ�� ��Ƚ��
        //public static  float   TOTAL_POSI_COUNT { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) nCount += ainfo.nPosiCount;  return nCount;  }  }

        //å �߰� �� �߰��ؾߵǴ� �κ�
    public static BOOKS_INFO[] CDB = new BOOKS_INFO[]  {        
		//new ARRANGE_INFO(ARRANGE.BOX,         "Box",       false, false, 0, 0.0f, 0, "�ڽ�"   ),             
        //new ARRANGE_INFO(ARRANGE.LAMP,        "Lamp",      false, false, 0, 0.0f, 0, "����"   ),                     
        //new ARRANGE_INFO(ARRANGE.GAME_PAD,    "GamePad",   false, false, 0, 0.0f, 0, "�����е�" ),    
        //new ARRANGE_INFO(ARRANGE.GLOBE,       "Globe",     false, false, 0, 0.0f, 0, "������"  ),             
        //new ARRANGE_INFO(ARRANGE.MOUTH_WASH,  "MouthWash", false, false, 0, 0.0f, 0, "�μ�"   ),             
  		
        new BOOKS_INFO(BOOKS.BOOK1, false, 0, 0.0f,0.0f, "å1"),
        new BOOKS_INFO(BOOKS.BOOK2, false, 0, 0.0f,0.0f, "å2"),
        new BOOKS_INFO(BOOKS.BOOK3, false, 0, 0.0f,0.0f, "å3"),
        new BOOKS_INFO(BOOKS.BOOK4, false, 0, 0.0f,0.0f, "å4"),
        new BOOKS_INFO(BOOKS.BOOK5, false, 0, 0.0f,0.0f, "å5"),
        //new BOOKS_INFO(BOOKS.BOOKS2, false, 0, 0.0f, "������2")

    };    
  
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/  
    // Public... Assigned in Editor.
    // Editor���� ������ ���� �켱�̹Ƿ� ���⼭�� ����Ʈ�� false�� �ݴϴ�.
    public BOOKS      m_eBooks       = BOOKS.NONE; //Editor���� Kind�� �Ҵ��� �ݴϴ�
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
        CDB[(int)m_eBooks].nGrabCount++;
        
    }
    //Release �Ǿ��� �� Action�� �����մϴ� called by Guide
    public void OnGrabRelease() {
        m_bGrabbed = false;
        if(CDB[(int)m_eBooks].bPositioned == true){
            SetPositonedBooks();
        }
        
        //if (m_Target.gameObject.activeSelf) m_Target.DeActivate();
    }

    void DropAt(Transform tf){
        transform.position      = tf.position;
        transform.eulerAngles   = tf.eulerAngles;
    }
    
    public void SetRigidBody(bool bOn){
        //false�� �־��� �� ����
        m_RigidBody.mass        = (bOn ? oMass        : 1);
        m_RigidBody.drag        = (bOn ? oDrag        : 0);
        m_RigidBody.angularDrag = (bOn ? oAngularDrag : 0); 
        m_RigidBody.useGravity  = (bOn ? oUseGravity  : true);
        m_RigidBody.constraints = (bOn ? oConstraints : RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
        gameObject.layer = 0;
        //grabbable ����
     }

    // ����ġ�� ���ٳ�
    // ���� ��������� �ƴѵ� �ٽ� ���� ���°��, ��������� ���� ���°�� �����Ұ�
    void SetPositonedBooks()  {
        m_Guide.BookPositioned(m_eBooks, gameObject.transform);
        //����ġ�� Target�� ��ġ�� �Ӵϴ�
        //if(!m_bDroppable) SetRigidBody(false); 
        //DropAt(m_Target.transform);
        //StartCoroutine("AdhereTarget");
        //Outlinable outline = GetComponent<Outlinable>();    //Ouline�� �����ͼ�
        //StartCoroutine(OulineBlinkOut(outline));            //����� ������ ����              
    }

    // �����Ұ����� ������ valid for only "not droppable"
    void Detach() {
        CDB[(int)m_eBooks].bPositioned = m_bPositioned = false;    //CDB����
    }

    // ��������� ��� ��ġ�� �ٲ�Ƿ� ���������� ��ٷȴٰ� �ٽ� Target��ġ�� ��
    /*
    IEnumerator AdhereTarget()  {
        while(Guide.m_eGrabbedArrange != BOOKS.NONE) yield return new WaitForSeconds(0.1f);
        DropAt(m_Target.transform);
        //������ ������ ��� ó�� ������ ���϶� ��ƼŬ ���� 
        if(!CDB[(int)m_eBook].bPositioned && m_bCleanable) {  
            CDB[(int)m_eBook].bPositioned = m_bPositioned = true; 
            m_Guide.SetFirstArranged(m_eBook, m_Target.transform);  //�����Ѱ��� Guide���� �˷���  
        }
        CDB[(int)m_eBook].bPositioned = m_bPositioned = true; 
        m_Guide.SetPositioned(m_eBook);  
        CDB[(int)m_eBook].nPosiCount++; // Eval            
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
    
    // Start is called before the first frame update
    //Original RigidBody Properties Backup
    float oMass, oDrag, oAngularDrag;  bool  oUseGravity ;  RigidbodyConstraints oConstraints;

    void Start() { 
        m_Guide       = GameObject.Find("Guide").GetComponent<Guide>();
        //m_Target      = GameObject.Find("/Objects/Targets/"+CDB[(int)m_eBook].Object_name).GetComponent<Target>();
        m_RigidBody   = GetComponent<Rigidbody>();
        oMass         = m_RigidBody.mass;
        oDrag         = m_RigidBody.drag;
        oAngularDrag  = m_RigidBody.angularDrag; 
        oUseGravity   = m_RigidBody.useGravity; 
        oConstraints  = m_RigidBody.constraints;
        
        //CDB�� �����Ϳ��� �����Ȱ� ����
        //CDB[(int)m_eBook].bCleanable  =  m_bCleanable;
        //CDB[(int)m_eBook].tPositioned =  m_bPositioned; 

        //���� ���ڸ��� �ִ� �����̸� ��ġ ����
        //if(m_bPositioned) {
            //DropAt(m_Target.transform);
            //if(!m_bDroppable) SetRigidBody(false); 
        //}  
    }

    // Update is called once per frame        
    
    void Update() {
            
        
        if(m_bGrabbed) CDB[(int)m_eBooks].fGrabTime += Time.deltaTime; //EVAL
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
        if (other.gameObject.tag == "Checker1"){
            Debug.Log("Book Positioned in");
            CDB[(int)m_eBooks].bPositioned = m_bPositioned = true;
            m_Guide.SetPositioned();




            }
        /*
        if(other.gameObject.tag == "Surface") {
                CDB[(int)m_eBook].tPositioned = m_bPositioned = true;
                SetRigidBody(false);
            Debug.Log("cleaned");
                //SetPositoned();//������ ������ ó��
        }

        */
    }
    void OnTriggerExit(Collider other)
    {
        //�����ȵȻ��¿��� TriggerEnter�� Attach
        if (other.gameObject.tag == "Checker1"){
            Debug.Log("Book Positioned out");
            CDB[(int)m_eBooks].bPositioned = m_bPositioned = false;


        }
        /*
        if(other.gameObject.tag == "Surface") {
                CDB[(int)m_eBook].tPositioned = m_bPositioned = true;
                SetRigidBody(false);
            Debug.Log("cleaned");
                //SetPositoned();//������ ������ ó��
        }

        */
    }
    } //namespace
}
