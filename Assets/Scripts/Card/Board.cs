using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
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

    [SerializeField] coinScript coinScript;

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
    bool _setup = false;
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
        //CreateNewVersionOfDeck += OnCreateNewVersionOfDeck;
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
        _setup = false;
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
        float moveStep = 0;
        float moveStepY = 0.05f;
        float moveStepZ = 0.1f;
        int counter = 0;

        foreach (Card card in _allCardsList)
        {
            card.transform.position = _deckPos.transform.position;
        }


        if (num >= 1)
        {
            foreach (var card in _deck)
            {
                card.transform.position = _deckPos.transform.position;
                card.transform.Rotate(90, 0, 90);
            }
            moveStep = 0.65f;
            moveStepY = 0.02f;
            moveStepZ = 0.01f;
            foreach (var card in _hand)
            {
                StartCoroutine(Lerp(card.transform, _handPos.transform.position + new Vector3(moveStepY, moveStepZ, moveStep)));
                //card.transform.position = _handPos.transform.position + new Vector3(moveStepY, 0, moveStep);
                /*
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
                */
                moveStepZ = moveStepZ + 0.001f;
                moveStep = moveStep - 0.05f;
                counter++;

                //card.transform.Rotate(0, 0, 90);
            }
            moveStep = -0.15f;
            foreach (var card in _tavern)
            {
                StartCoroutine(Lerp(card.transform, _tavernPos.transform.position + new Vector3(-0.2f, 0, moveStep)));
                //card.transform.position = _tavernPos.transform.position + new Vector3(0, 0, moveStep);
                moveStep = moveStep + 0.1f;
                card.transform.Rotate(90, 0, 90);
            }
            foreach (var card in _discards)
            {
                StartCoroutine(Lerp(card.transform, _discardsPos.transform.position + new Vector3(0, -0.02f, -0.05f)));

                //card.transform.position = _discardsPos.transform.position;
                card.transform.Rotate(90, 0, 90);

            }

        }

        //----------------------------------------------------
        else
        {

            foreach (var card in _deck)
            {
                StartCoroutine(Lerp(card.transform, _deckPos.transform.position));
                //card.transform.position = _deckPos.transform.position;
                card.transform.Rotate(90, 0, 0);
            }
            moveStep = -0.2f;
            moveStepY = 0.07f;
            moveStepZ = 0.01f;
            foreach (var card in _hand)
            {
                StartCoroutine(Lerp(card.transform, _handPos.transform.position + new Vector3(moveStep, moveStepZ, moveStepY)));
                //card.transform.position = _handPos.transform.position + new Vector3(moveStep, 0, moveStepY);
                /*
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
                */
                moveStepZ = moveStepZ - 0.001f;
                moveStep = moveStep - 0.05f;
                counter++;

                card.transform.Rotate(0, 0, 90);
            }
            moveStep = -0.15f;
            foreach (var card in _tavern)
            {
                StartCoroutine(Lerp(card.transform, _tavernPos.transform.position + new Vector3(moveStep, 0, -0.15f)));
                //card.transform.position = _tavernPos.transform.position + new Vector3(moveStep, 0, 0);
                moveStep = moveStep + 0.1f;

                card.transform.Rotate(90, 0, 0);
                /*
                card.transform.Rotate(-35, 0, 0);
                card.transform.position = card.transform.position + new Vector3(0, 0.03f, 0);
                */

            }
            foreach (var card in _discards)
            {
                StartCoroutine(Lerp(card.transform, (_discardsPos.transform.position + new Vector3(0, 0.4f, -0.35f)))); 
                card.transform.Rotate(90, 0, 0);
            }
        }
        _setup = true;

    }
    public void PublicLerp(Transform card, Vector3 target) {
        StartCoroutine(Lerp(card, target));
    }
    //LERP FUNCTION OF CARDS
    IEnumerator Lerp(Transform card, Vector3 target)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        float timeElapsed = 0;
        float lerpDuration = 2;
        float z = 0.1f;

        Vector3 firstTarget = target;
        target = ((firstTarget + card.position) / 2f) + new Vector3(0, z / 2f, z);

        if (card.gameObject.GetComponent<Card>().CardData.Position == Position.Discard)
        {
            while (timeElapsed < lerpDuration)
            {
                card.position = Vector3.Lerp(card.position, firstTarget + new Vector3(0, 0.08f,0), timeElapsed / lerpDuration);
                card.transform.rotation = Quaternion.Lerp(card.transform.rotation, Quaternion.AngleAxis(180, Vector3.up), timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {

            while (timeElapsed < lerpDuration)
            {
                //adjust 25%
                if (card.gameObject.GetComponent<Card>().CardData.Position == Position.Hand)
                {
                    if (ActiveTable == 0)
                    {
                        card.transform.rotation = Quaternion.Lerp(card.transform.rotation, Quaternion.AngleAxis(-75, Vector3.left), timeElapsed / lerpDuration);
                    }
                    else
                    {
                        card.transform.rotation = Quaternion.identity;
                        card.transform.Rotate(0, 90, 0);
                        card.transform.Rotate(75, 0, 0);
                    }


                //card.transform.rotation = Quaternion.AngleAxis(-75, Vector3.left);
                }
            //

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
        if (card.gameObject.GetComponent<Card>().CardData.Position == Position.Hand)
        {
            card.position = firstTarget + new Vector3(0, 0.1f,0);
        }
        card.position = firstTarget;
        if (_setup == true) {
            //adjust cards that were selected to go back to the hand
            if (card.gameObject.GetComponent<Card>().CardData.Position == Position.Hand)
            {
                if (_activeTable == 0)
                {
                    card.position = firstTarget + new Vector3(0, -0.0091f, -0.035f);
                }
                else
                {
                    card.position = firstTarget + new Vector3(-0.035f, -0.0091f, 0);
                }
            }
            else
            {
                if (_activeTable == 0)
                {
                    card.position = firstTarget + new Vector3(0, 0.0091f, 0.035f);
                }
                else {
                    card.position = firstTarget + new Vector3(0.035f, 0.0091f, 0);
                }
            }
        }
 
        if (card.gameObject.GetComponent<Card>().CardData.Position == Position.Discard)
        {
            card.transform.rotation = Quaternion.AngleAxis(90, Vector3.left);
            if (_activeTable > 0) {
                card.transform.Rotate(0, 0, 90);
                card.transform.localPosition = card.transform.localPosition + new Vector3(-0.15f, 0, 0.15f);
            }
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    //DRAWING MECHANICS
    // ADD LERP FOR MOVING CARDS LATER ON
    public void DrawCard(Vector3 pos)  
    {
        _hand.Add(_deck.ElementAt(0));
        StartCoroutine(Lerp(_deck.ElementAt(0).gameObject.transform, pos));
        //_deck.ElementAt(0).gameObject.transform.position = pos;

        UpdatePosition(_deck.ElementAt(0), Position.Hand);
        _deck.RemoveAt(0);
    }
    public void DiscardCard(Card card)
    {
        _discards.Add(card);
        _hand.Remove(card);
        DrawCard(card.transform.position);
        UpdatePosition(card, Position.Discard);
        StartCoroutine(Lerp(card.gameObject.transform, _discardsPos.transform.position + new Vector3(0, (float)0.001 * _discards.Count(), 0) + new Vector3(0, -0.05f, -0.15f)));
        //card.gameObject.transform.position = _discardsPos.transform.position + new Vector3 (0, (float) 0.001*_discards.Count(),0);

 
    }

    public void ExchangeTavernCard(Card cardHand, Card cardTavern) 
    {
        //add from deck to tavern
        _tavern.Add(_deck.ElementAt(0));
        //_deck.ElementAt(0).gameObject.transform.position = cardTavern.transform.position;
        StartCoroutine(Lerp(_deck.ElementAt(0).gameObject.transform, cardTavern.transform.position));

        UpdatePosition(_deck.ElementAt(0), Position.Tavern);
        _deck.RemoveAt(0);


        //adds to hand from tavern
        _hand.Add(cardTavern);
        _tavern.Remove(cardTavern);
        //cardTavern.gameObject.transform.position = cardHand.transform.position;
        StartCoroutine(Lerp(cardTavern.gameObject.transform, cardHand.transform.position));

        UpdatePosition(cardTavern, Position.Hand);

        //discards from hand
        _discards.Add(cardHand);
        _hand.Remove(cardHand);
        //cardHand.gameObject.transform.position = _discardsPos.transform.position;

        UpdatePosition(cardHand, Position.Discard);
        StartCoroutine(Lerp(cardHand.gameObject.transform, _discardsPos.transform.position + new Vector3(0, -0.05f, -0.15f)));

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

    public void GameWonGoNextTable()
    {
        _activeTable = _activeTable + 1;
        if (_activeTable == 1) { coinScript.MoveToTable2(); }
        if (_activeTable == 2) { coinScript.MoveToTable3(); }

    }

    public Transform GetTavernPos()
    {
        return _tavernPos.transform;
    }

    public Transform GetDeckPos()
    {
        return _deckPos.transform;
    }

    public Transform GetDiscardPos()
    {
        return _discardsPos.transform;
    }

    public int GetActiveTable()
    {
        return _activeTable;
    }
}
