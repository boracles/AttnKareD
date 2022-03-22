using EPOOutline;
using HutongGames.PlayMaker;
using UnityEngine;
using System.Collections;

namespace BNG
{
    /// <summary>
    /// A simple laser pointer that draws a line with a dot at the end
    /// </summary>
    public class LaserPointer_Tuto : MonoBehaviour
    {
        public float MaxRange = 2.5f;
        public LayerMask ValidLayers;
        public LineRenderer line1;
        public LineRenderer line2;
        public GameObject cursor;
        public GameObject _cursor;
        public GameObject LaserEnd;
        public GameObject hitObject;
 


        public bool Active = true;
        public bool Set = true;
    //    public PlayMakerFSM GoFsm;
        public Grabber graber;

        private int lineEndPosition;
        private FsmObject fsmObject;
        private FsmBool fsmBool;
        private FsmGameObject fsmG_Obj;

        /// <summary>
        /// 0.5 = Line Goes Half Way. 1 = Line reaches end.
        /// </summary>
        [UnityEngine.Tooltip("Example : 0.5 = Line Goes Half Way. 1 = Line reaches end.")]
        public float LineDistanceModifier = 0.8f;       



        private void Awake()
        {
          //  GoFsm.gameObject.GetComponent<PlayMakerFSM>();

            if (cursor)
            {
                _cursor = GameObject.Instantiate(cursor);
            }

            // If no Line Renderer was specified in the editor, check this Transform
            if (line1 == null)
            {
                line1 = GetComponent<LineRenderer>();
            }
            if (line2 == null)
            {
                line2 = GetComponent<LineRenderer>();
            }

            // Line Renderer is positioned using world space
            if (line1 != null)
            {
                line1.useWorldSpace = true;
            }
        }

        private void LateUpdate()
        {
            if (Active)
            {
                line1.enabled = true;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, MaxRange, ValidLayers, QueryTriggerInteraction.Ignore))
                {
                    // Add dot at line's end
                    LaserEnd.transform.position = hit.point;
                    LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                    if (hit.transform.gameObject.tag == "Necessary" || hit.transform.gameObject.tag == "Necessary_Book" || hit.transform.gameObject.tag == "Necessary_Pencil")
                    {                     
  /*                Doesn't Need Anymore(Outlinable parameters가 pointer가 들어갔다 나갈 때 마다 변경되어야 하기 때문)   
                    hitObject = hit.collider.gameObject;

                        if (hitObject.GetComponent<Outlinable>().enabled != true)w
                        {
                            hitObject.GetComponent<Outlinable>().enabled = true;

                            SendEvent("Enter");
                            fsmG_Obj.Value = hitObject;
                        }
                    }
*/
                    }
                }

                // Set position of the cursor
                if (_cursor != null)
                {
                    if (line1)
                    {
                        if (hit.distance > 0)
                        {
                            line1.enabled = true;
                            line2.enabled = false;
                            line1.useWorldSpace = false;
                            line1.SetPosition(0, Vector3.zero);
                            line1.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) * LineDistanceModifier));                            
                            //Debug.Log("hit coll = " + hit.collider.name);
                            //----------------***추가된 부분**----------------------- 
                            if(Set)
                            {
                                hitObject = hit.collider.gameObject;
                                Color outline = new Color(1f,0.8f, 0, 0.6f); //pointer hit 되면 노란 outline으로 변경
                                hitObject.GetComponent<Outlinable>().FrontParameters.Color = outline;
                                Set=false;
                                





                            }
                            
                            

                        }

                        if (hit.distance <= 0)
                        {
                            line1.enabled = false;
                            line2.enabled = true;
                            line2.SetPosition(0, Vector3.zero);
                            line2.SetPosition(1, new Vector3(0, 0, MaxRange));
                            LaserEnd.transform.position = new Vector3(0, 0, MaxRange);
                            LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);                            
                            //Debug.Log("no hit");   
                            //----------------***추가된 부분**----------------------- 
                            if(!Set)
                            {
                                Color outline = new Color(1f,1f, 1f, 0.6f); //pointer 가 나가면 원래 outline 색으로 돌아옴
                                hitObject.GetComponent<Outlinable>().FrontParameters.Color = outline;
                                Set=true;
                                




                            }                        
                        }

                    }
                    else
                    {
                        line1.useWorldSpace = false;
                        line1.SetPosition(0, Vector3.zero);
                        line1.SetPosition(1, new Vector3(0, 0, MaxRange));
                        line1.enabled = hit.distance > 0;
                        LaserEnd.transform.position = new Vector3(0, 0, MaxRange);
                        LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    }
                }
            }
            else
            {
                LaserEnd.gameObject.SetActive(false);

                if (line1)
                {
                    line1.enabled = false;
                }
                else if (line2)
                {
                    line2.enabled = false;
                }
            }
        }

         
        
    }
}