using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Talk To")]
public class TalkToQuest : Quest
{
    public string Character;
    public DialogueBranch Dialogue;
}
