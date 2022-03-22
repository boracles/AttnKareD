using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using HutongGames.PlayMaker;
using UserData;
using TMPro;


namespace BNG.UserData {
    public class TutorialManager : MonoBehaviour
    {
        DataManager dataManager;
        private float rTriggerValue;
        private float ltriggerValue;
        private float rGripValue;
        private float lGripValue;
        private InputBridge Controller;
        public GameObject XRRig;
        public PlayMakerFSM Playmaker;
        public GameObject Grabber;
        public GameObject Grabbed;
        public GameObject Note;
        public bool FSMCheck;
        public bool FSMCheck2;
        public GameObject Ghost;
        Outlinable[] childGrabbed;
        Outlinable prechildGrabbed;
        public GameObject heldGrabbable;
        public int trigInt;

        private string gradeLH;
        public string SceneName;



        // Start is called before the first frame update
        void Start()
        {
            trigInt = 0;
            GetGrade();
      

        }

        // Update is called once per frame
        void Update()
        {
            childGrabbed = Grabber.GetComponentsInChildren<Outlinable>();

            if (Grabber.GetComponent<Grabber>().HeldGrabbable != null)
            { heldGrabbable = Grabber.GetComponent<Grabber>().HeldGrabbable.transform.gameObject; }



            Controller = XRRig.GetComponent<InputBridge>();
            rTriggerValue = Controller.RightTrigger;
            ltriggerValue = Controller.LeftTrigger;
            lGripValue = Controller.LeftGrip;
            rGripValue = Controller.RightGrip;



        }

        private void LateUpdate()
        {
            if (FSMCheck)
            {
                //TriggerDown Check
                if (rTriggerValue > 0.7f && ltriggerValue > 0.7f) Playmaker.FsmVariables.GetFsmBool("triggerDown").Value = true;
                if (lGripValue > 0.7f && rGripValue > 0.7f) ghostSpeak(0);
            }

            if (FSMCheck2)
            {
                if (ltriggerValue >= 0.5){
                    Note.SetActive(true);
                    Playmaker.FsmVariables.GetFsmBool("lTriggerPressed").Value = true;
                }
                if (ltriggerValue <= 0.9)Note.SetActive(false);
            }



        }

        public void ghostSpeak(int strIndex)
        {
            int index = Random.Range(0, 3);
            string[] Speechstr = {"그 버튼이 아니야\n<color=#2e86de>(O _ O)!","바닥을 한번 살펴봐!","뒤를 돌아볼래?", "직접 걸어서\n다가가야해!" };
            StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak(Speechstr[strIndex],index,2.5f ));

        }


        public void GetGrade()
        {
            GameObject JasonManager = GameObject.Find("DataManager");
            dataManager = JasonManager.GetComponent<DataManager>();
            gradeLH = dataManager.userInfo.Grade;
            if(gradeLH == "L")SceneName= "DoorLock";
            if(gradeLH == "H") SceneName = "CleanUpMyRoom_2x2";
        }

       
    }

     
    






}
