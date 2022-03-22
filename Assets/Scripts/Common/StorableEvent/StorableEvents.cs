using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BNG;

namespace BNG_Cus_AddOn
{
    [RequireComponent(typeof(GameObject))]

    public abstract class StorableEvents : MonoBehaviour
    {
        protected GameObject store;
        protected GameObject thisStorer;

        protected InputBridge input;
        protected StoreRemote remoteInput;

        protected virtual void Awake()
        {
            store = GetComponent<GameObject>();
            input = InputBridge.Instance;
            remoteInput = StoreRemote.Instance;
        }

        public virtual void OnRelease(){

        }

        /// <summary>
        /// Amount of Trigger being held down on the grabbed items controller. Only fired if object is being held.
        /// </summary>
        /// <param name="triggerValue">0 - 1 Open / Closed</param>
        public virtual void OnTrigger(float triggerValue)
        {

        }

        /// <summary>
        /// Fires if trigger was pressed down on this controller this frame, but was not pressed last frame. Only fired if object is being held.
        /// </summary>
        public virtual void OnTriggerDown()
        {

        }

        /// <summary>
        /// Fires if Trigger is not held down this frame
        /// </summary>
        public virtual void OnTriggerUp()
        {

        }

        /// <summary>
        /// Grabbable has been successfully inserted into a SnapZone
        /// </summary>
        public virtual void OnSnapZone_S_Enter()
        {

        }

        /// <summary>
        /// Grabbable has been removed from a SnapZone
        /// </summary>
        public virtual void OnSnapZone_S_Exit()
        {

        }               
    }

    /// <summary>
    /// Customized Field by Josh
    /// A UnityEvent with a GameObject as the parameter
    [System.Serializable]
    public class StorableEvent : UnityEvent<GameObject> { }

    /// <summary>
    /// A UnityEvent with a Grabber as the parameter
    /// </summary>
    [System.Serializable]
    public class StorageEvent : UnityEvent<GameObject> { }
}


