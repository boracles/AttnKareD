using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

namespace BNG
{
    public class GameFlow_Manager : MonoBehaviour
    {

        public float LTrigger;
        public GameObject XRRig;
        private PlayMakerFSM fsm;
        bool alrdyStart;
       
        public bool strtCount;






        // Start is called before the first frame update

        // Update is called once per frame

        void Start()
        {
            alrdyStart = false;

        }



        void Update()
        {
            LTrigger = XRRig.GetComponent<InputBridge>().LeftTrigger;
            fsm = this.gameObject.GetComponent<PlayMakerFSM>();

            TriggerCheck();

        }

        void TriggerCheck() //처음 시작 321 카운트다운 트리거 누르기
        {

            if (strtCount == true) //countdown이 끝나면 FSM에서 remotely하게 false로 설정
            {

                if (LTrigger >= .9)//트리거가 눌렷는데
                {

                    if (alrdyStart == false) //countdown이 시작 상태가 아니라면
                    {

                        fsm.Fsm.Event("CountDown"); //COUNTDOWN event로 보냄
                        alrdyStart = true; 

                    }

                }

                if (LTrigger <= 0.9) //트리거가 눌리지 않았는데
                {

                    if (alrdyStart == true) //countdown은 시작됐다면
                    {

                        fsm.Fsm.Event("Reset"); //countdown을 Reset시킴
                        alrdyStart = false; 

                    }

                }

            }


        }

       
    }

}

