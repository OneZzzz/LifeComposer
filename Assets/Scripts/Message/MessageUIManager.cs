using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUIManager : MonoBehaviour
{
    public static MessageUIManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    private Button closeBu,readingCloseBu,dialogueButton;

    private Transform screen,scroller,content;

    private Transform screen_reading,scroller_reanding,content_reading, option;

    public List<MessageGroud> allMessageGroud;

    private GameObject message,leftDialogue,rightDialogue;

    private GameObject option01, option02;



    public void Init()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        screen = transform.Find("screen");
        scroller = screen.Find("scroller");
        content = scroller.Find("content");
        closeBu = screen.Find("return").GetComponent<Button>();

        screen_reading = transform.Find("screen_reading");
        readingCloseBu = screen_reading.Find("return").GetComponent<Button>();
        scroller_reanding = screen_reading.Find("scroller");
        content_reading = scroller_reanding.Find("content");
        option = screen_reading.Find("option");

        option01 = option.GetChild(0).gameObject;
        option02 = option.GetChild(1).gameObject;
        dialogueButton = screen_reading.Find("inputfile").GetComponent<Button>();


        closeBu.onClick.RemoveAllListeners();
        closeBu.onClick.AddListener(delegate ()
        { for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
            for (int i = 0; i < content.childCount; i++) Destroy(content.GetChild(i).gameObject);
        });

        readingCloseBu.onClick.RemoveAllListeners();
        readingCloseBu.onClick.AddListener(delegate () { screen_reading.gameObject.SetActive(false); CleanAllChild(content_reading);option01.SetActive(false);option02.SetActive(false); });

        message = Resources.Load<GameObject>("message");
        leftDialogue = Resources.Load<GameObject>("leftDialogue");
        rightDialogue = Resources.Load<GameObject>("rightDialogue");

        option01.SetActive(false);
        option02.SetActive(false);
        screen_reading.gameObject.SetActive(false);
        screen.gameObject.SetActive(true);
        CreatMessageList();

    }

    void CreatMessageList()
    {
        if (allMessageGroud == null) return;
        for (int i = 0; i < allMessageGroud.Count; i++)
        {
            MessageGroud messageGroud = allMessageGroud[i];
            GameObject messageList = Instantiate(message, content);
            if (messageGroud.isReading)
                messageList.transform.Find("isReading").gameObject.SetActive(false);
            else
                messageList.transform.Find("isReading").gameObject.SetActive(true);

            if (messageGroud.messages == null) continue;

            messageList.transform.Find("time").GetComponent<Text>().text = messageGroud.messages[messageGroud.messages.Count-1].time;
            messageList.transform.Find("number").GetComponent<Text>().text = messageGroud.number;
            messageList.transform.Find("textMessage").GetComponent<Text>().text = messageGroud.messages[messageGroud.messages.Count - 1].content;

            messageList.GetComponent<Button>().onClick.AddListener(() => ShowDialogue(messageGroud));

        }
    }
    void CleanAllChild(Transform trans)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            Destroy(trans.GetChild(i).gameObject);
        }
    }
    void AddDialogue(MessageGroud messageGroud,bool isRight,string content)
    {
        string ti = string.Empty;
        Message me = new Message(ti, content, isRight, "hello", "hi");
        if (messageGroud.messages == null) messageGroud.messages = new List<Message>();

        messageGroud.messages.Add(me);
        option01.SetActive(false);
        option02.SetActive(false);
        CleanAllChild(content_reading);
    }
    void ShowDialogue(MessageGroud messageGroud)
    {
        screen_reading.gameObject.SetActive(true);
        dialogueButton.onClick.RemoveAllListeners();

        messageGroud.isReading = true;

        if (messageGroud.messages == null) return;

        dialogueButton.onClick.AddListener(delegate ()
        {
            option01.SetActive(true);
            if (messageGroud.messages.Count != 0)
                option01.GetComponentInChildren<Text>().text = messageGroud.messages[messageGroud.messages.Count - 1].option01;
            option01.GetComponent<Button>().onClick.RemoveAllListeners();
            option01.GetComponent<Button>().onClick.AddListener(delegate () { AddDialogue(messageGroud, true, messageGroud.messages[messageGroud.messages.Count - 1].option01);ShowDialogue(messageGroud); });

        });
        dialogueButton.onClick.AddListener(delegate ()
        {
            option02.SetActive(true);
            if (messageGroud.messages.Count != 0)
                option02.GetComponentInChildren<Text>().text = messageGroud.messages[messageGroud.messages.Count - 1].option02;
            option02.GetComponent<Button>().onClick.RemoveAllListeners();
            option02.GetComponent<Button>().onClick.AddListener(delegate () { AddDialogue(messageGroud, true, messageGroud.messages[messageGroud.messages.Count - 1].option02); ShowDialogue(messageGroud); });
        });

        for (int j = 0; j < messageGroud.messages.Count; j++)
        {
            Message me = messageGroud.messages[j];
            if (me.content != null)
            {
                GameObject go;
                if (me.isRight)
                    go = rightDialogue;
                else
                    go = leftDialogue;
                GameObject tem = Instantiate(go, content_reading);
                tem.GetComponentInChildren<Text>().text = me.content;
            }
        }

    }
    
}
