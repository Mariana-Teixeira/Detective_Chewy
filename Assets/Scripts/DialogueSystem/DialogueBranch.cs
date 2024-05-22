using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue Branch")]
public class DialogueBranch : ScriptableObject
{
    public DialogueNode[] Nodes;
}