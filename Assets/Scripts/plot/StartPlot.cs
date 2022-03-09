using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class StartPlot : MonoBehaviour
{
    public static StartPlot instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private bool isInit;
    private Vector3 option01StartPos, option02StartPos, option03StartPos;
    private Transform option01, option02, option03,close;

    private PhoneManager phoneManager;

    private void Start()
    {
        option01 = transform.Find("option01");
        option02 = transform.Find("option02");
        option03 = transform.Find("option03");

        close = transform.Find("close");

        option01StartPos = option01.position;
        option02StartPos = option02.position;
        option03StartPos = option03.position;

        option01.GetComponentInChildren<Button>().onClick.AddListener(Option01);
        option02.GetComponentInChildren<Button>().onClick.AddListener(Option02);
        option03.GetComponentInChildren<Button>().onClick.AddListener(Option03);

        close.GetComponentInChildren<Button>().onClick.AddListener(ClosePlot);

        phoneManager = GameObject.FindObjectOfType<PhoneManager>();

    }
    private void Init()
    {
        option01.position = option01StartPos;
        option02.position = option02StartPos;
        option03.position = option03StartPos;


    }

    private void ClosePlot()
    {
        UIManager.instance.Fade(0, 1, delegate() { CameraManager.instance.CameraMoveRaw(new Vector3(0, 0, -10));phoneManager.StopReadinPhone(); }, 1, false);
    }

    public void OpenPlot()
    {
        Init();
        UIManager.instance.Fade(0, 1, ShowChapterName, 2, false);
    }
    private void ShowChapterName()
    {
       
        UIManager.instance.ShowLevelName("chapter 1", OpenFade);

    }

    private void OpenFade()
    {
        CameraManager.instance.CameraMoveRaw(transform.position);
        UIManager.instance.Fade(1, 0, OpenDialogu, 2, false);
    }

    private void OpenDialogu()
    {
        string message = "Click, scroll, fall asleep with my phone in my hand.\r\nThat's how my day ends,Maybe I should search something on the App";
        UIManager.instance.ShowIntroduceMessage(message, null);
    }
    private void ShowOption()
    {
        Transform target01 = transform.Find("target01");
        Transform target02 = transform.Find("target02");
        Transform target03 = transform.Find("target03");

        MoveManager.instance.Move(option01, target01.position, 2f, null);
        MoveManager.instance.Move(option02, target02.position, 2f, null);
        MoveManager.instance.Move(option03, target03.position, 2f, null);



    }

    private void OpenWb(Action action)
    {
        WbUIManager wbUI = GameObject.FindObjectOfType<WbUIManager>();
        if (wbUI == null)
        {
            GameObject wb = Resources.Load<GameObject>("WbUI");

            wbUI = Instantiate(wb).GetComponent<WbUIManager>();

        }
        wbUI.Init(action);
    }


    private void Option01()
    {
        if (MoveManager.instance.isInMove) return;
        OpenWb(Option01Event);
    }
    private void Option01Event()
    {
        UIManager.instance.AllScreenButton(()=>UIManager.instance.ShowIntroduceMessage("TODO:Puppies page", Option02Event));
    }

    private void Option02()
    {
        if (MoveManager.instance.isInMove) return;
        OpenWb(Option02Event);
    }
    private void Option02Event()
    {
        UIManager.instance.AllScreenButton(() => UIManager.instance.ShowIntroduceMessage("TODO:Life page", Option03Event));
    }

    private void Option03()
    {
        if (MoveManager.instance.isInMove) return;

        OpenWb(Option03Event);
    }
    private void Option03Event()
    {
        UIManager.instance.AllScreenButton(() => UIManager.instance.ShowIntroduceMessage("TODO:Love page", AfterOption03));
    }

    private void AfterOption03()
    {
        string message = "Wait, I remember this quote\r\n It's from When Breath Becomes Air\r\n I read it recently\r\nShould I leave a comment ? ";
        UIManager.instance.ShowIntroduceMessage(message, ShowCommentShadow);
    }
    private void ShowCommentShadow()
    {
        GameObject go= Resources.Load<GameObject>("plot/commentShadow");
        GameObject shadow= Instantiate(go, UIManager.instance.transform);
        Button button = shadow.GetComponentInChildren<Button>();
        button.onClick.AddListener(delegate() { Destroy(shadow);CommentShadowMethod(); });
    }
    private void CommentShadowMethod()
    {
        WbUIManager.instance.PlotHomeComment();
    }

    public void EndChapter01()
    {
        UIManager.instance.Fade(0, 1, null, 2,false);
        UIManager.instance.WaitTimeMethod(1.9f, FadeOutEndChapter01);
    }
    private void FadeOutEndChapter01()
    {
        UIManager.instance.Fade(1, 0, EndChapter01Message, 2, false); 
    }
    private void EndChapter01Message()
    {
        UIManager.instance.ShowIntroduceMessage("It's so nice to find someone to talk with", EndChapter01Message02);
    }
    private void EndChapter01Message02()
    {
        UIManager.instance.ShowIntroduceMessage("It's so nice to find someone to talk with", () => UIManager.instance.Fade(0, 1, OverEndChapter01Method, 2, false));
    }
    private void OverEndChapter01Method()
    {
        UIManager.instance.ShowLevelName("That's our first chat.\r\nI just can't help wanting to know more about her.", ClosePlot);
    }
}
