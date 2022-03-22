using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpController : MonoBehaviour
{
    public GameObject Info_PopUp;
    public TextMeshProUGUI PopUpText;

    public void DoAvtivatePopUp(string msg)
    {
        PopUpText.text = msg;

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ShowUp());
        }
    }

    IEnumerator ShowUp()
    {
        Info_PopUp.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        Info_PopUp.SetActive(false);
        PopUpText.text = "";
    }
}