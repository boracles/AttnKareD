using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using KetosGames.SceneTransition;

public class Manager_Paddle : MonoBehaviour
{
    public enum STAGE { ONE, TWO, THREE}
    public static int intStage =0;

    public struct STAGE_INFO
    {
        public STAGE eStage;
        public float fTime;
        public string strHANDLE;
        public string strORDER;
        public int intCount;
        public int intPercent;
      //  public int[] arrOrder;
      //  public int[] arrAnswer;

        public STAGE_INFO(STAGE eStage, float fTime, string strHANDLE, string strORDER, int intCount, int intPercent)
        {
            this.eStage = eStage;
            this.fTime = fTime;
            this.strHANDLE = strHANDLE;
            this.strORDER = strORDER;
            this.intCount = intCount;
            this.intPercent = intPercent;

        }
    }

    public static STAGE_INFO[] SDB = new STAGE_INFO[]  {
        new STAGE_INFO(STAGE.ONE, 2.5f, null, "FORWARD", 6, 6), // 6프로씩 6번 돌리기
        new STAGE_INFO(STAGE.TWO, 4.0f, null, "BACKWARD", 3, 8 ),
        new STAGE_INFO(STAGE.THREE, 3.0f, null, "FORWARD", 5, 8)

    };
       
}
