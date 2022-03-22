//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.Events;

//namespace BNG {
//    public class SnapZone_GameObject : MonoBehaviour {

//        [Header("Starting / Held Item")]
//        [Tooltip("The currently held item. Set this in the editor to equip on start.")]
//        public GameObject StoredItem;

//        [Header("Options")]
//        /// <summary>
//        /// If false, Item will Move back to inventory space if player drops it.
//        /// </summary>
//        [Tooltip("If false, Item will Move back to inventory space if player drops it.")]
//        public bool CanDropItem = true;

//        /// <summary>
//        /// If false the snap zone cannot have it's content replaced.
//        /// </summary>
//        [Tooltip("If false the snap zone cannot have it's content replaced.")]
//        public bool CanSwapItem = true;

//        /// <summary>
//        /// If false the item inside the snap zone may not be removed
//        /// </summary>
//        [Tooltip("If false the snap zone cannot have it's content replaced.")]
//        public bool CanRemoveItem = true;

//        /// <summary>
//        /// Multiply Item Scale times this when in snap zone.
//        /// </summary>
//        [Tooltip("Multiply Item Scale times this when in snap zone.")]
//        public float ScaleItem = 1f;
//        private float _scaleTo;

//        public bool DisableColliders = true;
//        List<Collider> disabledColliders = new List<Collider>();

//        [Tooltip("If true the item inside the SnapZone will be duplicated, instead of removed, from the SnapZone.")]
//        public bool DuplicateItemOnGrab = false;

//        /// <summary>
//        /// Only snap if Grabbable was dropped maximum of X seconds ago
//        /// </summary>
//        [Tooltip("Only snap if Grabbable was dropped maximum of X seconds ago")]
//        public float MaxDropTime = 0.1f;

//        /// <summary>
//        /// Last Time.time this item was snapped into
//        /// </summary>
//        [HideInInspector]
//        public float LastSnapTime;

//        [Header("Filtering")]
//        /// <summary>
//        /// If not empty, can only snap objects if transform name contains one of these strings
//        /// </summary>
//        [Tooltip("If not empty, can only snap objects if transform name contains one of these strings")]
//        public List<string> OnlyAllowNames;

//        /// <summary>
//        /// Do not allow snapping if transform contains one of these names
//        /// </summary>
//        [Tooltip("Do not allow snapping if transform contains one of these names")]
//        public List<string> ExcludeTransformNames;

//        [Header("Audio")]
//        public AudioClip SoundOnSnap;
//        public AudioClip SoundOnUnsnap;


//        [Header("Events")]
//        /// <summary>
//        /// Optional Unity Event  to be called when something is snapped to this SnapZone. Passes in the Grabbable that was attached.
//        /// </summary>
//        public StorableEvent OnSnap_G_Event;

//        /// <summary>
//        /// Optional Unity Event to be called when something has been detached from this SnapZone. Passes in the Grabbable is being detattached.
//        /// </summary>
//        public StorableEvent OnDetach_G_Event;
        
//        GrabbablesInTrigger gZone;

//        Rigidbody heldItemRigid;
//        bool heldItemWasKinematic;
//        Grabbable trackedItem; // If we can't drop the item, track it separately

//        // Closest Grabbable in our trigger
//        [HideInInspector]
//        public GameObject ClosestGameObject;

//        SnapZoneOffset offset;

//        // Start is called before the first frame update
//        void Start() {
//            gZone = GetComponent<GrabbablesInTrigger>();
//            _scaleTo = ScaleItem;

//            // Auto Equip item
//            if (StoredItem != null) {
//                Store_Obj(StoredItem);
//            }
//        }

//        // Update is called once per frame
//        void Update() {

//            ClosestGameObject = getClosestGameObject();

//            // Can we grab something
//            if (StoredItem == null && ClosestGameObject != null) {
//                float secondsSinceDrop = Time.time - ClosestGameObject.LastDropTime;
//                if (secondsSinceDrop < MaxDropTime) {
//                    Store_Obj(ClosestGameObject);
//                }
//            }

//            // Keep snapped to us or drop
//            if (StoredItem != null) {

//                // Something picked this up or changed transform parent
//                if (/* StoredItem.BeingHeld || */ StoredItem.transform.parent != transform) {
//                    ReleaseAll();
//                }
//                else {
//                    // Scale Item while inside zone.                                            
//                    StoredItem.transform.localScale = Vector3.Lerp(StoredItem.transform.localScale, StoredItem.OriginalScale * _scaleTo, Time.deltaTime * 30f);
                    
//                    // Make sure this can't be grabbed from the snap zone
//                    if(StoredItem.layer == 21 || (disabledColliders != null && disabledColliders.Count > 0 && disabledColliders[0] != null && disabledColliders[0].enabled)) {
//                        disableGameObject(StoredItem);
//                    }
//                }
//            }

//            // Can't drop item. Lerp to position if not being held
//            if (!CanDropItem && trackedItem != null && StoredItem == null) {
//                if (!trackedItem.BeingHeld) {
//                    Store_Obj(trackedItem);
//                }
//            }
//        }

//        GameObject getClosestGameObject() {

//            GameObject closest = null;
//            float lastDistance = 9999f;

//            if (gZone == null || gZone.NearbyGameObject == null) {
//                return null;
//            }

//            foreach (var g in gZone.NearbyGameObject) {

//                // Collider may have been disabled
//                if(g.Key == null) {
//                    continue;
//                }

//                float dist = Vector3.Distance(transform.position, g.Value.transform.position);
//                if(dist < lastDistance) {

//                    //  Not allowing secondary grabbables such as slides
//                    if(g.Value.OtherGrabbableMustBeGrabbed != null) {
//                        continue;
//                    }

//                    // Don't allow SnapZones in SnapZones
//                    if(g.Value.GetComponent<SnapZone>() != null) {
//                        continue;
//                    }

//                    // Don't allow InvalidSnapObjects to snap
//                    if (g.Value.CanBeSnappedToSnapZone == false) {
//                        continue;
//                    }

//                    // Must contain transform name
//                    if (OnlyAllowNames != null && OnlyAllowNames.Count > 0) {
//                        string transformName = g.Value.transform.name;
//                        bool matchFound = false;
//                        for (int x = 0; x < OnlyAllowNames.Count; x++) {
//                            string name = OnlyAllowNames[x];
//                            if (transformName.Contains(name)) {
//                                matchFound = true;                                
//                            }
//                        }

//                        // Not a valid match
//                        if(!matchFound) {
//                            continue;
//                        }
//                    }

//                    // Check for name exclusion
//                    if (ExcludeTransformNames != null) {
//                        string transformName = g.Value.transform.name;
//                        bool matchFound = false;
//                        for (int x = 0; x < ExcludeTransformNames.Count; x++) {
//                            // Not a valid match
//                            if (transformName.Contains(ExcludeTransformNames[x])) {
//                                matchFound = true;
//                            }
//                        }
//                        // Exclude this
//                        if (matchFound) {
//                            continue;
//                        }
//                    }

                    

//                    // Only valid to snap if being held or recently dropped
//                    if (g.Value.BeingHeld || (Time.time - g.Value.LastDropTime < MaxDropTime)) {
//                        closest = g.Value;
//                        lastDistance = dist;
//                    }
//                }
//            }

//            return closest;
//        }

//        public void Store_Obj(GameObject store) {

//            // Grab is already in Snap Zone
//            if(store.transform.parent != null && store.transform.parent.GetComponent<SnapZone>() != null) {
//                return;
//            }

//            if(StoredItem != null) {
//                ReleaseAll();
//            }

//            StoredItem = store;
//            heldItemRigid = StoredItem.GetComponent<Rigidbody>();

//            // Mark as kinematic so it doesn't fall down
//            if(heldItemRigid) {
//                heldItemWasKinematic = heldItemRigid.isKinematic;
//                heldItemRigid.isKinematic = true;
//            }
//            else {
//                heldItemWasKinematic = false;
//            }

//            // Set the parent of the object 
//            store.transform.parent = transform;

//            // Set scale factor            
//            // Use SnapZoneScale if specified
//            if (store.GetComponent<SnapZoneScale>()) {
//                _scaleTo = store.GetComponent<SnapZoneScale>().Scale;
//            }
//            else {
//                _scaleTo = ScaleItem;
//            }

//            // Is there an offset to apply?
//            SnapZoneOffset off = store.GetComponent<SnapZoneOffset>();
//            if(off) {
//                offset = off;
//            }
//            else {
//                offset = store.gameObject.AddComponent<SnapZoneOffset>();
//                offset.LocalPositionOffset = Vector3.zero;
//                offset.LocalRotationOffset = Vector3.zero;
//            }

//            // Lock into place
//            if (offset) {
//                StoredItem.transform.localPosition = offset.LocalPositionOffset;
//                StoredItem.transform.localEulerAngles = offset.LocalRotationOffset;
//            }
//            else {
//                StoredItem.transform.localPosition = Vector3.zero;
//                StoredItem.transform.localEulerAngles = Vector3.zero;
//            }

//            // Disable the grabbable. This is picked up through a Grab Action
//            disableGameObject(store);

//            // Call Grabbable Event from SnapZone
//            if (OnSnap_G_Event != null) {
//                OnSnap_G_Event.Invoke(store);
//            }

//            // Fire Off Events on Grabbable
//            GrabbableEvents[] ge = store.GetComponents<GrabbableEvents>();
//            if (ge != null) {
//                for (int x = 0; x < ge.Length; x++) {
//                    ge[x].OnSnapZoneEnter();
//                }
//            }

//            if (SoundOnSnap) {
//                // Only play the sound if not just starting the scene
//                if(Time.timeSinceLevelLoad > 0.1f) {
//                    VRUtils.Instance.PlaySpatialClipAt(SoundOnSnap, transform.position, 0.75f);
//                }

//                Debug.Log(Time.timeSinceLevelLoad);
//            }

//            LastSnapTime = Time.time;
//        }

//        void disableGameObject(GameObject store) {

//            if (DisableColliders) {
//                disabledColliders = store.GetComponentsInChildren<Collider>(false).ToList();
//                for (int x = 0; x < disabledColliders.Count; x++) {
//                    disabledColliders[x].enabled = false;
//                }
//            }

//            // Disable the grabbable. This is picked up through a Grab Action
//            store.layer = 22; 
//        }

//        /// <summary>
//        /// This is typically called by the GrabAction on the SnapZone
//        /// </summary>
//        /// <param name="grabber"></param>
//        public void GrabEquipped(Grabber grabber) {

//            if (grabber != null) {
//                if(StoredItem) {

//                    // Not allowed to be removed
//                    if(!CanBeRemoved()) {
//                        return;
//                    }

//                    var g = StoredItem;
//                    if(DuplicateItemOnGrab) {

//                        ReleaseAll();

//                        // Position next to grabber if somewhat far away
//                        if (Vector3.Distance(g.transform.position, grabber.transform.position) > 0.2f) {
//                            g.transform.position = grabber.transform.position;
//                        }

//                        // Instantiate the object before it is grabbed
//                        GameObject go = Instantiate(g.gameObject, transform.position, Quaternion.identity) as GameObject;
//                        Grabbable grab = go.GetComponent<Grabbable>();

//                        // Ok to attach it to snap zone now
//                        this.Store_Obj(grab);

//                        // Finish Grabbing the desired object
//                        grabber.GrabGrabbable(g);
//                    }
//                    else {
//                        ReleaseAll();

//                        // Position next to grabber if somewhat far away
//                        if (Vector3.Distance(g.transform.position, grabber.transform.position) > 0.2f) {
//                            g.transform.position = grabber.transform.position;
//                        }

//                        // Do grab
//                        grabber.GrabGrabbable(g);
//                    }
//                }
//            }
//        }

//        public virtual bool CanBeRemoved() {
//            // Not allowed to be removed
//            if (!CanRemoveItem) {
//                return false;
//            }

//            // Not a valid grab if we just snapped this item in an it's a toggle type
//            if (StoredItem.Grabtype == HoldType.Toggle && (Time.time - LastSnapTime < 0.1f)) {
//                return false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// Release  everything snapped to us
//        /// </summary>
//        public void ReleaseAll() {

//            // No need to keep checking
//            if (StoredItem == null) {
//                return;
//            }

//            // Still need to keep track of item if we can't fully drop it
//            if (!CanDropItem && StoredItem != null) {
//                trackedItem = StoredItem;
//            }

//            StoredItem.ResetScale();

//            if (DisableColliders && disabledColliders != null) {
//                foreach (var c in disabledColliders) {
//                    if(c) {
//                        c.enabled = true;
//                    }
//                }
//            }
//            disabledColliders = null;

//            // Reset Kinematic status
//            if(heldItemRigid) {
//                heldItemRigid.isKinematic = heldItemWasKinematic;
//            }

//            StoredItem.enabled = true;
//            StoredItem.transform.parent = null;

//            // Play Unsnap sound
//            if(StoredItem != null) {
//                if (SoundOnSnap) {
//                    VRUtils.Instance.PlaySpatialClipAt(SoundOnUnsnap, transform.position, 0.75f);
//                }

//                // Call event
//                if (OnDetachEvent != null) {
//                    OnDetachEvent.Invoke(StoredItem);
//                }

//                // Fire Off Grabbable Events
//                GrabbableEvents[] ge = StoredItem.GetComponents<GrabbableEvents>();
//                if (ge != null) {
//                    for (int x = 0; x < ge.Length; x++) {
//                        ge[x].OnSnapZoneExit();
//                    }
//                }
//            }

//            StoredItem = null;
//        }
//    }
//}
