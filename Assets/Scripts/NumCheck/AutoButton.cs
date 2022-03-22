using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BNG;
public class AutoButton : MonoBehaviour
{
    // Please, name correctly your variables.
    // Yourself, in 3 months will be grateful
    GameObject m_rectAuto;
    Vector3 m_target;
    public float speed = 1.0f;
    public GameObject Finger;
    public InputBridge XrRig;
    public UIPointer RighthandPointer;
    Guide_NumCheck guide;
    TextMeshProUGUI m_tmrpoText;
    Color activatedColor = Color.white;
    Color originalColor;
    bool m_bStart;
    public float m_fBothering; //방해받은 시간
    public bool m_bSet;
    TextandSpeech narration;
    void Start()
    {
        guide = GameObject.Find("Guide").GetComponent<Guide_NumCheck>();
        narration = GameObject.Find("Guide").GetComponent<TextandSpeech>();
    }

    void Update()
    {
        if (m_bStart)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            m_rectAuto.transform.localPosition = Vector3.MoveTowards(m_rectAuto.transform.localPosition, m_target, step);
            if (RighthandPointer.GetComponent<LineRenderer>().enabled == true)
            {
                if (XrRig.RightTrigger > 0.8f) { m_fBothering += Time.deltaTime; Bothering(); }
                if (XrRig.RightTrigger < 0.2f) m_bSet = true;
            }
        }
    }

    public void AutoMove()
    {
        guide.CannotGrab(null);
        StartCoroutine(SetPosition());
    }
    IEnumerator SetPosition()
    {
        originalColor = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goNum.GetComponentInChildren<TextMeshProUGUI>().color;
        Finger.SetActive(true);
        narration.Bothered(true);
        m_bStart = true;
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            m_rectAuto = Finger;
            int count = Random.Range(0, guide.arrPos.Count);
            m_target = guide.arrPos[count];
            yield return new WaitUntil(() => m_rectAuto.transform.localPosition == m_target);
            yield return new WaitForSeconds(1.2f);
        }
        m_target = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goNum.transform.localPosition;
        yield return new WaitUntil(() => m_rectAuto.transform.localPosition == m_target);
        m_tmrpoText = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goNum.GetComponentInChildren<TextMeshProUGUI>();
        //==== First Randomly roam around board to show that it is in process
        m_tmrpoText.color = activatedColor;
        yield return new WaitForSeconds(1.0f);
        Finger.SetActive(false);
        //=== change text color and disable finger when select correct number
        m_rectAuto = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goNum;
        m_target = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goTrig.transform.localPosition;
        yield return new WaitUntil(() => m_rectAuto.transform.localPosition == m_target);
        //=== move number
        m_bStart = false;
        m_tmrpoText.color = originalColor;
        narration.Bothered(false);
        guide.CanGrab();
        Guide_NumCheck.Index++;
    }
    void Bothering()
    {
        if (m_bSet)
        {
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(2));
            m_bSet = false;
        }
    }

}
