using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG_Cus_AddOn
{
    #region Enums
    public enum RemoteController
    {
        Left,
        Right,
        None
    }

    public enum HoldType
    {
        HoldDown, // Hold down the grab button
        Toggle,   // Click the grab button down to switch between hold and release
        Inherit   // Inherit from Grabber
    }

    #endregion

    public class StoreRemote : MonoBehaviour
    {
        #region Singleton
        /// <summary>
        /// Instance of our Singleton
        /// </summary>
        public static StoreRemote Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<StoreRemote>();
                    if (_instance == null)
                    {
                        _instance = new GameObject("StoreRemote").AddComponent<StoreRemote>();
                    }
                }
                return _instance;
            }
        }
        private static StoreRemote _instance;
        #endregion

        private void Awake()
        {
            // Destroy any duplicate instances that may have been created
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
        }
    }
}


