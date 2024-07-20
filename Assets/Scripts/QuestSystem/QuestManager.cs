using System;
using UnityEngine;

// Quite a bit of this code is messy and stupid, but it is very possible that we change the clue system,
// so I am currently not worried about it.
public class QuestManager : MonoBehaviour
{
    public static Action CompleteQuest;
    public static Func<Quest> CurrentQuest;

    public Quest[] Quests;
    private int QuestsIndex;

    private void OnEnable()
    {
        CompleteQuest += OnCompleteQuest;
        CurrentQuest += OnCurrentQuest;
    }

    private void OnDisable()
    {
        CompleteQuest -= OnCompleteQuest;
        CurrentQuest -= OnCurrentQuest;
    }


    private void Start()
    {
        InformationCanvasScript.UpdateQuestInformation?.Invoke(CurrentQuest?.Invoke());
        ClueManager.InitQueue?.Invoke();
    }

    private void OnCompleteQuest()
    {
        QuestsIndex++;

        if (QuestsIndex < Quests.Length)
        {
            InformationCanvasScript.UpdateQuestInformation?.Invoke(CurrentQuest?.Invoke());
            ClueManager.InitQueue?.Invoke();
        }
        else
        {
            MenuCanvasScript.PlayTransition?.Invoke(TransitionState.FromGame);
            PlayerStates.ChangeState?.Invoke(GameState.NULL);
        }
    }

    // I'm not saying it's pretty, I'm saying it works for me.
    public Quest OnCurrentQuest()
    {
        if (QuestsIndex >= Quests.Length) return null;

        var Q = Quests[QuestsIndex];
        return Q;
    }
}