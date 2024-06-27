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

    [SerializeField] List<Material> materials;
    [SerializeField] GameObject mesh;

    public TurnPhase Phase;

    public bool _isSelected, _canInteract, _isHovered;

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
        if(!_canInteract || _isHovered) return;

        if(_cardData.Position == Position.Hand)
        {
            _isHovered = true;
            this.transform.localPosition += this.transform.up * 0.015f;
        }

        //if (_cardData.Position == Position.Hand || _cardData.Position == Position.Tavern)
        //{
        //_outline.enabled = true;
        //}
    }

    void OnMouseExit()
    {
        if (!_canInteract || !_isHovered) return;

        if (_cardData.Position == Position.Hand && !_isSelected)
        {
            _isHovered = false;
            this.transform.localPosition -= this.transform.up * 0.015f;
        }

        //if (!_selected)
        //{
        //    _outline.enabled = false;
        //}
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
        _isSelected = false;
        //_outline.OutlineColor = Color.yellow;
        if (this.CardData.Position == Position.Hand)
        {

            if (_cardLogic.GetActiveTable() == 0)
            {
                //this.transform.localPosition = this.transform.localPosition + new Vector3(0, -0.0091f, -0.035f);
                this.transform.localPosition -= this.transform.up * 0.035f;
            }
            else
            {
                this.transform.localPosition = this.transform.localPosition + new Vector3(-0.035f, -0.0091f, 0);
            }
        }
        else
        {
            if (_cardLogic.GetActiveTable() == 0)
            {
                this.transform.localPosition = this.transform.localPosition + new Vector3(0, 0.0091f, 0.035f);
            }
            else
            {
                this.transform.localPosition = this.transform.localPosition + new Vector3(0.035f, 0.0091f, 0);
            }
        }
        _cardLogic.UnselectCard(this);
        //_outline.enabled = false;
    }

    public void SelectCard()
    {
        _isSelected = true;
        //_outline.OutlineColor = Color.green;
        if (this.CardData.Position == Position.Hand)
        {
            if (_cardLogic.GetActiveTable() == 0)
            {
                //this.transform.localPosition = this.transform.localPosition + new Vector3(0, 0.0091f, 0.035f);
                this.transform.localPosition += this.transform.up * 0.035f;
            }
            else
            {
                this.transform.localPosition = this.transform.localPosition + new Vector3(0.035f, 0.0091f, 0);
            }
        }
        else
        {
            if (_cardLogic.GetActiveTable() == 0)
            {
                this.transform.localPosition = this.transform.localPosition + new Vector3(0, -0.0091f, -0.035f);
            }
            else {
                this.transform.localPosition = this.transform.localPosition + new Vector3(-0.035f, -0.0091f, 0);
            }
        }
        _cardLogic.SelectCard(this);
        //_outline.enabled = true;
    }
}