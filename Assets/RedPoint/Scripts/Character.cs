using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Character 
{
    public CharacterInfo characterInfo;
    public List<PostDataBody> messages;
    public bool isMainCharacter = false;
    public Meta meta = null;

    private PostData GenerateChatGLMPost()
    {
        Character mainC = ChatGLMManager.Instance.mainCharacter;
        if(meta==null)
        {
            meta = new Meta()
            {
                user_name = mainC.characterInfo.name,
                user_info = GenerateInfo(mainC.characterInfo),
                bot_name = characterInfo.name,
                bot_info = GenerateInfo(characterInfo)
            };
        }
        PostData post = new PostData()
        {
            meta = this.meta,
            messages = this.messages
        };
        return post;
    }
    public string GenerateInfo(CharacterInfo charInfo)
    {
        string info = JsonUtility.ToJson(charInfo);
        return info;
    }

    public void SendMessage(string content,UnityAction<PostDataBody> callback)
    {
        if(messages==null)
        {
            messages = new List<PostDataBody>();
        }
        messages.Add(new PostDataBody("user", content));
        PostData post = GenerateChatGLMPost();
        ChatGLMManager.Instance.Send(post, (response) =>
        {
            Debug.Log(response);
            PostDataBody body = new PostDataBody("assistant", response);
            messages.Add(body);
            callback?.Invoke(body);
            ChatGLMManager.Instance.UpdateSO();
        });
    }
}

[Serializable]
public class CharacterInfo
{
    public string name;
    public bool isMainCharacter;
    public string worldSetting;
    public string gender;
    public int age;
    public string persenality;
    public List<string> hobbies;
    public string role;
    public string backstory;
}
