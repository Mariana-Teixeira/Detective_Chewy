using UnityEngine;

[CreateAssetMenu(menuName = "Quest")]
public class Quest : ScriptableObject
{
    public Target Target;
    public int ActiveTable;
    public int ClueArray;
    public bool ClueFound;
    public DialogueBranch DialogueBranch;

    [Space(50)]
    [Multiline]
    public string QuestDescription;
}