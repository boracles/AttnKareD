using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;


public class RaycastCircle : MonoBehaviour
{
    // Start is called before the first frame update

    public float radius;
    public GameObject canvas;
    public bool activeBool;
    public int objNum; 
    /* gizmos를 그리고자 하는 오브젝트에 스크립트 추가
     * objNum을 외부에서 할당해 오브젝트에 맞는 기능을 하게 끔 설정
     */
    



 



    // Update is called once per frame
    void Update()
    {

        

      //  activeBool = false;
      if(activeBool == true)
      {
          if(canvas.activeInHierarchy == false){

          
          canvas.SetActive(true);

          }
      }
      else
      {
          canvas.SetActive(false);
      }

        
    }

    void LateUpdate()
    {
        activeBool = false;
        
        Collider[] hitCollliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider hit in hitCollliders)
        {
            if(objNum == 1) //Bag Object
            {
            if(hit.tag=="Necessary_Book"||hit.tag =="Necessary") //필요한 물건이 가까이 닿으면
            {
                //activeBool = true;
                activeBool = true;
                
                
            }
            }

            /* if(objNum == 2) //pencilcase obj
            {
                if(hit.name=="RaycastCollider") //손이 닿으면
            {
                //activeBool = true;
                activeBool = true;
                
                
            }
            }
            Not Needed Anymore
            */

            if(objNum ==2) //button obj
            {
                if(hit.name == "RaycastCollider") //손이 닿으면
                {
                    activeBool = true;
                }
            }
 
        }



    }

    
   
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);


    }
}
