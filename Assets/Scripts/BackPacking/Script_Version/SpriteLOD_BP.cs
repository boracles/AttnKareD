using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteLOD_BP : MonoBehaviour
{
    //Change alpha value of sprite as user gets close
    //Create LOD effect
    public Image Image;
 
    Color m_cImage;
    float m_falpha; // alpha value chages as distance decrease
    float m_fdistance; //distance between object and user
    float m_fFarAlpha; //initial alpha value

    public float minRange;
    public float maxRange;
    // Start is called before the first frame update
    void Start()
    {
        m_cImage = Image.color;
        m_fFarAlpha = m_cImage.a;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "HeadCollision")
        {
            m_fdistance = Vector3.Distance(this.transform.position, other.transform.position);
            float lerpAmt = 1.0f - Mathf.Clamp01((m_fdistance - minRange) / (maxRange - minRange));
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, lerpAmt);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "HeadCollision")
        {
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, m_fFarAlpha);
        }
    }
}
