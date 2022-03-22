using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class GrabObj_BP : MonoBehaviour
{
    // Start is called before the first frame update
    public Object_BP.OBJ_BP eObj;
    public Object_BP.KIND_BP eKind;

    Vector3 m_v3Pos;
    Vector3 m_v3Rot;
    void Start()
    {
        m_v3Pos = this.transform.localPosition;
        m_v3Rot = transform.localEulerAngles;
    }

    public void ResetPosition()
    {
        transform.localPosition = m_v3Pos;
        transform.localEulerAngles = m_v3Rot;
        Debug.Log("RESET");
    }
}
