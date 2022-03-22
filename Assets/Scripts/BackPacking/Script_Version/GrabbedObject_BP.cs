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
            if (Grabbed != null)// prechild에는 아직 obj가 저장되어있을 때
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
    void UnnCheck(bool check, Outlinable child_outline) // 불필요한 물건 집은 시간
    {
        if (check) GrabbedOutlinable.enabled = false; //잡은 순간 오브젝트의 outline을 꺼야함
        if (!check)
        {
            GrabbedOutlinable.enabled = true; //Grabber가 집은 물건이 사라진다면 outline을 꺼벌림
            GrabbedOutlinable = null;
        }
    }
}
