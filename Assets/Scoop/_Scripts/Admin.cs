using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Admin : MonoBehaviour
{
    [SerializeField] Transform Camera;
    [SerializeField] Transform LHand;
    [SerializeField] Transform RHand;
    [SerializeField] GameObject Scoreboard;
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject pauseMessage;
    [SerializeField] Volume ppProfile;
    private ColorAdjustments colorAdjustment;

    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Pause the game manually
        if (Input.GetKeyDown("space"))
        {
            if (!paused)
            {
                pauseMessage.SetActive(true);
                /*Camera.GetComponent<Rigidbody>().Sleep();*/
                LHand.gameObject.SetActive(false);
                RHand.gameObject.SetActive(false);
                Scoreboard.GetComponent<roomScoreboard>().FreezeBalls();
                ppProfile.GetComponent<Volume>().profile.TryGet(out colorAdjustment);
                colorAdjustment.saturation.value = -100f;
                Timer.GetComponent<Timer>().enabled = false;
                paused = true;
            }
            else
            {
                pauseMessage.SetActive(false);
                /*Camera.GetComponent<Rigidbody>().WakeUp();*/
                LHand.gameObject.SetActive(true);
                RHand.gameObject.SetActive(true);
                Scoreboard.GetComponent<roomScoreboard>().MeltBalls();
                ppProfile.GetComponent<Volume>().profile.TryGet(out colorAdjustment);
                colorAdjustment.saturation.value = -16f;
                Timer.GetComponent<Timer>().enabled = true;
                paused = false;
            }
            
        }

        // Move on to another scene

        // End the scene manually
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Scoreboard.GetComponent<roomScoreboard>().FinishGameManually();

            //////////////////////////////////////////////
            // Add Exit Scene Code Here
            //////////////////////////////////////////////
        }
    }
}
