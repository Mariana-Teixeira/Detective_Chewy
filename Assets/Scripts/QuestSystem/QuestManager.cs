using System;
using UnityEngine;

// Quite a bit of this code is messy and stupid, but it is very possible that we change the clue system,
// so I am currently not worried about it.
public class QuestManager : MonoBehaviour
{
    public static Action CompleteQuest;
    public static Func<string, Quest> CurrentQuest;

    public Quest[] Quests;
    private int QuestsIndex;

    private void Awake()
    {
        CompleteQuest += OnCompleteQuest;
        CurrentQuest += OnCurrentQuest;
    }

    private void Start()
    {
        InformationCanvasScript.UpdateQuestInformation?.Invoke(CurrentQuest?.Invoke(""));
    }

    private void OnCompleteQuest()
    {
        QuestsIndex++;
        InformationCanvasScript.UpdateQuestInformation?.Invoke(CurrentQuest?.Invoke(""));
    }

    // I'm not saying it's pretty, I'm saying it works for me.
    public Quest OnCurrentQuest(string type)
    {
        var Q = Quests[QuestsIndex];
        
        switch (type)
        {
            case "TalkTo":
                return (TalkToQuest)Q;
            case "CollectThing":
                return (CollectThingQuest)Q;
            case "PlayGame":
                return (PlayGameQuest)Q;
            default:
                return Q;
        }
    }
}