using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        // If Ball Hits boundary, return ball back to original position (Only when ball escapes room)
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("Ball Hit Boundary");
            GetComponentInChildren<Scoreboard>().ResetBalls();
        }
    }
}
