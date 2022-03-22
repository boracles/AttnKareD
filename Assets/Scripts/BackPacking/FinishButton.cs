using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using KetosGames.SceneTransition;
using TMPro;

public class FinishButton : MonoBehaviour
{
    private bool bFin = false;
    private bool bActive = false;
    public CanvasGroup FinishCanvas;
    private Coroutine coroutine = null;
    Transform Fin1;
    Transform Fin2;
    public UnityEvent NextEvent;

    // Start is called before the first frame update
    void Start()
    {
        Fin1 = FinishCanvas.transform.GetChild(0);
        Fin2 = FinishCanvas.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        bActive = false;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.25f);
        foreach(Collider hit in hitColliders)
        {
            if(hit.name == "RaycastCollider") bActive = true;
        }
    }

    public void OnButtonDown()
    {
        if (coroutine != null)StopCoroutine(coroutine);
        coroutine = bFin ? bActive ? StartCoroutine(NextScene()) : null : StartCoroutine(PressedFirst());
    }

    IEnumerator PressedFirst()
    {
        float lerpTime = 0f;
        while (FinishCanvas.alpha < 1.0f) //fade in
        {
            lerpTime += Time.deltaTime;
            FinishCanvas.alpha = Mathf.Lerp(0, 1, lerpTime / .8f);
        }
        yield return new WaitForSeconds(0.8f);
        bFin = true;
        yield return new WaitForSeconds(2.5f);
        lerpTime = 0f;
        while (FinishCanvas.alpha > 0f) //fade out
        {
            lerpTime += Time.deltaTime;
            FinishCanvas.alpha = Mathf.Lerp(1, 0, lerpTime / 1.2f);
        }
        yield return new WaitForSeconds(7.0f);
        bFin = false; //if not pressed for 7 seconds turn off 
    }


    IEnumerator NextScene()
    {
        FinishCanvas.alpha = 1f;
        Fin1.gameObject.SetActive(false);
        Fin2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1.0f);

        if(NextEvent != null)
        {
            NextEvent.Invoke();
        }
    }
  



}
