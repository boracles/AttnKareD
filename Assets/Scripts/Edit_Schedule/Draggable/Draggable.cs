using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

[RequireComponent(typeof(Grabbable))]
public class Draggable : MonoBehaviour
{
    // VR interactive item component
    Grabbable grabbable;

    // States
    enum State { Ready, Dragging, Blocked}

    // Current state
    State currState;

    //Initial position
    Vector3 initPos;

    //Initial rotation
    Quaternion initRot;

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();

        currState = State.Ready;

        // Save initial position
        initPos = transform.position;

        // Save initial rotation
        initRot = transform.rotation;        
    }

    private void OnEnable()
    {
       
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
