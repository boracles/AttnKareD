using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoop : MonoBehaviour
{
    Vector3 scoopPos;
    Vector3 scoopRot;
    float timer;
    public GameObject scoreboard;

    [Tooltip("Center Camera of XR Rig")]
    public Transform headCamera;

    // Start is called before the first frame update
    void Start()
    {
        scoopPos = gameObject.transform.localPosition;
        scoopRot = gameObject.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // If Scoop escapes room, return it to its original position (based on distance between camera and scoop)
        timer += Time.deltaTime;

        if (timer > 2)
        {
            if (Vector3.Distance(gameObject.transform.position, headCamera.position) > 150f)
            {
                ResetScoop();
                Debug.Log("Too Far Away");
            }
            timer = 0;
        }

        /*if (GetComponent<BNG.Grabbable>().BeingHeld)
        {
            Debug.Log("You are grabbing " + gameObject.name);
            Debug.Log("Hand: " + gameObject.transform.parent.parent);

            GetComponent<BoxCollider>().enabled = false;

            for (int i = 0; i < gameObject.transform.childCount; i++){
                if (gameObject.transform.GetChild(i).GetComponent<BoxCollider>() != null)
                {
                    gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
                }
                else if (gameObject.transform.GetChild(i).GetComponent<CapsuleCollider>() != null)
                {
                    gameObject.transform.GetChild(i).GetComponent<CapsuleCollider>().enabled = false;
                }
            }

            if(gameObject.transform.parent.parent.name == "RightController")
            {
                Debug.Log(gameObject.name + " has found right controller");
                RShovelCollider.gameObject.SetActive(true);
            }
            else if(gameObject.transform.parent.parent.name == "LeftController")
            {
                Debug.Log(gameObject.name + " has found left controller");
                LShovelCollider.gameObject.SetActive(true);
            }
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).GetComponent<BoxCollider>() != null)
                {
                    gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                }
                else if (gameObject.transform.GetChild(i).GetComponent<CapsuleCollider>() != null)
                {
                    gameObject.transform.GetChild(i).GetComponent<CapsuleCollider>().enabled = true;
                }
            }

            RShovelCollider.gameObject.SetActive(false);
            LShovelCollider.gameObject.SetActive(false);
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If ball hits boundary outside room, return it to its original position (Only when object escapes room due to extreme force applied)
        if (collision.gameObject.CompareTag("Boundary") || collision.gameObject.CompareTag("Terrain"))
        {
            ResetScoop();
            Debug.Log("Hit Boundary");
        }
    }

    private void ResetScoop()
    {
        gameObject.transform.localPosition = scoopPos;
        gameObject.transform.eulerAngles = scoopRot;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("Reset Scoop Function called");
    }
}
