using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendMessageManager : MonoBehaviour
{
    public TMP_InputField inputText;
    public Button sendButton;

    public ShowMessageController SMC;
    
    private void Start()
    {
        sendButton.onClick.AddListener(SendMsg);
        SMC = GetComponent<ShowMessageController>();
    }
    
    public void SendMsg()
    {
        if(inputText.text.Count() > 0)
        {
            PostDataBody body = new PostDataBody("user", inputText.text);
            SMC.currentButton.UpdateMsg(body);
            SMC.GenerateMessage(body);
            string content = inputText.text;
            ChatGLMManager.Instance.currentCharacter.SendMessage(content,(body)=>{
                SMC.GenerateMessage(body);
                SMC.currentButton.UpdateMsg(body);
            });
            inputText.text = string.Empty;
        }
    }
}
