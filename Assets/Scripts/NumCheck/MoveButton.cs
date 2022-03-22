using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using BNG;

public class MoveButton : MonoBehaviour, IPointerDownHandler //,IPointerUpHandler
{
    public InputBridge XrRig;
    public UIPointer RighthandPointer;
    public GameObject Trigger;
    public bool triggered;
    public bool click;
    public string btnNum;
    public string color = "#FFFFFF";
    public bool distraction = false;
    public bool bColor;
    Transform Canvas;
    Transform originalParent;
    RectTransform rect;
    Vector3 OrginPos;
    TextMeshProUGUI btnText;
    Color activatedColor = Color.white;
    public Color originalColor;
    Color colorRed = Color.red;
    Color colorYellow = Color.yellow;
    Transform parentCursor;
    // Start is called before the first frame update
    public bool bStage = false;
    Guide_NumCheck Guide;
    public Guide_NumCheck.INDEX m_eIndex;
    int a;
    void Awake()
    {
        Guide = GameObject.Find("Guide").GetComponent<Guide_NumCheck>();
        ColorUtility.TryParseHtmlString("#FF0900", out colorRed);
        ColorUtility.TryParseHtmlString("#FF9B00", out colorYellow);
        Debug.Log(Guide.name);
    }
    void Start()
    {

        parentCursor = RighthandPointer.GetComponent<UIPointer>()._cursor.transform;
        OrginPos = transform.position;
        originalParent = this.transform.parent;
        Canvas = this.transform.parent.parent;
        Debug.Log(Canvas.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(click)
        {
            if(RighthandPointer.GetComponent<LineRenderer>().enabled == true)
            {
                if(XrRig.RightTrigger > 0.5f)
                    transform.position = new Vector3(parentCursor.position.x, parentCursor.position.y, transform.position.z);
                if (XrRig.RightTrigger < 0.2f) {
                    this.transform.SetParent(originalParent);
                    if (Trigger) { if(!bStage)Guide.NumInTrigger(this, Trigger); if (bStage) Guide.NumStage2(this, Trigger); }
                    if (!Trigger) { ResetButton(); Guide.CanGrab(); }
                    click = false;
                }
            }
            if(RighthandPointer.GetComponent<LineRenderer>().enabled == false)
            {
                this.transform.SetParent(originalParent);
                ResetButton();
                Guide.CanGrab();
                click = false;
            }
            
        }
    }

    public void SetBtnNum(bool color){
        transform.name = btnNum;
        btnText = transform.GetComponentInChildren<TextMeshProUGUI>();
        btnText.text = btnNum;
        if (color) btnText.color = colorRed;
        if (!color) btnText.color = colorYellow;
        originalColor = btnText.color;
    }
    public void SetBtnStage2(bool color) //솔직히 위에꺼랑 같아서 삭제 가능
    {
        bColor = color;
        transform.name = btnNum + bColor;
        btnText = transform.GetComponentInChildren<TextMeshProUGUI>();
        btnText.text = btnNum;
        if (color) btnText.color = colorRed;
        if (!color) btnText.color = colorYellow;
        originalColor = btnText.color;
    }

    public void OnPointerDown(PointerEventData pointerEventData){

        this.transform.SetParent(Canvas);
        if(distraction)
        {
            Guide.GetComponent<CheckData_NumCheck>().distractedBy += Time.deltaTime;
        }
        click = true;
        btnText.color = activatedColor;
        Guide.CannotGrab(transform.GetComponent<MoveButton>());
    }
  
    public void ResetButton(){
        Trigger = null;
        transform.position = OrginPos;
        btnText.color = originalColor;

    }

    public void SetButton(){ 
        transform.position = Trigger.transform.position;
        btnText.color = originalColor;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Necessary")
        { 
            Guide.active = true;
            Trigger = collision.gameObject;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Necessary")
        {
            Trigger = null;
        }
    }

}
