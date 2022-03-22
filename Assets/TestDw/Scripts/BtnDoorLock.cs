using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using BNG;

namespace dw.game.doorlock
{
    public class BtnDoorLock : MonoBehaviour
    {
        //[System.Serializable]
        //public class ButtonEvent : UnityEvent { }

        //public float pressLength;
        //public bool pressed;
        //public ButtonEvent downEvent;
        //Vector3 startPos;
        GameObject pad;
        GameObject pad_emission;

        void Start()
        {
            pad = transform.Find("pad").gameObject;
            pad_emission = transform.Find("pad_emission").gameObject;
            pad.SetActive(true);
            pad_emission.SetActive(false);
            //gameObject.GetComponent<Button>().onButtonUp.AddListener(() =>
            //{
 
            //});

            gameObject.GetComponent<Button>().onButtonDown.AddListener(() =>
            {

                StartCoroutine(DelayButtonDown(true));
            });
        }

        IEnumerator DelayButtonDown(bool isDown)
        {
            var pad = transform.Find("pad").gameObject;
            var pad_emission = transform.Find("pad_emission").gameObject;

            pad.SetActive(!isDown);
            pad_emission.SetActive(isDown);

            yield return new WaitForSeconds(0.25f);

            pad.SetActive(isDown);
            pad_emission.SetActive(!isDown);

        }

      
    }
}
