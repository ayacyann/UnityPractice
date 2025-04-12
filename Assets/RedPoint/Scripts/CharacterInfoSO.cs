using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "ScriptableObjects/CharacterInfo")]
public class CharacterInfoSO : ScriptableObject
{
    public List<CharacterInfo> characterInfos;
    public List<CharacterMessage> characterMessages;
    public List<Sprite> sprites;

    public Sprite FindSpriteFromName(string name)
    {
        int idx = characterInfos.FindIndex(x => x.name == name);
        return sprites[idx];
    }
    
    public void SaveMessageWithCharacter(Character character)
    {
        int idx = characterInfos.FindIndex(x => x.name == character.characterInfo.name);
        CharacterInfo CI = characterInfos[idx];
        characterMessages[idx].messages = character.messages;
        EditorUtility.SetDirty(this);
    }
    
    public CharacterInfo FindMainCharacter(out List<CharacterInfo> otherCharacters,out List<CharacterMessage> otherMessages)
    {
        otherCharacters = new List<CharacterInfo>();
        otherMessages = new List<CharacterMessage>();
        CharacterInfo maincharacter = null;
        for(int i = 0; i < characterInfos.Count; i++)
        {
            if(characterInfos[i].isMainCharacter)
            {
                maincharacter = characterInfos[i];
            }
            else
            {
                otherCharacters.Add(characterInfos[i]);
                otherMessages.Add(characterMessages[i]);
            }
        }
        return maincharacter;
    }
}

[Serializable]
public class CharacterMessage
{
    public List<PostDataBody> messages;
}
