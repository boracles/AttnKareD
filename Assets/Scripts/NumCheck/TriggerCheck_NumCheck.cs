using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TriggerCheck_NumCheck : MonoBehaviour
{
    [Header("Manager")]
    public NumCheckManager Manager;
    public DataCheck_NumMatch DataCheck;
    public string orderNum;


    NumCard card;
    
    int arrNum;
    // Start is called before the first frame update
    void Start()
    {
        arrNum = int.Parse(orderNum) - 1;
    }


    public void TriggerIn()
    {
        card = transform.GetComponentInChildren<NumCard>();
        if(card)
        {
            Manager.arrOrder[arrNum] = card.cardNum;
            Manager.answerInt += 1;
            if (orderNum != card.cardNum)
            {
                DataCheck.WrongOrder += 1;
            }

            Manager.GetComponent<NumCheckManager>().CompareArr();

        }    
       
    }

    public void TriggerOut()
    {
        Manager.arrOrder[arrNum] = null;
        Manager.answerInt -= 1;
    }
}
