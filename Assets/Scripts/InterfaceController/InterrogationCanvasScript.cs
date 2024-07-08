using System;
using UnityEngine;
using UnityEngine.UI;

public enum ClueState
{
    RightClue,
    WrongClue,
    WrongStatement,
    NullClue
}

public class InterrogationCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;

    private CanvasGroup _canvas;
    private Animator _animator;

    public Image FlushyExpression;

    public Sprite SmugFlushy;
    public Sprite NervousFlushy;

    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ToggleVisibility += OnToggleVisibility;
    }

    public void OnToggleVisibility(bool isVisible)
    {
        if (isVisible)
        {
            _canvas.alpha = 1f;
            _canvas.interactable = true;
            _canvas.blocksRaycasts = true;
        }
        else
        {
            _canvas.alpha = 0f;
            _canvas.interactable = false;
            _canvas.blocksRaycasts = false;
        }
    }

    public void ReactToClue(ClueState state)
    {
        if(state == ClueState.RightClue)
        {
            FlushyExpression.sprite = NervousFlushy;
        }
        else
        {
            FlushyExpression.sprite = SmugFlushy;
        }

        _animator.SetTrigger("Click_Clue");
    }
}
