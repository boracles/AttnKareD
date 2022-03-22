using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Transform MainManager;

    public bool NowActivated = false;

    public Transform Hand_Left;
    public Transform Hand_Right;

    bool NowHolding = false;

    GameObject colObj;

    Vector3 StartPos;

    int ReTryCnt = 0;


    private void Start()
    {
        StartPos = this.transform.position;
        NowHolding = false;
    }

    void Update()
    {
        if (NowActivated)
        {
            if (!NowHolding && (Hand_Left.GetComponent<BNG.HandController>().GripAmount != 0 || Hand_Right.GetComponent<BNG.HandController>().GripAmount != 0))
            {
                NowHolding = true;
                Debug.Log("HOLD");
            }

            if (NowHolding)
            {
                if (Hand_Left.GetComponent<BNG.HandController>().GripAmount == 0 && Hand_Right.GetComponent<BNG.HandController>().GripAmount == 0)
                {
                    Debug.Log("DROP");
                    SetCubeToSlot();
                    NowHolding = false;
                }
            }

            if (ReTryCnt == 5)
            {
                MainManager.GetComponent<PlayBlockManager>().ReDoStage();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colObj = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        colObj = null;
    }

    public void SetCubeToSlot()
    {
        if (colObj != null)
        {
            if (colObj.tag == "SLOT")
            {
                if (colObj.transform.GetComponent<SlotController>().BlockType == "MY" && !colObj.transform.GetComponent<SlotController>().IsBooked)
                {
                    Debug.Log("DROP_DONE");
                    transform.position = colObj.transform.position;
                    transform.localEulerAngles = Vector3.zero; // colObj.transform.localEulerAngles;
                    colObj.transform.GetComponent<SlotController>().IsBooked = true;

                    MainManager.GetComponent<PlayBlockManager>().SetNextMyBlock();


                    NowActivated = false;
                    ReTryCnt = 0;
                }
                else
                {
                    //fail - 이미 있는곳에 또 놓은 경우
                    ReTryMyBlock();
                    Debug.Log("DROP_FAIL_1");
                }
            }
            else
            {
                //fail - 잘못된 자리에 둔 경우
                ReTryMyBlock();
                Debug.Log("DROP_FAIL_2");
            }
        }
        else
        {
            //fail - 떨어뜨리거나 던졌다고 판정
            ReTryMyBlock();
            Debug.Log("DROP_FAIL_3");
        }
    }


    void ReTryMyBlock() //다시하기
    {
        this.transform.position = StartPos;
        this.transform.localEulerAngles = Vector3.zero;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        NowHolding = false;
        colObj = null;

        ReTryCnt += 1;
    }



    public void ResetBlock() //초기화 - 실패
    {
        ReTryMyBlock();
        this.gameObject.SetActive(false);
        NowActivated = false;
        ReTryCnt = 0;
    }



}
