using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public static PhoneManager instance;
    private void Awake()
    {
        if(instance = null)
            instance = this;
    }
    private void Start()
    {
        InitPhone();
        Invoke("CallPhone", 3f);
    }

    private int shakeAngle = 2;
    private GameObject mask,nameUI;
    private bool isCallPhone,isInAnimation;
    private Vector3 phoneStartPos, screenCenterPosition;
    private PhoneController phone;

    public void InitPhone()
    {
        mask = transform.GetChild(0).gameObject;
        phoneStartPos = transform.position;
        nameUI = transform.parent.parent.Find("nameUI").gameObject;
        screenCenterPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
        screenCenterPosition = new Vector3(screenCenterPosition.x, screenCenterPosition.y, 0);
        mask.SetActive(false);
    }

    public void CallPhone()
    {
        isCallPhone = true;
        StartCoroutine(StartShake());
    }
    public void StopCallPhone()
    {
        StopAllCoroutines();
        mask.SetActive(false);
        transform.eulerAngles = Vector3.zero;
    }
    public void ReadingPhone()
    {
        StopAllCoroutines();
        isInAnimation = true;
        StartCoroutine(PhoneMoveAnimation(screenCenterPosition,CreatPhoneUI));
    }
    public void StopReadinPhone()
    {
        
        StartCoroutine(PhoneMoveAnimation(phoneStartPos,DestoryPhoneUI));
        if(phone!=null)
        phone.gameObject.SetActive(false);
        nameUI.SetActive(true);
    }
    private void CreatPhoneUI()
    {
        phone = Instantiate(Resources.Load<GameObject>("PhoneUI")).GetComponent<PhoneController>();
        phone.closeButton.onClick.AddListener(StopReadinPhone);
        nameUI.SetActive(false);
    }
    private void DestoryPhoneUI()
    {
        transform.eulerAngles = Vector3.zero;
        isInAnimation = false;
        if(phone!=null)
        Destroy(phone.gameObject);
    }


    IEnumerator PhoneMoveAnimation(Vector3 targetPosition,Action action)
    {
        int moveInter = 60;
        float x = targetPosition.x - transform.position.x;
        float y = targetPosition.y - transform.position.y;
        float posX = x / moveInter;
        float posY = y / moveInter;
        for (int i = 0; i < moveInter; i++)
        {
            transform.Translate(posX, posY, 0);
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = targetPosition;
        action();
    }

    IEnumerator StartShake()
    {
        mask.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            int count = 0;
            for (int j = 0; j < shakeAngle*4; j++)
            {
                if (count <= shakeAngle)
                {
                    transform.Rotate(Vector3.forward);
                }
                else if (count <= 3 * shakeAngle)
                {
                    transform.Rotate(Vector3.back);
                }
                else if (count <= 4 * shakeAngle)
                {
                    transform.Rotate(Vector3.forward);
                }
                count++;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    private void OnMouseDown()
    {
        if (!isCallPhone) return;
        if (isInAnimation) return;
        if (UIManager.GetUIClick()) return;
        ReadingPhone();
    }
}
