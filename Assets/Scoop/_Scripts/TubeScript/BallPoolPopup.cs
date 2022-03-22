using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPoolPopup : MonoBehaviour
{
    [SerializeField] GameObject popups;
    bool ballPoolShown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BallPool") && !ballPoolShown)
        {
            Debug.Log("Ball Pool Message");
            StartCoroutine(popups.GetComponent<PopupManager>().ShowMessage(popups.GetComponent<PopupManager>().ballPool, 10f)); // Show Guide Message
            ballPoolShown = true;
        }
    }
}
