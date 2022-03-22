using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BNG;

// 이 스크립트는 Child 오브젝트에 붙여서 사용해야 합니다
public class ReAct : MonoBehaviour
{
    [Header("Auto Insert")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject frame;
    [SerializeField] GameObject getParents;

    [Header("Find a reference and put it in")]
    [SerializeField] GrabbablesInTrigger grInTrigger;    

    Rigidbody p_Rigidbody;
    NavMeshAgent p_NavMeshAgent;
    //bool grab;

    void Start()
    {
        player = GameObject.Find("PlayerController");
        frame = transform.Find("Frame").gameObject;
        getParents = transform.parent.gameObject;
        p_Rigidbody = getParents.GetComponent<Rigidbody>();
        p_NavMeshAgent = getParents.GetComponent<NavMeshAgent>();
        //grab = false;
    }

    void Update()
    {
        // Y축만 회전시켜 target을 바라보게 한다
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y,
                                                player.transform.position.z);
        transform.LookAt(targetPosition);

        // Nav Mesh Agent 작동 때문에 RigidbodyConstraints.FreezeAll 되어 있는 것을
        // 그랩할때 RigidbodyConstraints.None 해주는 코드 / None이 아니면 그랩불가
        if (grInTrigger.ClosestRemoteGrabbable != null && grInTrigger.ClosestRemoteGrabbable.tag == "Unnecessary") 
        {            
            RemoteConstraints02();
        }       
    }

    IEnumerator ReAct01()
    {
        frame.SetActive(true);
        yield return new WaitForSeconds(2f);
        frame.SetActive(false);
    }    

    IEnumerator RemoteConstraints()
    {       
        if (InputBridge.Instance.RightTriggerDown == true)
        {            
            p_Rigidbody.constraints = RigidbodyConstraints.None;
        }

        if (InputBridge.Instance.RightTriggerDown == false)
        {
            p_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        
        yield return null;
    }

    void RemoteConstraints02()
    {
        if (InputBridge.Instance.RightTriggerDown == true)
        {
            p_NavMeshAgent.enabled = false;
            p_Rigidbody.constraints = RigidbodyConstraints.None;
        }                
    }    
}
