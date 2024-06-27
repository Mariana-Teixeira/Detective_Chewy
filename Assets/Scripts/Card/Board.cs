using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using Random = System.Random;

public class Board : MonoBehaviour
{
    public static Action CreateNewVersionOfDeck;

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject deck;
    [SerializeField] GameObject hand;
    [SerializeField] GameObject tavern;
    [SerializeField] GameObject discards;
    [SerializeField] GameObject _DeckStartPosition;

    [SerializeField] CoinScript _coinScript;

    [SerializeField] InteractWith _interactWith;

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
        _activeTable = 0;
    }

    // I haven't figured out why this doesn't work when placed on Awaken! Maybe never will~ uuhh~
    private void Start()
    {
        CreateNewVersionOfDeck += OnCreateNewVersionOfDeck;
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
        _DeckStartPosition.transform.position = _deck.ElementAt(0).gameObject.transform.position;
        ResetDeck();
    }

    public void ResetDeck()
    {
        foreach (Card card in _allCardsList)
        {
            card.transform.position = _DeckStartPosition.transform.position;
            card.transform.rotation = _DeckStartPosition.transform.rotation;
            card.transform.Rotate(90, 90, 0);
        }
    }

    public void OnCreateNewVersionOfDeck()
    {
        _deck.Clear();
        _hand.Clear();
        _tavern.Clear();
        _discards.Clear();
        Random random = new Random();
        _allCardsList = _allCardsList.OrderBy(x => random.Next()).ToList();
        
        foreach (Card card in _allCardsList)
        {
            _deck.Add(card);
            card.transform.rotation = Quaternion.identity;
        }

        CreateHand();
        CreateTavern();
        UpdateFirstPositions();
        PlaceCardsInitial(_activeTable);
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
            card.transform.parent = hand.transform;
            card.transform.transform.localPosition = Vector3.zero;
        }

        foreach (var card in _tavern)
        {
            card.CardData.SetPosition(Position.Tavern);
            card.transform.parent = tavern.transform;
            card.transform.transform.localPosition = Vector3.zero;
        }
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

        float moveStep = 0f;
        float moveStepZ = 0f;

        int counter = 0;

        foreach (Card card in _allCardsList)
        {
            card.transform.position = _deckPos.transform.position;
        }

        foreach (var card in _deck)
        {
            card.transform.rotation = table.transform.rotation;
            card.transform.Rotate(90, 0, 0);
            card.transform.position = _deckPos.transform.position;
        }

        moveStep = -0.2f;
        moveStepZ = 0.01f;

        foreach (var card in _hand)
        {
            card.transform.rotation = table.transform.rotation;
            card.transform.Rotate(90, 0, 0);

            var newHandPosition = _handPos.transform.position +
                card.transform.right * moveStep + card.transform.forward * moveStepZ + card.transform.up * 0.0f;

            StartCoroutine(Lerp(card.transform, newHandPosition));

            moveStepZ += 0.001f;
            moveStep -= 0.05f;
            counter++;
        }

        moveStep = -0.15f;

        foreach (var card in _tavern)
        {
            card.transform.rotation = table.transform.rotation;
            card.transform.Rotate(90, 0, 0);

            var newTavernPosition = _tavernPos.transform.position +
                card.transform.right * moveStep + card.transform.forward * 0.0f + card.transform.up * 0.0f;

            StartCoroutine(Lerp(card.transform, newTavernPosition));

            moveStep = moveStep + 0.1f;
        }
    }

    IEnumerator Lerp(Transform card, Vector3 target, Vector3 movementVector = default)
    {
        var cardScript = card.gameObject.GetComponent<Card>();
        cardScript._canInteract = false;
        cardScript._isSelected = false;
        cardScript._isHovered = false;

        float timeElapsed = 0;
        float lerpDuration = 2;

        var HandRotation = -10f;
        var HandRotateTo = card.transform.rotation * Quaternion.Euler(HandRotation, 0f, 0f);

        var DiscardRotation = 180f - HandRotation;
        var DiscardRotateTo = card.transform.rotation * Quaternion.Euler(DiscardRotation, 0f, 0f);

        if (cardScript.CardData.Position == Position.Discard)
        {
            while (timeElapsed < lerpDuration)
            {
                card.transform.rotation = Quaternion.Lerp(card.transform.rotation, DiscardRotateTo, timeElapsed / lerpDuration);
                card.position = Vector3.Lerp(card.position, target, timeElapsed / lerpDuration);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

        }
        else
        {
            if (movementVector != Vector3.zero)
            {
                if (movementVector.y > 0)
                {
                    target -= movementVector * 0.035f;
                }
                else if (movementVector.y < 0)
                {
                    target += movementVector * 0.035f;
                }
            }

            while (timeElapsed < lerpDuration)
            {
                if (cardScript.CardData.Position == Position.Hand)
                {
                    card.transform.rotation = Quaternion.Lerp(card.transform.rotation, HandRotateTo, timeElapsed / lerpDuration);
                    card.position = Vector3.Lerp(card.position, target, timeElapsed / lerpDuration);
                }
                else
                {
                    card.position = Vector3.Lerp(card.position, target, (timeElapsed - 1) / (lerpDuration * 2));
                }

                timeElapsed += Time.deltaTime;

                yield return null;
            }
        }

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

    public void CollectPoints(List<Card> cards) 
    {
        foreach (Card card in cards) 
        {
            _discards.Add(card);
            _hand.Remove(card);

            UpdatePosition(card, Position.Discard);
            StartCoroutine(Lerp(card.gameObject.transform, _discardsPos.transform.position));
        }
    }

    public void FinishTurn(List<CardPositionAndNormal> cards)
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
}
