using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static Action CompleteQuest;
    public static Func<Quest> CurrentQuest;

    [SerializeField]
    private QuestOrder m_questOrder;
    private int m_questIndex;

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
        m_questIndex++;

        if (m_questIndex < m_questOrder.Quests.Length)
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

    public Quest OnCurrentQuest()
    {
        if (m_questIndex >= m_questOrder.Quests.Length) return null;

        var Q = m_questOrder.Quests[m_questIndex];
        return Q;
    }
}