using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
public class TempScriptJason : MonoBehaviour
{
    PlayMakerFSM FSM;
    public float[] arrFloat;
    float data_501;
    float data_502;
    float data_503;
    float data_504;
    float data_505;
    float data_506;
    float data_507;
    float data_508;
    float data_509;
    float data_510;

    // Start is called before the first frame update
    void Start()
    {
        FSM = this.transform.GetComponent<PlayMakerFSM>();
        arrFloat = new float[] { data_501, data_502, data_503, data_504, data_505, data_506, data_507, data_508, data_509, data_510 };
    }

    public void End()
    {
        int index = 0;
        for(int i = 501; i <511; i++)
        {
            string name = "data_" + i.ToString();
            FsmFloat fsmFloat = FSM.FsmVariables.GetFsmFloat(name);
            arrFloat[index] = fsmFloat.Value;
            index++;
        }
    }
}
