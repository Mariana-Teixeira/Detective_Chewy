using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    private List<Card> cards;
    private Phase currentPhase;

    public int[] pointsList;
    [SerializeField] TextMeshProUGUI phaseText;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI errorText;
    private int _boardPointsCollected;
    private string _usualText = "POINTS: ";

    private bool _gotSetThisTurn = false;
    private int lastAddedPoints = 0;
    public int _turnCounter;

    private List<Vector3> pointsCardsPositions;

    [SerializeField] Canvas cardGameCanvas;
    [SerializeField] Button nextPhaseBtn;
    [SerializeField] Button confirmBtn;
    [SerializeField] Button exitBtn;
   

    [SerializeField] Board _gameBoard;

    private bool tavernCardSelectedBuyPhase = false;
    private bool handCardSelectedBuyPhase = false;


    private void Awake()
    {
        _turnCounter = 1;
        cards = new List<Card> ();
        pointsCardsPositions = new List<Vector3> ();
        currentPhase = Phase.Discard;
        nextPhaseBtn.onClick.AddListener(ChangeToNextPhase);
        confirmBtn.onClick.AddListener(Confirm);
        exitBtn.onClick.AddListener(ExitCardGame);
        StartNewBoard();
        phaseText.text = "DISCARD PHASE";
        nextPhaseBtn.interactable = false;
    }
    public void SelectCard(Card card) 
    {
        cards.Add(card);
    }

    public void UnselectCard(Card card) 
    {
        cards.Remove(card);
    }

    public void ExitCardGame() {
        PlayerStates.ChangeState?.Invoke(GameState.SITTING);
    }

    public delegate void ChangePhase(Phase phase);
    public static event ChangePhase OnChangePhase;

    public void ChangeToNextPhase() 
    {
        //SKIP PHASE
        /*
         * DONT LET PLAYER SKIP DISCARD PHASE
        if (currentPhase == Phase.Discard)
        {
            currentPhase = Phase.Buy;
            phaseText.text = "BUY PHASE";
            OnChangePhase(Phase.Buy);
        }
        */
        if (currentPhase == Phase.Buy)
        {
            UnSelectTavernCardBuyPhase();
            UnSelectHandCardBuyPhase();
            currentPhase = Phase.Points;
            phaseText.text = "POINTS PHASE";
            OnChangePhase(Phase.Points);
            errorText.gameObject.SetActive(false);
        }
        else if (currentPhase == Phase.Points)
        {
            currentPhase = Phase.Discard;
            phaseText.text = "DISCARD PHASE";
            OnChangePhase(Phase.Discard);
            if (pointsCardsPositions.Count > 0) { 
                _gameBoard.FinishTurn(pointsCardsPositions);
                pointsCardsPositions.Clear();
            }
            _turnCounter++;
            Debug.Log("TURN: " +_turnCounter);
            nextPhaseBtn.interactable = false;
            errorText.gameObject.SetActive(false);
            _gotSetThisTurn = false;
            if (_turnCounter > 5) { GameOver(); }
        }
        UnselectAllCards();
    }

    public void Confirm() 
    {
        if (currentPhase == Phase.Discard)
        {
            if (cards.Count == 1)
            {
                //
                //DISCARD CARD AND DRAW A NEW ONE IN ITS PLACE
                _gameBoard.DiscardCard(cards[0]);
                //
                currentPhase = Phase.Buy;
                phaseText.text = "BUY PHASE";
                OnChangePhase(Phase.Buy);
                nextPhaseBtn.interactable = true;
                UnselectAllCards();
                errorText.gameObject.SetActive(false);
            }
            else { 
                Debug.Log("Select more cards to confirm");
                errorText.text = "Select more cards to confirm";
                errorText.gameObject.SetActive(true);
            }
        }

        else if (currentPhase == Phase.Buy)
        {
            if (cards.Count == 2)
            {
                if ((cards[0].CardData.Position == Position.Tavern && cards[0].CardData.Value <= cards[1].CardData.Value)
                    ||
                    (cards[1].CardData.Position == Position.Tavern && cards[1].CardData.Value <= cards[0].CardData.Value))
                {
                    //
                    //DISCARD HAND CARD
                    //REPLACE HAND CARD WITH TAVERN CARD
                    //REPLACE TAVERN CARD WITH DECK CARD
                    //
                    if (cards[0].CardData.Position == Position.Tavern) { _gameBoard.ExchangeTavernCard(cards[1], cards[0]); }
                    else { _gameBoard.ExchangeTavernCard(cards[0], cards[1]); }
                    
                    UnSelectTavernCardBuyPhase();
                    UnSelectHandCardBuyPhase();
                    currentPhase = Phase.Points;
                    phaseText.text = "POINTS PHASE";
                    OnChangePhase(Phase.Points);

                    UnselectAllCards();
                    errorText.gameObject.SetActive(false);
                }
                else { 
                    Debug.Log("Cant sell a card that is worth less then the one you are buying"); 
                    errorText.text = "Cant sell a card that is worth less then the one you are buying";
                    errorText.gameObject.SetActive(true);
                }
            }
            else { 
                Debug.Log("Select more cards to confirm or skip");
                errorText.text = "Select more cards to confirm or skip";
                errorText.gameObject.SetActive(true);
            }
        }
        else if (currentPhase == Phase.Points)
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
                //
                //CHECK IF 3 CARDS CAN BE USED FOR POINTS
                //CALCULATE POINTS AND GIVE THEM TO THE USER
                //
                if (((cards[0].CardData.Value == cards[1].CardData.Value) && (cards[1].CardData.Value == cards[2].CardData.Value))
                    ||
                    (((cards[0].CardData.Suit == cards[1].CardData.Suit) && (cards[1].CardData.Suit == cards[2].CardData.Suit))
                    &&
                    (((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1)-1)&&(tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2)-2)) 
                    ||
                    ((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1) - 11) && (tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2) - 12))
                    ||
                    ((tmpNumList.ElementAt(0) == tmpNumList.ElementAt(1) - 1) &&(tmpNumList.ElementAt(0) == tmpNumList.ElementAt(2) - 12))
                    )
                    )
                    )
                {
                    //Add flag that SET was converted to points this turn, if another is converted, set score to 2x
                    if (((cards[0].CardData.Value == cards[1].CardData.Value) && (cards[1].CardData.Value == cards[2].CardData.Value))) { multiScore = 1;
                        if (_gotSetThisTurn == true) { multiScore = 2; }
                        if (_gotSetThisTurn == false) { _gotSetThisTurn = true; }
                    }
                    //Otherwise set score to 1.5f for RUNS
                    else { multiScore = 1.5f; }

                    //Add points of all cards
                    foreach (Card card in cards) {
                        collectedPoints = collectedPoints + card.CardData.Score; 
                        pointsCardsPositions.Add(card.transform.position);
                    }


                    _gameBoard.CollectPoints(cards);
                    UnselectAllCards();
                    errorText.gameObject.SetActive(false);

                    //add points from previous SET if second SET was used during these turn
                    if (multiScore == 2)
                    {
                        _boardPointsCollected = _boardPointsCollected + lastAddedPoints;
                    }
                    _boardPointsCollected = _boardPointsCollected + Convert.ToInt32(Math.Floor(collectedPoints*multiScore));

                    lastAddedPoints = Convert.ToInt32(Math.Floor(collectedPoints * multiScore));
    
                    pointsText.text = _usualText + _boardPointsCollected + " / " + pointsList[0];

                    //GAME WON IF POINTS OVER NEEDED POINTS
                    // CHANGE 0 DEPENDING ON TABLES
                    if (_boardPointsCollected >= pointsList[0]) 
                    {
                        GameWon();
                    };
                }
                else { 
                    Debug.Log("You can not use those cards for points");
                    errorText.text = "You can not use those cards for points";
                    errorText.gameObject.SetActive(true);
                }
            }
            else { Debug.Log("Select more cards to confirm or skip");
                errorText.text = "Select more cards to confirm or skip";
                errorText.gameObject.SetActive(true);
            } 
        }
    }

    public void GameWon() 
    {
        Debug.Log("GAME WON");
    }

    public void GameOver() 
    {
        Debug.Log("GAME LOST");
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

    public void StartNewBoard() {
        _boardPointsCollected = 0;
        _turnCounter = 1;
    }
    public void SelectHandCardBuyPhase() { handCardSelectedBuyPhase = true; }
    public void UnSelectHandCardBuyPhase() { handCardSelectedBuyPhase = false; }
    public void SelectTavernCardBuyPhase() { tavernCardSelectedBuyPhase = true; }
    public void UnSelectTavernCardBuyPhase() { tavernCardSelectedBuyPhase = false; }
    public bool IsHandCardSelectedBuyPhase() { return handCardSelectedBuyPhase; }
    public bool IsTavernCardSelectedBuyPhase() { return tavernCardSelectedBuyPhase; }

}

