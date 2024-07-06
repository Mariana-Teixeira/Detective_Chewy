using System;
using UnityEngine;

[Serializable]
public struct DialogueNode
{
    [TextArea(2, 10)]
    public string DialogueText;
    public AudioClip DialogueSound;
    public string Evidence;
}