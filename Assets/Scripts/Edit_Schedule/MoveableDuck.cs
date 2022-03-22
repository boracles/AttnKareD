using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BNG;

public class MoveableDuck : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    Vector3 lastPosition = Vector3.zero;

    [SerializeField]
    Transform[] targets = new Transform[4];

    [SerializeField]
    Transform parentTarget;

    [SerializeField]
    float speed;

    [SerializeField]
    int currentNum;

    [SerializeField]
    bool isGoing;

    [Header("Find a reference and put it in")]    
    [SerializeField] Grabber grabber;

    public float Speed { get => speed; set => speed = value; }

    Rigidbody m_Rigidbody;
    //NavMeshAgent agent;

    private void Awake()
    {        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {        
        for(int i = 0; i < targets.Length; i++)
        {
           targets[i] = parentTarget.transform.GetChild(i).transform;
        }

        isGoing = false;
        currentNum = 0;
        StartCoroutine(GoToTarget());
        m_Rigidbody = GetComponent<Rigidbody>();
        //agent = GetComponent<NavMeshAgent>();
    }
  
    void Update()
    {
        Speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (Speed > 0.01f)
        {
            if (!isGoing)
            {
                isGoing = true;
                animator.SetInteger("Status", 1);
            }
        }

        else if (Speed < 0.01f)
        {
            if(animator.GetInteger("Status") != 2)
            {
                animator.SetInteger("Status", 0);
            }
            
            if (isGoing && Speed == 0 && agent.enabled == true)
            {                
                    isGoing = false;
                    StartCoroutine(GoToTarget());                
            }
        }
    }
  
    IEnumerator GoToTarget()
    {       
        agent.SetDestination(targets[currentNum].position);
        AddCurrentNum();

        yield return new WaitForEndOfFrame();        
    }

    void AddCurrentNum()
    {        
        currentNum++;

        if (currentNum >= targets.Length)
        {
                currentNum = 0;
        }          
    }

    void SpeedUp()
    {       
        if(agent.speed < 2)
        {
            agent.speed += 0.2f;
        }

        if(animator.GetFloat("MoveSpeed") < 20)
        {
            animator.SetFloat("MoveSpeed", animator.GetFloat("MoveSpeed") + 1f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && grabber.HeldGrabbable == null)
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            agent.enabled = true;            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall" && grabber.HeldGrabbable == null)
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            agent.enabled = true;            
        }
    }
}
