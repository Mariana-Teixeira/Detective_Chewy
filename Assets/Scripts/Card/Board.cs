using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using System.Collections;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject deck;
    [SerializeField] GameObject hand;
    [SerializeField] GameObject tavern;
    [SerializeField] GameObject discards;
    [SerializeField] GameObject _DeckStartPosition;

    [SerializeField] List<GameObject> tables;
    private int _activeTable;

    private bool[] _cluesFound = new bool[3] {false, false, false};

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
        _activeTable = 0;
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
            card.transform.rotation = Quaternion.identity;
            card.transform.Rotate(90, 90, 0);
        }
    }

    public void CreateNewVerionOfDeck(TableScript table)
    {
        _deck.Clear();
        _hand.Clear();
        _tavern.Clear();
        _discards.Clear();
        Random random = new Random();
        _allCardsList = _allCardsList.OrderBy(x => random.Next()).ToList();
        foreach (Card card in _allCardsList) {
            _deck.Add(card);
        }
        /*
        int cnt = 0;

        foreach (GameObject go in tables)
        {
            if (go == table.gameObject) { _activeTable = cnt; }
            cnt++;
        }
        */
        CreateHand();
    }

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
        PlaceCardsInitial(_activeTable);
    }

    public void UpdatePosition(Card card, Position pos)
    {
        if (pos == Position.Discard)
        {
            card.CardData.Position = pos;
        }
        else {
            card.CardData.Position = pos;
        }
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

        foreach (Card card in _allCardsList) {
            card.transform.position = _deckPos.transform.position;
        }
        
        if (num == 1 || num == 2) {
            foreach (var card in _deck)
            {
                card.transform.position = _deckPos.transform.position;
            }
            moveStep = 0.2f;
            foreach (var card in _hand)
            {
                StartCoroutine(Lerp(card.transform, _handPos.transform.position + new Vector3(moveStepY, 0, moveStep)));
                //card.transform.position = _handPos.transform.position + new Vector3(moveStepY, 0, moveStep);
                if (counter == 4)
                {
                    moveStepY = +0.2f;
                    moveStep = moveStep + 0.1f;
                }
                if (counter < 4)
                {
                    moveStep = moveStep + 0.1f;
                }
                else
                {
                    moveStep = moveStep - 0.1f;
                }
                counter++;
            }
            moveStep = -0.15f;
            foreach (var card in _tavern)
            {
                StartCoroutine(Lerp(card.transform, _tavernPos.transform.position + new Vector3(0, 0, moveStep)));
                //card.transform.position = _tavernPos.transform.position + new Vector3(0, 0, moveStep);
                moveStep = moveStep + 0.1f;
            }
            foreach (var card in _discards)
            {
                StartCoroutine(Lerp(card.transform, _discardsPos.transform.position));
                //card.transform.position = _discardsPos.transform.position;
            }
        }
        //----------------------------------------------------
        else {
            foreach (var card in _deck)
            {
                StartCoroutine(Lerp(card.transform, _deckPos.transform.position));
                //card.transform.position = _deckPos.transform.position;
                card.transform.Rotate(0, 0, 90);
            }
            moveStep = -0.2f;
            foreach (var card in _hand)
            {
                StartCoroutine(Lerp(card.transform, _handPos.transform.position + new Vector3(moveStep, 0, moveStepY)));
                //card.transform.position = _handPos.transform.position + new Vector3(moveStep, 0, moveStepY);
                if (counter == 4)
                {
                    moveStepY = 0.2f;
                    moveStep = moveStep - 0.1f;
                }
                if (counter < 4)
                {
                    moveStep = moveStep - 0.1f;
                }
                else
                {
                    moveStep = moveStep + 0.1f;
                }
                counter++;

                card.transform.Rotate(0, 0, 90);
            }
            moveStep = -0.15f;
            foreach (var card in _tavern)
            {
                StartCoroutine(Lerp(card.transform, _tavernPos.transform.position + new Vector3(moveStep, 0, 0)));
                //card.transform.position = _tavernPos.transform.position + new Vector3(moveStep, 0, 0);
                moveStep = moveStep + 0.1f;

                card.transform.Rotate(0, 0, 90);
                /*
                card.transform.Rotate(-35, 0, 0);
                card.transform.position = card.transform.position + new Vector3(0, 0.03f, 0);
                */

            }
            foreach (var card in _discards)
            {

                card.transform.position = _discardsPos.transform.position;

                card.transform.Rotate(0, 0, 90);
            }
        }

    }

    //LERP FUNCTION OF CARDS
    IEnumerator Lerp(Transform card, Vector3 target) 
    {
        float timeElapsed = 0;
        float lerpDuration = 2;
        float z = 0.1f;

        Vector3 firstTarget = target;
        target = ((firstTarget + card.position) /2f) + new Vector3(0, z/2f, z);

        if (card.gameObject.GetComponent<Card>().CardData.Position == Position.Discard)
        {
            while (timeElapsed < lerpDuration)
            {
                card.position = Vector3.Lerp(card.position, firstTarget, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {

            while (timeElapsed < lerpDuration)
            {
                if (timeElapsed < lerpDuration / 2)
                {
                    card.position = Vector3.Lerp(card.position, target, timeElapsed / (lerpDuration * 2));
                }
                else
                {
                    card.position = Vector3.Lerp(card.position, firstTarget, (timeElapsed - 1) / (lerpDuration * 2));
                }

                timeElapsed += Time.deltaTime;

                yield return null;
            }
        }
        card.position = firstTarget;

    }

    //DRAWING MECHANICS
    // ADD LERP FOR MOVING CARDS LATER ON
    public void DrawCard(Vector3 pos)  
    {
        _hand.Add(_deck.ElementAt(0));
        UpdatePosition(_deck.ElementAt(0), Position.Hand);
        StartCoroutine(Lerp(_deck.ElementAt(0).gameObject.transform, pos));
        //_deck.ElementAt(0).gameObject.transform.position = pos;
        _deck.RemoveAt(0);
    }
    public void DiscardCard(Card card)
    {
        _discards.Add(card);
        UpdatePosition(card, Position.Discard);
        _hand.Remove(card);
        DrawCard(card.transform.position);
        StartCoroutine(Lerp(card.gameObject.transform, _discardsPos.transform.position + new Vector3(0, (float)0.001 * _discards.Count(), 0)));
        //card.gameObject.transform.position = _discardsPos.transform.position + new Vector3 (0, (float) 0.001*_discards.Count(),0);
    }

    public void ExchangeTavernCard(Card cardHand, Card cardTavern) 
    {
        //add from deck to tavern
        _tavern.Add(_deck.ElementAt(0));
        UpdatePosition(_deck.ElementAt(0), Position.Tavern);
        //_deck.ElementAt(0).gameObject.transform.position = cardTavern.transform.position;
        StartCoroutine(Lerp(_deck.ElementAt(0).gameObject.transform, cardTavern.transform.position));
        _deck.RemoveAt(0);


        //adds to hand from tavern
        _hand.Add(cardTavern);
        UpdatePosition(cardTavern, Position.Hand);
        _tavern.Remove(cardTavern);
        //cardTavern.gameObject.transform.position = cardHand.transform.position;
        StartCoroutine(Lerp(cardTavern.gameObject.transform, cardHand.transform.position));


        //discards from hand
        _discards.Add(cardHand);
        UpdatePosition(cardHand, Position.Discard);
        _hand.Remove(cardHand);
        //cardHand.gameObject.transform.position = _discardsPos.transform.position;
        StartCoroutine(Lerp(cardHand.gameObject.transform, _discardsPos.transform.position));
    }

    public void CollectPoints(List<Card> cards) 
    {
        foreach (Card card in cards) 
        {
            _discards.Add(card);
            _hand.Remove(card);
            StartCoroutine(Lerp(card.gameObject.transform, _discardsPos.transform.position));
            UpdatePosition(card, Position.Discard);
            //card.gameObject.transform.position = _discardsPos.transform.position;
        }

    }

    public void FinishTurn(List<Vector3> cards)
    {
        foreach (Vector3 card in cards)
        {
            DrawCard(card);
        }
    }

    public void ClueFound(int i) {
        _cluesFound[i] = true;
    }

    public bool CheckIfTablePlayable(int i)
    {
        if (i == _activeTable && _cluesFound[i]) { return true; }
        return false;
    }

    public void GameWonGoNextTable() {
        _activeTable = _activeTable + 1;
    }

    public Transform GetTavernPos() { return _tavernPos.transform; }
    public Transform GetDeckPos() { return _deckPos.transform; }
    public Transform GetDiscardPos() { return _discardsPos.transform; }

    public int GetActiveTable() { return _activeTable; }
}
