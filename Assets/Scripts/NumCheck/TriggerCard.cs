using BNG;
using UnityEngine;

public class TriggerCard : MonoBehaviour
{
    public InputBridge xrRig;
    public NumCheckManager Manager;
    public string orderNum;
    GameObject card;
  //bool alrdyIN = false;
    bool inout = false;
    int arrNum;
   // bool alrdyIn = false;
    MeshRenderer meshRenderer;



    GrabbablesInTrigger gZone;
    // Start is called before the first frame update
    void Start()
    {
        gZone = GetComponent<GrabbablesInTrigger>(); //to get all closet grabbables
        arrNum = int.Parse(orderNum) - 1;
        meshRenderer = GetComponent<MeshRenderer>();
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Necessary") )
        {

            TriggerCheck(other);
           
        }
     
       
    }

    public void OnTriggerExit(Collider other)
    {
        
        if(other.CompareTag("Necessary"))
        {
            if(other.gameObject == card)
            {
                TriggerExitCheck();
            }
            
        }
    }

    public void TriggerCheck(Collider other)
    {
        other.GetComponent<Rigidbody>().isKinematic = true;

        if (xrRig.RightTrigger < 0.2f)
    {
        //   other.GetComponent<Rigidbody>().isKinematic = true;
        
        Manager.arrOrder[arrNum] = other.transform.GetComponent<NumCard>().cardNum;
        Manager.answerInt += 1;
            meshRenderer.enabled = false;
     //   other.transform.GetComponent<NumCard>().SetPosRot(this.transform);
        card = other.gameObject;
        inout = true;
        Manager.GetComponent<NumCheckManager>().CompareArr();

    }


    }

    public void TriggerExitCheck()
    {
        if (inout){
            meshRenderer.enabled = true;
            Manager.arrOrder[arrNum] = null;
            Manager.answerInt -= 1;
            // card = null;
            inout = false;

        }
           


    }


}
