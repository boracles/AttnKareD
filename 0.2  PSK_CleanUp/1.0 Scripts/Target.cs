using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;//for Outlinable
using BNG;
namespace CleanUp {
public class Target : MonoBehaviour {
 
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/  
    public Arrange.ARRANGE m_eArrange;     //Editor에서 Kind를 할당해 줍니다

    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //사용자가 물건을 놓을때 Target을 3초간 지연후 disable시켜줍니다.-called by arrange
    public void DeActivate() {        
        StartCoroutine("DelayedOut");
    }
    //Arrange가 안되면 3초 뒤에 사라지게(던졌을때 콜라이더 출동하는시간동안 기다림)
    IEnumerator DelayedOut() {          
        for(int i=0; i < 30; i++) {
            if(m_Arrange.m_bGrabbed) yield break;
            yield return new WaitForSeconds(0.1f); 
        }         
        gameObject.SetActive(m_Arrange.m_bGrabbed); 
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
    Arrange     m_Arrange;  //나에게 와야할 src object 기억

    // Start is called before the first frame update
    void Start() {
        m_Arrange     = GameObject.Find("/Objects/Arranges/"+Arrange.CDB[(int)m_eArrange].Object_name).GetComponent<Arrange>();
        m_aRenderers  = GetComponentsInChildren<MeshRenderer>(true);  
        gameObject.SetActive(false);
    }

    // Update is called once per frame        
    float next01SecUpdate = 0;
    float next05SecUpdate = 0;
    float next1SecUpdate  = 0;
    void Update() {
        if (Time.time > next01SecUpdate) { Do01SecTask(); next01SecUpdate = Time.time + 0.1f; } //시간갱신
        if (Time.time > next05SecUpdate) { Do05SecTask(); next05SecUpdate = Time.time + 0.5f; } //시간갱신
        if (Time.time > next1SecUpdate)  { Do1SecTask();  next1SecUpdate  = Time.time + 1.0f; } //시간갱신       
    }
    
    // 콜라이더와 접촉되면 감춤니다
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == gameObject.name) gameObject.SetActive(false);
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
