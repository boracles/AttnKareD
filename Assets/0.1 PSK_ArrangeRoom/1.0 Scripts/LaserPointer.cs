using System.Collections;
using EPOOutline;
using UnityEngine;
using BNG;
namespace Arrange {
// A simple laser pointer that draws a line with a dot at the end
public class LaserPointer : MonoBehaviour
{
    /**************************************************************************
    // Global Constant / Parameter Definition
    ***************************************************************************/
    public const int    LAYER_GRABBABLE     = 10;    //
    public float        MAX_RANGE           = 2.5f;  //Grabbable Disstance. This value is preceded by Editor setting

    /**************************************************************************
    // Database structure Definition
    ***************************************************************************/

    /**************************************************************************
    // Editor Assigned Resources
    ***************************************************************************/
    public LayerMask    m_ValidLayers;
    public LineRenderer m_rendLine1;
    public LineRenderer m_rendLine2;
    public GameObject   m_goCursor;    
    public GameObject   m_goLaserEnd;        
    public Grabber      m_RightGrabber;

    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/
    bool                m_bActive;    //LaserPonter Active ?    
    bool                m_bPrevRTriggerDown;  //이전 Frame에서 RightTriggerDown ?            
    // 0.5 = Line Goes Half Way. 1 = Line reaches end.    
    [UnityEngine.Tooltip("Example : 0.5 = Line Goes Half Way. 1 = Line reaches end.")]
    public float        LineDistanceModifier = 0.8f;

    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //LaserPonter를 보여주거나 감춤니다, for later called by superior object
    public void Enable(bool bOn) {
        m_bActive           = bOn;
        m_goLaserEnd.SetActive(bOn);
        m_rendLine1.enabled = bOn;            
        m_rendLine2.enabled = bOn;  
    }

    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    Guide   m_Guide;
    HUD     m_Hud;

    void Awake() {           
       // if (m_goCursor)  _cursor = Instantiate(m_goCursor);                        
        m_rendLine1.useWorldSpace = true;         // Line Renderer is positioned using world space
    }

    void Start() {                
        m_bActive = true;  // for Test
        m_Guide     = GameObject.Find("Guide").GetComponent<Guide>();
        m_Hud       = GameObject.Find("HUD").GetComponent<HUD>();
    }

    //void Update() {     }
    
    void CheckGrabbable(GameObject go){
        bool bChecked = go.GetComponent<Outlinable>().enabled;        
        if(go.tag == Manager.saTag[(int)Manager.TAG.NECESSARY]) {
            if(bChecked) return;
            go.GetComponent<Outlinable>().enabled = true;            
            //Manager.CLEAN clean = Manager.GetCleanByName(go.name);
            //Manager.CDB[(int)clean].bFound = true; //SDB갱신                       
            //m_Hud.NoteUpdateFind(Manager.saCleanK[(int)clean]);  //찾은 물건 문자열 갱신            
            //m_Guide.SetFound(go);   //찾은것을 Guide에게 알려줌
            // Debug.Log(go.name+" Necessay Triggerd");
        }else { //unnecssay
            if (m_RightGrabber.HeldGrabbable == null){
            // Debug.Log(go.name+" UNecessay Triggerd");
            }
        }
    }

    void LateUpdate() {
        if (!m_bActive) return;
        m_rendLine1.enabled = true;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, MAX_RANGE, m_ValidLayers, QueryTriggerInteraction.Ignore))  {
            //Draw Render Line
            m_rendLine1.enabled = true;
            m_rendLine2.enabled = false;
            m_rendLine1.useWorldSpace = false;
            m_rendLine1.SetPosition(0, Vector3.zero); //시작
            m_rendLine1.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) * LineDistanceModifier)); //끝             

            // Add dot at line's end
            m_goLaserEnd.transform.position = hit.point;
            m_goLaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

            //Check Grabble Object
            if(hit.transform.gameObject.layer == LAYER_GRABBABLE && InputBridge.Instance.RightTriggerDown  &&  !m_bPrevRTriggerDown  ) 
               CheckGrabbable(hit.collider.gameObject);                                                                              
        } else { // not hit
            m_rendLine1.enabled = false;
            m_rendLine2.enabled = true;
            m_rendLine2.SetPosition(0, Vector3.zero); //시작
            m_rendLine2.SetPosition(1, new Vector3(0, 0, MAX_RANGE)); //끝
            m_goLaserEnd.transform.position = new Vector3(0, 0, MAX_RANGE);
            m_goLaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);                                                        
        }
        m_bPrevRTriggerDown = InputBridge.Instance.RightTriggerDown;
    } 
   } 
}
