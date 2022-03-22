//using System.Collections;
//using System.Collections.Generic;

//using System;

//using UnityEngine.SceneManagement;
//using System.IO;
//using UnityEngine;
//using Valve.VR;

//public class GetControllerPosAndClickCnt : MonoBehaviour
//{
//    //손하민학생 논문 연구용 데이터 추출 스크립트
//    //추후 삭제될 수도 있고, 계속 사용할 수도 있음

//    public Transform HEAD_Controller; //Vive머리 --- XR Rig Advanced/PlayerController/CameraRig/TrackingSpace/CenterEyeAnchor
//    public Transform Hand_L_Controller; //왼손 컨트롤러 --- XR Rig Advanced/PlayerController/CameraRig/TrackingSpace/LeftHandAnchor/LeftControllerAnchor
//    public Transform Hand_R_Controller; //오른손 컨트롤러 --- XR Rig Advanced/PlayerController/CameraRig/TrackingSpace/RightHandAnchor/RightControllerAnchor

//    int Click_L = 0; //왼손클릭
//    int Click_R = 0; //오른손클릭

//    float Timer = 0; //1초 체크용 타이머

//    string path_Pos;
//    string path_Hand_L;
//    string path_Hand_R;

//    FileStream PosInfo;
//    StreamWriter PosWriter;

//    FileStream LeftInfo;
//    StreamWriter LeftWriter;

//    FileStream RightInfo;
//    StreamWriter RightWriter;

//    string FilePath = Application.streamingAssetsPath + "/Hippo/";
//    string SaveLeft;
//    string SaveRight;

//    void Start()
//    {
//        string SaveTime = DateTime.Now.ToString("yyyyMMddHHmmss");

//        path_Pos = FilePath + SceneManager.GetActiveScene().name + "_" + SaveTime + "_Pos" + ".txt";
//        path_Hand_L = FilePath + SceneManager.GetActiveScene().name + "_" + SaveTime + "_Left" + ".txt";
//        path_Hand_R = FilePath + SceneManager.GetActiveScene().name + "_" + SaveTime + "_Right" + ".txt";

//        SteamVR_Actions.vRIF_LeftTriggerDown.AddOnStateDownListener(TriggerLeftPressed, SteamVR_Input_Sources.Any);
//        SteamVR_Actions.vRIF_RightTriggerDown.AddOnStateDownListener(TriggerRightPressed, SteamVR_Input_Sources.Any);
//    }

//    void Update()
//    {
//        Timer += Time.deltaTime;

//        if (Timer > .2f)
//        {
//            PosInfo = new FileStream(path_Pos, FileMode.Append, FileAccess.Write);
//            PosWriter = new StreamWriter(PosInfo, System.Text.Encoding.Unicode);
//            PosWriter.WriteLine(GetDetailPos());
//            PosWriter.Close();

//            Timer = 0;
//        }
//    }

//    string GetDetailPos()
//    {
//        string Head_x = HEAD_Controller.localPosition.x.ToString();
//        string Head_y = HEAD_Controller.localPosition.y.ToString();
//        string Head_z = HEAD_Controller.localPosition.z.ToString();

//        string Hand_Lx = Hand_L_Controller.localPosition.x.ToString();
//        string Hand_Ly = Hand_L_Controller.localPosition.y.ToString();
//        string Hand_Lz = Hand_L_Controller.localPosition.z.ToString();

//        string Hand_Rx = Hand_R_Controller.localPosition.x.ToString();
//        string Hand_Ry = Hand_R_Controller.localPosition.y.ToString();
//        string Hand_Rz = Hand_R_Controller.localPosition.z.ToString();

//        return "HEAD (" + Head_x + ", " + Head_y + ", " + Head_z + ")     Hand_L (" + Hand_Lx + ", " + Hand_Ly + ", " + Hand_Lz + ")     Hand_R (" + Hand_Rx + ", " + Hand_Ry + ", " + Hand_Rz + ")";
//    }

//    private void TriggerLeftPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
//    {
//        Click_L += 1;
//        Debug.Log("Click_L" + Click_L.ToString());

//        SaveLeft = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff");

//        LeftInfo = new FileStream(path_Hand_L, FileMode.Append, FileAccess.Write);
//        LeftWriter = new StreamWriter(LeftInfo, System.Text.Encoding.Unicode);
//        LeftWriter.WriteLine(SaveLeft);
//        LeftWriter.Close();
//    }

//    private void TriggerRightPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
//    {
//        Click_R += 1;
//        Debug.Log("Click_R" + Click_R.ToString());

//        SaveRight = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff");

//        RightInfo = new FileStream(path_Hand_R, FileMode.Append, FileAccess.Write);
//        RightWriter = new StreamWriter(RightInfo, System.Text.Encoding.Unicode);
//        RightWriter.WriteLine(SaveRight);
//        RightWriter.Close();
//    }
//}
