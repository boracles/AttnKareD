using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeBall : MonoBehaviour
{
    Vector3 initP; // Initial Position of Ball
    Vector3 initR; // Initial Rotation of Ball
    public int ballMatID;

    float timer;

    [Header("Materials")]
    [SerializeField] Material tubeBall1;
    [SerializeField] Material tubeBall2;
    [SerializeField] Material tubeBall3;

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

        if(GetComponent<Renderer>().sharedMaterial == tubeBall1)
        {
            ballMatID = 1;
        }
        else if (GetComponent<Renderer>().sharedMaterial == tubeBall2)
        {
            ballMatID = 2;
        }
        else if (GetComponent<Renderer>().sharedMaterial == tubeBall3)
        {
            ballMatID = 3;
        }
        else
        {
            ballMatID = -1;
        }

        if(ballMatID == -1)
        {
            Destroy(gameObject);
        }
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
                resetBall();
            }
            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When ball hits the floor (drop from shovel or bounce out)
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Boundary")
        {
            // When Ball Hits anything other than the start container return ball to start container and increment drop count

            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().totalDrops++;
            resetBall();
            /*Debug.Log("Ball out");*/
            ScoreCheck = false;
            gameObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().BallUpdate(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Yellow Tube
        if (other.gameObject.tag == "Checker1" && GetComponent<Renderer>().sharedMaterial == tubeBall1)
        {
            ScoreCheck = true;
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().BallUpdate(gameObject);
        } 
        else if (other.gameObject.tag == "Checker1" && GetComponent<Renderer>().sharedMaterial != tubeBall1)
        {
            // When Tube is not filled
            if (GetComponentInParent<TubeScoreboard>().score1 < GetComponentInParent<TubeScoreboard>().stageBalls)
            {
                gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().wrongColor++;
            }
            else
            {
                gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().wrongExcess++;
            }
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().soundEffects.GetComponent<SoundEffects>().WrongBall(); // Play sound when wrong ball is put in
            resetBall();
            gameObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().BallUpdate(gameObject);
        }

        // Light Purple Tube
        if (other.gameObject.tag == "Checker2" && GetComponent<Renderer>().sharedMaterial == tubeBall2)
        {
            ScoreCheck = true;
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().BallUpdate(gameObject);
        }
        else if (other.gameObject.tag == "Checker2" && GetComponent<Renderer>().sharedMaterial != tubeBall2)
        {
            if (GetComponentInParent<TubeScoreboard>().score2 < GetComponentInParent<TubeScoreboard>().stageBalls)
            {
                gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().wrongColor++;
            }
            else
            {
                gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().wrongExcess++;
            }
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().soundEffects.GetComponent<SoundEffects>().WrongBall(); // Play sound when wrong ball is put in
            resetBall();
            gameObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().BallUpdate(gameObject);
        }

        // Turqoise Tube
        if (other.gameObject.tag == "Checker3" && GetComponent<Renderer>().sharedMaterial == tubeBall3)
        {
            ScoreCheck = true;
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().BallUpdate(gameObject);
        }
        else if (other.gameObject.tag == "Checker3" && GetComponent<Renderer>().sharedMaterial != tubeBall3)
        {
            if (GetComponentInParent<TubeScoreboard>().score3 < GetComponentInParent<TubeScoreboard>().stageBalls)
            {
                gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().wrongColor++;
            }
            else
            {
                gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().wrongExcess++;
            }
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().soundEffects.GetComponent<SoundEffects>().WrongBall(); // Play sound when wrong ball is put in
            resetBall();
            gameObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<TubeScoreboard>().BallUpdate(gameObject);
        }
    }

    // Function to reset ball back to where it was instantiated
    public void resetBall()
    {
        // Reset Velocity, Position and Angle
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.position = initP;
        gameObject.transform.eulerAngles = initR;
    }
}
