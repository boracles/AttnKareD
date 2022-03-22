using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action moveForword;
    public event Action moveBack;
    public event Action moveLeft;
    public event Action moveRight;

    public void Forword()
    {
        if(moveForword != null)
        {
            moveForword();
        }
    }

    public void Back()
    {
        if (moveBack != null)
        {
            moveBack();
        }
    }

    public void Left()
    {
        if (moveLeft != null)
        {
            moveLeft();
        }
    }

    public void Right()
    {
        if (moveRight != null)
        {
            moveRight();
        }
    }
}
