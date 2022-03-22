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
    public Arrange.ARRANGE m_eArrange;     //Editor���� Kind�� �Ҵ��� �ݴϴ�

    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //����ڰ� ������ ������ Target�� 3�ʰ� ������ disable�����ݴϴ�.-called by arrange
    public void DeActivate() {        
        StartCoroutine("DelayedOut");
    }
    //Arrange�� �ȵǸ� 3�� �ڿ� �������(�������� �ݶ��̴� �⵿�ϴ½ð����� ��ٸ�)
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
    Arrange     m_Arrange;  //������ �;��� src object ���

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
        if (Time.time > next01SecUpdate) { Do01SecTask(); next01SecUpdate = Time.time + 0.1f; } //�ð�����
        if (Time.time > next05SecUpdate) { Do05SecTask(); next05SecUpdate = Time.time + 0.5f; } //�ð�����
        if (Time.time > next1SecUpdate)  { Do1SecTask();  next1SecUpdate  = Time.time + 1.0f; } //�ð�����       
    }
    
    // �ݶ��̴��� ���˵Ǹ� ����ϴ�
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == gameObject.name) gameObject.SetActive(false);
    }    

    //0.1sec�ֱ�� �������� Task�� ����ϼ���
    void Do01SecTask()  {
    
    }

    //0.5sec�ֱ�� �������� Task�� ����ϼ���
    void Do05SecTask() {    
        bShowMesh = !bShowMesh;
        SetVisible(bShowMesh);  //���Mesh�� �������ݴϴ�.
    }

    //1sec�ֱ�� �������� Task�� ����ϼ���
    void Do1SecTask()   {
      
    }

}
}
