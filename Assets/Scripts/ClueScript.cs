using System;
using UnityEngine;

[SelectionBase]
public class ClueScript : InteractableObject
{
    public static Action<Quest> EnableClue;
    public Target Target;
    public int ClueID;
    public Board GameBoard;

    private void Awake()
    {
        EnableClue += OnEnableClue;
    }

    private void Start()
    {
        base.SetCamera();
        base.SetOutline();
    }

    public void OnEnableClue(Quest Q)
    {
        if (Q.Target == this.Target)
        {
            Debug.Log("Quest Requirements Met");
            this.gameObject.SetActive(true);
        }
    }

    // Needs to run dialogue system and inspection system at the same time.
    public void GatherClue()
    {
        this.gameObject.SetActive(false);
        GameBoard.ClueFound(ClueID);
        QuestManager.CompleteQuest?.Invoke();
    }
}
