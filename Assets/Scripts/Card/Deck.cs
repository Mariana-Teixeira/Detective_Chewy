using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<CardData> _deck;
    private List<CardData> _hand;
    private List<CardData> _discards;

    private void Awake()
    {
        _deck = new List<CardData>();
        _hand = new List<CardData>();
        _discards = new List<CardData>();
    }

    private void Start()
    {
        PlayerHand.Draw += OnDraw;
        InitDeck();
    }

    private void InitDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            var suit = (Suit)i;
            for (int j = 0; j < 13; j++)
            {
                var value = j + 1;
                var card = new CardData(suit, value);
                _deck.Add(card);
            }
        }
    }

    private CardData[] OnDraw()
    {
        CardData[] cards =
        {
            _deck[0],
            _deck[1],
            _deck[2],
        };

        return cards;
    }

    private void DiscardCard(Card card)
    {
        throw new NotImplementedException();
    }
}
