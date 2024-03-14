using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Board : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject deck;
    public GameObject hand;
    public GameObject tavern;
    public GameObject discards;

    private List<Card> _deck;
    private List<Card> _hand;
    private List<Card> _tavern;
    private List<Card> _discards;

    private void Awake()
    {
        _deck = new List<Card>();
        _hand = new List<Card>();
        _tavern = new List<Card>();
        _discards = new List<Card>();
    }
    public void InstantiateCards(List<CardData> cards)
    {
        foreach (var card in cards)
        {
            Card cardComponent = Instantiate(cardPrefab, deck.transform).GetComponent<Card>();
            cardComponent.CardData = card;
            cardComponent.UpdateUI();
            _deck.Add(cardComponent);
        }
        CreateHand();
    }

    public void CreateHand()
    {
        for (int i = 0; i < 10; i++)
        {
            _hand.Add(_deck.ElementAt(0));
            _deck.RemoveAt(0);
        }
        CreateTavern();
    }

    public void CreateTavern()
    {
        for (int i = 0; i < 4; i++)
        {
            _tavern.Add(_deck.ElementAt(0));
            _deck.RemoveAt(0);
        }
        UpdatePositions();
    }

    public void UpdatePositions()
    {
        foreach (var card in _deck)
        {
            card.CardData.setPosition(Position.Deck);
            card.transform.parent = deck.transform;
            card.transform.transform.localPosition = Vector3.zero;
        }
        foreach (var card in _hand)
        {
            card.CardData.setPosition(Position.Hand);
            card.transform.parent = hand.transform;
            card.transform.transform.localPosition = Vector3.zero;
        }
        foreach (var card in _tavern)
        {
            card.CardData.setPosition(Position.Tavern);
            card.transform.parent = tavern.transform;
            card.transform.transform.localPosition = Vector3.zero;
        }
    }

    public void UpdatePosition(Card card, Position pos)
    {
        card.CardData.Position = pos;
    }

    public void DrawCard()  
    {
        _hand.Add(_deck.ElementAt(0));
        UpdatePosition(_deck.ElementAt(0), Position.Hand);
        _deck.RemoveAt(0);
    }
    public void DiscardCard(Card card)
    {
        _discards.Add(card);
        UpdatePosition(card, Position.Discard);
        _hand.Remove(card);
    }

    public void ExchangeTavernCard(Card cardHand, Card cardTavern) 
    {
        _discards.Add(cardHand);
        UpdatePosition(cardHand, Position.Discard);
        _hand.Remove(cardHand);
        _hand.Add(cardTavern);
        UpdatePosition(cardTavern, Position.Hand);
        _tavern.Remove(cardTavern);
        _tavern.Add(_deck.ElementAt(0));
        UpdatePosition(_deck.ElementAt(0), Position.Tavern);
        _deck.RemoveAt(0);
    }

    public void CollectPoints(Card[] cards) 
    { 
        foreach (Card card in cards) 
        {
            _discards.Add(card);
            UpdatePosition(card, Position.Discard);
            _hand.Remove(card);
        }
        DrawCard();
        DrawCard();
        DrawCard();
    }
}
