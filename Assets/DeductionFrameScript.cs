using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeductionFrameScript : MonoBehaviour
{
    public static Action<TalkToQuest> GetQuest;
    
    private Image _image;

    public DialogueManager _dialogueManager;    
    private DialogueNode _currentNode;

    public Clue[] Clues;
    private List<string> _clueChecker;
    private Clue _clue;
    private Clue _nullClue;

    private void Awake()
    {
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
        Clues = quest.Items;

        foreach (var item in Clues)
        {
            _clueChecker.Add(item.ClueName);
        }
    }

    public void OnClueChange(int i)
    {
        _clue = Clues[i];
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