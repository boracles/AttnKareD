using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class DataCheck_NumMatch : MonoBehaviour
{
    [Tooltip("When card number is not in correct order number")]
    public float WrongOrder=0;
    [Tooltip("When Card is Grabbed")]
    public float GrabbedCount=0;
    [Tooltip("When Unnecessary Card is Grabbed")]
    public float UnGrabbedCount=0;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDebug()
    {
        Debug.Log(GrabbedCount);
        Debug.Log(UnGrabbedCount);
        Debug.Log(WrongOrder);
    }
}
