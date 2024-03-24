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
    private int _boardPointsCollected;
    private string _usualText = "POINTS: ";
    public int _turnCounter;

    private List<Vector3> pointsCardsPositions;

    [SerializeField] Canvas cardGameCanvas;
    [SerializeField] Button nextPhaseBtn;
    [SerializeField] Button confirmBtn;
   

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
        StartNewBoard();
        phaseText.text = "DISCARD PHASE";
    }
    public void SelectCard(Card card) 
    {
        cards.Add(card);
    }

    public void UnselectCard(Card card) 
    {
        cards.Remove(card);
    }

    public delegate void ChangePhase(Phase phase);
    public static event ChangePhase OnChangePhase;

    public void ChangeToNextPhase() 
    {
        //SKIP PHASE
        if (currentPhase == Phase.Discard)
        {
            currentPhase = Phase.Buy;
            phaseText.text = "BUY PHASE";
            OnChangePhase(Phase.Buy);
        }
        else if (currentPhase == Phase.Buy)
        {
            UnSelectTavernCardBuyPhase();
            UnSelectHandCardBuyPhase();
            currentPhase = Phase.Points;
            phaseText.text = "POINTS PHASE";
            OnChangePhase(Phase.Points);
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

                UnselectAllCards();
            }
            else { Debug.Log("Select more cards to confirm or skip"); }
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
                }
                else { Debug.Log("Cant sell a card that is worth less then the one you are buying"); }
            }
            else { Debug.Log("Select more cards to confirm or skip"); }
        }
        else if (currentPhase == Phase.Points)
        {
            int collectedPoints = 0;
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
                    foreach (Card card in cards) {
                        collectedPoints = collectedPoints + card.CardData.Value; 
                        pointsCardsPositions.Add(card.transform.position);
                    }

                    _gameBoard.CollectPoints(cards);

                    UnselectAllCards();

                    _boardPointsCollected = _boardPointsCollected + collectedPoints;
                    pointsText.text = _usualText + _boardPointsCollected + " / " + pointsList[0];
                }
                else { Debug.Log("You can not use those cards for points"); }
            }
            else { Debug.Log("Select more cards to confirm or skip"); } 
        }
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

