using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




namespace BNG
{
    public class BackToTutorial : MonoBehaviour
    {
        public InputBridge Controller;
        bool xButton;
        bool aButton;
        bool waitB;

        // Update is called once per frame
        void Update()
        {

            aButton = Controller.AButtonDown;
            xButton = Controller.XButtonDown;

            if(aButton &&xButton)
            {

                waitB = true;
                
            }
            else if(!aButton || !xButton)
            {

                waitB= false;

            }


        }


        IEnumerator SceneChange()
        {

            while (waitB)
            {

                yield return new WaitForSeconds(3.0f);

                SceneManager.LoadScene("BagPacking2X2");

            }


        }



    }




}
