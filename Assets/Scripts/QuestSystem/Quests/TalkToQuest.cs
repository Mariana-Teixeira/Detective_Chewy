using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Talk To")]
public class TalkToQuest : Quest
{
    public string Character;
    public DialogueBranch Dialogue;
    [Space]
    public bool Interrogate;
    public Clue[] Items;
}
