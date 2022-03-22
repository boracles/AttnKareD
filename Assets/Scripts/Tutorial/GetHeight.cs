using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHeight : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform CENTEREYE;
    public Transform FLOOR;
    public static float HEIGHT;
public void Height()
    {
        HEIGHT = CENTEREYE.position.y - FLOOR.position.y;
        Debug.Log(HEIGHT + "  " + CENTEREYE.position.y + "  ");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
