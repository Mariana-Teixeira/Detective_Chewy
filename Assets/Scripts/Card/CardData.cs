using UnityEngine;

public enum Suit
{
    Heart = 0,
    Diamond = 1,
    Spades = 2,
    Clubs = 3
}

public enum Position
{
    Deck = 0,
    Hand = 1,
    Discard = 2,
    Tavern = 3
}
public class CardData
{
    public Suit Suit;
    public Position Position;
    public int Value;

    public CardData(Suit suit, int value)
    {
        Suit = suit;
        Value = value;
    }

    public override string ToString()
    {
        return $"[{Suit}, {Value}]";
    }

    public void setPosition(Position position)
    {
        this.Position = position;
    }
    public Position getPosition()
    {
        return this.Position;
    }
}
