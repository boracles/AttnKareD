using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumCard : MonoBehaviour
{
    public string cardNum;
    public TextMeshProUGUI cardText;
    Vector3 initPosition;
    Vector3 initRotation;
 
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        initRotation = transform.localEulerAngles;

    }

    public void SetCardNum()
    {
        cardText = transform.GetComponentInChildren<TextMeshProUGUI>();
        cardText.text = cardNum;
    }

    public void SetPosRot(Vector3 posTrigger)//Transform posTrigger
    {
        transform.position = posTrigger;
        transform.localEulerAngles = new Vector3(90, 180, 0);
    }

    public void ResetPosRot()
    {
        transform.position = initPosition;
        transform.localEulerAngles = initRotation;
    }

    






}
