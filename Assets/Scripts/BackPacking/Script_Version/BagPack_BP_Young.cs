using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class BagPack_BP_Young : MonoBehaviour
{

    Object_BP.STATE m_eState;
    // Start is called before the first frame updateq
    Transform m_tCol;
    public Transform m_tChild;
    UI_BP Hud;
    Object_BP Manager;
    public int unnecessary;
    public int necessary;
    public float fStage1Try; // 1단계 물건을 여기 넣은 횟수
    public float WrongPut; //이걸 왜 넣었냐... 위에 있는 두 변수는 나중에 혹시 필요하지 않을까 해서 일단 모아두는것
    public bool bStage2 = false;
    public HapticController_Joon HapticShake;
    int m_nPosIndex = 0;
    Transform m_tParent;
    Transform m_tPrevParent;
    GrabObj_BP m_GOBJ;
    Transform m_tCrayon;
    Transform m_tWaterBottle;
    Transform m_tGlue;
    Transform m_tPencilCase;
    Transform m_tParentBag; //Bag
    //완성된후 이펙트 추가
    GameObject m_goParticle;
    GameObject m_goBagComplete;
    GameObject m_goBottom;
    GameObject m_goArrow;
    int m_nAllDone = 0;

    void Start()
    {
        Hud = GameObject.Find("UI").GetComponent<UI_BP>();
        Manager = GameObject.Find("GameFlow_Manager").GetComponent<Object_BP>();
        m_eState = Object_BP.STATE.EXIT;
        m_tCrayon = transform.Find("crayon").transform;
        m_tWaterBottle = transform.Find("WaterBottle").transform;
        m_tParentBag = transform.parent; 
        m_tPencilCase = transform.Find("PencilCase_Final").transform;
        m_tGlue = transform.Find("glue").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_eState)
        {
            case Object_BP.STATE.ENTER: Enter(); break;
            case Object_BP.STATE.EXIT: break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<GrabObj_BP>() & m_tCol == null) { m_tCol = collision.transform; m_eState = Object_BP.STATE.ENTER; }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform == m_tCol) { m_eState = Object_BP.STATE.EXIT; m_tCol = null; }
    }

    void Enter()
    {
        if(Object_BP.bGrabbed) return; //wait until trigger is released
        if (!Object_BP.bGrabbed) { CheckObj(m_tCol); m_eState = Object_BP.STATE.EXIT; }
    }
    void CheckObj(Transform obj)
    {
        if (!bStage2) // if stage 1, cannot put obj in bag
        {
            fStage1Try += 1;
            ResetVariable(obj, 4); //add warning
            return;
        }
        switch (obj.tag)
        {
            case "Necessary": CheckCorrect(obj); break;
            case "Necessary_Pencil": necessary++; WrongPut += 1; ResetVariable(obj, 1); break;
            case "Unnecessary": unnecessary++; WrongPut += 1; ResetVariable(obj, 3); break;
        }
        m_tCol = null;
    }
    void CheckCorrect(Transform obj)
    {
        m_tParent = obj;
        m_GOBJ = m_tParent.GetComponent<GrabObj_BP>();
        m_tChild = m_tParent.GetComponentInChildren<MeshRenderer>().transform;
        switch (m_GOBJ.eObj)
        {
            case Object_BP.OBJ_BP.CRAYON     : SetPosition(m_tCrayon); break;
            case Object_BP.OBJ_BP.WATERBOTTLE: SetPosition(m_tWaterBottle); break;
            case Object_BP.OBJ_BP.PCASE      : SetPosition(m_tPencilCase); break;
            case Object_BP.OBJ_BP.GLUE       : SetPosition(m_tGlue); break;
            default: Debug.Log(m_tChild.name); return;
        }
    }

    void SetPosition(Transform m_tTarget)
    {
        Hud.EffectSound("CORRECT");
        m_nAllDone++;
        m_tPrevParent = m_tChild.parent;
        m_tChild.SetParent(this.transform);
        m_tChild.localPosition = m_tTarget.localPosition;
        m_tChild.localEulerAngles = m_tTarget.localEulerAngles;
        m_tChild.localScale = m_tTarget.localScale;
        m_tPrevParent.GetComponent<Outlinable>().enabled = false; //off outlinable
        Destroy(m_tTarget.gameObject);
        Destroy(m_tPrevParent.gameObject);
        if (m_nAllDone >= 4) StartCoroutine(AllDone());

    }

    void ResetVariable(Transform obj, int index)
    {
        switch (index)
        {
            case 1: StartCoroutine(Hud.WrongBag(index)); break; //pencil
            case 2: StartCoroutine(Hud.WrongBag(index)); break; //book
            case 3: StartCoroutine(Hud.WrongBag(index)); break; //memo
            case 4: StartCoroutine(Hud.WrongBag(index)); break;
        }
        obj.GetComponent<GrabObj_BP>().ResetPosition();
        m_tParent = m_tChild = m_tCol = null;
        m_GOBJ = null;
    }

    IEnumerator AllDone()
    {
        Destroy(m_tParentBag.GetComponentInChildren<RaycastCircle>().gameObject); //destroy overlapsphere
        m_tParentBag.GetComponent<Animator>().SetBool("Done", true);
        Hud.BagAllPacked();
        //turn off time
        Debug.Log("AllDone");
        yield return new WaitForSeconds(2.5f);
        BagEffect();
        yield return new WaitForSeconds(1.0f);
        Hud.EffectSound("COMPLETE");
        StartCoroutine(Manager.GameDone());
    }

    void BagEffect()
    {
        m_goParticle = m_tParentBag.Find("VfxBagComplete").gameObject;
        m_goBagComplete = m_tParentBag.Find("BagDone").gameObject;
        m_goArrow = m_tParentBag.Find("Arrow").gameObject;
        m_goBottom = m_tParentBag.Find("bottom").gameObject;
        m_goParticle.SetActive(true);
        m_goBagComplete.SetActive(true);
        m_goBottom.SetActive(false);
        m_goArrow.SetActive(false);
        m_tParentBag.GetComponent<MeshRenderer>().enabled = false;
    }
    //책 잘못 넣으면 시간표 확인
    //물건 잘못 넣으면 알림장 확인
    //연필을 넣으면 아니라고 하기
}
