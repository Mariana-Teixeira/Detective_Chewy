using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Talk To")]
public class TalkToQuest : Quest
{
    public string Character;
    public DialogueBranch Dialogue;
    [Space(50)]
    public bool Interrogate;
    public DialogueBranch ContinuingDialogue;
    public Clue[] Items;
}
