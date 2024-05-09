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

    private void Start()
    {
        CompleteQuest += OnCompleteQuest;
    }

    private void OnCompleteQuest()
    {
        QuestsIndex++;
    }
}