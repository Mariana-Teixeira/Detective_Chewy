using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static Action CompleteQuest;

    public Quest[] Quests;
    private int QuestsIndex;

    public Quest CurrentQuest
    {
        get
        {
            return Quests[QuestsIndex];
        }
    }

    private void Awake()
    {
        CompleteQuest += OnCompleteQuest;
    }

    private void Start()
    {
        InformationCanvasScript.UpdateQuestInformation?.Invoke(CurrentQuest);
        ClueScript.EnableClue?.Invoke(CurrentQuest);
    }

    // It's a bit annoying to call them this way, but it's the cleanest way I could think of on the fly.
    private void OnCompleteQuest()
    {
        QuestsIndex++;
        InformationCanvasScript.UpdateQuestInformation?.Invoke(CurrentQuest);
        ClueScript.EnableClue?.Invoke(CurrentQuest);
    }
}