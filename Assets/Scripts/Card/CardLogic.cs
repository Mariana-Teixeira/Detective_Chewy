using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class CardLogic : MonoBehaviour
{
    public CardGameCanvasScript GameCanvas;

    #region Scores
    public bool UseMultiplier
    {
        get
        {
            return _gameBoard.GetActiveTableLogic.UseMultiplier;
        }
    }
    public bool UseTimer
    {
        get
        {
            return _gameBoard.GetActiveTableLogic.UseTimer;
        }
    }
    private int _matchPoint;
    public int CurrentMatchObjective
    {
        get
        {
            return _matchPoint;
        }
        set
        {
            _matchPoint = value;
        }
    }
    public float CurrentMatchTime
    {
        get
        {
            return _gameBoard.GetActiveTableLogic.MatchTime;
        }
    }
    #endregion

    #region Multipliers
    public int BoardPointsCollected;
    public int NumberOfSetsThisTurn;
    public float BaseValue;
    public float Multiplier = 1f;
    public float RunMultiplier = 1.5f;
    public int DebtMultiplier = 10;
    #endregion

    #region Timer
    public bool TimerOn;
    float currentTime;
    #endregion

    public static Action<TurnPhase> ChangeTurnPhase;
    private TurnPhase currentTurnPhase;

    [SerializeField] Board _gameBoard;
    [SerializeField] CoinScript _coinScript;
    private List<Card> cards;

    public int DeckNumber
    {
        get
        {
            return _gameBoard.DeckNumber;
        }
    }

    private List<CardPositionAndDirection> playCardPositions;

    private bool tavernCardSelectedBuyPhase = false;
    private bool handCardSelectedBuyPhase = false;

    public Animator _animator;
    private bool _hasSeenTutorial;

    public GameObject _lastHovered;

    private void Awake()
    {
        cards = new List<Card> ();
        playCardPositions = new List<CardPositionAndDirection>();

        currentTurnPhase = TurnPhase.Discard;
    }

    private void Update()
    {
        if(Gamepad.current != null)
        { 
            //test for controller click
            if ((Input.GetButtonDown("Fire2") || Gamepad.current.rightTrigger.isPressed) && _lastHovered!=null)
            {
             _lastHovered.GetComponent<Card>().OnMouseDown();
            }
        }
    }

    public void Reposition()
    {
        var currentTable = _gameBoard.GetActiveTableLogic._tableObject;
        this.transform.position = currentTable.transform.position;
        this.transform.position += Vector3.up * 0.81f;
        UnselectAllCards();
    }

    public void SelectCard(Card card) 
    {
        cards.Add(card);
    }

    public void UnselectCard(Card card) 
    {
        cards.Remove(card);
    }

    public void ShowTutorial()
    {
        if (_hasSeenTutorial) return;

        if (currentTurnPhase == TurnPhase.Trade)
        {
            CardGameState.ChangeGamePhase?.Invoke(GamePhase.Tutorial);
        }
        else if (currentTurnPhase == TurnPhase.Play)
        {
            CardGameState.ChangeGamePhase?.Invoke(GamePhase.Tutorial);
            _hasSeenTutorial = true;
        }
    }

    public void EnterDiscard()
    {
        _coinScript.FlipTheCoin("discard");

        currentTurnPhase = TurnPhase.Discard;
        GameCanvas.ChangeTurn(TurnPhase.Discard);

        if (playCardPositions.Count > 0)
        {
            _gameBoard.FinishTurn(playCardPositions);
            playCardPositions.Clear();
        }
    }

    public void EnterTrade()
    {
        _coinScript.FlipTheCoin("sell");

        currentTurnPhase = TurnPhase.Trade;
        GameCanvas.ChangeTurn(TurnPhase.Trade);

    }

    public void EnterPlay()
    {
        _coinScript.FlipTheCoin("buy");

        currentTurnPhase = TurnPhase.Play;
        GameCanvas.ChangeTurn(TurnPhase.Play);
    }

    public void OnChangeTurnPhase() 
    {
        if (currentTurnPhase == TurnPhase.Discard)
        {
            ChangeTurnPhase?.Invoke(TurnPhase.Trade);
            EnterTrade();
            ShowTutorial();
        }
        else if (currentTurnPhase == TurnPhase.Trade)
        {
            ChangeTurnPhase?.Invoke(TurnPhase.Play);
            EnterPlay();
            ShowTutorial();
        }
        else // Play
        {
            ChangeTurnPhase?.Invoke(TurnPhase.Discard);
            EnterDiscard();
        }

        UnselectAllCards();
    }

    public void OnConfirm() 
    {
        if (currentTurnPhase == TurnPhase.Discard)
        {
            DiscardCards();
        }
        else if (currentTurnPhase == TurnPhase.Trade)
        {
            TradeCards();
        }
        else // Play
        {
            PlayCards();
        }

        UnSelectTavernCardBuyPhase();
        UnSelectHandCardBuyPhase();
        UnselectAllCards();
    }

    public void ResetMultiplier()
    {
        NumberOfSetsThisTurn = 0;
        BaseValue = 0f;
        Multiplier = 0f;
        GameCanvas.ResetPointDisplay();
    }

    private void DiscardCards()
    {
        if (!CheckSelectionNumber(1)) return;

        // Discard Multiplier
        if (_gameBoard.GetActiveTableLogic.UseMultiplier)
        {
            Multiplier = cards[0].CardData.Value;
            GameCanvas.UpdateDiscardMultiplier(Multiplier);
        }

        // Debt Multiplier
        if (_gameBoard.GetActiveTableLogic.UseDiscardDebt)
        {
            int acumulatedDebt = cards[0].CardData.Value * DebtMultiplier;
            _matchPoint += acumulatedDebt;

            GameCanvas.UpdateMatchPoints();
        }

        _gameBoard.DiscardCard(cards[0]);

        OnChangeTurnPhase();
    }

    private void TradeCards()
    {
        if (!CheckSelectionNumber(2)) return;

        if (cards[0].CardData.Position == Position.Tavern)
        {
            _gameBoard.ExchangeTavernCard(cards[1], cards[0]);
        }
        else
        {
            _gameBoard.ExchangeTavernCard(cards[0], cards[1]);
        }

        OnChangeTurnPhase();
    }

    private void PlayCards()
    {
        if (!CheckSelectionNumber(3)) return;

        #region Calculating Values
        var card1 = cards[0].CardData;
        var card2 = cards[1].CardData;
        var card3 = cards[2].CardData;
        bool equalSuit = card1.Suit == card2.Suit && card2.Suit == card3.Suit;
        bool differentSuit = card1.Suit != card2.Suit && card2.Suit != card3.Suit;
        bool equalValue = card1.Value == card2.Value && card2.Value == card3.Value;
        bool runValue = AreTheyInOrder(card1.Value, card2.Value, card3.Value);
        bool set = equalValue;
        bool run = equalSuit && runValue;
        #endregion

        if (!set && !run)
        {
            // Error by type
            return;
        }

        if (set || run)
        {
            foreach (Card card in cards)
            {
                BaseValue += card.CardData.Score;
                playCardPositions.Add(new CardPositionAndDirection(card.transform.position, card.transform.up));
            }
            _gameBoard.MoveCardsFromPlay(cards);

            if (set)
            {
                NumberOfSetsThisTurn++;
                Multiplier *= NumberOfSetsThisTurn;
                GameCanvas.UpdateDiscardMultiplier(Multiplier);
            }

            if (run)
            {
                Multiplier *= RunMultiplier;
                GameCanvas.UpdateDiscardMultiplier(Multiplier);
            }

            // Collect Points
            if (_gameBoard.GetActiveTableLogic.UseMultiplier)
            {
                float points = Multiplier * BaseValue;
                BoardPointsCollected += (int)points;
            }
            else
            {
                BoardPointsCollected += (int)BaseValue;
            }

            GameCanvas.UpdateTotalPoints(BoardPointsCollected);
            ResetMultiplier();

            if (BoardPointsCollected >= _matchPoint) GameCanvas.CallStatusWindow(true, 3f);
        }
    }

    private bool AreTheyInOrder(params int[] values)
    {
        values = BubbleSort(values);

        // Looping "EdgeCase"
        if (values[0] == 1 && values[1] == 2 && values[2] == 13 ||
            values[0] == 1 && values[1] == 12 && values[2] == 13) return true;

        if (NextToEachOther(values[0], values[1], values[2])) return true;
        else return false;
    }

    private int[] BubbleSort(int[] values)
    {
        for (int i = 0; i < values.Length - 1; i++)
        {
            for (int j = 0; j < values.Length - i - 1; j++)
            {
                if (values[j] > values[j + 1])
                {
                    var v = values[j];
                    values[j] = values[j + 1];
                    values[j + 1] = v;
                }
            }
        }
        return values;
    }

    private bool NextToEachOther(int a, int b, int c)
    {
        return MathF.Abs(a - b) == 1 && MathF.Abs(b - c) == 1;
    }

    public bool CheckSelectionNumber(int checkTo)
    {
        if (cards.Count == checkTo)
        {
            return true;
        }
        else
        {
            GameCanvas.CallError(ErrorType.WrongNumber, cards);
            return false;
        }
    }

    public void GameWon() 
    {
        StopCoroutine("StartTimer");
        TimerOn = false;
        ResetGame();
        _gameBoard.SetNextActiveTable();
    }

    public void GameOver() 
    {
        StopCoroutine("StartTimer");
        TimerOn = false;
        ResetGame();
    }

    public void ResetGame()
    {
        UnselectAllCards();
        _gameBoard.ResetDeck();
        StartNewBoard();
    }

    public int SelectedCardsLength()
    {
        return cards.Count;
    }

    public void UnselectAllCards()
    {
        var tmpsCards = new List<Card>(cards);
        foreach (Card c in tmpsCards)
        {
            c.UnselectCard();
        }

        UnSelectTavernCardBuyPhase();
        UnSelectHandCardBuyPhase();
    }

    public void StartNewBoard()
    {
        BoardPointsCollected = 0;
        currentTurnPhase = TurnPhase.Discard;

        GameCanvas.ResetCanvas();
        playCardPositions.Clear();
    }

    public IEnumerator StartTimer(float t)
    {
        TimerOn = true;
        currentTime = t;

        while (currentTime > 0)
        {
            GameCanvas.TickTimerText(currentTime.ToString("000"));
            currentTime -= Time.deltaTime;
            yield return null;
        }

        GameCanvas.CallStatusWindow(false, 3f);
    }

    public void SelectHandCardBuyPhase()
    {
        handCardSelectedBuyPhase = true;
    }

    public void UnSelectHandCardBuyPhase()
    {
        handCardSelectedBuyPhase = false;
    }

    public void SelectTavernCardBuyPhase()
    {
        tavernCardSelectedBuyPhase = true; 
    }

    public void UnSelectTavernCardBuyPhase()
    {
        tavernCardSelectedBuyPhase = false;
    }

    public bool IsHandCardSelectedBuyPhase()
    {
        return handCardSelectedBuyPhase;
    }

    public bool IsTavernCardSelectedBuyPhase()
    {
        return tavernCardSelectedBuyPhase;
    }
}

public struct CardPositionAndDirection
{
    public CardPositionAndDirection(Vector3 position, Vector3 normal)
    {
        cardPosition = position;
        cardNormal = normal;
    }

    public Vector3 cardPosition;
    public Vector3 cardNormal;
}
