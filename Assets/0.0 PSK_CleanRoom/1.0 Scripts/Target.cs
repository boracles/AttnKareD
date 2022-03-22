using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;//for Outlinable
using BNG;
namespace CleanRoom {
public class Target : MonoBehaviour {
 
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/  
    public   bool       m_bNearCatch;    //갖다놓을 장소 근처에 오면 인식여부
    GameObject          m_SrcObject;     //갖다놓기 해야할 게임오브젝트
    Manager.CLEAN_INFO  m_CleanInfo;
    bool                m_bPositoned;

    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //Editor에서 Kind를 할당해 줍니다
    public Manager.CLEAN m_eClean;      //목표물건에서 할당하십시요

    // 내위치에 갖다놈
    void SetPositioned(GameObject go)  {
        //더이상 update 및 OnTriggerEnter에서 검사하지 못하게
        m_bPositoned = true;        
        GetComponentInChildren<MeshRenderer>().enabled = false; //녹색으로된 Tager을 감추고                
        //Collider를 상속받은 Mesh, Sphere Box Collider를 받기위해 Collider로 받음
        GetComponent<Collider>().enabled = false;  //target에 콜라이이더 제거
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.mass = 0; rb.drag = 0; rb.angularDrag = 0; rb.useGravity = false; //물리속성제거
        //해당 물건을 내위치에 둡니다        
        go.transform.parent             = transform;
        go.transform.localPosition      = Vector3.zero;
        go.transform.localEulerAngles   = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //Ouline을 흰색으로 변경합니다
        go.GetComponent<Outlinable>().OutlineParameters.Color = HUD.COLOR_OUTLINE_WHITE;        
        //찾은물건은 다시 잡을 수 없게 만들어줌
        go.GetComponent<Grabbable>().enabled = false;
        //CDB갱신
        Manager.CDB[(int)m_eClean].bClean = true;        
        //정리한 물건 문자열 갱신
        m_Hud.NoteUpdateClean(Manager.saCleanK[(int)m_eClean]);        
        //정리한것을 Guide에게 알려줌        
        m_Guide.SetCleaned(this.gameObject);
    }

    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    Guide                m_Guide;   
    HUD                  m_Hud;

    // Start is called before the first frame update
    void Start() {
        m_CleanInfo = Manager.CDB[(int)m_eClean];   
        m_SrcObject = GameObject.Find(m_CleanInfo.src_Object_name); //찾아야할 게임오브젝트
        m_Guide     = GameObject.Find("Guide").GetComponent<Guide>();
        m_Hud       = GameObject.Find("HUD").GetComponent<HUD>();
    }

    // Update is called once per frame
    void Update() {
        if(m_bPositoned) return;
        //반경안에 들어오면 갖다놓은것으로 처리합니다.    
        if (m_bNearCatch) {
            if(Vector3.Distance(transform.position, m_SrcObject.transform.position)< m_CleanInfo.accept_radius) 
            SetPositioned(m_SrcObject);
        }
    }
    
    // 콜라이더와 접촉되면 갖다놓은것으로 처리합니다.
    void OnTriggerEnter(Collider other) {
        if(m_bPositoned) return;
        if(other.gameObject.name == m_CleanInfo.src_Object_name) {                                    
            SetPositioned(other.gameObject);//정리한 것으로 처리
        }
    }    
}
}
