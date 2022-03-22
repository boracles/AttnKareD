using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject objectToTrack;
    [SerializeField] Vector3 objectPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = objectToTrack.transform.localPosition;

        objectPosition.x = objectToTrack.transform.position.x;
        objectPosition.y = objectToTrack.transform.position.y;
        objectPosition.z = objectToTrack.transform.position.z;
    }
}
