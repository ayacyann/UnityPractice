using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Text Data", menuName = "ScriptableObjects/Text Data")]
public class TextDataSO : ScriptableObject
{
    public List<string> textData;
}
