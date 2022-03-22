using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class ClosetGrabbables_NumCheck : MonoBehaviour
{
    [Header("Get Closest Grabbables")]
    GrabbablesInTrigger GetGrabbable;
    public Grabbable currentGrabbable;
    Grabbable prevGrabbable;

    // Start is called before the first frame update
    void Start()
    {
        GetGrabbable = GetComponent<GrabbablesInTrigger>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GetGrabbable.ClosestGrabbable)
        {
            currentGrabbable = GetGrabbable.ClosestGrabbable;
            GrabbableKinematic(currentGrabbable, true);
        }
        if(!GetGrabbable.ClosestGrabbable && currentGrabbable)
        {
            GrabbableKinematic(currentGrabbable, false);
        }
        
        
    }

    private void GrabbableKinematic(Grabbable goGrabed, bool check)
    {
        if(goGrabed.tag == "Necessary")
        {
            if (check)
            {
                goGrabed.GetComponent<Rigidbody>().isKinematic = check;
                prevGrabbable = goGrabed;
            }
            if (!check)
            {
                goGrabed.GetComponent<Rigidbody>().isKinematic = check;

                if (prevGrabbable)
                {
                    prevGrabbable.GetComponent<Rigidbody>().isKinematic = false;
                    prevGrabbable = null;

                }
            }

        }
        
        
        
    }
}
