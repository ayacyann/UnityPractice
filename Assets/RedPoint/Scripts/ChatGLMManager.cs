using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ChatGLMManager : MonoBehaviour
{
    public Character mainCharacter;
    public List<Character> otherCharacters = new List<Character>();
    public CharacterInfoSO characterInfoSO;
    public Character currentCharacter;
    private ChatGLMManager() {}
    public string chatGLMurl = "https://open.bigmodel.cn/api/paas/v4/chat/completions";
    public string apiKey = "8e35049c911828a1935f110833dac438.i0EPoK5ZC2CxBBiM";
    private static ChatGLMManager instance;
    public static ChatGLMManager Instance
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ChatGLMManager>();
                if( instance == null )
                {
                    GameObject go = new GameObject("ChatGLMManager");
                    instance = go.AddComponent<ChatGLMManager>();
                }
            }
            return instance;
        }
    }

    public Character GenerateCharacter(CharacterInfo charInfo)
    {
        Character character = new Character();
        character.characterInfo = charInfo;
        character.isMainCharacter = charInfo.isMainCharacter;
        return character;
    }

    public void SetCurrentCharacter(Character character)
    {
        if(currentCharacter==null)
        {
            currentCharacter = new Character();
        }
        currentCharacter.characterInfo = character.characterInfo;
        currentCharacter.isMainCharacter = character.isMainCharacter;
        currentCharacter.messages = character.messages;
    }

    private void Awake() 
    {
        //创建当前的所有角色信息
        List<CharacterInfo> otherCharacterInfos;
        List<CharacterMessage> otherMessages;
        CharacterInfo mainCharacterInfo = characterInfoSO.FindMainCharacter(out otherCharacterInfos,out otherMessages);
        mainCharacter = GenerateCharacter(mainCharacterInfo);
        for(int i=0;i< otherCharacterInfos.Count;i++)
        {
            Character C = GenerateCharacter(otherCharacterInfos[i]);
            otherCharacters.Add(C);
            C.messages = otherMessages[i].messages;
        }
    }

    public void UpdateSO()
    {
        characterInfoSO.SaveMessageWithCharacter(currentCharacter);
    }
    
    public void Send(PostData post,UnityAction<string> callback)
    {
        StartCoroutine(ChatGLMSend(post,callback));
    }

    IEnumerator ChatGLMSend(PostData post, UnityAction<string> callback)
    {
        using(UnityWebRequest request=new UnityWebRequest(chatGLMurl,"POST"))
        {
            string json = JsonUtility.ToJson(post);
            byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if(request.responseCode==200)
            {
                ChatGLMResponse response = JsonUtility.FromJson<ChatGLMResponse>(request.downloadHandler.text);
                if(response!=null&&response.choices.Count>=1)
                {
                    string responsedMsg = response.choices[0].message.content;
                    callback?.Invoke(responsedMsg);
                }
            }
        }
    }
}

[Serializable]
public class ChatGLMResponse
{
    public string id;
    public string model;
    public List<ChatGLMResponseBody> choices;
}

[Serializable]
public class ChatGLMResponseBody
{
    public PostDataBody message;
}

[Serializable]
public class PostData
{
    public string model = "charglm-3";
    public Meta meta;
    public List<PostDataBody> messages;
    // //描述：控制模型输出的随机性，范围通常在 0.0 到 1.0 之间。较高的值会使回复更随机、创造性更强，而较低的值则使回复更确定和保守。
    // public float temperature = 0.8f;
    // //描述：限制模型生成的回复中最大 token 数。这个字段可以控制生成回复的长度，避免生成过长的内容。
    // public int max_tokens = 100;
    // //描述：使用 nucleus 采样策略，top_p 控制选择的生成结果的累积概率。top_p 值越低，模型生成的内容越保守。
    // public float top_p = 0.9f;
}

[Serializable]
public class Meta
{
    public string user_info;
    public string bot_info;
    public string bot_name;
    public string user_name;
}

[Serializable]
public class PostDataBody
{
    public string role;
    public string content;

    public PostDataBody(string role, string content)
    {
        this.role = role;
        this.content = content;
    }
}
