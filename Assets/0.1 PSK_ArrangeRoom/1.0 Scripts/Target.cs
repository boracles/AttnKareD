using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;//for Outlinable
using BNG;
namespace Arrange {
public class Target : MonoBehaviour {
 
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/  
    public   bool       m_bNearCatch;    //갖다놓을 장소 근처에 오면 인식여부
    GameObject          m_SrcObject;     //갖다놓기 해야할 게임오브젝트
    Manager.ARRANGE_INFO m_ArrangeInfo;
    bool                m_bPositoned;

    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //Editor에서 Kind를 할당해 줍니다
    public Manager.ARRANGE m_eArrange;      //목표물건에서 할당하십시요

    //사용자가 물건을 놓을때 Target을 3초간 지연후 disable시켜줍니다.-called by guide
    public void DeActivate() {        
        StartCoroutine("DelayedOut");
    }
    //Arrange가 안되면 3초 뒤에 사라지게(던졌을때 콜라이더 출동하는시간동안 기다림)
    IEnumerator DelayedOut() {           
       yield return new WaitForSeconds(3f); 
       if(m_bPositoned) yield break;
       else this.gameObject.SetActive(false); 
    }

    // 내위치에 갖다놈
    void SetPositioned(GameObject go)  {        
        m_bPositoned = true;        //더이상 update 및 OnTriggerEnter에서 검사하지 못하게
        go.tag = Manager.saTag[(int)Manager.TAG.UNNECESSARY]; //레이져 포인터를 더이상 받지 않도록
        SetVisible(false);                        //녹색으로된 Tager Mesh를 감춤
        GetComponent<Collider>().enabled = false; //target에 콜라이이더 무효화       
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.mass = 0; rb.drag = 0; rb.angularDrag = 0; rb.useGravity = false; //물리속성제거
        //해당 물건을 내위치에 둡니다        
        go.transform.parent             = transform;
        go.transform.localPosition      = Vector3.zero;
        go.transform.localEulerAngles   = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //정리되면 Ouline을 흰색으로 3초간 점멸후 제거

        //Ouline을 가져와서
        Outlinable outline = go.GetComponent<Outlinable>();
        //흰색을 점멸후 제거
        StartCoroutine(OulineBlinkOut(outline));
        //찾은물건은 다시 잡을 수 없게 만들어줌        
        go.GetComponent<Grabbable>().enabled = false;
        //CDB갱신
        Manager.CDB[(int)m_eArrange].bArranged = true;        
        //정리한 물건 문자열 갱신
        //m_Hud.NoteUpdateArrange(Manager.saArrangeK[(int)m_eArrange]);
        //
        //정리한것을 Guide에게 알려줌        
        m_Guide.SetArranged(this.gameObject);
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

    // for Child Green Mesh blink
    MeshRenderer[] m_aRenderers;
    bool bShowMesh;
    void SetVisible(bool bShow) {
        foreach(MeshRenderer _renderer in m_aRenderers) _renderer.enabled = bShow;
    }
    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    Guide                m_Guide;   
    HUD                  m_Hud;
    
    // Start is called before the first frame update
    
    void Start() {
        m_aRenderers  = GetComponentsInChildren<MeshRenderer>();
        m_ArrangeInfo = Manager.CDB[(int)m_eArrange];   
        m_SrcObject   = GameObject.Find(m_ArrangeInfo.src_Object_name); //찾아야할 게임오브젝트
        m_Guide       = GameObject.Find("Guide").GetComponent<Guide>();
        m_Hud         = GameObject.Find("HUD").GetComponent<HUD>();
    }

    // Update is called once per frame        
    float next01SecUpdate = 0;
    float next05SecUpdate = 0;
    float next1SecUpdate  = 0;
    void Update() {
        if(m_bPositoned) return;
        if (Time.time > next01SecUpdate) { Do01SecTask(); next01SecUpdate = Time.time + 0.1f; } //시간갱신
        if (Time.time > next05SecUpdate) { Do05SecTask(); next05SecUpdate = Time.time + 0.5f; } //시간갱신
        if (Time.time > next1SecUpdate)  { Do1SecTask();  next1SecUpdate  = Time.time + 1.0f; } //시간갱신
                
        //반경안에 들어오면 갖다놓은것으로 처리합니다.    
        if (m_bNearCatch) {
            if(Vector3.Distance(transform.position, m_SrcObject.transform.position)< m_ArrangeInfo.accept_radius) 
            SetPositioned(m_SrcObject);
        }        
    }
    
    // 콜라이더와 접촉되면 갖다놓은것으로 처리합니다.
    void OnTriggerEnter(Collider other) {
        if(m_bPositoned) return;
        if(other.gameObject.name == m_ArrangeInfo.src_Object_name) {                                    
            SetPositioned(other.gameObject);//정리한 것으로 처리
        }
    }    

    //0.1sec주기로 업데이할 Task를 등록하세요
    void Do01SecTask()  {
    
    }

    //0.5sec주기로 업데이할 Task를 등록하세요
    void Do05SecTask() {    
        bShowMesh = !bShowMesh;
        SetVisible(bShowMesh);  //녹색Mesh를 점멸해줍니다.
    }

    //1sec주기로 업데이할 Task를 등록하세요
    void Do1SecTask()   {
      
    }

}
}
