using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG {
    public class MemoCheck_BP : MonoBehaviour
    {
        public GameObject XRRig;
        public GameObject Note;
        [SerializeField]
        public float fNote;
        float lTrigger;
        public bool set = true;
        void Update()
        {
            lTrigger = XRRig.GetComponent<InputBridge>().LeftTrigger;
            if (lTrigger >= 0.5) { if (set) { StartCoroutine(ThreeSeconds()); } };
            if (lTrigger < 0.5) { StopAllCoroutines(); Note.SetActive(false); set = true; };
        }
        IEnumerator ThreeSeconds()
        {
            set = false;
            Note.SetActive(true);
            fNote += 1;
            yield return new WaitForSeconds(3.2f);
            Note.SetActive(false);

        }
    }

}
