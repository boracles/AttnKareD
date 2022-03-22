using UnityEngine;
using HutongGames.PlayMaker;
using BNG;

public class OnReleaseChecker : MonoBehaviour
{
    Animator animator;
    Grabber grabber;
    string grabbableName;
    GameObject foundGameObj;

    GameObject objToFind;    

    public PlayMakerFSM GoDuckFsm;


    void Start()
    {
        grabber = GetComponent<Grabber>();
    }
    
    string OnGrabObjInfoCheck()
    {
        Initialize();

        grabbableName = grabber.HeldGrabbable.name;

        if (grabber.HeldGrabbable.tag == "Unnecessary")
        {
            SendEvent("Angry sound");
            animator =  grabber.HeldGrabbable.GetComponent<Animator>();
            animator.SetInteger("Status", 2);
        }

        return grabbableName;
    }

    void ReleaseObjInfoCheck()
    {
        FindGameObj(grabbableName);

        if(foundGameObj.tag == "Necessary")
        {

        }

        else if (foundGameObj.tag == "Unnecessary")
        {
            SendEvent("Quacking sound");
            foundGameObj.GetComponent<MoveableDuck>().SendMessage("SpeedUp");
            animator = foundGameObj.GetComponent<Animator>();
            animator.SetInteger("Status", 1);
        }
    }

    GameObject FindGameObj(string name)
    {
        foundGameObj = GameObject.Find(name);
        return foundGameObj;
    }

    void Initialize()
    {
        grabbableName = null;
        foundGameObj = null;
    }    

    private void SendEvent(string currentEvent)
        {
            //fsmG_Obj = GoDuckFsm.FsmVariables.GetFsmGameObject("grab_g");
            GoDuckFsm.SendEvent(currentEvent);
        }
}
