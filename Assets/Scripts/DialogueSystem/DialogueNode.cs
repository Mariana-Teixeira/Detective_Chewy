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
    public string DialogueText;
    public string DialogueSound;
}