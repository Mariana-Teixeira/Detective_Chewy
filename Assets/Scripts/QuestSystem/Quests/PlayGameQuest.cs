using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Play Game")]
public class PlayGameQuest : Quest
{
    public string Game;
    public DialogueBranch StartingGameDialogue;
    public DialogueBranch FirstThresholdDialogue;
    public DialogueBranch SecondThresholdDialogue;
    public DialogueBranch WinningGameDialogue;
}