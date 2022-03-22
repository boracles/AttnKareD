using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;


public class switchObjName : MonoBehaviour
{
    public PlayMakerFSM goFsm;
    
    private FsmString fsm_s;
    private string setName;  

    private void SwitchName(string getName)
    {
        string setName = getName;
        fsm_s = goFsm.FsmVariables.GetFsmString("kor_name");

        switch (setName)
        {
            case "Box01":                
                fsm_s.Value = "박스";
                break;
            case "Lamp01":
                fsm_s.Value = "스탠드";
                break;
            case "Book02":
                fsm_s.Value = "책";
                break;
            case "Globe01":
                fsm_s.Value = "지구본";
                break;
            case "Hammer01":
                fsm_s.Value = "뿅망치";
                break;
            case "Mouthwash01":
                fsm_s.Value = "로션";
                break;
            case "Apple01":
                fsm_s.Value = "사과";
                break;
            case "ToyCar01":
                fsm_s.Value = "장난감자동차";
                break;
            case "Game_Controller01":
                fsm_s.Value = "게임패드";
                break;
            default: break;
        }

        goFsm.SendEvent("returnKor");
    }    
}

