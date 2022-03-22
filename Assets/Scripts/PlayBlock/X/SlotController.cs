using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    public Transform MainManager;

    public string BlockType = "";

    public bool IsBooked = false;

    public Material Color_Empty;

    public Material Color_Correct_My;
    public Material Color_Correct_Bot;
    public Material Color_Fail;

    public Material Color_Tip_My;
    public Material Color_Tip_Bot;

    public bool CheckIn = false;
    float timer = 0;

    bool ShowTip = false;
    float TipTimer = 0;

    private void Update()
    {
        if (CheckIn)
        {
            timer += Time.deltaTime;

            if (timer > .7f)
            {
                if (BlockType == "MY")
                {
                    this.transform.GetComponent<MeshRenderer>().material = Color_Correct_My;
                }
                else if (BlockType == "BOT")
                {
                    this.transform.GetComponent<MeshRenderer>().material = Color_Fail;
                }

                timer = 0;
                CheckIn = false;
            }
        }

        if (ShowTip)
        {
            TipTimer -= Time.deltaTime;

            if (TipTimer < 0)
            {
                ShowTip = false;
                SetSlotColor();
            }
        }
    }

    void SetSlotColor()
    {
        if (IsBooked)
        {
            if (BlockType == "MY")
            {
                this.transform.GetComponent<MeshRenderer>().material = Color_Correct_My;
            }
            else if (BlockType == "BOT")
            {
                this.transform.GetComponent<MeshRenderer>().material = Color_Correct_Bot;
            }
        }
        else
        {
            this.transform.GetComponent<MeshRenderer>().material = Color_Empty;
        }
    }

    public void ShowTipUp(int Level)
    {
        if (Level < 4)
        {
            TipTimer = 3f;
        }
        else if (Level >=4 && Level < 7)
        {
            TipTimer = 5f;
        }
        else if (Level >= 7 && Level < 10)
        {
            TipTimer = 8f;
        }
        else
        {
            TipTimer = 10f;
        }

        if (BlockType == "MY")
        {
            this.transform.GetComponent<MeshRenderer>().material = Color_Tip_My;
        }
        else if (BlockType == "BOT")
        {
            this.transform.GetComponent<MeshRenderer>().material = Color_Tip_Bot;
        }

        ShowTip = true;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "MYBLOCK")
        {
            CheckIn = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        timer = 0;
        CheckIn = false;
        this.transform.GetComponent<MeshRenderer>().material = Color_Empty;
    }

    public void ResetAllSlotData()
    {
        this.transform.GetComponent<MeshRenderer>().material = Color_Empty;

        CheckIn = false;
        IsBooked = false;
        ShowTip = false;

        timer = 0;
        TipTimer = 0;
        BlockType = "";
    }





}
