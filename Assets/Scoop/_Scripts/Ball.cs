using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 initP; // Initial Position of Ball
    Vector3 initR; // Initial Rotation of Ball

    float timer;

    // Property of Ball to check if Ball is in the Container
    public bool ScoreCheck
    {
        get; set;
    }

    // Start is called before the first frame update
    void Start()
    {
        initP = transform.position; // Save initial position of ball
        initR = transform.eulerAngles; // Save initial angle of ball
    }

    // Update is called once per frame
    void Update()
    {
        // Return Ball if it escapes room
        timer += Time.deltaTime;
        if (timer > 2)
        {
            if (Vector3.Distance(gameObject.transform.position, initP) > 150f)
            {
                Debug.Log("Ball Lost");
                resetBall(gameObject);
            }
            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When ball hits quad inside the brown container
        if(collision.gameObject.tag == "Surface")
        {
            /*Debug.Log("Ball in");*/
            ScoreCheck = true;
            Debug.Log("Surface is Hit");
        }

        // When ball hits the floor (drop from shovel or bounce out)
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "StartContainer" || collision.gameObject.tag == "Desk")
        {
            // When Ball Hits anything other than the start container return ball to start container and increment drop count
            if(collision.gameObject.tag != "StartContainer")
            {
                resetBall(gameObject);
                GetComponentInParent<Scoreboard>().totalDrops++;
            }

            /*Debug.Log("Ball out");*/
            ScoreCheck = false;
        }

        if (collision.gameObject.tag == "Boundary")
        {
            Debug.Log("Ball Hit Boundary");
            resetBall(gameObject);
            ScoreCheck = false;
            if(GetComponentInParent<Scoreboard>().totalDrops > 0) GetComponentInParent<Scoreboard>().totalDrops--;
        }

        // Calls scoreUpdate function in Scoreboard script every time ball collides with the environment
        GetComponentInParent<Scoreboard>().scoreUpdate();
    }

    // Function to reset ball back to where it was instantiated
    public void resetBall(GameObject ball)
    {
        // Reset Velocity, Position and Angle
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.transform.position = initP;
        ball.transform.eulerAngles = initR;
    }
}
