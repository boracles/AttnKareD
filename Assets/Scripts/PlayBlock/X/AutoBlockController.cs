using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBlockController : MonoBehaviour
{
    public Transform Target;
    public bool isDone = false;

    //float Delay = 0;

    void Update()
    {
/*        if (Target != null)
        {
            Delay += Time.deltaTime;

            if (Delay > 1.5f)
            {
                float step = 4 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, Target.position, step);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Target.rotation, step * 10);
                isDone = false;
            }

            if (transform.position == Target.position)
            {
                //delay ?
                Target.gameObject.SetActive(false);
                transform.GetComponent<BoxCollider>().enabled = false;
                isDone = true;
            }
        }*/
    }
}
