using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using BNG;

public class GrabbedObject_BP : MonoBehaviour
{
    // Start is called before the first frame update
    Grabber Grabber;
    Outlinable GrabbedOutlinable;
    public GameObject Grabbed;
    string m_sTag;
    Outlinable preChild;
    Object_BP Manager;

    public AddDelimiter delimiters;

    GameObject dataGameobject;

    bool nothing = true;//or when grabbing nothing
    bool disturbed = true; // for when grabbing unnecessary
    public float m_fbpUnpkT;
    public float m_fbpUnpC;
    // Start is called before the first frame update
    // Update is called once per frame



    void Start()
    {
        Grabber = this.transform.GetComponent<Grabber>();
        Manager = GameObject.Find("GameFlow_Manager").GetComponent<Object_BP>();
    }
    void Update()
    {
        if (Grabber.HeldGrabbable)
        {
            Grabbed = Grabber.HeldGrabbable.gameObject;
            m_sTag = Grabbed.tag;
            GrabbedOutlinable = Grabbed.GetComponent<Outlinable>();
            UnnCheck(true, GrabbedOutlinable);
            if (!nothing) //Stop IDLE in data log
            {
                delimiters.addIDLE(nothing);
                nothing = true;
            }
            switch (m_sTag)
            {
                case "Necessary": if (Manager.m_bStageChangeTime) Manager.m_bStageChangeTime = false; break;
                case "Necessary_Book": if (Manager.m_bStageChangeTime) Manager.m_bStageChangeTime = false; break;
                case "Unnecessary": UnnecessaryCheck(); break;
            }
        }
        if (Grabber.HeldGrabbable == null)
        {
            if (nothing) //START IDLE in data log
            {
                Debug.Log("NOTHIG");
                delimiters.addIDLE(nothing);
                nothing = false;
            }
            if (!disturbed)
            {
                Debug.Log("DISTURBED");
                delimiters.addDISTURB(disturbed);
                disturbed = true;
            }
            if (Grabbed != null)// prechild���� ���� obj�� ����Ǿ����� ��
            {
                UnnCheck(false, preChild);
                Grabbed = null;
            }
        }
    }

    void UnnecessaryCheck()
    {
        m_fbpUnpkT += Time.deltaTime; //Unn_timeCheck.SetActive(true);
        if (disturbed)
        {
            m_fbpUnpC += 1;   
            delimiters.addDISTURB(disturbed);
            disturbed = false;
        }
    }
    void UnnCheck(bool check, Outlinable child_outline) // ���ʿ��� ���� ���� �ð�
    {
        if (check) GrabbedOutlinable.enabled = false; //���� ���� ������Ʈ�� outline�� ������
        if (!check)
        {
            GrabbedOutlinable.enabled = true; //Grabber�� ���� ������ ������ٸ� outline�� ������
            GrabbedOutlinable = null;
        }
    }
}
