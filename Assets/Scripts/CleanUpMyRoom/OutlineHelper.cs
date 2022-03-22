using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPOOutline;


namespace BNG {

    /// <summary>
    /// Shows a ring at the grab point of a grabbable if within a certain distance
    /// </summary>
    public class OutlineHelper : MonoBehaviour {              

        /// <summary>
        /// Don't show grab rings if left and right controllers / grabbers are  holding something
        /// </summary>
        public bool HideIfHandsAreFull = true;

        Grabbable grabbable;
        Outlinable outlinable;

        /// <summary>
        /// Used to determine if hands are full
        /// </summary>
        Grabber leftGrabber;
        Grabber rightGrabber;
        bool handsFull = false;

        // Animate opacity
        private float _initalOpacity;
        private float _currentOpacity;

        Transform mainCam;

        // Start is called before the first frame update
        void Start() {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;

            grabbable = transform.parent.GetComponent<Grabbable>();
            outlinable = transform.parent.GetComponent<Outlinable>();
            
            outlinable.enabled = false;
            _currentOpacity = _initalOpacity;

            // Assign left / right grabbers
            Grabber[] grabs = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Grabber>();
            foreach(var g in grabs) {
                if(g.HandSide == ControllerHand.Left) {
                    leftGrabber = g;
                }
                else if (g.HandSide == ControllerHand.Right) {
                    rightGrabber = g;
                }
            }
        }

        void Update() {

            // Bail if Text Component was removed or doesn't exist
            if (mainCam == null) {
                return;
            }

            bool grabbersExist = leftGrabber != null && rightGrabber != null;

            // Holding Item
            handsFull = grabbersExist && leftGrabber.HoldingItem && rightGrabber.HoldingItem;

            // Not a valid Grab
            if(grabbersExist && grabbable.GrabButton == GrabButton.Grip && !leftGrabber.FreshGrip && !rightGrabber.FreshGrip) {
                handsFull = true;
            }

            bool showOutlines = handsFull;
            

            // If being held or not active, immediately hide the ring
            if (grabbable.BeingHeld || !grabbable.isActiveAndEnabled) {
                
                return;
            }

            // Show if within range
            float currentDistance = Vector3.Distance(transform.position, mainCam.position);
            if(!handsFull && currentDistance <= grabbable.RemoteGrabDistance) {
                showOutlines = true;
            }
            else {
                showOutlines = false;                
            }

            // Animate ring opacity in / out
            if(showOutlines) {
                
                // If a valid grabbable, increase size a bit
                if (grabbable.GetClosestGrabber() != null && grabbable.IsGrabbable()) {
                    outlinable.enabled = true;
                }
                else {
                    outlinable.enabled = false;
                }                                
            }
            else {
                outlinable.enabled = false;
            }
        }

        Grabber closestGrabber;        
    }
}

