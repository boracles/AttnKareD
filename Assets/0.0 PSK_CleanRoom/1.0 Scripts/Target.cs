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
    public   bool       m_bNearCatch;    //���ٳ��� ��� ��ó�� ���� �νĿ���
    GameObject          m_SrcObject;     //���ٳ��� �ؾ��� ���ӿ�����Ʈ
    Manager.CLEAN_INFO  m_CleanInfo;
    bool                m_bPositoned;

    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //Editor���� Kind�� �Ҵ��� �ݴϴ�
    public Manager.CLEAN m_eClean;      //��ǥ���ǿ��� �Ҵ��Ͻʽÿ�

    // ����ġ�� ���ٳ�
    void SetPositioned(GameObject go)  {
        //���̻� update �� OnTriggerEnter���� �˻����� ���ϰ�
        m_bPositoned = true;        
        GetComponentInChildren<MeshRenderer>().enabled = false; //������ε� Tager�� ���߰�                
        //Collider�� ��ӹ��� Mesh, Sphere Box Collider�� �ޱ����� Collider�� ����
        GetComponent<Collider>().enabled = false;  //target�� �ݶ����̴� ����
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.mass = 0; rb.drag = 0; rb.angularDrag = 0; rb.useGravity = false; //�����Ӽ�����
        //�ش� ������ ����ġ�� �Ӵϴ�        
        go.transform.parent             = transform;
        go.transform.localPosition      = Vector3.zero;
        go.transform.localEulerAngles   = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //Ouline�� ������� �����մϴ�
        go.GetComponent<Outlinable>().OutlineParameters.Color = HUD.COLOR_OUTLINE_WHITE;        
        //ã�������� �ٽ� ���� �� ���� �������
        go.GetComponent<Grabbable>().enabled = false;
        //CDB����
        Manager.CDB[(int)m_eClean].bClean = true;        
        //������ ���� ���ڿ� ����
        m_Hud.NoteUpdateClean(Manager.saCleanK[(int)m_eClean]);        
        //�����Ѱ��� Guide���� �˷���        
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
        m_SrcObject = GameObject.Find(m_CleanInfo.src_Object_name); //ã�ƾ��� ���ӿ�����Ʈ
        m_Guide     = GameObject.Find("Guide").GetComponent<Guide>();
        m_Hud       = GameObject.Find("HUD").GetComponent<HUD>();
    }

    // Update is called once per frame
    void Update() {
        if(m_bPositoned) return;
        //�ݰ�ȿ� ������ ���ٳ��������� ó���մϴ�.    
        if (m_bNearCatch) {
            if(Vector3.Distance(transform.position, m_SrcObject.transform.position)< m_CleanInfo.accept_radius) 
            SetPositioned(m_SrcObject);
        }
    }
    
    // �ݶ��̴��� ���˵Ǹ� ���ٳ��������� ó���մϴ�.
    void OnTriggerEnter(Collider other) {
        if(m_bPositoned) return;
        if(other.gameObject.name == m_CleanInfo.src_Object_name) {                                    
            SetPositioned(other.gameObject);//������ ������ ó��
        }
    }    
}
}
