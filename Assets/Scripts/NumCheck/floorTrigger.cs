using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class floorTrigger : MonoBehaviour
{
    public InputBridge XRrig;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Necessary") && other.gameObject.layer == LayerMask.NameToLayer("numCard"))
        {

            StartCoroutine(TriggerCheck(other.transform));

        }


    }
    
    IEnumerator TriggerCheck(Transform other)
    {
        while(XRrig.RightTrigger > 0.3f)
        {
            yield return null;
            
                

        }
        if(XRrig.RightTrigger <= 0.3f)
        {
            other.GetComponent<NumCard>().ResetPosRot();
        }
    }


   
}
