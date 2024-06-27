using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
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

    #region Scores
    public int[] MatchesScoreObjective;
    float firstThresholdPercentage = 0.4f;
    float secondThresholdPercentage = 0.7f;
    bool reachedFirstThreshold;
    bool reachedSecondThreshold;
    #endregion

    #region UI Elements
    [SerializeField] TextMeshProUGUI _turnPhaseText;
    [SerializeField] Slider _pointsSlider;
    [SerializeField] TextMeshProUGUI _objectivePoints;
    [SerializeField] TextMeshProUGUI _pointsText;
    [SerializeField] TextMeshProUGUI _errorText;

    [SerializeField] Canvas cardGameCanvas;
    [SerializeField] Button _nextPhaseButton;
    [SerializeField] Button _confirmButton;
    #endregion

    public static Action<TurnPhase> ChangeTurnPhase;
    private TurnPhase currentTurnPhase;

    [SerializeField] Board _gameBoard;
    [SerializeField] CoinScript _coinScript;
    private List<Card> cards;


    private List<CardPositionAndDirection> pointsCardsPositions;
    private int _boardPointsCollected;

    private bool _gotSetThisTurn = false;
    private int lastAddedPoints = 0;
    public int _turnCounter;

    private bool tavernCardSelectedBuyPhase = false;
    private bool handCardSelectedBuyPhase = false;

    private void Awake()
    {
        #region UI Elements
        _nextPhaseButton.onClick.AddListener(ChangeToNextPhase);
        _confirmButton.onClick.AddListener(Confirm);

        _turnPhaseText.text = DiscardText;
        _nextPhaseButton.interactable = false;
        #endregion

        cards = new List<Card> ();
        pointsCardsPositions = new List<CardPositionAndDirection>();

        _turnCounter = 1;
        currentTurnPhase = TurnPhase.Discard;

        StartNewBoard();
    }

    public void SelectCard(Card card) 
    {
        cards.Add(card);
    }

    public void UnselectCard(Card card) 
    {
        cards.Remove(card);
    }

    public void ChangeToNextPhase() 
    {
        if (currentTurnPhase == TurnPhase.Trade)
        {
            UnSelectTavernCardBuyPhase();
            UnSelectHandCardBuyPhase();
            currentTurnPhase = TurnPhase.Play;
            _turnPhaseText.text = PlayText;
            ChangeTurnPhase(TurnPhase.Play);
            _coinScript.FlipTheCoin("sell");

            _errorText.gameObject.SetActive(false);
        }

        else if (currentTurnPhase == TurnPhase.Play)
        {
            currentTurnPhase = TurnPhase.Discard;
            _turnPhaseText.text = DiscardText;
            ChangeTurnPhase(TurnPhase.Discard);
            _coinScript.FlipTheCoin("discard");

            if (pointsCardsPositions.Count > 0)
            {
                _gameBoard.FinishTurn(pointsCardsPositions);
                pointsCardsPositions.Clear();
            }

            _turnCounter++;
            Debug.Log("Current Turn: " + _turnCounter);
            _nextPhaseButton.interactable = false;
            _errorText.gameObject.SetActive(false);
            _gotSetThisTurn = false;

            if (_turnCounter > 5)
            {
                GameOver();
            }
        }

        UnselectAllCards();
    }

    public void Confirm() 
    {
        if (currentTurnPhase == TurnPhase.Discard)
        {
            if (cards.Count == 1)
            {
                _gameBoard.DiscardCard(cards[0]);
                currentTurnPhase = TurnPhase.Trade;
                _turnPhaseText.text = TradeText;
                ChangeTurnPhase(TurnPhase.Trade);
                _coinScript.FlipTheCoin("buy");
                _nextPhaseButton.interactable = true;
                UnselectAllCards();
            }
            else
            {
                // Didn't select enough cards.
                StartCoroutine(ErrorAppeared(InvalidSelectionByNumber, _errorMessageDuration));
            }
        }

        else if (currentTurnPhase == TurnPhase.Trade)
        {
            if (cards.Count == 2)
            {
                if (cards[0].CardData.Position == Position.Tavern)
                {
                    _gameBoard.ExchangeTavernCard(cards[1], cards[0]);
                }
                else
                {
                    _gameBoard.ExchangeTavernCard(cards[0], cards[1]);
                }
                    
                UnSelectTavernCardBuyPhase();
                UnSelectHandCardBuyPhase();
                currentTurnPhase = TurnPhase.Play;
                _turnPhaseText.text = PlayText;
                ChangeTurnPhase(TurnPhase.Play);

                _coinScript.FlipTheCoin("sell");

                UnselectAllCards();
            }
            else
            {
                // Didn't select enough cards.
                StartCoroutine(ErrorAppeared(InvalidSelectionByNumber, _errorMessageDuration));
            }
        }
        else if (currentTurnPhase == TurnPhase.Play)
        {
            int collectedPoints = 0;
            float multiScore = 1;
            List<int>  tmpNumList = new List<int>();

            if (cards.Count == 3)
            {
                tmpNumList.Add(cards[0].CardData.Value);
                tmpNumList.Add(cards[1].CardData.Value);
                tmpNumList.Add(cards[2].CardData.Value);
                tmpNumList.Sort();

                //CHECK IF 3 CARDS CAN BE USED FOR POINTS
                //CALCULATE POINTS AND GIVE THEM TO THE USER
                if (((cards[0].CardData.Value == cards[1].CardData.Value) && (cards[1].CardData.Value == cards[2].CardData.Value))
                    ||
                    (((cards[0].CardData.Suit == cards[1].CardData.Suit) && (cards[1].CardData.Suit == cards[2].CardData.Suit))
                    &&
                    (((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1) - 1) && (tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2) - 2))
                    ||
                    ((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1) - 11) && (tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2) - 12))
                    ||
                    ((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1) - 1) && (tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2) - 12)))))
                {
                    //Add flag that SET was converted to points this turn, if another is converted, set score to 2x
                    if (((cards[0].CardData.Value == cards[1].CardData.Value) && (cards[1].CardData.Value == cards[2].CardData.Value)))
                    {
                        multiScore = 1;

                        if (_gotSetThisTurn == true)
                        {
                            multiScore = 2;
                        }

                        if (_gotSetThisTurn == false)
                        {
                            _gotSetThisTurn = true;
                        }
                    }
                    //Otherwise set score to 1.5f for RUNS
                    else
                    {
                        multiScore = 1.5f;
                    }

                    //Add points of all cards
                    foreach (Card card in cards)
                    {
                        collectedPoints = collectedPoints + card.CardData.Score;
                        Vector3 cardPosition = card.transform.position;
                        Vector3 cardDirection = card.transform.up;
                        pointsCardsPositions.Add(new CardPositionAndDirection(cardPosition, cardDirection));
                    }

                    _gameBoard.CollectPoints(cards);

                    UnselectAllCards();

                    //add points from previous SET if second SET was used during these turn
                    if (multiScore == 2)
                    {
                        _boardPointsCollected = _boardPointsCollected + lastAddedPoints;
                    }

                    _boardPointsCollected = _boardPointsCollected + Convert.ToInt32(Math.Floor(collectedPoints * multiScore));
                    lastAddedPoints = Convert.ToInt32(Math.Floor(collectedPoints * multiScore));

                    // Update Score UI
                    _pointsText.text = _boardPointsCollected.ToString();
                    _pointsSlider.value = _boardPointsCollected;

                    #region Thresholds
                    var currentMatchScores = MatchesScoreObjective[_gameBoard.GetActiveTable()];
                    if (_boardPointsCollected >= (currentMatchScores * firstThresholdPercentage) && !reachedFirstThreshold)
                    {
                        reachedFirstThreshold = true;
                    };
                    if (_boardPointsCollected >= (currentMatchScores * secondThresholdPercentage) && !reachedSecondThreshold)
                    {
                        reachedSecondThreshold = true;
                    };
                    if (_boardPointsCollected >= currentMatchScores)
                    {
                        GameWon();
                    };
                    #endregion
                }
                else
                {
                    // Selected wrong cards to score.
                    StartCoroutine(ErrorAppeared(InvalidSelectionByType, _errorMessageDuration));
                }
            }
            else
            {
                // Didn't select enough cards.
                StartCoroutine(ErrorAppeared(InvalidSelectionByNumber, _errorMessageDuration));
            } 
        }
    }

    IEnumerator ErrorAppeared (string message, float displayTime)
    {
        var timeElapsed = 0f;

        foreach (var card in cards)
        {
            card.DenyCard();
        }

        _errorText.text = message;
        _errorText.enabled = true;
        while (timeElapsed < displayTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _errorText.enabled = false;
    }

    public void GameWon() 
    {
        PlayerStates.ChangeState?.Invoke(GameState.SITTING);
        _gameBoard.SetNextActiveTable();
        _gameBoard.ResetDeck();
        UnselectAllCards();
        StartNewBoard();
    }

    public void GameOver() 
    {
        PlayerStates.ChangeState?.Invoke(GameState.SITTING);
        _gameBoard.ResetDeck();
        UnselectAllCards();
        StartNewBoard();
    }

    public int SelectedCardsLength()
    {
        return cards.Count;
    }

    public void UnselectAllCards()
    {
        var tmpsCards = new List<Card>(cards);
        foreach (Card c in tmpsCards)
        {
            c.UnselectCard();
        }
    }

    public void StartNewBoard()
    {
        _boardPointsCollected = 0;
        _turnCounter = 1;

        _pointsText.text = _boardPointsCollected.ToString();
        _pointsSlider.value = _boardPointsCollected;

        currentTurnPhase = TurnPhase.Discard;
        _turnPhaseText.text = DiscardText;
        _coinScript.FlipTheCoin("discard");

        _objectivePoints.text = MatchesScoreObjective[_gameBoard.GetActiveTable()].ToString();
        _pointsSlider.maxValue = MatchesScoreObjective[_gameBoard.GetActiveTable()];

        pointsCardsPositions.Clear();
    }

    public void SelectHandCardBuyPhase()
    {
        handCardSelectedBuyPhase = true;
    }

    public void UnSelectHandCardBuyPhase()
    {
        handCardSelectedBuyPhase = false;
    }

    public void SelectTavernCardBuyPhase()
    {
        tavernCardSelectedBuyPhase = true; 
    }

    public void UnSelectTavernCardBuyPhase()
    {
        tavernCardSelectedBuyPhase = false;
    }

    public bool IsHandCardSelectedBuyPhase()
    {
        return handCardSelectedBuyPhase;
    }

    public bool IsTavernCardSelectedBuyPhase()
    {
        return tavernCardSelectedBuyPhase;
    }

}

public struct CardPositionAndDirection
{
    public CardPositionAndDirection(Vector3 position, Vector3 normal)
    {
        cardPosition = position;
        cardNormal = normal;
    }

    public Vector3 cardPosition;
    public Vector3 cardNormal;
}