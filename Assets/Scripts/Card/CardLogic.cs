using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    #region PhaseText
    string PlayCards = "Play";
    string TradeCards = "Trade";
    string DiscardCard = "Discard";
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
    [SerializeField] coinScript _coinScript;
    private List<Card> cards;

    public int[] MatchesScoreObjective;
    private List<Vector3> pointsCardsPositions;
    private int _boardPointsCollected;

    private bool _gotSetThisTurn = false;
    private int lastAddedPoints = 0;
    public int _turnCounter;

    private bool tavernCardSelectedBuyPhase = false;
    private bool handCardSelectedBuyPhase = false;

    bool reachedFirstThreshold;
    bool reachedSecondThreshold;

    private void Awake()
    {
        #region UI Elements
        _nextPhaseButton.onClick.AddListener(ChangeToNextPhase);
        _confirmButton.onClick.AddListener(Confirm);

        _turnPhaseText.text = "DISCARD PHASE";
        _nextPhaseButton.interactable = false;
        #endregion

        cards = new List<Card> ();
        pointsCardsPositions = new List<Vector3>();

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

    public void ExitCardGame()
    {
        PlayerStates.ChangeState?.Invoke(GameState.SITTING);
        _gameBoard.ResetDeck();
        UnselectAllCards();
        StartNewBoard();
    }

    public void ChangeToNextPhase() 
    {
        if (currentTurnPhase == TurnPhase.Trade)
        {
            UnSelectTavernCardBuyPhase();
            UnSelectHandCardBuyPhase();
            currentTurnPhase = TurnPhase.Play;
            _turnPhaseText.text = PlayCards;
            ChangeTurnPhase(TurnPhase.Play);
            _coinScript.FlipTheCoin("sell");

            _errorText.gameObject.SetActive(false);
        }
        else if (currentTurnPhase == TurnPhase.Play)
        {
            currentTurnPhase = TurnPhase.Discard;
            _turnPhaseText.text = DiscardCard;
            ChangeTurnPhase(TurnPhase.Discard);
            _coinScript.FlipTheCoin("discard");

            if (pointsCardsPositions.Count > 0)
            { 
                _gameBoard.FinishTurn(pointsCardsPositions, new Vector3(0.0f, 0.26f, 0.97f)); // HARD CODED VALUE!!
                pointsCardsPositions.Clear();
            }

            _turnCounter++;
            Debug.Log("TURN: " +_turnCounter);
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
                _turnPhaseText.text = TradeCards;
                ChangeTurnPhase(TurnPhase.Trade);
                _coinScript.FlipTheCoin("buy");
                _nextPhaseButton.interactable = true;
                UnselectAllCards();
                _errorText.gameObject.SetActive(false);
            }
            else { 
                Debug.Log("Select more cards to confirm");
                _errorText.text = "Select more cards to confirm";
                _errorText.gameObject.SetActive(true);
            }
        }

        else if (currentTurnPhase == TurnPhase.Trade)
        {
            if (cards.Count == 2)
            {
                if ((cards[0].CardData.Position == Position.Tavern && cards[0].CardData.Value <= cards[1].CardData.Value)
                    ||
                    (cards[1].CardData.Position == Position.Tavern && cards[1].CardData.Value <= cards[0].CardData.Value))
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
                    _turnPhaseText.text = PlayCards;
                    ChangeTurnPhase(TurnPhase.Play);

                    _coinScript.FlipTheCoin("sell");

                    UnselectAllCards();
                    _errorText.gameObject.SetActive(false);
                }
                else
                { 
                    Debug.Log("Cant sell a card that is worth less then the one you are buying"); 
                    _errorText.text = "Cant sell a card that is worth less then the one you are buying";
                    _errorText.gameObject.SetActive(true);
                }
            }
            else
            { 
                Debug.Log("Select more cards to confirm or skip");
                _errorText.text = "Select more cards to confirm or skip";
                _errorText.gameObject.SetActive(true);
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
                    (((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1)-1)&&(tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2)-2)) 
                    ||
                    ((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1) - 11) && (tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2) - 12))
                    ||
                    ((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1) - 1) &&(tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2) - 12)))))
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
                        pointsCardsPositions.Add(card.transform.position);
                    }

                    _gameBoard.CollectPoints(cards);

                    UnselectAllCards();

                    _errorText.gameObject.SetActive(false);

                    //add points from previous SET if second SET was used during these turn
                    if (multiScore == 2)
                    {
                        _boardPointsCollected = _boardPointsCollected + lastAddedPoints;
                    }

                    _boardPointsCollected = _boardPointsCollected + Convert.ToInt32(Math.Floor(collectedPoints*multiScore));

                    lastAddedPoints = Convert.ToInt32(Math.Floor(collectedPoints * multiScore));

                    //_pointsText.text = "POINTS: " + _boardPointsCollected + " / " + MatchesScoreObjective[_gameBoard.GetActiveTable()];
                    _pointsText.text = _boardPointsCollected.ToString();
                    _pointsSlider.value = _boardPointsCollected;

                    #region Thresholds
                    if (_boardPointsCollected >= (MatchesScoreObjective[_gameBoard.GetActiveTable()] * 0.4f) && !reachedFirstThreshold)
                    {
                        CardGameState.ChangeGamePhase?.Invoke(GamePhase.First_Threshold);
                        reachedFirstThreshold = true;
                    };
                    if (_boardPointsCollected >= (MatchesScoreObjective[_gameBoard.GetActiveTable()] * 0.7f) && !reachedSecondThreshold)
                    {
                        CardGameState.ChangeGamePhase?.Invoke(GamePhase.Second_Threshold);
                        reachedSecondThreshold = true;
                    };
                    if (_boardPointsCollected >= MatchesScoreObjective[_gameBoard.GetActiveTable()])
                    {
                        GameWon();
                    };
                    #endregion
                }
                else
                { 
                    Debug.Log("You can not use those cards for points");
                    _errorText.text = "You can not use those cards for points";
                    _errorText.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Select more cards to confirm or skip");
                _errorText.text = "Select more cards to confirm or skip";
                _errorText.gameObject.SetActive(true);
            } 
        }
    }

    public void GameWon() 
    {
        Debug.Log("GAME WON");
        _gameBoard.GameWonGoNextTable();
        QuestManager.CompleteQuest?.Invoke();
        ExitCardGame();
    }

    public void GameOver() 
    {
        Debug.Log("GAME LOST");
        ExitCardGame();
    }

    public int SelectedCardsLength()
    {
        return cards.Count;
    }

    public void UnselectAllCards() {
        var tmpsCards = new List<Card>(cards);
        foreach (Card c in tmpsCards) {
            c.UnselectCard();
            }
    }

    public void StartNewBoard()
    {
        _boardPointsCollected = 0;
        _turnCounter = 1;

        //_pointsText.text = "POINTS: " + _boardPointsCollected + " / " + MatchesScoreObjective[_gameBoard.GetActiveTable()];
        _pointsText.text = _boardPointsCollected.ToString();
        _pointsSlider.value = _boardPointsCollected;

        currentTurnPhase = TurnPhase.Discard;
        _turnPhaseText.text = DiscardCard;
        _coinScript.FlipTheCoin("discard");

        _objectivePoints.text = MatchesScoreObjective[_gameBoard.GetActiveTable()].ToString();
        _pointsSlider.maxValue = MatchesScoreObjective[_gameBoard.GetActiveTable()];
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

    public int GetActiveTable() { 
        return _gameBoard.GetActiveTable();
    }

}

