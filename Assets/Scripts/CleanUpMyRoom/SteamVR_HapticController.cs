using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Valve.VR;

public class SteamVR_HapticController : MonoBehaviour
{
    public bool HapticsOnCollision = true;

    public float VibrateFrequency = 0.3f;
    public float VibrateAmplitude = 0.1f;
    public float VibrateDuration = 0.1f;

    void doHaptics()
    {
        //SteamVR_Actions.vRIF_Haptic.Execute(0, VibrateFrequency, VibrateAmplitude, VibrateDuration, SteamVR_Input_Sources.RightHand);        
    }
}
