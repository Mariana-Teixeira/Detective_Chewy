using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    private List<Card> cards;
    private Phase currentPhase;

    public int[] pointsList;

    [SerializeField] Canvas cardGameCanvas;
    [SerializeField] Button nextPhaseBtn;
    [SerializeField] Button confirmBtn;
    [SerializeField] TextMeshProUGUI pointsText;

    private bool tavernCardSelectedBuyPhase = false;
    private bool handCardSelectedBuyPhase = false;


    private void Awake()
    {
        cards = new List<Card> ();
        currentPhase = Phase.Discard;
        nextPhaseBtn.onClick.AddListener(ChangeToNextPhase);
        confirmBtn.onClick.AddListener(Confirm);
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
        if(currentPhase == Phase.Discard) {
            currentPhase = Phase.Buy;
            OnChangePhase(Phase.Buy);
        }
        else if(currentPhase == Phase.Buy)
        {
            UnSelectTavernCardBuyPhase();
            UnSelectHandCardBuyPhase();
            currentPhase = Phase.Points;
            OnChangePhase(Phase.Points);
        }
        else if (currentPhase == Phase.Points)
        {   
            currentPhase = Phase.Discard;
            OnChangePhase(Phase.Discard);
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
                //
                currentPhase = Phase.Buy;
                OnChangePhase(Phase.Buy);

                UnselectAllCards();
            }
            else { } //ERROR MSG
        }
        else if (currentPhase == Phase.Buy)
        {
            if (cards.Count == 2)
            {
                //
                //CHECK IF HAND CARD VALUE >= TAVERN CARD VALUE
                //REPLACE HAND CARD WITH TAVERN CARD
                //REPLACE TAVERN CARD WITH DECK CARD
                //
                UnSelectTavernCardBuyPhase();
                UnSelectHandCardBuyPhase();
                currentPhase = Phase.Points;
                OnChangePhase(Phase.Points);

                UnselectAllCards();
            }
            else { } //ERROR MSG
        }
        else if (currentPhase == Phase.Points)
        {
            if (cards.Count == 3)
            {
                //
                //CHECK IF 3 CARDS CAN BE USED FOR POINTS
                //CALCULATE POINTS AND GIVE THEM TO THE USER
                //
                currentPhase = Phase.Discard;
                OnChangePhase(Phase.Discard);

                UnselectAllCards();
            }
            else { } //ERROR MSG
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

    public void SelectHandCardBuyPhase() { handCardSelectedBuyPhase = true; }
    public void UnSelectHandCardBuyPhase() { handCardSelectedBuyPhase = false; }
    public void SelectTavernCardBuyPhase() { tavernCardSelectedBuyPhase = true; }
    public void UnSelectTavernCardBuyPhase() { tavernCardSelectedBuyPhase = false; }
    public bool IsHandCardSelectedBuyPhase() { return handCardSelectedBuyPhase; }
    public bool IsTavernCardSelectedBuyPhase() { return tavernCardSelectedBuyPhase; }

}

