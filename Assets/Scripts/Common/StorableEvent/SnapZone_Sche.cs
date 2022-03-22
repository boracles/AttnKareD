using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

namespace BNG_Cus_AddOn
{
    public class SnapZone_Sche : MonoBehaviour
    {
        [Header("Starting / Held Item")]
        public GameObject HeldItem;

        [Tooltip("If false, Item will Move back to inventory space if player drops it.")]
        public bool CanDropItem = true;

        public bool DisableColliders = true;
        List<Collider> disabledColliders = new List<Collider>();

        [Header("Audio")]
        public AudioClip SoundOnSnap;
        public AudioClip SoundOnUnsnap;

        SnapZoneOffset offset;

        GameObject trackedItem; // If we can't drop the item, track it separately

        [Header("Events")]
        /// <summary>
        /// Optional Unity Event  to be called when something is snapped to this SnapZone. Passes in the Grabbable that was attached.
        /// </summary>
        public StorableEvent OnSnap_S_Event;

        /// <summary>
        /// Optional Unity Event to be called when something has been detached from this SnapZone. Passes in the Grabbable is being detattached.
        /// </summary>
        public StorableEvent OnDetach_S_Event;

        public void ReleaseAll()
        {
            // No need to keep checking
            if (HeldItem == null)
            {
                return;
            }

            // Still need to keep track of item if we can't fully drop it
            if (!CanDropItem && HeldItem != null)
            {
                trackedItem = HeldItem;
            }

            if (DisableColliders && disabledColliders != null)
            {
                foreach (var c in disabledColliders)
                {
                    if (c)
                    {
                        c.enabled = true;
                    }
                }
            }
            disabledColliders = null;

            // Reset Kinematic status       

            //HeldItem.enabled = true;
            HeldItem.transform.parent = null;

            // Play Unsnap sound
            if (HeldItem != null)
            {
                if (SoundOnSnap)
                {
                    VRUtils.Instance.PlaySpatialClipAt(SoundOnUnsnap, transform.position, 0.75f);
                }

                // Call event
                if (OnDetach_S_Event != null)
                {
                    OnDetach_S_Event.Invoke(HeldItem);
                }

                // Fire Off Stoable Events
                StorableEvents[] ge = HeldItem.GetComponents<StorableEvents>();
                if (ge != null)
                {
                    for (int x = 0; x < ge.Length; x++)
                    {
                        ge[x].OnSnapZone_S_Exit();
                    }
                }
            }

            HeldItem = null;
        }
    }
}

