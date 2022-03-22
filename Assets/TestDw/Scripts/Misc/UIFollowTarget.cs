using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    public Transform targetTrans;
    RectTransform rect;
    Canvas canvas;

    public Vector3 offset;

    void Start()
    {
        canvas = transform.parent.GetComponent<Canvas>();
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetTrans.position + offset);
        float x = screenPos.x - (Screen.width / 2);
        float y = screenPos.y - (Screen.height / 2);
        float s = canvas.scaleFactor;
        rect.anchoredPosition = new Vector2(x, y) / s;
    }
}
