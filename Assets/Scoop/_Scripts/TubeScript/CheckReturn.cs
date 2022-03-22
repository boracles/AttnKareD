using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckReturn : MonoBehaviour
{
    [SerializeField] GameObject scoreboard;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
