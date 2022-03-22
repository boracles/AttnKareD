using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanSlotController : MonoBehaviour
{
    public GameObject passenger;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PLAN")
        {
            this.transform.GetComponent<Image>().color = new Color(0, 255, 0, 0.5f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "PLAN")
        {
            this.transform.GetComponent<Image>().color = new Color(255, 255, 255, 0.15f);
        }
    }

    public void resetPlanSlot()
    {
        passenger = null;
        this.transform.GetComponent<Image>().color = new Color(255, 255, 255, 0.15f);
    }
}
