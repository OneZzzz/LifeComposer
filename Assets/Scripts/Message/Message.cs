using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Message 
{
    public Message(string time,string content,bool isRight,string option01,string option02)
    {
        this.time = time;
        this.content = content;
        this.isRight = isRight;
        this.option01 = option01;
        this.option02 = option02;
    }


    public string time;
    public string content;
    public Sprite sprite;
    public bool isRight;
    public string option01="Hello", option02="Hi";
}
