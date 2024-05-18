using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardData _cardData;
    private Outline _outline;

    private CardLogic _cardLogic;

    private bool _selected = false;

    [SerializeField] List<Material> materials;
    [SerializeField] GameObject mesh;
    public TurnPhase Phase;

    public static Action<bool> ToggleInteraction;
    bool _canInteract;
    public void OnToggleInteraction(bool isInteractable)
    {
        _canInteract = isInteractable;
    }


    public CardData CardData
    {
        get
        {
            return _cardData;
        }
        set
        {
            _cardData = value;
        }
    }
    private void Awake()
    {
        _cardLogic = FindFirstObjectByType<CardLogic>();
        _outline = GetComponent<Outline>();
    }

    private void Start()
    {
        CardLogic.ChangeTurnPhase += OnChangePhase;
        ToggleInteraction += OnToggleInteraction;
    }

    public void OnChangePhase(TurnPhase phase)
    {
        this.Phase = phase;
    }

    public void UpdateUI()
    {
        int ind = 0;

        if (CardData.Suit == Suit.Heart)
        {
            ind = 2;
        }
        else if (CardData.Suit == Suit.Diamond)
        {
            ind = 1;
        }
        else if (CardData.Suit == Suit.Spades)
        {
            ind = 3;
        }
        else
        {
            ind = 0;
        }

        ind = ind + CardData.Value * 4 - 4;
        mesh.GetComponent<MeshRenderer>().material = materials.ElementAt(ind);
        float indF = ind / 100000f;
        mesh.transform.position = new Vector3 (mesh.transform.position.x, mesh.transform.position.y + indF, mesh.transform.position.z );
    }

    void OnMouseOver()
    {
        if(!_canInteract) return;

        if(_cardData.Position == Position.Hand || _cardData.Position == Position.Tavern)
        { 
            _outline.enabled = true;
        }
    }

    void OnMouseExit()
    {
        if (!_canInteract) return;

        if (!_selected)
        {
            _outline.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (!_canInteract) return;

        if (Phase == TurnPhase.Discard)
        {
            if ((_cardLogic.SelectedCardsLength() < 1) && this.CardData.Position == Position.Hand)
            {
                if (!_selected)
                {
                    SelectCard();
                }
            }
            else if (this.CardData.Position == Position.Hand)
            {
                if (_selected)
                {
                    UnselectCard();
                }
            }
        }
        else if (Phase == TurnPhase.Trade) 
        {
            if (_cardLogic.IsHandCardSelectedBuyPhase() == true && this.CardData.Position == Position.Hand)
            {
                if (_selected)
                {
                    UnselectCard();
                    _cardLogic.UnSelectHandCardBuyPhase();
                }
            }
            else if (_cardLogic.IsTavernCardSelectedBuyPhase() == true && this.CardData.Position == Position.Tavern)
            {
                if (_selected)
                {
                    UnselectCard();
                    _cardLogic.UnSelectTavernCardBuyPhase();
                }
            }

            else if (_cardLogic.IsHandCardSelectedBuyPhase() == false && this.CardData.Position == Position.Hand)
            {
                SelectCard();
                _cardLogic.SelectHandCardBuyPhase();
            }
            else if (_cardLogic.IsTavernCardSelectedBuyPhase() == false && this.CardData.Position == Position.Tavern)
            {
                SelectCard();
                _cardLogic.SelectTavernCardBuyPhase();
            }
        }
        else
        { 
            if (_cardLogic.SelectedCardsLength() == 3)
            {
                if (_selected) 
                {
                    UnselectCard();
                }
            }
            else
            { 
                if (!_selected && this.CardData.Position == Position.Hand) 
                {
                    SelectCard();
                }
                else if (_selected)
                {
                    UnselectCard();
                }
            }
        }
    }

    public void UnselectCard() 
    {
        _selected = false;
        _outline.OutlineColor = Color.yellow;
        _cardLogic.UnselectCard(this);
        _outline.enabled = false;
    }

    public void SelectCard()
    {
        _selected = true;
        _outline.OutlineColor = Color.green;
        _cardLogic.SelectCard(this);
        _outline.enabled = true;
    }
}