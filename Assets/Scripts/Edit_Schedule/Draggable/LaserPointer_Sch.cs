using System.Collections;
using EPOOutline;
using UnityEngine;

namespace BNG
{
    /// <summary>
    /// A simple laser pointer that draws a line with a dot at the end
    /// </summary>
    public class LaserPointer_Sch : MonoBehaviour
    {
        public float MaxRange = 2.5f;
        public LayerMask ValidLayers;
        public LineRenderer line1;
        public LineRenderer line2;
        public GameObject cursor;
        public GameObject _cursor;
        public GameObject LaserEnd;
        public GameObject rightGrabber;
        public GameObject hitObject;
        public GameObject selectObject;
        public float t;

        public bool Active = true;
        
        public Grabber grabber;
        
        [SerializeField] private bool switchOn;
        private int lineEndPosition;

        private Vector3 vec2Pos;
        private Vector3 zPos;

        /// <summary>
        /// 0.5 = Line Goes Half Way. 1 = Line reaches end.
        /// </summary>
        [UnityEngine.Tooltip("Example : 0.5 = Line Goes Half Way. 1 = Line reaches end.")]
        public float LineDistanceModifier = 0.8f;
  
        void MoveCard(GameObject selected)
        {
            // LaserEnd의 포지션을 따라간다
            Vector2 a = selected.transform.position;
            Vector2 b = LaserEnd.transform.position;
            vec2Pos = Vector2.Lerp(a, b, t);
            vec2Pos.z = zPos.z;
            selected.transform.position = vec2Pos;
            Debug.Log("연속 로그찍는 상태면 옮기려고 계속 시도중");
        }


        IEnumerator TriggeredPlan(GameObject hitObj)
        {
            yield return new WaitForSeconds(0.1f);

            if (InputBridge.Instance.RightTriggerDown == true)
            {
                Debug.Log("i am here!");
                selectObject = hitObj;
                // switchOn이 true 일때  fixed Update 함수에서 MoveCard 함수 발동
                switchOn = true;

                yield return selectObject;
            }

            else yield return null;
        }

        //private IEnumerator TriggeredNece(GameObject hitObj)
        //{
        //    yield return new WaitForSeconds(0.1f);

        //    if (InputBridge.Instance.RightTriggerDown == true && switchOn == false)
        //    {
        //        switchOn = true;

        //        if (hitobject.getcomponent<outlinable>().enabled != true)
        //        {
        //            hitobject.getcomponent<outlinable>().enabled = true;

        //            sendevent("neceenter");
        //            fsmg_obj.value = hitobject;
        //        }
        //    }
        //}

        //private IEnumerator TriggeredUnnece(GameObject hitObj)
        //{
        //    yield return new WaitForSeconds(0.1f);

        //    Debug.Log(hitObj);

        //    if (InputBridge.Instance.RightTriggerDown == true && switchOn == false)
        //    {
        //        switchOn = true;                

        //        yield return new WaitForSeconds(0.1f);
        //        if (grabber.HeldGrabbable == null)
        //        {
        //            //SendEvent("UnneceEnter");
        //            //fsmG_Obj.Value = hitObj;
        //        }
        //    }
        //}

        private void Awake()
        {                       

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

        private void Start()
        {
            zPos.z = 1.5f;
            switchOn = false;
            grabber = rightGrabber.GetComponent<Grabber>();
        }

        private void Update()
        {            
            if (InputBridge.Instance.RightTriggerDown == false)
            {
                switchOn = false;
                selectObject = null;                
            }
        }

        private void FixedUpdate()
        {
            if(switchOn && InputBridge.Instance.RightTriggerDown == true)
            {
                MoveCard(selectObject);
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

                    if(hit.transform.gameObject != null)
                    {
                        if (hit.transform.gameObject.layer == 21)
                        {
                            if (hit.transform.gameObject.tag == "PLAN" && InputBridge.Instance.RightTriggerDown == false)
                            {
                                hitObject = hit.collider.gameObject;

                                // pointer가 Plan Card 위에서 select하길 대기 완료 
                                StartCoroutine(TriggeredPlan(hitObject));
                            }

                            else if (hit.transform.gameObject.tag == "Unnecessary" && InputBridge.Instance.RightTriggerDown == false)
                            {
                                hitObject = hit.collider.gameObject;

                                //StartCoroutine(TriggeredUnnece(hitObject));
                            }
                        }

                        else if (hit.transform.gameObject.layer == 10)
                        {

                        }
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
                            Debug.Log("hit coll = " + hit.collider.name);                            
                            //hitObject = hit.collider.gameObject;                            
                        }

                        else if (hit.distance <= 0)
                        {
                            line1.enabled = false;
                            line2.enabled = true;
                            line2.SetPosition(0, Vector3.zero);
                            line2.SetPosition(1, new Vector3(0, 0, MaxRange));
                            LaserEnd.transform.position = new Vector3(0, 0, MaxRange);
                            LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);                                                        
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