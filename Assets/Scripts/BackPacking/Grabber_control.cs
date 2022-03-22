using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using HutongGames.PlayMaker;
using BNG;

public class Grabber_control : MonoBehaviour
{

    public GameObject Grabbed;
    bool check; //집었을 떄 outline을 끄고 내려놓ㅎ았을때 outline을 키기
    Outlinable preChild;
    public float UnObject_Pick; // == bpUnpkT(필요하지 않은 물건을 집은 시간)
    
    public bool stageCheck;
    public bool stageCheck2;

    public PlayMakerFSM UnnGrab;
    public GameObject Unn_timeCheck;
    public AddDelimiter delimiters;

    GameObject dataGameobject;

    bool nothing = true;//or when grabbing nothing
    bool disturbed = true; // for when grabbing unnecessary

    // Start is called before the first frame update
    // Update is called once per frame




    void Update()
    {
        Outlinable[] child_outline = gameObject.GetComponentsInChildren<Outlinable>();


        if(child_outline.Length>0) //if grabber has child
        {
            if(!nothing) //Stop IDLE in data log
            {
                delimiters.addIDLE(nothing);
                nothing = true;
            }
            
            preChild = child_outline[0]; //가장 첫번째 child(but there's only one child anyway)
            Grabbed = preChild.gameObject; // send Grabbed to FSM
            check = true;
            UnnCheck(check, child_outline[0]);
            Grabbed.GetComponent<Collider>().isTrigger = true;


            if(preChild.tag == "Unnecessary")
            {
                Unn_timeCheck.SetActive(true);
                if(disturbed)
                {
                    UnnGrab.SendEvent("UnnecessaryObjectGrab");
                    delimiters.addDISTURB(disturbed);
                    disturbed = false;
                }
                
            }

            if( preChild.tag == "Necessary" || preChild.tag == "Necessary_Book")
            {
                if (stageCheck2 == true) //외부 gamemanager_Fsm 에서 stsge2가 시작되면 set property로 stagecheck2 를 true로 설정해줌
                {
                    stageCheck = true; //Stage1to2_Manager에서 스테이지 2로 옮기고 필요한 물건을 집으면 stagecheck을 true로 설정해 "stage 1 이후 stage 2를 시작하기까지 걸리는 시간"을 측정
                }

            }

        }
        else if(child_outline.Length==0 ) //Grabber아래에는 gameobject가 안들어있는데 
        {
            if (nothing) //START IDLE in data log
            {
                delimiters.addIDLE(nothing);
                nothing = false;

            }
            if (!disturbed)
            {
                delimiters.addDISTURB(disturbed);
                disturbed = true;

            }



            if(preChild != null)// prechild에는 아직 obj가 저장되어있을 때
            {
                check = false;
                UnnCheck(check, preChild);
                Unn_timeCheck.SetActive(false);
                Grabbed.GetComponent<Collider>().isTrigger = false;

                Grabbed = null;
            }
            
            


        }


        
    }

    void UnnCheck(bool check, Outlinable child_outline) // 불필요한 물건 집은 시간
    {
        if(check) //
        {
            child_outline.enabled= false; //잡은 순간 오브젝트의 outline을 꺼야함
        }

        else if(!check) 
        {

            child_outline.enabled = true; //Grabber가 집은 물건이 사라진다면 outline을 꺼벌림
            preChild = null;
            
        }

    }

   
}
