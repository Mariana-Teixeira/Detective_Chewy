using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DialogueInvoker))]
public class DeductionFrameScript : MonoBehaviour
{
    public static Action<TalkToQuest> GetQuest;
    
    public Transform ClueGroup;
    
    private Image _image;

    public DialogueManager _dialogueManager;    
    private DialogueInvoker _invoker;
    private DialogueNode _currentNode;

    private TalkToQuest _quest;

    private Clue[] _clues;
    private List<string> _clueChecker;
    private Clue _clue;
    private Clue _nullClue;

    private void Awake()
    {
        _invoker = GetComponent<DialogueInvoker>();
        _image = GetComponent<Image>();
        _nullClue = new Clue();
    }

    private void Start()
    {
        GetQuest += SetupClues;
        _clueChecker = new List<string>();
    }

    public void SetupClues(TalkToQuest quest)
    {
        _quest = quest;
        _clues = quest.Items;

        foreach (var item in _clues)
        {
            _clueChecker.Add(item.ClueName);
            ClueGroup.Find(item.ClueName).gameObject.SetActive(true);
        }
    }

    public void OnClueChange(int i)
    {
        _clue = _clues[i];
        ChangeSprite();
    }

    public void ChangeSprite()
    {
        _image.sprite = _clue.ClueSprite;
    }

    public void OnObjection()
    {
        _currentNode = _dialogueManager.CurrentNode;

        if (_currentNode.Evidence == string.Empty)
        {
            Debug.Log("No Evidence Needed");
            return;
        }

        if (_currentNode.Evidence == _clue.ClueName)
        {
            Debug.Log("Correct");

            _clueChecker.Remove(_clue.ClueName);
            _dialogueManager.UpdateNode(_clue.DialogueNode);

            if (_clueChecker.Count <= 0)
            {
                FinishInterrogation();
            }
        }
        else
        {
            Debug.Log("Wrong Clue");
        }

        OnResetSelection();
    }

    public void OnResetSelection()
    {
        _clue = _nullClue;
        _image.sprite = null;
    }

    private void FinishInterrogation()
    {
        Debug.Log("Finish Interrogation");
        _invoker.SendDialogueBranch(_quest.ContinuingDialogue, true);
        PlayerStates.ChangeState?.Invoke(GameState.TALKING);
    }
}

[Serializable]
public struct Clue
{
    public string ClueName;
    public Sprite ClueSprite;
    public DialogueNode DialogueNode;
}