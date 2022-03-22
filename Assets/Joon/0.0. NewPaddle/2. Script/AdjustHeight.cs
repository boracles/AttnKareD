using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeight : MonoBehaviour
{
    public Transform floor;
    public float ApprxHeight;
    public Transform MidPoint;
    float m_fMid;
    float m_fFloor;
    public bool m_bHeight = true;
    // Start is called before the first frame update
    float height;
    int a;
    private void Awake()
    {
       
    }
    void Start()
    {
        height = GetHeight.HEIGHT;
        ApprxHeight = MidPoint.position.y - floor.position.y;
        float m_fTaller = height - ApprxHeight;
        float m_fSmaller = ApprxHeight - height;
        if (height > ApprxHeight) {floor.position = new Vector3(floor.position.x, floor.position.y - (m_fTaller), floor.position.z); Debug.Log(height + "  " + ApprxHeight + "  " + m_fTaller + "   " + m_fSmaller + "  "); return; } //taller
        if (height < ApprxHeight) floor.position = new Vector3(floor.position.x, floor.position.y + (m_fSmaller), floor.position.z);
        Debug.Log(height + "  " + ApprxHeight + "  " + m_fTaller + "   " + m_fSmaller + "  ");
    }

    // Update is called once per frame
    void Update()
    {/*
        if (m_bHeight)
        {

            if (m_fMid < m_fFloor)
            {
                floor.position = new Vector3(floor.position.x, floor.position.y+0.01f, floor.position.z);
                m_fMid = Mathf.Abs(CenterEye.position.y - MidPoint.position.y);
                m_fFloor = Mathf.Abs(floor.position.y - MidPoint.position.y);
                if (m_fMid >= m_fFloor) m_bHeight = false;
            }
            if (m_fMid > m_fFloor)
            {
                floor.position = new Vector3(floor.position.x, floor.position.y - 0.01f, floor.position.z);
                m_fMid = Mathf.Abs(CenterEye.position.y - MidPoint.position.y);
                m_fFloor = Mathf.Abs(floor.position.y - MidPoint.position.y);
                if (m_fMid <= m_fFloor) m_bHeight = false;
            }
            
        }
        */
    }
}
