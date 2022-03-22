using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoController : MonoBehaviour
{
    Animation Anim;

    void Start()
    {
        Anim = GetComponent<Animation>();
    }

    public void SetPaddleAnimSpeed(int NowSpeed)
    {
        if (NowSpeed == 1)
        {
            Anim.Play("Paddle_Speed_1");
        }
        else if (NowSpeed == 2)
        {
            Anim.Play("Paddle_Speed_2");
        }
        else if (NowSpeed == 3)
        {
            Anim.Play("Paddle_Speed_3");
        }
        else if (NowSpeed == 0)
        {
            Anim.Stop();
        }
    }

    public void GameFinish()
    {
        Anim.Play("Auto_Finish");
    }

}
