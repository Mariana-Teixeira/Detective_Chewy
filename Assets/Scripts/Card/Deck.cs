using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Deck : MonoBehaviour
{
    [SerializeField] Board board;
    private List<CardData> _deck;

    private void Awake()
    {
        _deck = new List<CardData>();
    }

    public void InitDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            var suit = (Suit)i;
            for (int j = 0; j < 13; j++)
            {
                var value = j + 1;
                var score = value >= 10 ? 10 : value;

                var card = new CardData(suit, value, score);
                _deck.Add(card);
            }
        }

        //Randomize position of card in deck
        RandomInitOnBoard();
    }

    public void RandomInitOnBoard()
    {
        Random random = new Random();
        _deck = _deck.OrderBy(x => random.Next()).ToList();
        board.InstantiateCards(_deck);
    }

    public void OnRandomOnNewBoard()
    {
        board.CreateNewVersionOfDeck();
    }
}