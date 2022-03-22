using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLeavingCheck : MonoBehaviour
{
    [SerializeField] private GameObject objToFind;
    [SerializeField] private bool isException;

    private void Awake()
    {
        isException = true;
    }

    void Start()
    {        
        objToFind = FindObjectOfType<BNG.CollectData>().gameObject;        
    }

    bool EndException()
    {
        isException = false;
        return isException;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isException)
        {
            objToFind.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE END");            
            Debug.Log("In");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            objToFind.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE START");
            EndException();
            Debug.Log("OUT");
        }
    }
}
