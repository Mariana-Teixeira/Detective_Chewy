using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    private List<Card> cards;
    private Phase currentPhase;

    [SerializeField] Canvas cardGameCanvas;
    [SerializeField] Button nextPhaseBtn;
    [SerializeField] Button confirmBtn;

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
        if (cards.Count == 3) 
        {
            Debug.Log("Do stuff");
            //Check if it can be used for points 
            // dont let player select more cards 

            foreach (Card c in cards) 
            {
                c.UnselectCard();
            }
            cards.Clear();
        }
    }

    public void UnselectCard(Card card) {
        cards.Remove(card);
    }

    public delegate void ChangePhase(Phase phase);
    public static event ChangePhase OnChangePhase;

    public void ChangeToNextPhase() 
    {
        if(currentPhase == Phase.Discard) {
            OnChangePhase(Phase.Buy);
        }
        else if(currentPhase == Phase.Buy)
        {
            OnChangePhase(Phase.Points);
        }
        else if (currentPhase == Phase.Points)
        {
            OnChangePhase(Phase.Discard);
        }
    }

    public void Confirm() { }


}
