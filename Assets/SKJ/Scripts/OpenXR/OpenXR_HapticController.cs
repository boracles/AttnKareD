using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class OpenXR_HapticController :MonoBehaviour
{
    public float frequency;
    public float amplitude;
    public float duration;

    public InputBridge inputBridge;
    // Start is called before the first frame update
    
    void DoHaptics_R()
    {
        inputBridge.VibrateController(frequency, amplitude, duration, ControllerHand.Right);        
    }

    void DoHaptics_L()
    {
        inputBridge.VibrateController(frequency, amplitude, duration, ControllerHand.Left);
    }
}
