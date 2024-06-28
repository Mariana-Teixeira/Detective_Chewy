using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardData _cardData;

    private CardLogic _cardLogic;

    [SerializeField] List<Material> materials;
    [SerializeField] GameObject mesh;

    public TurnPhase Phase;

    public bool _isSelected, _canInteract, _isHovered;

    Animator _animator;

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
        _animator = GetComponent<Animator>();
        _cardLogic = FindFirstObjectByType<CardLogic>();
    }

    private void Start()
    {
        CardLogic.ChangeTurnPhase += OnChangePhase;
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
    }

    void OnMouseOver()
    {
        float amount = 0.015f;

        if(!_canInteract || _isHovered) return;

        if(_cardData.Position == Position.Hand)
        {
            _isHovered = true;
            if (!_isSelected) this.transform.localPosition += this.transform.up * amount;
        }
        else if (_cardData.Position == Position.Tavern)
        {
            _isHovered = true;
            if (!_isSelected) this.transform.localPosition -= this.transform.forward * amount;
        }
    }

    void OnMouseExit()
    {
        float amount = 0.015f;

        if (!_canInteract || !_isHovered) return;

        if (_cardData.Position == Position.Hand)
        {
            _isHovered = false;
            if (!_isSelected) this.transform.localPosition -= this.transform.up * amount;
        }
        else if (_cardData.Position == Position.Tavern)
        {
            _isHovered = false;
            if (!_isSelected) this.transform.localPosition += this.transform.forward * amount;
        }
    }

    private void OnMouseDown()
    {
        if (!_canInteract) return;

        if (Phase == TurnPhase.Discard)
        {
            if ((_cardLogic.SelectedCardsLength() < 1) && this.CardData.Position == Position.Hand)
            {
                if (!_isSelected)
                {
                    SelectCard();
                }
            }
            else if (this.CardData.Position == Position.Hand)
            {
                if (_isSelected)
                {
                    UnselectCard();
                }
            }
        }
        else if (Phase == TurnPhase.Trade) 
        {
            if (_cardLogic.IsHandCardSelectedBuyPhase() == true && this.CardData.Position == Position.Hand)
            {
                if (_isSelected)
                {
                    UnselectCard();
                    _cardLogic.UnSelectHandCardBuyPhase();
                }
            }
            else if (_cardLogic.IsTavernCardSelectedBuyPhase() == true && this.CardData.Position == Position.Tavern)
            {
                if (_isSelected)
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
                if (_isSelected) 
                {
                    UnselectCard();
                }
            }
            else
            { 
                if (!_isSelected && this.CardData.Position == Position.Hand) 
                {
                    SelectCard();
                }
                else if (_isSelected)
                {
                    UnselectCard();
                }
            }
        }
    }

    public void UnselectCard()
    {
        float amount = 0.020f;
        if (!_isHovered) amount += 0.015f;

        _isSelected = false;
        if (this.CardData.Position == Position.Hand)
        {
            this.transform.localPosition -= this.transform.up * amount;

        }
        else
        {
            this.transform.localPosition += this.transform.forward * amount;
        }
        _cardLogic.UnselectCard(this);
    }

    public void SelectCard()
    {
        float amount = 0.020f;

        _isSelected = true;
        if (this.CardData.Position == Position.Hand)
        {
            this.transform.localPosition += this.transform.up * amount;
        }
        else
        {
            this.transform.localPosition -= this.transform.forward * amount;
        }

        _cardLogic.SelectCard(this);
    }

    public void DenyCard()
    {
        _animator.SetTrigger("denied");
    }
}