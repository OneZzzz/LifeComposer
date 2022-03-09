using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public float moveTime = 1.5f;

    public bool isInMove;

    public void CameraMoveRaw(Vector3 targetPosition)
    {
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        transform.position = targetPosition;
    }

    public void CameraMoveMethod(Vector3 targetPosition)
    {
        if (isInMove) return;
        isInMove = true;
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        StartCoroutine(Move(targetPosition, null));
    }

    public  void CameraMoveMethod(Vector3 targetPosition,Action action)
    {
        if (isInMove) return;
        isInMove = true;
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        StartCoroutine(Move(targetPosition,action));
    }
    IEnumerator Move(Vector3 targetPosition,Action action)
    {
        int count = (int)(moveTime * 100);

        Vector3 moveDistance =  targetPosition- transform.position;
        moveDistance = new Vector3(moveDistance.x / count, moveDistance.y / 100, moveDistance.z / 100);
        for (int i = 0; i < count; i++)
        {
            transform.Translate(moveDistance);
            if (Vector3.Distance(transform.position, targetPosition) < 0.08f)
                break;
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = targetPosition;
        yield return new WaitForSeconds(0.05f);
        isInMove = false;
        if(action!=null)
        action();
    }

}
