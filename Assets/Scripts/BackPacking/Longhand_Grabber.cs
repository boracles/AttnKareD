using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class Longhand_Grabber : MonoBehaviour
    {

        public GameObject XRRig;
        public GameObject grabTong;
        public GameObject laserPointer;
        public GameObject Hitobject;
        public GameObject hitcollider;
        public float speed = 1;

        private float rTrigger;
        private Transform hitParent;
  

        



        Vector3 startPos;

        private void Start()
        {
            startPos = grabTong.transform.localPosition;

            
        }

        private void Update()
        {
           
            rTrigger = XRRig.GetComponent<InputBridge>().RightTrigger;

        }

        private void LateUpdate()
        {
            bool btest;

            if (rTrigger >= 0.5)
            {
               
                btest = true;
                TriggerSpringMove(btest);
          
            }

            if (rTrigger <= 0.5)
            {
                Hitobject = null;
               
                btest = false;
                TriggerSpringMove(btest);


            }

        }



        public void OnTriggerEnter(Collider other)
        {
            if (other.transform.gameObject == Hitobject)
            {
                if (other.transform.gameObject.tag == "Necessary" || other.transform.gameObject.tag == "Necessary_Book" || other.transform.gameObject.tag == "Necessary_Pencil" || other.transform.gameObject.tag == "Unnecessary")
                {

                    hitcollider = other.transform.gameObject;
                    hitParent = hitcollider.transform.parent;
                    hitcollider.GetComponent<Rigidbody>().useGravity = false;
                    hitcollider.GetComponent<Collider>().enabled = false;


                }
            }
     
          

        }
        
        public void TriggerSpringMove(bool bTest)
        {


            if (bTest)
            {
                if(laserPointer.GetComponent<LaserPointer_V4_1>().hit.distance > 0)
                {

                    Hitobject = laserPointer.GetComponent<LaserPointer_V4_1>().hitObject;

                }


                if (hitcollider == null && Hitobject)
                {
                    laserPointer.SetActive(false);
                    grabTong.transform.position = Vector3.Lerp(grabTong.transform.position, Hitobject.transform.position, speed * Time.deltaTime);
 

                }
                if (hitcollider != null)
                {
                    laserPointer.GetComponent<LaserPointer_V4_1>().LaserEnd.SetActive(false);
                    hitcollider.transform.SetParent(grabTong.transform);
                    grabTong.transform.localPosition = Vector3.Lerp(grabTong.transform.localPosition, startPos, speed * Time.deltaTime);






                }
                



            }
            else if(bTest==false)
            {
                laserPointer.GetComponent<LaserPointer_V4_1>().LaserEnd.SetActive(true);
                laserPointer.SetActive(true);
                

                if (hitcollider != null )
                {


                    hitcollider.transform.SetParent(hitParent);
                    hitcollider.GetComponent<Rigidbody>().useGravity = true;
                    hitcollider.GetComponent<Collider>().enabled = true;
                    hitcollider = null;
                    

                    grabTong.transform.localPosition = Vector3.Lerp(grabTong.transform.localPosition, startPos, speed * Time.deltaTime );


                }
                if(hitcollider == null)
                {

                    grabTong.transform.localPosition = Vector3.Lerp(grabTong.transform.localPosition, startPos, speed * Time.deltaTime );
                }
   


            }
            return;




        }


    }

}

