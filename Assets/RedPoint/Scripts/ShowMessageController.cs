using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessageController : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform parent;
    public Transform msgParent;
    public GameObject mainPrefab;
    public GameObject NPCprefab;
    public AppButtonClick ABC;
    public GameObject messageSend;
    public MessageButton currentButton;

    // Start is called before the first frame update
    void Start() 
    {
        InstantiateMessage(ChatGLMManager.Instance.otherCharacters);
        messageSend.SetActive(false);
    }

    public void GenerateMessage(PostDataBody msg)
    {
        GameObject go;
        Sprite sprite;
        if(msg.role == "user")
        {
            go = Instantiate(mainPrefab, msgParent);
            sprite = ChatGLMManager.Instance.characterInfoSO.sprites[0];
        }
        else
        {
            go = Instantiate(NPCprefab, msgParent);
            sprite = ChatGLMManager.Instance.characterInfoSO.FindSpriteFromName(ChatGLMManager.Instance.currentCharacter.characterInfo.name);
        }
        go.GetComponentInChildren<TMP_Text>().text = msg.content;
        go.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;
        if (msgParent.childCount > 7)
        {
            Destroy(msgParent.GetChild(0).gameObject);
        }
    }
    
    public void GenerateMessage(Character character)
    {
        for (int i = msgParent.childCount - 1; i >= 0 ; i--)
        {
            Destroy(msgParent.GetChild(i).gameObject);
        }
        int num = character.messages.Count-7;
        foreach(var msg in character.messages)
        {
            Sprite sprite = ChatGLMManager.Instance.characterInfoSO.FindSpriteFromName(character.characterInfo.name);
            if (num>0)
            {
                num--;
                continue;
            }
            GameObject go;
            if(msg.role == "user")
            {
                go = Instantiate(mainPrefab, msgParent);
                sprite = ChatGLMManager.Instance.characterInfoSO.sprites[0];
            }
            else
            {
                go = Instantiate(NPCprefab, msgParent);
            }
            go.GetComponentInChildren<TMP_Text>().text = msg.content;
            go.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;
        }
    }

    public void InstantiateMessage(List<Character> characters)
    {
        int currentNum = parent.childCount;
        while (currentNum < characters.Count)
        {
            Instantiate(messagePrefab, parent);
            currentNum += 1;
        }
        for (int i = 0; i < characters.Count; i++)
        {
            Character character = characters[i];
            MessageButton mb = parent.GetChild(i).GetComponent<MessageButton>();
            mb.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ChatGLMManager.Instance.characterInfoSO.sprites[i + 1];
            ABC.messageButtons.Add(mb);
            mb.Init(characters[i]);
            mb.gameObject.SetActive(true);
            Button button = mb.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                ChatGLMManager.Instance.SetCurrentCharacter(character);
                GenerateMessage(character);
                mb.numText.text = "0";
                mb.numParent.SetActive(false);
                ABC.UpdateNum();
                messageSend.SetActive(true);
                currentButton = mb;
            });
        }
    }
}
