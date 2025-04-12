using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTree
{
    public string nodeName = "root";
    public Dictionary<string,MessageTree> childrens = new Dictionary<string, MessageTree>();
    public int messageNum;
    public MessageTree parent;

    public MessageTree()
    {

    }

    public MessageTree(string nodeName,MessageTree par)
    {
        this.nodeName = nodeName;
        parent = par;
    }
}
