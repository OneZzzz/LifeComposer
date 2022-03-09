using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WbUIManager : MonoBehaviour
{
    public static WbUIManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }


    public Button homeBu, messageBu, mineBu,diaBu,closeBu;
    public GameObject homeGo, messageGo, mineGo,diaGo,menuGo,sendMessageGo,dialogueOption,homeCommentOption;

    private bool isInit;
    private GameObject rightDia, leftDia,contens;
    public Transform dialogueContent,homeContent;

    private void Start()
    {
        if (!isInit)
            Init();
    }
    //初始化后执行方法
    public void Init(Action action)
    {
        Init();
        action();
    }

    public void Init()
    {
        if (!isInit)
        {
            rightDia = Resources.Load<GameObject>("rightWbDialogue");
            leftDia = Resources.Load<GameObject>("leftWbDialogue");
            contens = Resources.Load<GameObject>("contens");
            homeBu.onClick.AddListener(() => OpenPage(homeGo));
            messageBu.onClick.AddListener(() => OpenPage(messageGo));
            mineBu.onClick.AddListener(() => OpenPage(mineGo));
            diaBu.onClick.AddListener(delegate () { OpenPage(diaGo); menuGo.SetActive(false); sendMessageGo.SetActive(true); });
            diaGo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OpenPage(messageGo); menuGo.SetActive(true); sendMessageGo.SetActive(false); });
            sendMessageGo.GetComponent<Button>().onClick.AddListener(DialogueOptionAddListion);
            closeBu.onClick.AddListener(CloseAll);

            HomeCommentAddListion();

            isInit = true;
        }

        OpenPage(homeGo);
    }
    private void OpenPage(GameObject go)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        closeBu.gameObject.SetActive(true);
        homeGo.SetActive(false);
        messageGo.SetActive(false);
        mineGo.SetActive(false);
        diaGo.SetActive(false);
        sendMessageGo.SetActive(false);
        dialogueOption.SetActive(false);
        go.SetActive(true);
    }

    private void CloseAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 默认的聊天对话选项的添加
    /// </summary>
    private void DialogueOptionAddListion()
    {
        //for (int i = 0; i < dialogueOption.transform.childCount; i++)
        //{
        //    Transform op = dialogueOption.transform.GetChild(i);
        //    Button bu = op.GetComponent<Button>();
        //    bu.onClick.RemoveAllListeners();
        //    bu.onClick.AddListener(delegate()
        //    {
        //        dialogueOption.SetActive(false);
        //        string me = op.GetComponentInChildren<Text>().text;
        //        AddDialogue(false, me);
        //    });
        //}
        DialogueOptionSetting("yes", "no", "hello", "hi", null, null, null, null, "yes", "no", "hello", "hi");


    }
    /// <summary>
    /// 所有对话选项的设置
    /// </summary>
    /// <param name="o1"></param>
    /// <param name="o2"></param>
    /// <param name="o3"></param>
    /// <param name="o4"></param>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    /// <param name="a3"></param>
    /// <param name="a4"></param>
    private void DialogueOptionSetting(string o1,string o2,string o3,string o4,Action a1,Action a2,Action a3,Action a4 ,string optionDialogue1, string optionDialogue2, string optionDialogue3, string optionDialogue4)
    {
        dialogueOption.SetActive(true);
        if (o1==null || o1 == "")
            dialogueOption.transform.GetChild(0).gameObject.SetActive(false);
        else
        {
            dialogueOption.transform.GetChild(0).gameObject.SetActive(true);
            OneDialogueOptionSetting(0,o1, a1, optionDialogue1);        
        }
        if (o2 == null || o2 == "")
            dialogueOption.transform.GetChild(1).gameObject.SetActive(false);
        else
        {
            dialogueOption.transform.GetChild(1).gameObject.SetActive(true);
            OneDialogueOptionSetting(1,o2, a2, optionDialogue2);
        }

        if (o3 == null || o3 == "")
            dialogueOption.transform.GetChild(2).gameObject.SetActive(false);
        else
        {
            dialogueOption.transform.GetChild(2).gameObject.SetActive(true);
            OneDialogueOptionSetting(2,o3 ,a3, optionDialogue3);
        }
        if (o4 == null || o4 == "")
            dialogueOption.transform.GetChild(3).gameObject.SetActive(false);
        else
        {
            dialogueOption.transform.GetChild(3).gameObject.SetActive(true);
            OneDialogueOptionSetting(3,o4, a4, optionDialogue4);
        }




    }
    /// <summary>
    /// 单个对话选项的设置
    /// </summary>
    /// <param name="index"></param>
    /// <param name="action"></param>
    private void OneDialogueOptionSetting(int index,string optionName,Action action,string optionDialogue)
    {
        Transform op = dialogueOption.transform.GetChild(index);
        Button bu = op.GetComponent<Button>();
        op.GetComponentInChildren<Text>().text = optionName;
        bu.onClick.RemoveAllListeners();
        bu.onClick.AddListener(delegate ()
        {
            dialogueOption.SetActive(false);
            AddDialogue(false, optionDialogue);
            if(action!=null)
            action();
        });
    }

    /// <summary>
    /// 添加对话气泡
    /// </summary>
    /// <param name="isLeft"></param>
    /// <param name="message"></param>
    private void AddDialogue(bool isLeft,string message)
    {
        GameObject go;
        if (!isLeft)
           go= Instantiate(rightDia, dialogueContent);
        else
            go = Instantiate(leftDia, dialogueContent);
        
        go.GetComponentInChildren<Text>().text = message;
    }

    /// <summary>
    /// 为首页每个微博的评论按钮添加功能：显示各个选项，并为各个选项添加事件
    /// </summary>
    private void HomeCommentAddListion()
    {
        for (int i = 0; i < homeContent.childCount; i++)
        {
            Transform trans = homeContent.GetChild(i);
            Button bu = trans.GetComponentInChildren<Button>();
            if (bu == null) continue;
            bu.onClick.RemoveAllListeners();
            bu.onClick.AddListener(()=>OpenHomeCommentOption(trans,i));
        }
    }
    /// <summary>
    /// 打开选项按钮，关闭菜单栏，为各个选项添加事件
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="index"></param>
    private void OpenHomeCommentOption(Transform trans,int index )
    {
        menuGo.SetActive(false);
        homeCommentOption.SetActive(true);
        for (int i = 0; i < homeContent.childCount; i++)
        {
            homeContent.GetChild(i).gameObject.SetActive(false);
        }

        trans.gameObject.SetActive(true);

        for (int j = 0; j < homeCommentOption.transform.childCount; j++)
        {
            Button ou = homeCommentOption.transform.GetChild(j).GetComponent<Button>();

            ou.onClick.RemoveAllListeners();
            string meT = homeCommentOption.transform.GetChild(j).GetComponentInChildren<Text>().text;
            ou.onClick.AddListener(() => HomeCommentOptionEvent(meT, trans));
        }
    }
    /// <summary>
    /// 首页评论选项，点击后生成评论
    /// </summary>
    /// <param name="message"></param>
    /// <param name="trans"></param>
    private void HomeCommentOptionEvent(string message,Transform trans)
    {
        for (int i = 0; i < homeContent.transform.childCount; i++)
        {
            homeContent.transform.GetChild(i).gameObject.SetActive(true);
        }

        menuGo.SetActive(true);
        homeCommentOption.SetActive(false);
        int index = trans.GetSiblingIndex();
        List<Transform> allTrans = new List<Transform>();
        for (int i = 0; i < homeContent.transform.childCount; i++)
        {
            if (i>index)
            {
                allTrans.Add(homeContent.transform.GetChild(i));
            }
        }
        for (int i = 0; i < allTrans.Count; i++)
        {
            allTrans[i].SetParent(transform);
        }

        GameObject go= Instantiate(contens,homeContent);
        go.transform.SetAsLastSibling();

        for (int i = 0; i < allTrans.Count; i++)
        {
            allTrans[i].SetParent(homeContent);
        }


       

        go.GetComponentInChildren<Text>().text = message;
    }

    /// <summary>
    /// 为首页每个微博的评论按钮添加功能：显示各个选项，并为各个选项添加事件，并在执行完毕后使用某方法
    /// </summary>
    /// <param name="message"></param>
    /// <param name="trans"></param>
    /// <param name="action"></param>
    private void HomeCommentOptionEvent(string message, Transform trans,Action action)
    {
        HomeCommentOptionEvent(message, trans);
        action();
    }
    /// <summary>
    /// 剧情模式下，为首页第一个微博的评论按钮添加事件，并设置各个评论按钮点击后的事件
    /// </summary>
    public void PlotHomeComment()
    {
        menuGo.SetActive(false);
        homeCommentOption.SetActive(true);
        for (int i = 0; i < homeContent.childCount; i++)
        {
            homeContent.GetChild(i).gameObject.SetActive(false);
        }
        homeContent.GetChild(0).gameObject.SetActive(true);

        for (int j = 0; j < homeCommentOption.transform.childCount; j++)
        {
            Button ou = homeCommentOption.transform.GetChild(j).GetComponent<Button>();

            ou.onClick.RemoveAllListeners();

            string met=string.Empty;

            if (j == 0)
            {
                ou.GetComponentInChildren<Text>().text = "NO";
                met = "NO";
                ou.onClick.AddListener(() => HomeCommentOptionEvent(met, homeContent.GetChild(0),PlotHomeCommentNo));
            }
            else if (j == 1)
            {
                ou.GetComponentInChildren<Text>().text = "ASK";
                met = "Is this from When Breath Becomes Air? I just read this book recently!";
                ou.onClick.AddListener(() => HomeCommentOptionEvent(met, homeContent.GetChild(0),PlotHomeCommentAsk));
            }
            else
                ou.GetComponentInChildren<Text>().text = "";
            
        }
    }

    private void PlotHomeCommentNo()
    {

    }
    private void PlotHomeCommentAsk()
    {
        UIManager.instance.WaitTimeMethod(5,PlotGetFirstMessage);
        
    }
    //剧情模式评论后显示获得一条聊天消息,并且点击消息列表按钮后自动播放对话
    private void PlotGetFirstMessage()
    {
        UIManager.instance.GetMessage(null, 2f);
        AddDialogue(true, "Hiii, I just read your comment.Do you like the book ?");
        diaBu.onClick.AddListener(PlotDiaButtonAutoShowDialogue);
    }
    //剧情模式点击聊天按钮后自动播放对话,并且移除这个事件
    private void PlotDiaButtonAutoShowDialogue()
    {
        UIManager ui = UIManager.instance;
        diaBu.onClick.RemoveListener(PlotDiaButtonAutoShowDialogue);
        sendMessageGo.GetComponent<Button>().onClick.RemoveAllListeners();
        ui.WaitTimeMethod(1f,
            delegate ()
            {
                AddDialogue(false, "Yeah, I love it.");
                ui.WaitTimeMethod(1f, delegate ()
                {
                    AddDialogue(true, "What do you love about?");
                    ui.WaitTimeMethod(1f, delegate ()
                    {
                        AddDialogue(false, "idk, maybe the life lessons it taught me?");
                        ui.WaitTimeMethod(1f, delegate () { AddDialogue(true, "it's nice to find someone who has the same taste."); PlotDia02(); });
                    });
                });
            });




    }

    private void PlotDia02()
    {
        //未点击主页
        if (!PlotSave.isClickHomePage)
        {
            DialogueOptionSetting("Name", null, null, null, PlotNameOption, null, null, null, "What's the meaning of your name? It doesn't look like English.", null, null, null);
        }
        else
        {
            DialogueOptionSetting("Name", "Avatar", null, null, PlotNameOption, PlotAvatarOption, null, null, "What's the meaning of your name? It doesn't look like English.", "Is your porfile picture from Harry Porter?", null, null);
        }
    }
    private void PlotNameOption()
    {
        UIManager ui = UIManager.instance;
        ui.WaitTimeMethod(1f,
            delegate ()
            {
                AddDialogue(true, "Oh it's actually German. ");
                ui.WaitTimeMethod(1f, delegate ()
                {
                    AddDialogue(true, "Weniger means Less and Reden means Talk.");
                    ui.WaitTimeMethod(1f, delegate ()
                    {
                        AddDialogue(true, "\"Less Talk\" basiclly.");
                        ui.WaitTimeMethod(1f, delegate () { AddDialogue(false, "Wow that's cool"); DialogueOptionSetting("German", "Time", null, null, PlotGermanOption, PlotTimeOption, null, null, "So... Are you German?", "It's already 1 a.m. Why are you still on your phone?", null, null); });
                    });
                });
            });
    }
    private void PlotGermanOption()
    {
        UIManager ui = UIManager.instance;
        ui.WaitTimeMethod(1f,
            delegate ()
            {
                AddDialogue(true, "Yep");
                ui.WaitTimeMethod(1f, delegate ()
                {
                    AddDialogue(true, "But I might go to US if I had time.");
                    ui.WaitTimeMethod(1f, delegate ()
                    {
                        AddDialogue(true, "You are in US right?");
                        ui.WaitTimeMethod(1f, delegate ()
                        {
                            AddDialogue(false, "How did you know?"); ui.WaitTimeMethod(1f, delegate ()
                            {
                                AddDialogue(true, "well, your introduction tells me");
                                DialogueOptionSetting(null, "Avatar", null, null,null, PlotAvatarOption, null, null, null, "Is your porfile picture from Harry Porter?", null, null);
                            });
                        });
                    });
                });
            });
    }
    private void PlotTimeOption()
    {
        UIManager ui = UIManager.instance;
        ui.WaitTimeMethod(1f,
            delegate ()
            {
                AddDialogue(true, "I am Germany, so it's 7 am for me :D");
                ui.WaitTimeMethod(1f, delegate ()
                {
                    AddDialogue(false, "Such an early bird");
                    DialogueOptionSetting(null, "Avatar", null, null, null, PlotAvatarOption, null, null, null, "Is your porfile picture from Harry Porter?", null, null);
                });
            });
    }


    private void PlotAvatarOption()
    {
        UIManager ui = UIManager.instance;
        ui.WaitTimeMethod(1f,
            delegate ()
            {
                AddDialogue(true, "Yeah, do you like Harry Porter?");
                ui.WaitTimeMethod(1f, delegate ()
                {
                    AddDialogue(false, "I love it!");
                    ui.WaitTimeMethod(1f, delegate() { ui.WaitTimeMethod(3f, CloseAll);StartPlot.instance.EndChapter01(); });
                });
            });
    }
}
