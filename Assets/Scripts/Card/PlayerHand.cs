using System;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private Board _board;
    public static Func<CardData[]> Draw;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }

    public void DrawCard()
    {
        var cards = Draw.Invoke();
        _board.InstantiateCards(cards);
    }
}
