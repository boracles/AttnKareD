using Tobii.G2OM;
using UnityEngine;

public class HighlightAtGaze_BP : MonoBehaviour, IGazeFocusable
{
    // Start is called before the first frame update
    public Color HighlightColor = Color.red;
    public float AnimationTime = 0.1f;

    public bool watchCheck;
    public Object_BP.GAZE_BP MyName;
    GazedTime_BP GazedObjects;
    private Renderer _renderer;
    private Color _originalColor;
    private Color _targetColor;
    //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus
    public void GazeFocusChanged(bool hasFocus)
    {
        watchCheck = hasFocus; //튜토리얼 중 시간표를 한번 확인하면 particle system이 나오게 하기 위해

        //If this object received focus, fade the object's color to highlight color
        if (hasFocus)
        {
            GazedObjects.goGazed = this.gameObject;
            GazedObjects.GazedObject = MyName;
        }
        //If this object lost focus, fade the object's color to it's original color
        else
        {
            GazedObjects.GazedObject = Object_BP.GAZE_BP.NOTWATCHING;
        }
    }

    private void Start()
    {
        GazedObjects = GameObject.Find("HighlightAtGaze").GetComponent<GazedTime_BP>();
    }

   
}

