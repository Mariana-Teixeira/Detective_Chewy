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
    public int Score;

    public CardData(Suit suit, int value, int score)
    {
        Suit = suit;
        Value = value;
        Score = score;
    }

    public override string ToString()
    {
        return $"[{Suit}, {Value}]";
    }

    public void SetPosition(Position position)
    {
        this.Position = position;
    }

    public Position GetPosition()
    {
        return this.Position;
    }
}
