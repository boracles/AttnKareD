using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Pool;

using TMPro;

//SimpleMove(lTrans.position);
//speechBubble.SetText("Messagesadnasdkandkandkadnkadnkasndakn");


public partial class ActorVideo : MonoBehaviour
{
    private const string PARAM_ANIM_IS_WALK = "isWalk";
    private const string PARAM_ANIM_SPEED = "AnimationSpeed";

    SpriteRenderer[] spriteRenderers;
    Animator animator;
    Quaternion originalRotation;

    public float moveTime = 3f;
    public GameObject speechBubble;
    public AudioClip[] audioSource;

    void Start()
    {
        originalRotation = transform.rotation;
        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

        InitAnimation();
        //TestMove(Vector3.left * 3f);
        //SimpleMove(Vector3.left );
    }

    public IEnumerator MoveGhost(Vector3 des, float speed)
    {


        this.GetComponent<Animator>().SetBool("isWalk", true);
        if (this.transform.position.x > des.x)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 180f,transform.localEulerAngles.z);

        }
        if (transform.position.x < des.x)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);

        }
        Vector3 prevPos = transform.position;
        float t = 0;
        while (true)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(prevPos, des, t);


            if (t >= 1)
            {
                transform.position = des;
                GetComponent<Animator>().SetBool("isWalk", false);
                break;

            }
            yield return null;
        }



    }


    public IEnumerator ghostSpeak(string input, int index)
    {
        AudioSource ghostAudio = transform.GetComponent<AudioSource>();
        ghostAudio.clip = audioSource[index];
        ghostAudio.Play();
        speechBubble.gameObject.SetActive(true);
        speechBubble.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = input;

        yield return new WaitForSeconds(2.5f);

        speechBubble.SetActive(false);

    }




    /*
    void TestMove(Vector3 dir)
    {
        SimpleMove(dir, () =>
        {
            dir = 3f * (dir.x > 0 ? Vector3.left : Vector3.right);
            TestMove(dir);
        });
    }

    public void SimpleMove(Transform trans, System.Action onComplete = null) => SimpleMove(trans.position, onComplete);
    public void SimpleMove(Vector3 position, System.Action onComplete = null)
    {
        var dest = position;
        dest.y = transform.position.y;
        animator.SetBool(PARAM_ANIM_IS_WALK, true);

        var tween = transform.DOMove(dest, moveTime).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
         
        tween.OnUpdate(() =>
        {
            var dir = dest - transform.position;

            if (dir.magnitude <= 0.25f)
            {
                animator.SetFloat(PARAM_ANIM_SPEED, 1f);
                animator.SetBool(PARAM_ANIM_IS_WALK, false);
            }
            else
            {
                animator.SetFloat(PARAM_ANIM_SPEED, 1f + dir.magnitude * 0.5f);
            }

            spriteRenderers.ToList().ForEach(_ =>
            {
                if (dir.x != 0)
                {
                    _.flipX = dir.x > 0;
                }
            });
        });
    }
    */
}

public partial class ActorVideo//+Ani,Fx
{
    

    void InitAnimation()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool(PARAM_ANIM_IS_WALK, false);
        
    }
    public void OnFootStep()
    {
        
    }
}

/*

public partial class Actor//CameraFacing
{
    Camera referenceCamera;
    public enum Axis { up, down, left, right, forward, back };
    public bool reverseFace = false;
    public Axis axis = Axis.up;
    void Awake()
    {
        // if no camera referenced, grab the main camera
        if (!referenceCamera)
            referenceCamera = Camera.main;
    }
    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        var a = Camera.main.transform.forward;
        a.y = 0f;

        transform.forward = a;
        
    }
    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.down:
                return Vector3.down;
            case Axis.forward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
        }

        // default is Vector3.up
        return Vector3.up;
    }
}
*/

