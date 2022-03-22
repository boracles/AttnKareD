using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideCollider_BP : MonoBehaviour
{
    // Start is called before the first frame update
    GrabObj_BP m_Object;
    Object_BP.OBJ_BP m_kind;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<GrabObj_BP>())
        {
            m_Object = collision.transform.GetComponent<GrabObj_BP>();
            m_kind = m_Object.eObj;
            if (m_kind != Object_BP.OBJ_BP.DISTURB) m_Object.ResetPosition();
            if (m_kind == Object_BP.OBJ_BP.DISTURB) { }
        }
    }
}
