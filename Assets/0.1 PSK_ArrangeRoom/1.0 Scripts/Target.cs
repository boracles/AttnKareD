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
    public   bool       m_bNearCatch;    //���ٳ��� ��� ��ó�� ���� �νĿ���
    GameObject          m_SrcObject;     //���ٳ��� �ؾ��� ���ӿ�����Ʈ
    Manager.ARRANGE_INFO m_ArrangeInfo;
    bool                m_bPositoned;

    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //Editor���� Kind�� �Ҵ��� �ݴϴ�
    public Manager.ARRANGE m_eArrange;      //��ǥ���ǿ��� �Ҵ��Ͻʽÿ�

    //����ڰ� ������ ������ Target�� 3�ʰ� ������ disable�����ݴϴ�.-called by guide
    public void DeActivate() {        
        StartCoroutine("DelayedOut");
    }
    //Arrange�� �ȵǸ� 3�� �ڿ� �������(�������� �ݶ��̴� �⵿�ϴ½ð����� ��ٸ�)
    IEnumerator DelayedOut() {           
       yield return new WaitForSeconds(3f); 
       if(m_bPositoned) yield break;
       else this.gameObject.SetActive(false); 
    }

    // ����ġ�� ���ٳ�
    void SetPositioned(GameObject go)  {        
        m_bPositoned = true;        //���̻� update �� OnTriggerEnter���� �˻����� ���ϰ�
        go.tag = Manager.saTag[(int)Manager.TAG.UNNECESSARY]; //������ �����͸� ���̻� ���� �ʵ���
        SetVisible(false);                        //������ε� Tager Mesh�� ����
        GetComponent<Collider>().enabled = false; //target�� �ݶ����̴� ��ȿȭ       
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.mass = 0; rb.drag = 0; rb.angularDrag = 0; rb.useGravity = false; //�����Ӽ�����
        //�ش� ������ ����ġ�� �Ӵϴ�        
        go.transform.parent             = transform;
        go.transform.localPosition      = Vector3.zero;
        go.transform.localEulerAngles   = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //�����Ǹ� Ouline�� ������� 3�ʰ� ������ ����

        //Ouline�� �����ͼ�
        Outlinable outline = go.GetComponent<Outlinable>();
        //����� ������ ����
        StartCoroutine(OulineBlinkOut(outline));
        //ã�������� �ٽ� ���� �� ���� �������        
        go.GetComponent<Grabbable>().enabled = false;
        //CDB����
        Manager.CDB[(int)m_eArrange].bArranged = true;        
        //������ ���� ���ڿ� ����
        //m_Hud.NoteUpdateArrange(Manager.saArrangeK[(int)m_eArrange]);
        //
        //�����Ѱ��� Guide���� �˷���        
        m_Guide.SetArranged(this.gameObject);
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
        m_SrcObject   = GameObject.Find(m_ArrangeInfo.src_Object_name); //ã�ƾ��� ���ӿ�����Ʈ
        m_Guide       = GameObject.Find("Guide").GetComponent<Guide>();
        m_Hud         = GameObject.Find("HUD").GetComponent<HUD>();
    }

    // Update is called once per frame        
    float next01SecUpdate = 0;
    float next05SecUpdate = 0;
    float next1SecUpdate  = 0;
    void Update() {
        if(m_bPositoned) return;
        if (Time.time > next01SecUpdate) { Do01SecTask(); next01SecUpdate = Time.time + 0.1f; } //�ð�����
        if (Time.time > next05SecUpdate) { Do05SecTask(); next05SecUpdate = Time.time + 0.5f; } //�ð�����
        if (Time.time > next1SecUpdate)  { Do1SecTask();  next1SecUpdate  = Time.time + 1.0f; } //�ð�����
                
        //�ݰ�ȿ� ������ ���ٳ��������� ó���մϴ�.    
        if (m_bNearCatch) {
            if(Vector3.Distance(transform.position, m_SrcObject.transform.position)< m_ArrangeInfo.accept_radius) 
            SetPositioned(m_SrcObject);
        }        
    }
    
    // �ݶ��̴��� ���˵Ǹ� ���ٳ��������� ó���մϴ�.
    void OnTriggerEnter(Collider other) {
        if(m_bPositoned) return;
        if(other.gameObject.name == m_ArrangeInfo.src_Object_name) {                                    
            SetPositioned(other.gameObject);//������ ������ ó��
        }
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
