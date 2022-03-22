using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Valve.VR;

public class HapticController_Joon : MonoBehaviour 
{
    //왼쪽 controller에도 진동 기능 추가하게 수정
    //알림장 확인하라고 알려줄 때 진동

    public bool HapticsOnCollision = true;

    public float VibrateFrequency = 0.3f;
    public float VibrateAmplitude = 0.1f;
    public float VibrateDuration = 0.1f;

    void doHaptics()
    {
        //SteamVR_Actions.vRIF_Haptic.Execute(0, VibrateFrequency, VibrateAmplitude, VibrateDuration, SteamVR_Input_Sources.RightHand);        
    }

    void doHaptics_Left()
    {
        //SteamVR_Actions.vRIF_Haptic.Execute(0, VibrateFrequency, VibrateAmplitude, VibrateDuration, SteamVR_Input_Sources.LeftHand);
    }
}
