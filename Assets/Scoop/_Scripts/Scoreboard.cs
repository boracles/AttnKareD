using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [Header("Ball Debugger")]

    [Tooltip("Instantiated Balls")]
    // List of balls instantiated
    public List<GameObject> clonedBalls = new List<GameObject>();
    [Tooltip("Successfully moved balls")]
    // List of balls that are successfully moved
    public List<string> successBalls = new List<string>();

    [Header("Score Board")]
    /*public TextMesh scoreBoard; // Score Text*/
    public GameObject scoreText;
    public int totalDrops = 0; // Total number of drops throughout game
    public string clearTime = ""; // Clear Time, shown after game finishes
    private int score = 0; // Game Score
    private float stageCounter = 1; // Stage number
    private int stageDrops = 0; // Number of Drops after stage is cleared, updated after each stage finishes

    [Header("Prefabs and Objects")]
    public Transform clone; // Ball prefab
    public GameObject timer; // Timer Text
    public GameObject waitMessage;

    float delayTimer;
    float startTime = 0;
    public bool endOfGame = false;
    public bool endGame = false;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate 1 ball
        createBall();

    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;

        if (clonedBalls.Count == score && delayTimer - startTime > 4.8f && delayTimer - startTime < 5.2f && !endOfGame)
        {
            Debug.Log("Timer Finished: " + delayTimer);
            StartCoroutine(stageClear());
        }
    }

    public void setBallsVisible(bool isVisible)
    {
        foreach (GameObject ball in clonedBalls)
        {
            ball.GetComponent<Renderer>().enabled = isVisible;
        }
    }

    // Function called when stage is cleared (Reset Stage)
    public void reset()
    {
        foreach (GameObject ball in clonedBalls)
        {
            GetComponentInChildren<Ball>().resetBall(ball);
            ball.GetComponent<Ball>().ScoreCheck = false;
            /*ball.GetComponentInChildren<Ball>().dropCount = 0;*/
        }
        createBall();
        successBalls.Clear();
        score = 0;
        stageCounter++;
        setBallsVisible(false);
        scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\n남은 공: " + (clonedBalls.Count - score).ToString() + " 개\n\n떨어뜨린 횟수: " + totalDrops.ToString() + "\n\n";
        /*scoreBoard.text = "Stage " + stageCounter + "\n\n남은 공: " + (clonedBalls.Count - score).ToString() + " 개\n\nDrops: " + totalDrops.ToString();*/
    }

    // Updates score on each ball collision
    public void scoreUpdate()
    {
        // Calls Update function every time any ball collides with environment
        foreach (GameObject ball in clonedBalls)
        {
            ballUpdate(ball);
        }

        // Updates Scoreboard Text
        scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\n남은 공: " + (clonedBalls.Count - score).ToString() + " 개\n\n떨어뜨린 횟수: " + totalDrops.ToString() + "\n\n";
        /*scoreBoard.text = "Stage " + stageCounter + "\n\n남은 공: " + (clonedBalls.Count - score).ToString() + " 개\n\nDrops: " + totalDrops.ToString();*/

        if (clonedBalls.Count == score)
        {
            startTime = delayTimer;
            Debug.Log("Timer Started: " + startTime);
            /*if(clonedBalls.Count == score && delayTimer-startTime > 4.8f && delayTimer-startTime < 5.2f)
            {
                StartCoroutine(stageClear());
            }*/

        }
        else
        {
            startTime = 0;
            /*StopAllCoroutines();*/
        }
    }

    // Update ball status
    void ballUpdate(GameObject ball)
    {
        // When ball hits quad but is not yet listed
        if (ball.GetComponent<Ball>().ScoreCheck && !successBalls.Contains(ball.name))
        {
            successBalls.Add(ball.name);
            score++;
        }
        // When ball hits floor(outside of container) and was already listed
        else if (!ball.GetComponent<Ball>().ScoreCheck && successBalls.Contains(ball.name))
        {
            successBalls.Remove(ball.name);
            score--;
        }
    }

    // Instantiates Ball
    public void createBall()
    {
        Transform dummy = Instantiate(clone, gameObject.transform);
        dummy.localPosition = new Vector3(UnityEngine.Random.Range(-25f, -13.1f), -23.1f, UnityEngine.Random.Range(-56.5f, -44.5f));
        dummy.gameObject.name = (clonedBalls.Count + 1).ToString();
        clonedBalls.Add(dummy.gameObject);
    }

    // Delay
    /*IEnumerator Wait5()
    {
        yield return new WaitForSeconds(5);
    }*/

    IEnumerator Wait()
    {
        Debug.Log("Start Wait Coroutine");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Wait Coroutine Finished");
    }

    // Function called when stage is cleared
    IEnumerator stageClear()
    {
        stageDrops = totalDrops;

        // Wait 5 seconds after successfully moving all balls
        /*yield return StartCoroutine(Wait5());*/

        // If any ball escapes container before 5 second countdown, break out of this function
        /*if (clonedBalls.Count != score)
        {
            yield break;
        }*/

        // If score is 10, end game
        if (score == 3)
        {
            clearTime = timer.GetComponent<Text>().text;
            foreach (GameObject ball in clonedBalls)
            {
                Destroy(ball);
            }
            scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 횟수: " + totalDrops.ToString() + "\n\nClear Time: " + clearTime;
            /*scoreBoard.text = "Finish!\n\nDrops: " + totalDrops.ToString() + "\n\nClear Time: " + clearTime;*/
            timer.SetActive(false);
            endOfGame = true;
        }
        // If score is not 10, move onto next stage
        else if (clonedBalls.Count == score)
        {
            reset();
            scoreText.SetActive(false);
            waitMessage.SetActive(true);

            while (true)
            {
                Debug.Log("Start waiting...");
                yield return StartCoroutine(Wait());
                Debug.Log("Finished waiting...");

                if (totalDrops == stageDrops) break;

                //////////////////////////////////////////////////////

                Debug.Log("Ball Overlapped");

                totalDrops = stageDrops;

                foreach (GameObject ball in clonedBalls)
                {
                    ball.transform.localPosition = new Vector3(UnityEngine.Random.Range(-25f, -13.1f), -23.1f, UnityEngine.Random.Range(-56.5f, -44.5f));
                }
            }

            yield return StartCoroutine(Wait());

            score = 0;
            waitMessage.SetActive(false);
            scoreText.SetActive(true);
            setBallsVisible(true);
        }
    }

    // Function to reset stage, when unintentional overlapping of ball objects occur and drop count is incremented
    /*public void errorCheck()
    {
        Debug.Log("Error Check Function Called");
        ResetBalls();
    }*/

    // Reset all balls to random position in container
    public void ResetBalls()
    {
        Debug.Log("Reset Balls Function Called From Room.cs");

        totalDrops = stageDrops;

        foreach (GameObject ball in clonedBalls)
        {
            ball.transform.localPosition = new Vector3(UnityEngine.Random.Range(-25f, -13.1f), -23.1f, UnityEngine.Random.Range(-56.5f, -44.5f));
        }

        if (totalDrops == stageDrops)
        {
            totalDrops = stageDrops;
            return;
        }
    }

    public void FreezeBalls()
    {
        foreach (GameObject ball in clonedBalls)
        {
            ball.GetComponent<Rigidbody>().useGravity = false;
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    public void MeltBalls()
    {
        foreach (GameObject ball in clonedBalls)
        {
            ball.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void FinishGameManually()
    {
        clearTime = timer.GetComponent<Text>().text;
        foreach (GameObject ball in clonedBalls)
        {
            Destroy(ball);
        }
        scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 횟수: " + totalDrops.ToString();
        /*scoreBoard.text = "Finish!\n\nDrops: " + totalDrops.ToString() + "\n\nClear Time: " + clearTime;*/
        timer.SetActive(false);
        endOfGame = true;
    }

    // When game is terminated, record data
    private void OnApplicationQuit()
    {
        if (clearTime != "")
        {
            GetComponent<SaveScoopData>().SaveTempSceneData("Drops: " + totalDrops.ToString() + "\n\nClear Time: " + clearTime + "\n");
        }
        else
        {
            GetComponent<SaveScoopData>().SaveTempSceneData("Drops: " + totalDrops.ToString() + "\n\nTerminated(Stage " + stageCounter + ")\n");
        }

    }
}
