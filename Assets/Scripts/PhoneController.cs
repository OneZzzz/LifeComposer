using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhoneController : MonoBehaviour
{
    public Button closeButton,messageButton,wbButton,chapterButton;

    private StartPlot plot;

    private void Start()
    {
        messageButton.onClick.AddListener(ShowMessageUI);
        wbButton.onClick.AddListener(ShowWbUI);
        plot = GameObject.FindObjectOfType<StartPlot>();
        chapterButton.onClick.AddListener(delegate() { plot.OpenPlot();Destroy(gameObject,2f); });
    }
    public void ShowMessageUI()
    {
        MessageUIManager messageUI = GameObject.FindObjectOfType<MessageUIManager>();
        if (messageUI==null)
        {
            GameObject meUI = Resources.Load<GameObject>("MessageUI");

             messageUI =  Instantiate(meUI).GetComponent<MessageUIManager>();
          
        }
        messageUI.Init();
    }
    public void ShowWbUI()
    {
        WbUIManager wbUI = GameObject.FindObjectOfType<WbUIManager>();
        if (wbUI == null)
        {
            GameObject wb = Resources.Load<GameObject>("WbUI");

            wbUI = Instantiate(wb).GetComponent<WbUIManager>();

        }
        wbUI.Init();
    }
}
