using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ErrorType
{
    WrongSelection,
    WrongNumber
}

public class CardGameCanvasScript : MonoBehaviour
{
    public CardLogic _logic;

    #region PhaseText
    const string PlayText = "Play";
    const string TradeText = "Trade";
    const string DiscardText = "Discard";
    #endregion

    #region ErrorMessages
    float _errorMessageDuration = 3.0f;
    const string InvalidSelectionByType = "Your selection is invalid.";
    const string InvalidSelectionByNumber = "Wrong number of cards selected";
    #endregion

    public static Action<bool> ToggleVisibility;
    private Canvas _cardGameCanvas;

    #region UI Elements
    [SerializeField] TextMeshProUGUI _turnPhaseText;

    [SerializeField] TextMeshProUGUI _currentPointsText;
    [SerializeField] TextMeshProUGUI _objectivePoints;
    [SerializeField] Slider _pointsSlider;
        
    [SerializeField] TextMeshProUGUI _errorText;
    
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] TextMeshProUGUI _baseScoreText;
    [SerializeField] TextMeshProUGUI _multiplierText;
    [SerializeField] TextMeshProUGUI _deckNumberText;

    [SerializeField] Button _nextPhaseButton;
    [SerializeField] Button _confirmButton;

    [SerializeField] CanvasGroup StatusWindow;
    TextMeshProUGUI _statusText;

    AudioSource _audioSource;
    [SerializeField] AudioClip _winningGame;
    [SerializeField] AudioClip _loosingGame;
    #endregion

    private void Awake()
    {
        _statusText = StatusWindow.GetComponentInChildren<TextMeshProUGUI>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _cardGameCanvas = GetComponent<Canvas>();
        ToggleVisibility += OnToggleVisibility;
    }

    public void ResetCanvas()
    {
        _deckNumberText.text = _logic.DeckNumber.ToString();

        _currentPointsText.text = _logic.BoardPointsCollected.ToString();
        _pointsSlider.value = _logic.BoardPointsCollected;

        _objectivePoints.text = _logic.CurrentMatchObjective.ToString();
        _pointsSlider.maxValue = _logic.CurrentMatchObjective;

        _turnPhaseText.text = DiscardText;
        _nextPhaseButton.interactable = false;

        _timerText.text = "";

        ResetPointDisplay();
    }

    public void ResetPointDisplay()
    {
        _multiplierText.text = "1";
        _baseScoreText.text = "0";
    }

    public void TickTimerText(string time)
    {
        _timerText.text = time;
    }

    public void UpdateDeckNumber(int n)
    {
        _deckNumberText.text = n.ToString();
    }

    public void UpdateObjectivePoints()
    {
        _objectivePoints.text = _logic.CurrentMatchObjective.ToString();
        _pointsSlider.maxValue = _logic.CurrentMatchObjective;
    }

    public void UpdateTotalPoints(int points)
    {
        _pointsSlider.value = points;
        _currentPointsText.text = points.ToString();
    }

    public void UpdateBaseScore(float score)
    {
        _baseScoreText.text = score.ToString();
    }

    public void UpdateMultiplier(float score)
    {
        _multiplierText.text = score.ToString();
    }

    public void OnToggleVisibility(bool isVisible)
    {
        _cardGameCanvas.enabled = isVisible;
    }

    public void ChangeTurn(TurnPhase phase)
    {
        switch(phase)
        {
            case TurnPhase.Discard:
                _turnPhaseText.text = DiscardText;
                _nextPhaseButton.interactable = false;
                break;
            case TurnPhase.Trade:
                _turnPhaseText.text = TradeText;
                _nextPhaseButton.interactable = true;
                break;
            case TurnPhase.Play:
                _turnPhaseText.text = PlayText;
                break;
        }
    }

    public void CallWinCanvas()
    {
        _audioSource.clip = _winningGame;
        _statusText.text = "Win!";
        _audioSource.Play();
    }

    public void CallLoseCanvas()
    {
        _audioSource.clip = _loosingGame;
        _statusText.text = "Lost.";
        _audioSource.Play();
    }

    public void ToggleStatusWindows(bool visible)
    {
        if (visible)
        {
            StatusWindow.alpha = 1;
            StatusWindow.interactable = true;
            StatusWindow.blocksRaycasts = true;
        }
        else
        {
            StatusWindow.alpha = 0;
            StatusWindow.interactable = false;
            StatusWindow.blocksRaycasts = false;
        }
    }

    public void CallError(ErrorType type, List<Card> cards)
    {
        switch (type)
        {
            case ErrorType.WrongSelection:
                StartCoroutine(ErrorAppeared(InvalidSelectionByType, cards));
                break;
            case ErrorType.WrongNumber:
                StartCoroutine(ErrorAppeared(InvalidSelectionByNumber, cards));
                break;
        }
    }

    IEnumerator ErrorAppeared(string message, List<Card> cards)
    {
        var timeElapsed = 0f;

        foreach (var card in cards)
        {
            card.DenyAnimation();
        }

        _errorText.text = message;
        _errorText.gameObject.SetActive(true);
        while (timeElapsed < _errorMessageDuration)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _errorText.gameObject.SetActive(false);
    }
}
