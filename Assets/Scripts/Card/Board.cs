using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject deck;
    [SerializeField] GameObject hand;
    [SerializeField] GameObject tavern;
    [SerializeField] GameObject discards;

    [SerializeField] List<GameObject> tables;

    private List<Card> _allCardsList;
    private List<Card> _deck;
    private List<Card> _hand;
    private List<Card> _tavern;
    private List<Card> _discards;

    private GameObject _deckPos;
    private GameObject _handPos;
    private GameObject _tavernPos;
    private GameObject _discardsPos;

    private void Awake()
    {
        _allCardsList = new List<Card>();
        _deck = new List<Card>();
        _hand = new List<Card>();
        _tavern = new List<Card>();
        _discards = new List<Card>();
    }
    //CREATE CARDS
    public void InstantiateCards(List<CardData> cards)
    {
        foreach (var card in cards)
        {
            Card cardComponent = Instantiate(cardPrefab, deck.transform).GetComponent<Card>();
            cardComponent.CardData = card;
            cardComponent.UpdateUI();
            _deck.Add(cardComponent);
            _allCardsList.Add(cardComponent);
        }
        CreateHand();
    }

    public void CreateNewVerionOfDeck() {  }//TO DO

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
        UpdateFirstPositions();
    }

    public void UpdateFirstPositions()
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
        //TEST
        PlaceCardsInitial(0);
    }

    public void UpdatePosition(Card card, Position pos)
    {
        card.CardData.Position = pos;
    }

    //PLACE CARDS ON TABLE NUM
    public void PlaceCardsInitial(int num)
    {
        GameObject table = tables.ElementAt(num);
        _deckPos = table.transform.GetChild(1).GetChild(0).gameObject;
        _handPos = table.transform.GetChild(1).GetChild(1).gameObject;
        _tavernPos = table.transform.GetChild(1).GetChild(2).gameObject;
        _discardsPos = table.transform.GetChild(1).GetChild(3).gameObject;
        float moveStep = 0;
        float moveStepY = 0;
        int counter = 0;
        foreach (var card in _deck)
        {
            card.transform.position = _deckPos.transform.position;
        }
        moveStep = 0.1f;
        foreach (var card in _hand)
        {
            card.transform.position = _handPos.transform.position + new Vector3(moveStepY, 0, moveStep);
            if (counter == 4) 
            {
                moveStepY = +0.2f;
                moveStep = moveStep + 0.15f;
            }
            if (counter < 4)
            {
                moveStep = moveStep + 0.15f;
            }
            else 
            {
                moveStep = moveStep - 0.15f;
            }
            counter++;
        }
        moveStep = -0.15f;
        foreach (var card in _tavern)
        {
            card.transform.position = _tavernPos.transform.position + new Vector3(0, 0, moveStep);
            moveStep = moveStep + 0.1f;
        }
        foreach(var card in _discards)
        {
            card.transform.position = _discardsPos.transform.position;
        }
    }

    //DRAWING MECHANICS
    // ADD LERP FOR MOVING CARDS LATER ON
    public void DrawCard(Vector3 pos)  
    {
        _hand.Add(_deck.ElementAt(0));
        UpdatePosition(_deck.ElementAt(0), Position.Hand);
        _deck.ElementAt(0).gameObject.transform.position = pos;
        _deck.RemoveAt(0);
    }
    public void DiscardCard(Card card)
    {
        _discards.Add(card);
        UpdatePosition(card, Position.Discard);
        _hand.Remove(card);
        DrawCard(card.transform.position);
        card.gameObject.transform.position = _discardsPos.transform.position;
    }

    public void ExchangeTavernCard(Card cardHand, Card cardTavern) 
    {
        //add from deck to tavern
        _tavern.Add(_deck.ElementAt(0));
        UpdatePosition(_deck.ElementAt(0), Position.Tavern);
        _deck.ElementAt(0).gameObject.transform.position = cardTavern.transform.position;
        _deck.RemoveAt(0);

        //adds to hand from tavern
        _hand.Add(cardTavern);
        UpdatePosition(cardTavern, Position.Hand);
        _tavern.Remove(cardTavern);
        cardTavern.gameObject.transform.position = cardHand.transform.position;

        //discards from hand
        _discards.Add(cardHand);
        UpdatePosition(cardHand, Position.Discard);
        _hand.Remove(cardHand);
        cardHand.gameObject.transform.position = _discardsPos.transform.position;
    }

    public void CollectPoints(List<Card> cards) 
    {
        foreach (Card card in cards) 
        {
            _discards.Add(card);
            UpdatePosition(card, Position.Discard);
            _hand.Remove(card);
            card.gameObject.transform.position = _discardsPos.transform.position;
        }

    }

    public void FinishTurn(List<Vector3> cards)
    {
        foreach (Vector3 card in cards)
        {
            DrawCard(card);
        }
    }

    public Transform GetTavernPos() { return _tavernPos.transform; }
    public Transform GetDeckPos() { return _deckPos.transform; }
    public Transform GetDiscardPos() { return _discardsPos.transform; }
}
