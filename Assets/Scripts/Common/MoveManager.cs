using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveManager : MonoBehaviour
{
    public static MoveManager instance;

    public bool isInMove;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    public void Move(Transform moveGo,Vector3 target,float moveSpeed,Action action)
    {
        StartCoroutine(MoveMethod(moveGo, target, moveSpeed, action));
    }
    IEnumerator MoveMethod(Transform moveGo, Vector3 target, float moveSpeed, Action action)
    {
        isInMove = true;
        
        while (true)
        {
            Vector3 dirction = ( target-moveGo.position ).normalized;
            yield return new WaitForSeconds(0.02f);
            moveGo.Translate(dirction * 0.02f * moveSpeed);
            if (Mathf.Abs( Vector3.Distance(moveGo.position, target) )<= 0.1)
            {
                moveGo.position = target;
                break;
            }
        }
        if (action != null)
            action();
        isInMove = false;
    }
}
