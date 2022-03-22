using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;//for Outlinable
using BNG;
namespace CleanUp {
public class Trash : MonoBehaviour {

        //쓰레기 추가 시 추가해야되는 부분
    public enum TRASH  { 
        TRASH1, 
        TRASH2,
        TRASH3,
        TRASH4,
        TRASH5, TRASH6, TRASH7, TRASH8, TRASH9,
        //TRASH10,
        NONE
        }        
    
    //최종적으로 각 물건에 대한 평가결과 저장
    public struct TRASH_INFO   {
        public TRASH eTrash;         // 종류
        //public string  Object_name;      //나에게 와야할 GameObject Name                   
        //public bool    bCleanable;       //청소대상여부
        public bool    tPositioned;      //제위치여부
        public int     nGrabCount;       //Player가 내 물건을 잡은 횟수
        public float   fGrabTime;        //Player가 내 물건을 잡은 시간
        //public int     nPosiCount;       //자기위치에 둔 횟수
        public string  kor_name;         //한글명
                                         
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
    // 잡을 수 있는 물건수 ; 13
    public static  int     TOTAL_TRASH    { get { return CDB.Length;}  }  
    // 정리할 물건수 : initial 8
    //public static  int     TOTAL_TOCLEAN    { get { int nCount = 0; foreach(TRASH_INFO ainfo in CDB) if(ainfo.bCleanable) nCount++; return nCount;  } }
    // 정리된 물건수
    //public static  int     TOTAL_CLEANED    { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) if(ainfo.bCleanable && ainfo.bPositioned) nCount++; return nCount;   } }
    //현제 제 위치에 있는 물건수 : initial 5
    public static  int     TOTAL_POSITIONED { get { int nCount = 0; foreach(TRASH_INFO ainfo in CDB) if(ainfo.tPositioned) nCount++; return nCount;  } }
    // 총 Grab Count
    public static  int     TOTAL_GRAB_COUNT { get { int nCount = 0; foreach(TRASH_INFO ainfo in CDB) nCount+= ainfo.nGrabCount;  return nCount;  }  }
    // 총 Grab Time
    public static  float   TOTAL_GRAB_TIME  { get { float time = 0; foreach(TRASH_INFO ainfo in CDB) time += ainfo.fGrabTime;  return time;  }  }
    // 총 자기위치에 둔횟수
    //public static  float   TOTAL_POSI_COUNT { get { int nCount = 0; foreach(ARRANGE_INFO ainfo in CDB) nCount += ainfo.nPosiCount;  return nCount;  }  }


    //쓰레기 추가 시 추가해야되는 부분
    public static TRASH_INFO[] CDB = new TRASH_INFO[]  {        
		//new ARRANGE_INFO(ARRANGE.BOX,         "Box",       false, false, 0, 0.0f, 0, "박스"   ),             
        //new ARRANGE_INFO(ARRANGE.LAMP,        "Lamp",      false, false, 0, 0.0f, 0, "램프"   ),                     
        //new ARRANGE_INFO(ARRANGE.GAME_PAD,    "GamePad",   false, false, 0, 0.0f, 0, "게임패드" ),    
        //new ARRANGE_INFO(ARRANGE.GLOBE,       "Globe",     false, false, 0, 0.0f, 0, "지구본"  ),             
        //new ARRANGE_INFO(ARRANGE.MOUTH_WASH,  "MouthWash", false, false, 0, 0.0f, 0, "로션"   ),             
  		
        new TRASH_INFO(TRASH.TRASH1, false, 0, 0.0f, "쓰레기1"),
        new TRASH_INFO(TRASH.TRASH2, false, 0, 0.0f, "쓰레기2"),
        new TRASH_INFO(TRASH.TRASH3, false, 0, 0.0f, "쓰레기3"),
        new TRASH_INFO(TRASH.TRASH4, false, 0, 0.0f, "쓰레기4"),
        new TRASH_INFO(TRASH.TRASH5, false, 0, 0.0f, "쓰레기5"),
        new TRASH_INFO(TRASH.TRASH6, false, 0, 0.0f, "쓰레기6"),
        new TRASH_INFO(TRASH.TRASH7, false, 0, 0.0f, "쓰레기7"),
        new TRASH_INFO(TRASH.TRASH8, false, 0, 0.0f, "쓰레기8"),
        new TRASH_INFO(TRASH.TRASH9, false, 0, 0.0f, "쓰레기9")
    };    
  
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/  
    // Public... Assigned in Editor.
    // Editor에서 설정한 값이 우선이므로 여기서는 디폴트로 false를 줍니다.
    public TRASH      m_eTrash       = TRASH.NONE; //Editor에서 Kind를 할당해 줍니다
    //public bool       m_bCleanable     = false;       //청소할 대상인지여부
    public bool       m_bPositioned    = false;       //제자리에 위치여부    
    //public bool       m_bDroppable     = false;       //Positon뒤에 Drop 여부
    //public bool       m_bNearCatch     = false;       //갖다놓을 장소 근처에 오면 인식여부
    //public float      m_fAccept_radius = 0.2f;        //콜라이더외에 반경안에 들어오면 갖다 놓은것으로 간주
    public bool       m_bGrabbed;                     //현재 잡혔는지
    //Private
    Rigidbody         m_RigidBody;
    
    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //Grabbed 되었을 때 Action을 정의합니다 called by Guide
    public void OnGrab() {        
        m_bGrabbed   = true;
        //SetRigidBody(true);     //droppable 물건은 Rigidbocy가 disable되어 있으므로 손으로 잡으면 Rigidbody 활성화
        //m_Target.gameObject.SetActive(true);  //갖다놓을 녹색 Target을 보여주고
        CDB[(int)m_eTrash].nGrabCount++;
        
    }
    //Release 되었을 때 Action을 정의합니다 called by Guide
    public void OnGrabRelease() {
        m_bGrabbed = false;
        //if (m_Target.gameObject.activeSelf) m_Target.DeActivate();
    }

    void DropAt(Transform tf){
        transform.position      = tf.position;
        transform.eulerAngles   = tf.eulerAngles;
    }
    
    public void SetRigidBody(bool bOn){
        //false를 주었을 때 멈춤
        m_RigidBody.mass        = (bOn ? oMass        : 0);
        m_RigidBody.drag        = (bOn ? oDrag        : 0);
        m_RigidBody.angularDrag = (bOn ? oAngularDrag : 0); 
        m_RigidBody.useGravity  = (bOn ? oUseGravity  : true);
        m_RigidBody.constraints = (bOn ? oConstraints : RigidbodyConstraints.FreezeAll);
        gameObject.layer = 0;
        //grabbable 해제
     }
    public void SetRigidBody2(bool bOn)
    {
            //false를 주었을 때 멈춤
         m_RigidBody.mass = (bOn ? oMass : 1);
         m_RigidBody.drag = (bOn ? oDrag : 0);
         m_RigidBody.angularDrag = (bOn ? oAngularDrag : 0);
         m_RigidBody.useGravity = (bOn ? oUseGravity : true);
         m_RigidBody.constraints = (bOn ? oConstraints : RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
         gameObject.layer = 0;
            //grabbable 해제
    }

        // 내위치에 갖다놈
        // 원래 정리대상이 아닌데 다시 갖다 놓는경우, 정리대상을 갖다 놓는경우 구분할것
        void SetPositonedTrash()  {
        m_Guide.TrashCleaned(m_eTrash, m_TrashParticle.transform);
        //내위치를 Target의 위치에 둡니다
        //if(!m_bDroppable) SetRigidBody(false); 
        //DropAt(m_Target.transform);
        //StartCoroutine("AdhereTarget");
        //Outlinable outline = GetComponent<Outlinable>();    //Ouline을 가져와서
        //StartCoroutine(OulineBlinkOut(outline));            //흰색을 점멸후 제거              
        }

    // 정리할곳에서 떨어짐 valid for only "not droppable"
    void Detach() {
        CDB[(int)m_eTrash].tPositioned = m_bPositioned = false;    //CDB갱신
    }

    // 잡고있으면 계속 위치가 바뀌므로 놓을때까지 기다렸다가 다시 Target위치에 둠
    /*
    IEnumerator AdhereTarget()  {
        while(Guide.m_eGrabbedArrange != TRASH.NONE) yield return new WaitForSeconds(0.1f);
        DropAt(m_Target.transform);
        //정리할 물건일 경우 처음 정리된 것일때 파티클 전시 
        if(!CDB[(int)m_eTrash].bPositioned && m_bCleanable) {  
            CDB[(int)m_eTrash].bPositioned = m_bPositioned = true; 
            m_Guide.SetFirstArranged(m_eTrash, m_Target.transform);  //정리한것을 Guide에게 알려줌  
        }
        CDB[(int)m_eTrash].bPositioned = m_bPositioned = true; 
        m_Guide.SetPositioned(m_eTrash);  
        CDB[(int)m_eTrash].nPosiCount++; // Eval            
    }
    
    //정리했을때 Outline 흰색을 점멸후 제거
    IEnumerator OulineBlinkOut(Outlinable outline) {
        if(!outline) yield break;
        outline.OutlineParameters.Color = HUD.COLOR_OUTLINE_WHITE; //흰색으로 변경
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
        
        //CDB에 에디터에서 지정된값 저장
        //CDB[(int)m_eTrash].bCleanable  =  m_bCleanable;
        //CDB[(int)m_eTrash].tPositioned =  m_bPositioned; 

        //원래 제자리에 있는 물건이면 위치 고정
        //if(m_bPositioned) {
            //DropAt(m_Target.transform);
            //if(!m_bDroppable) SetRigidBody(false); 
        //}  
    }

    // Update is called once per frame        
    void Update() {        
        if(m_bGrabbed) CDB[(int)m_eTrash].fGrabTime += Time.deltaTime; //EVAL
        //가져다놓을 위치와 거리를 계산해 둡니다.
        //float distance = Vector3.Distance(transform.position, m_Target.transform.position);
        /*
        if(distance < m_fAccept_radius) { //반경안에 있고
            if (!m_bPositioned && m_bNearCatch)   SetPositoned(); //아직정리가 안된상태이면서 NearCatch가 설정되어 있으면 정리처리
        } else{ //반경밖에 있고
            if ( !m_bDroppable && m_bPositioned)   Detach(); //정리된상태에서 Droppable이 아닌것이 멀어지면 Detach처리
        }
        */
    }
    
    // 트리거(Target)와 접촉되면 갖다놓은것으로 처리합니다.
    void OnTriggerEnter(Collider other) {        
        //정리안된상태에서 TriggerEnter는 Attach
        if(other.gameObject.tag == "Surface") {
            CDB[(int)m_eTrash].tPositioned = m_bPositioned = true;
            SetRigidBody2(false);
            gameObject.layer = 0;
            SetPositonedTrash();//정리한 것으로 처리
            m_Guide.SetPositioned();
            }
    }    
}
}
