using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BlinkCountDown : MonoBehaviour
{
    /*
     제한시간이 1분씩 지날때마다
    깜빡거리는 효과를 위해 추가
    FSM으로 만드는 것보다 스크립트로 하는게 쉬워서 스크립트로 짬
     */
    public TextMeshProUGUI text;

    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        
    }

    // Update is called once per frame
    IEnumerator Blink()
    {
        while(true)
        {
            text.enabled = false;
            yield return new WaitForSeconds(.5f);
            text.enabled = true;
            yield return new WaitForSeconds(.5f);
            text.enabled = false;
            yield return new WaitForSeconds(.5f);
            text.enabled = true;
            yield return new WaitForSeconds(.5f);
            text.enabled = false;
            yield return new WaitForSeconds(.5f);
            text.enabled = true;
            yield return new WaitForSeconds(.5f);
        }



    }

    public void StartBlinking()
    { 
        StartCoroutine(Blink());
    }
      
}
