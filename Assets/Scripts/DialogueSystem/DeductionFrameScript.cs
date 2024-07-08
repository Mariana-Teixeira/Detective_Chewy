using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DialogueInvoker))]
public class DeductionFrameScript : MonoBehaviour
{
    public static Action<TalkToQuest> GetQuest;
    
    public Transform ClueGroup;
    
    public Image _image;
    public TextMeshProUGUI _description;
    public TextMeshProUGUI _devDebug;

    private InterrogationCanvasScript _canvasScript;
    public DialogueManager _dialogueManager;    
    private DialogueInvoker _invoker;
    private DialogueNode _currentNode;

    private TalkToQuest _quest;

    private Clue[] _clues;
    private int _clueIndex;
    private List<string> _clueChecker;
    private Clue _clue;
    public Clue _nullClue;

    private Image[] _dots;
    public Transform DotParent;
    public GameObject Dot;

    private void Awake()
    {
        _invoker = GetComponent<DialogueInvoker>();
        _canvasScript = GetComponentInParent<InterrogationCanvasScript>();
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

        // Instatiate Dot and Dots Array
        _dots = new Image[quest.Items.Length];
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i] = Instantiate(Dot, DotParent).GetComponent<Image>();
            _dots[i].color = Color.green;
        }

        foreach (var item in _clues)
        {
            _clueChecker.Add(item.ClueName);
            ClueGroup.Find(item.ClueName).gameObject.SetActive(true);
        }
    }

    public void OnClueChange(int i)
    {
        _clueIndex = i;
        _clue = _clues[i];
        _image.sprite = _clue.ClueSprite;
        _description.text = _clue.ClueDescription;
    }

    public void OnObjection()
    {
        if (_clue.ClueName == "Null_Clue") return;

        _currentNode = _dialogueManager.CurrentNode;

        if (_currentNode.Evidence == string.Empty)
        {
            _canvasScript.ReactToClue(ClueState.WrongStatement);
            _devDebug.text = "Not a lie.";
            return;
        }

        if (_currentNode.Evidence == _clue.ClueName)
        {
            _canvasScript.ReactToClue(ClueState.RightClue);
            _devDebug.text = "You chose the correct clue!";

            _clueChecker.Remove(_clue.ClueName);
            _dialogueManager.UpdateNode(_clue.DialogueNode);

            _dots[_clueIndex].color = Color.white;

            if (_clueChecker.Count <= 0)
            {
                FinishInterrogation();
            }
        }
        else
        {
            _canvasScript.ReactToClue(ClueState.WrongClue);
            _devDebug.text = "That's the wrong clue.";
        }

        OnResetSelection();
    }

    public void OnResetSelection()
    {
        _clue = _nullClue;
        _image.sprite = _nullClue.ClueSprite;
        _description.text = string.Empty;
    }

    private void FinishInterrogation()
    {
        _invoker.SendDialogueBranch(_quest.ContinuingDialogue, true);
        PlayerStates.ChangeState?.Invoke(GameState.TALKING);
    }
}

[Serializable]
public struct Clue
{
    public string ClueName;
    [TextArea]
    public string ClueDescription;
    public Sprite ClueSprite;
    public DialogueNode DialogueNode;
}