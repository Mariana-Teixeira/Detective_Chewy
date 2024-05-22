using System;
using UnityEngine;

[Serializable]
public struct DialogueNode
{
    [TextArea(2, 10)]
    public string DialogueText;
    public string DialogueSound;
    public string Evidence;
}