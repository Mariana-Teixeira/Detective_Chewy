using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Collect Thing")]
public class CollectThingQuest : Quest
{
    public string Clue;
    public DialogueBranch Dialogue;
}