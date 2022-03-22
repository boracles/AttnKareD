using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BNG;

public class TriggerButton : MonoBehaviour //IPointerUpHandler // IPointerExitHandler,
{
    public GameObject crnt;
    public GameObject prev;
    public GameObject temp;
    public string trigNum;
    int trigNum_tmp;

    /*
private void Start()
{
    trigNum_tmp = int.Parse(trigNum) - 1;
}

public void OnCollisionExit()
{
    if(temp)
    {
        temp.GetComponent<MoveButton>().triggered = false;
        temp = null;
    }
}


public void OnPointerUp(PointerEventData pointerEventData)
{

    if (temp)
    {
        if (temp.GetComponent<MoveButton>().click == false)
        {
            crnt = temp;
            MoveButton button = crnt.GetComponent<MoveButton>();
            Manager.CardInTrigger(button, this);
        }
    }

}

public void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.tag == "Necessary")
    {
        if (Manager.currentButton != null)
        {
            Manager.active = true;
            temp = Manager.currentButton;
            temp.GetComponent<MoveButton>().triggered = true;
            temp.GetComponent<MoveButton>().Trigger = this.gameObject;
        }
    }
}
*/
}
