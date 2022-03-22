using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] GameObject barrier;

    int frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, -25 * Time.deltaTime, 0);
        frame++;
        if(frame == 1000)
        {
            barrier.SetActive(false);
        }
    }
}
