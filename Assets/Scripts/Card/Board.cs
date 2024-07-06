using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Board : MonoBehaviour
{
    bool hasSpawnedCards = false;
    public float duration = 1.0f;

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject _deckStartPosition;
    [SerializeField] GameObject deck;

    [SerializeField] CoinScript _coinScript;
    [SerializeField] CardLogic _cardLogic;

    [SerializeField] List<GameObject> tables;
    [SerializeField] int _activeTable;

    private List<Card> _allCardsList;
    private List<Card> _deck;
    private List<Card> _hand;
    private List<Card> _tavern;
    private List<Card> _discards;

    private GameObject _deckPos;
    private GameObject _handPos;
    private GameObject _tavernPos;
    private GameObject _discardsPos;

    WaitForSeconds _cardHandWaitTime;
    WaitForSeconds _cardTableWaitTime;
    float _handWaitTime = 0.05f;
    float _tableWaitTime = 0.02f;

    private bool[] _seenTutorialsByTable;

    public int ActiveTable
    {
        get
        {
            return _activeTable;
        }
    }

    private void Awake()
    {
        _allCardsList = new List<Card>();
        _deck = new List<Card>();
        _hand = new List<Card>();
        _tavern = new List<Card>();
        _discards = new List<Card>();
        _seenTutorialsByTable = new bool[tables.Count];
        _activeTable = 0;

        _cardHandWaitTime = new WaitForSeconds(_handWaitTime);
        _cardTableWaitTime = new WaitForSeconds(_tableWaitTime);
    }

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
        _deckStartPosition.transform.position = _deck.ElementAt(0).gameObject.transform.position;
        ResetDeck();
    }

    public void ResetDeck()
    {
        foreach (Card card in _allCardsList)
        {
            card.transform.position = _deckStartPosition.transform.position;
            card.transform.rotation = _deckStartPosition.transform.rotation;
            card.transform.Rotate(90, 90, 0);
        }
    }

    public void CreateNewVersionOfDeck()
    {
        // Not a clean way to do code. If the cards have not been spawned, I spawn them here.
        if (!hasSpawnedCards) SpawnCards();

        ResetLists();
        AddCardsToDeck();

        CreateHand();
        CreateTavern();
        UpdateFirstPositions();
        StartCoroutine(PlaceCards());
    }

    public void SpawnCards()
    {
        var deck = this.transform.parent.GetComponentInChildren<Deck>();
        deck.InitializeDeck();
        hasSpawnedCards = true;

        _coinScript.gameObject.SetActive(true);
    }

    public void ResetLists()
    {
        _deck.Clear();
        _hand.Clear();
        _tavern.Clear();
        _discards.Clear();
        Random random = new Random();
        _allCardsList = _allCardsList.OrderBy(x => random.Next()).ToList();
    }

    public void AddCardsToDeck()
    {
        foreach (Card card in _allCardsList)
        {
            _deck.Add(card);
            card.transform.rotation = Quaternion.identity;
        }
    }

    public void CreateHand()
    {
        for (int i = 0; i < 10; i++)
        {
            _hand.Add(_deck.ElementAt(0));
            _deck.RemoveAt(0);
        }
    }

    public void CreateTavern()
    {
        for (int i = 0; i < 4; i++)
        {
            _tavern.Add(_deck.ElementAt(0));
            _deck.RemoveAt(0);
        }
    }

    public void UpdateFirstPositions()
    {
        foreach (var card in _deck)
        {
            card.CardData.SetPosition(Position.Deck);
            card.transform.parent = deck.transform;
            card.transform.transform.localPosition = Vector3.zero;
        }

        foreach (var card in _hand)
        {
            card.CardData.SetPosition(Position.Hand);
            card.transform.parent = deck.transform;
            card.transform.transform.localPosition = Vector3.zero;
        }

        foreach (var card in _tavern)
        {
            card.CardData.SetPosition(Position.Tavern);
            card.transform.parent = deck.transform;
            card.transform.transform.localPosition = Vector3.zero;
        }
    }

    public void UpdatePosition(Card card, Position pos)
    {
        card.CardData.Position = pos;
    }

    //PLACE CARDS ON TABLE NUM
    IEnumerator PlaceCards()
    {
        GameObject table = tables.ElementAt(_activeTable);
        _deckPos = table.transform.GetChild(1).GetChild(0).gameObject;
        _handPos = table.transform.GetChild(1).GetChild(1).gameObject;
        _tavernPos = table.transform.GetChild(1).GetChild(2).gameObject;
        _discardsPos = table.transform.GetChild(1).GetChild(3).gameObject;

        float moveStep = 0f;
        float moveStepZ = 0f;

        foreach (Card card in _allCardsList)
        {
            card.transform.position = _deckPos.transform.position;
            card.transform.rotation = table.transform.rotation;
            card.transform.Rotate(90, 0, 0);
        }

        moveStep = -0.2f;
        moveStepZ = 0.01f;

        // Place Cards in Hand
        yield return StartCoroutine(LerpCardToHand(moveStep, moveStepZ));

        moveStep = -0.15f;

        // Place Cards on Tavern
        yield return StartCoroutine(LerpCardToTavern(moveStep));

        // Waiting Time For Cards to Be Positioned
        var waitTime = _handWaitTime * 10f + _tableWaitTime * 4f;
        yield return new WaitForSeconds(waitTime);

        // Display Proper Tutorial
        DisplayTutorial();
    }

    public void DisplayTutorial()
    {
        if (!_seenTutorialsByTable[ActiveTable])
        {
            CardGameState.ChangeGamePhase?.Invoke(GamePhase.Tutorial);
            _seenTutorialsByTable[ActiveTable] = true;
        }
        else
        {
            CardGameState.ChangeGamePhase?.Invoke(GamePhase.Play);
        }
    }

    IEnumerator LerpCardToHand(float moveStep, float moveStepZ)
    {
        foreach (var card in _hand)
        {
            var newHandPosition = _handPos.transform.position +
                card.transform.right * moveStep + card.transform.forward * moveStepZ + card.transform.up * 0.0f;

            StartCoroutine(Lerp(card.transform, newHandPosition));
            yield return _cardHandWaitTime;

            moveStepZ += 0.0005f;
            moveStep -= 0.05f;
        }
    }

    IEnumerator LerpCardToTavern(float moveStep)
    {
        foreach (var card in _tavern)
        {
            var newTavernPosition = _tavernPos.transform.position +
                card.transform.right * moveStep + card.transform.forward * 0.0f + card.transform.up * 0.0f;

            StartCoroutine(Lerp(card.transform, newTavernPosition));
            yield return _cardTableWaitTime;

            moveStep = moveStep + 0.1f;
        }
    }

    IEnumerator Lerp(Transform card, Vector3 target, Vector3 movementVector = default)
    {
        var cardScript = card.gameObject.GetComponent<Card>();
        cardScript._canInteract = false;
        cardScript._isSelected = false;
        cardScript._isHovered = false;

        var HandRotation = -10f;
        var HandRotateTo = card.transform.rotation * Quaternion.Euler(HandRotation, 0f, 0f);

        var DiscardRotation = 0f + HandRotation;
        var DiscardRotateTo = card.transform.rotation * Quaternion.Euler(DiscardRotation, 180f, 0f);

        var amount = cardScript.CardHoverAmount + cardScript.CardSelectAmount;

        // This one can also be refactor to make mor sense, but I'll keep it for now.
        if (movementVector != Vector3.zero)
        {
            if (movementVector.y > 0)
            {
                target -= movementVector * amount;
            }
            else if (movementVector.y < 0)
            {
                target += movementVector * amount;
            }
        }

        // I mostly changed this function, because it was easier to do math if I lerped between fixed values.
        Vector3 initialPosition = card.position;
        Quaternion initialRotation = card.rotation;
        float timeElapsed = 0;
        var t = 0f;
        do
        {
            t = Mathf.Lerp(0, 1, BadMath.LerpOutSmooth(timeElapsed, duration));

            if (cardScript.CardData.Position == Position.Discard)
            {
                card.rotation = Quaternion.Lerp(initialRotation, DiscardRotateTo, t);
            }
            else if (cardScript.CardData.Position == Position.Hand)
            {
                card.rotation = Quaternion.Lerp(initialRotation, HandRotateTo, t);
            }

            card.position = Vector3.Lerp(initialPosition, target, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        } while (timeElapsed < duration);

        if (cardScript.CardData.Position == Position.Discard) card.rotation = DiscardRotateTo;
        else if (cardScript.CardData.Position == Position.Hand) card.rotation = HandRotateTo;
        card.position = target;

        cardScript._canInteract = true;
    }

    public void DrawCard(Vector3 position, Vector3 movementVector = default)  
    {
        _hand.Add(_deck.ElementAt(0));
        StartCoroutine(Lerp(_deck.ElementAt(0).gameObject.transform, position, movementVector));

        UpdatePosition(_deck.ElementAt(0), Position.Hand);
        _deck.RemoveAt(0);
    }

    public void DiscardCard(Card card)
    {
        _discards.Add(card);
        _hand.Remove(card);

        DrawCard(card.transform.position, card.transform.up);
        UpdatePosition(card, Position.Discard);
        
        StartCoroutine(Lerp(card.gameObject.transform, _discardsPos.transform.position));
    }

    public void ExchangeTavernCard(Card cardHand, Card cardTavern) 
    {
        // From Deck to Tavern
        _tavern.Add(_deck.ElementAt(0));

        StartCoroutine(Lerp(_deck.ElementAt(0).gameObject.transform, cardTavern.transform.position, cardTavern.transform.forward));
        UpdatePosition(_deck.ElementAt(0), Position.Tavern);

        _deck.RemoveAt(0);

        // From Tavern to Hand
        _hand.Add(cardTavern);
        _tavern.Remove(cardTavern);

        StartCoroutine(Lerp(cardTavern.gameObject.transform, cardHand.transform.position, cardHand.transform.up));

        UpdatePosition(cardTavern, Position.Hand);

        // From Hand to Discard
        _discards.Add(cardHand);
        _hand.Remove(cardHand);

        UpdatePosition(cardHand, Position.Discard);

        StartCoroutine(Lerp(cardHand.gameObject.transform, _discardsPos.transform.position));

    }

    public void MoveCardsFromPlay(List<Card> cards) 
    {
        foreach (Card card in cards) 
        {
            _discards.Add(card);
            _hand.Remove(card);

            UpdatePosition(card, Position.Discard);
            StartCoroutine(Lerp(card.gameObject.transform, _discardsPos.transform.position));
        }
    }

    public void FinishTurn(List<CardPositionAndDirection> cards)
    {
        foreach (var card in cards)
        {
            DrawCard(card.cardPosition, card.cardNormal);
        }
    }

    public void SetNextActiveTable()
    {
        _activeTable += 1;
        _coinScript.MoveToTable(_activeTable);
    }

    public int GetActiveTable()
    {
        return _activeTable;
    }

    public GameObject GetActiveTableObject()
    {
        return tables[_activeTable];
    }
}
