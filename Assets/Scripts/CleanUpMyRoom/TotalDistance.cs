using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalDistance : MonoBehaviour
{
    public float dist;

    private Vector3 v1;
    private Vector3 v2;

    // Update is called once per frame
    void Update()
    {
        v1 = transform.position;
        dist += Vector3.Distance(v1, v2);
        v2 = transform.position;        
    }
}
