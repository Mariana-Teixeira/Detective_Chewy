public enum Suit
{
    Heart = 0,
    Diamond = 1,
    Spades = 2,
    Clubs = 3
}
public class CardData
{
    public Suit Suit;
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
}
