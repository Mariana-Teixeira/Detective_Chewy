using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue Branch")]
public class DialogueBranch : ScriptableObject
{
    public DialogueNode[] Nodes;
}

[Serializable]
public struct DialogueNode
{
    [TextArea(2, 10)]
    public string DialogueText;
    public SoundLibrary DialogueSound;
}

public enum SoundLibrary
{
    NULL
}