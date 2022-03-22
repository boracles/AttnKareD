using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

namespace BNG
{
    /// <summary>
    /// A simple laser pointer that draws a line with a dot at the end
    /// </summary>
    public class LaserPointer_V2 : MonoBehaviour
    {

        public float MaxRange = 2.5f;
        public LayerMask ValidLayers;
        public LineRenderer line;
        public GameObject cursor;
        public GameObject _cursor;
        public GameObject LaserEnd;

        public bool Active = true;

        private int lineEndPosition;
        private bool isIn;

        /// <summary>
        /// 0.5 = Line Goes Half Way. 1 = Line reaches end.
        /// </summary>
        [UnityEngine.Tooltip("Example : 0.5 = Line Goes Half Way. 1 = Line reaches end.")]
        public float LineDistanceModifier = 0.8f;

        public PlayMakerFSM MyFsm;
        void Awake()
        {
            if (cursor)
            {
                _cursor = GameObject.Instantiate(cursor);
            }

            // If no Line Renderer was specified in the editor, check this Transform
            if (line == null)
            {
                line = GetComponent<LineRenderer>();
            }

            // Line Renderer is positioned using world space
            if (line != null)
            {
                line.useWorldSpace = true;
            }
        }
        private void Start()
        {
            isIn = false;
        }
        void LateUpdate()
        {
            if (Active)
            {
                line.enabled = true;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, MaxRange, ValidLayers, QueryTriggerInteraction.Ignore))
                {
                    // Add dot at line's end                          
                    LaserEnd.transform.position = hit.point;
                    LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);                    
                    if (!isIn)
                    {
                        OnEnterEvent("On Pointer");
                    }                    
                }

                // Set position of the cursor
                if (_cursor != null)
                {
                    //_cursor.transform.position = hit.point;
                    //_cursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    //_cursor.SetActive(hit.distance > 0);

                    if (line)
                    {
                        line.useWorldSpace = false;
                        line.SetPosition(0, Vector3.zero);
                        line.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) * LineDistanceModifier));
                        line.enabled = hit.distance > 0;
                        if (!line.enabled)
                        {
                            LaserEnd.transform.position = new Vector3(0, 1000, 0);
                            ExitEvent("Exit Pointer");
                        }
                    }

                    else
                    {
                        line.useWorldSpace = false;
                        line.SetPosition(0, Vector3.zero);
                        line.SetPosition(1, new Vector3(0, 0, MaxRange));
                        line.enabled = hit.distance > 0;
                        LaserEnd.transform.position = new Vector3(0, 0, MaxRange);
                        LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    }
                }
            }
            else
            {
                LaserEnd.gameObject.SetActive(false);

                if (line)
                {
                    line.enabled = false;
                }
            }
        }

        private void OnEnterEvent(string currentEvent)
        {
            isIn = true;
            MyFsm.SendEvent(currentEvent);
        }

        private void ExitEvent(string currentEvent)
        {
            isIn = false;
            MyFsm.SendEvent(currentEvent);
        }
    }
}