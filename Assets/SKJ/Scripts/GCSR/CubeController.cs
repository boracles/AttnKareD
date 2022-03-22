using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeController : MonoBehaviour
{
    private Vector3 thisPos;
    void Start()
    {
        thisPos = GetComponent<Transform>().position;
        
        GameEvents.current.moveForword += mCubeForword;
        GameEvents.current.moveBack += mCubeBack;
        GameEvents.current.moveLeft += mCubeLeft;
        GameEvents.current.moveRight += mCubeRight;
    }

    private void mCubeForword()
    {
        Debug.Log("이동전 포지션" + thisPos);

        transform.DOMoveZ(thisPos.z + 1, 0.3f).SetEase(Ease.InOutBounce);
        thisPos.z += 1;
        posValue(thisPos);        
    }

    private void mCubeBack()
    {
        Debug.Log("이동전 포지션" + thisPos);

        transform.DOMoveZ(thisPos.z - 1, 0.3f).SetEase(Ease.InOutBounce);
        thisPos.z -= 1;
        posValue(thisPos);
    }

    private void mCubeLeft()
    {
        Debug.Log("이동전 포지션" + thisPos);

        transform.DOMoveX(thisPos.x - 1, 0.3f).SetEase(Ease.InOutBounce);
        thisPos.x -= 1;
        posValue(thisPos);
    }

    private void mCubeRight()
    {
        Debug.Log("이동전 포지션" + thisPos);

        transform.DOMoveX(thisPos.x + 1, 0.3f).SetEase(Ease.InOutBounce);
        thisPos.x += 1;
        posValue(thisPos);
    }

    private Vector3 posValue(Vector3 currPos)
    {
        Debug.Log("이동 후 " + currPos);        
        return currPos;
    }
}
