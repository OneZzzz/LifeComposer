using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    [HideInInspector]
    public bool mouseClick = false;


    private void LateUpdate()
    {
        mouseClick = false;
    }

    public bool CheckMouseDown()
    {
        return EventSystem.current.IsPointerOverGameObject();
        
    }

    public static bool GetUIClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }

    public void ShowIntroduceMessage(string message,Action action)
    {
        GameObject go = Resources.Load<GameObject>("introduceMessage");

        GameObject introduceMessageGO = Instantiate(go, transform);

        Text t = introduceMessageGO.GetComponentInChildren<Text>();

        StartCoroutine(TypeWriter(message, 20, t, action,introduceMessageGO));
    }

    public void ClickEvent(Action action)
    {
        
        GameObject go = Resources.Load<GameObject>("ClickCheck");

        GameObject click = Instantiate(go, transform);

        Button bu = click.GetComponent<Button>();

        bu.onClick.AddListener(delegate() { action();Destroy(click); });

    }

    public void Fade(float startAlpha, float stopAlpha, Action action,float fadeTime,bool autoKeep)
    {
        StartCoroutine(FadeMethod(startAlpha, stopAlpha, action,fadeTime,autoKeep));

    }
    IEnumerator FadeMethod(float startAlpha,float stopAlpha,Action action,float fadeTime,bool autoKeep)
    {
        GameObject go = Resources.Load<GameObject>("fade");

        GameObject fade = Instantiate(go, transform);

        Image image = fade.GetComponent<Image>();
        Color startColor = image.color;

        int count =(int) (100 * fadeTime);

        float alphaDistance = stopAlpha - startAlpha;
        alphaDistance = alphaDistance / (100*fadeTime);

        image.color = new Color(startColor.r, startColor.g, startColor.b, startAlpha);

        for (int i = 0; i < count; i++)
        {
            image.color = new Color(startColor.r, startColor.g, startColor.b, image.color.a + alphaDistance);
            yield return new WaitForSeconds(0.01f);
        }
        image.color= new Color(startColor.r, startColor.g, startColor.b, stopAlpha);
        if (action != null) action();
        if (!autoKeep)
            Destroy(fade);

    }



    public  void ShowLevelName(string levelName,Action action)
    {
        GameObject go = Resources.Load<GameObject>("levelName");

        GameObject levelNameGO= Instantiate(go, transform);
        Text t= levelNameGO.GetComponentInChildren<Text>();
        t.text = levelName;

        Destroy(levelNameGO, 2.1f);
        StartCoroutine(WaitTime(2f, action));
        
    }
    public void WaitTimeMethod(float time, Action action)
    {
        StartCoroutine(WaitTime(2f, action));
    }

    IEnumerator WaitTime(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
    IEnumerator TypeWriter(string message, float speed, Text t, Action action, GameObject go)
    {
        float waitTime = 1f / (float)speed;
        string showMessage = string.Empty;
        for (int i = 0; i < message.Length; i++)
        {
            yield return new WaitForSeconds(waitTime);
            showMessage += message[i];
            t.text = showMessage;
        }
        yield return new WaitForSeconds(0.5f);
        if (action != null)
            ClickEvent(delegate () { if (action != null) action(); Destroy(go); });
        else
            Destroy(go);
    }
    public void AllScreenButton(Action action)
    {
        GameObject go= Resources.Load<GameObject>("allScreenButton");
        GameObject screenButtonGo= Instantiate(go,transform);
        Button button = screenButtonGo.GetComponent<Button>();
        button.enabled = false;
        button.onClick.AddListener(delegate () { if (!mouseClick) { Destroy(screenButtonGo); action(); } });
        button.enabled = true;
    }

    public void GetMessage(string message, float destoryTime)
    {
        GameObject go = Resources.Load<GameObject>("GetMessage");
        GameObject getMessageUI = Instantiate(go, transform);
        if(message!=null)
        getMessageUI.GetComponentInChildren<Text>().text = message;
        else
            getMessageUI.GetComponentInChildren<Text>().text = "You got a message!";
        Destroy(getMessageUI, destoryTime);
    }
}
