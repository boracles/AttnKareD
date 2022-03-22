using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyTubeBall : MonoBehaviour
{
    Vector3 initP; // Initial Position of Ball
    Vector3 initR; // Initial Rotation of Ball
    public int ballMatID;

    float timer;

    [Header("Materials")]
    [SerializeField] Material tubeBall1;
    [SerializeField] Material tubeBall2;
    [SerializeField] Material tubeBall3;

    [SerializeField] GameObject popups;

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

            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().totalDrops++;
            resetBall();
            /*Debug.Log("Ball out");*/
            ScoreCheck = false;
            gameObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().BallUpdate(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Yellow Tube
        if (other.gameObject.tag == "Checker1" && GetComponent<Renderer>().sharedMaterial == tubeBall1)
        {
            ScoreCheck = true;
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().BallUpdate(gameObject);
        } 
        else if (other.gameObject.tag == "Checker1" && GetComponent<Renderer>().sharedMaterial != tubeBall1)
        {
            if(GetComponentInParent<EasyTubeScoreboard>().score1 == 0)
            {
                gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().wrongColor++;
            }
            else
            {
                gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().wrongExcess++;
            }
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().soundEffects.GetComponent<SoundEffects>().WrongBall();
            resetBall();
            gameObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().BallUpdate(gameObject);
        }

        // Light Purple Tube
        if (other.gameObject.tag == "Checker2" && GetComponent<Renderer>().sharedMaterial == tubeBall2)
        {
            ScoreCheck = true;
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().BallUpdate(gameObject);
        }
        else if (other.gameObject.tag == "Checker2" && GetComponent<Renderer>().sharedMaterial != tubeBall2)
        {
            if (GetComponentInParent<EasyTubeScoreboard>().score2 == 0)
            {
                gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().wrongColor++;
            }
            else
            {
                gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().wrongExcess++;
            }
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().soundEffects.GetComponent<SoundEffects>().WrongBall();
            resetBall();
            gameObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().BallUpdate(gameObject);
        }

        // Turqoise Tube
        if (other.gameObject.tag == "Checker3" && GetComponent<Renderer>().sharedMaterial == tubeBall3)
        {
            ScoreCheck = true;
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().BallUpdate(gameObject);
        }
        else if (other.gameObject.tag == "Checker3" && GetComponent<Renderer>().sharedMaterial != tubeBall3)
        {
            if (GetComponentInParent<EasyTubeScoreboard>().score3 == 0)
            {
                gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().wrongColor++;
            }
            else
            {
                gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().wrongExcess++;
            }
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().soundEffects.GetComponent<SoundEffects>().WrongBall();
            resetBall();
            gameObject.SetActive(false);
            gameObject.transform.parent.GetComponentInParent<EasyTubeScoreboard>().BallUpdate(gameObject);
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
