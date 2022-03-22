using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BNG;

namespace BNG_Cus_AddOn
{

    // <summary>
    // This will call a specified event, and then DROP this object. It is meant to be used as a proxy. If you want to call an event on Grab, use GrabbableEvents instead.
    // For example, if you grab a trigger with a GrabAction, you can spawn a different item in the users hand (such as a weapon clip, arrow, etc.).
    // </summary>
    public class StoreAction : StorableEvents
    {
        // slot에 오브젝트를 담는 이벤트
        //public StorageEvent OnStoreEvent;

        //Grabbable g;
        //float lastGrabTime = 0;
        //float minTimeBetweenGrabs = 0.2f; // In Seconds

        //public override void OnGrab(Grabber grabber)
        //{

        //    if (g == null)
        //    {
        //        g = GetComponent<Grabbable>();
        //    }

        //    // Never hold this item
        //    g.DropItem(grabber, false, false);

        //    // Don't grab this if we are currently grabbing / remote grabbing a different item
        //    if (grabber.RemoteGrabbingItem || grabber.HoldingItem)
        //    {
        //        return;
        //    }

        //    // Call the event
        //    if (OnStoreEvent != null)
        //    {

        //        // Don't want to repeatedly do grabs if this is a hold item
        //        if (Time.time - lastGrabTime >= minTimeBetweenGrabs)
        //        {
        //            OnStoreEvent.Invoke(grabber);
        //            lastGrabTime = Time.time;
        //        }
        //    }
        //}
    }
}