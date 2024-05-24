using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Play Game")]
public class PlayGameQuest : Quest
{
    public string Game;
    public DialogueBranch FirstThresholdDialogue;
    public DialogueBranch SecondThresholdDialogue;
}