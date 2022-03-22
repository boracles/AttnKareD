using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class SwapPossible : MonoBehaviour
{
    public InputBridge xrrig;
    // Start is called before the first frame update
    SnapZone Snapped;
    Collider collided;
    bool triggerPressed = false;

    private void Start()
    {
        Snapped = transform.GetComponent<SnapZone>();
    }

    private void Update()
    {
        if(xrrig.RightTrigger < 0.2f)
        {
            triggerPressed = false;
        }
        if(xrrig.RightTrigger >0.5f)
        {
            triggerPressed = true;
        }
        
    }
    public void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.name  == "Card"&& triggerPressed)
        {
            collided = other;
            StartCoroutine(TriggerIn(other));
        }
                

            
        
    }
    public void OnTriggerExit(Collider other)
    {
        if(collided == other)
        {
            StopAllCoroutines();
        }
       
    }

    IEnumerator TriggerIn(Collider other)
    {
        yield return new WaitUntil(()=>triggerPressed == false);
        CardOut(other);

    }




    private void CardOut(Collider other)
    {
        

        
        if (Snapped.HeldItem != other && Snapped.HeldItem != null)
        {
            Grabbable prevHeld = Snapped.HeldItem;
            Snapped.ReleaseAll();
            prevHeld.GetComponent<NumCard>().ResetPosRot();
            Snapped.HeldItem = other.transform.GetComponent<Grabbable>();
            transform.GetComponent<TriggerCheck_NumCheck>().TriggerIn();

        }
        
    }

    public void CardReset()
    {
        Grabbable prevHeld = Snapped.HeldItem;
        Snapped.ReleaseAll();
        prevHeld.GetComponent<NumCard>().ResetPosRot();
    }

}
