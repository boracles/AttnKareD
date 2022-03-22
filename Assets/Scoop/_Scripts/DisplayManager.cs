using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera subCamera1;
    public Camera subCamera2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        foreach (Display display in Display.displays)
        {
            Debug.Log(display);
        }

        Display.displays[0].Activate();
        Display.displays[1].Activate();
        Display.displays[2].Activate();
        /*Display.displays[0].Activate();*/

        Debug.Log(Display.displays[1]);
        Debug.Log(Display.displays[2]);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
