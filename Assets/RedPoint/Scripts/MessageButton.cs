using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageButton : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text messageText;
    public TMP_Text numText;
    public GameObject numParent;
    public int remainedLength = 10;

    public void UpdateMsg(PostDataBody body)
    {
        string content = body.content;
        int length = Math.Min(content.Length, remainedLength);
        messageText.text = content.Substring(0, length);
        if (content.Length > remainedLength)
        {
            messageText.text += "...";
        }
    }

    public void Init(Character character)
    {
        nameText.text = character.characterInfo.name;
        numText.text = "0";
        if(character.messages.Count>0)
        {
            List<PostDataBody> bodys = character.messages;
            string content = bodys[bodys.Count-1].content;
            int length = Math.Min(content.Length, remainedLength);
            messageText.text = content.Substring(0, length);
            if(content.Length> remainedLength)
            {
                messageText.text += "...";
            }
            int num = 0;
            for (int i = bodys.Count - 1; i >= 0 ; i--)
            {
                PostDataBody body = bodys[i];
                if(body.role == "assistant")
                {
                    num++;
                }
                else
                {
                    numText.text = num.ToString();
                    break;
                }
            }
            if(num==0)
            {
                numParent.SetActive(false);
            }
        }
    }
}
