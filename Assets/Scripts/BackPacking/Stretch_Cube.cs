using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Stretch_Cube : MonoBehaviour
{
    public Transform worldAnchor;
    public Transform PinPoint_Cube;
   

    private Vector3 InitialScale;
    // Start is called before the first frame update
    void Start()
    {
        InitialScale = transform.localScale;

        
    }
    // Update is called once per frame
    void Update()
    {

        Vector3 localAnchor = worldAnchor.position;
        Vector3 localTong = PinPoint_Cube.position;

       

       // Vector3 CubelocalPosition = new Vector3(PinPoint_Cube.position.x, PinPoint_Cube.position.y, PinPoint_Cube.position.z);
       // Vector3 CubeworldPosition = PinPoint_Cube.transform.TransformPoint(CubelocalPosition);

       float distance = Vector3.Distance(localAnchor,localTong);
       transform.localScale = new Vector3(InitialScale.x, distance/2f, InitialScale.z);

        transform.position = (localAnchor + localTong) / 2f;

        Vector3 rotationDirection = (localTong - localAnchor);
        transform.up = rotationDirection;

        
        
        
    }
}
