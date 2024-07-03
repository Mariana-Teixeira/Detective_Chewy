using UnityEngine;

[CreateAssetMenu(menuName = "Branch/Dialogue")]
public class DialogueBranch : ScriptableObject
{
    public DialogueNode[] Nodes;
}