using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardData _cardData;

    private CardLogic _cardLogic;

    [SerializeField] List<Material> materials;
    [SerializeField] GameObject face;

    public TurnPhase Phase;

    public bool _isSelected, _canInteract, _isHovered;

    Animator _animator;

    float cardHoverAmount;
    float cardSelectAmount;

    public float CardHoverAmount
    {
        get
        {
            return cardHoverAmount;
        }
    }

    public float CardSelectAmount
    {
        get
        {
            return cardSelectAmount;
        }
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
        _animator = GetComponent<Animator>();
        _cardLogic = FindFirstObjectByType<CardLogic>();

        face = transform.transform.GetChild(0).GetChild(0).gameObject;
        cardHoverAmount = 0.005f;
        cardSelectAmount = 0.01f;
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
        face.GetComponent<MeshRenderer>().material = materials.ElementAt(ind);
    }

    void OnMouseOver()
    {
        if(!_canInteract || _isHovered) return;

        if(_cardData.Position == Position.Hand)
        {
            _isHovered = true;
            if (!_isSelected) this.transform.localPosition += this.transform.up * cardHoverAmount;
        }
        else if (_cardData.Position == Position.Tavern)
        {
            _isHovered = true;
            if (!_isSelected) this.transform.localPosition -= this.transform.forward * cardHoverAmount;
        }
    }

    void OnMouseExit()
    {
        if (!_canInteract || !_isHovered) return;

        if (_cardData.Position == Position.Hand)
        {
            _isHovered = false;
            if (!_isSelected) this.transform.localPosition -= this.transform.up * cardHoverAmount;
        }
        else if (_cardData.Position == Position.Tavern)
        {
            _isHovered = false;
            if (!_isSelected) this.transform.localPosition += this.transform.forward * cardHoverAmount;
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
        var amount = cardSelectAmount;
        if (!_isHovered) amount = cardSelectAmount + cardHoverAmount;

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
        var amount = cardSelectAmount;
        if (!_isHovered) amount = cardSelectAmount + cardHoverAmount;

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

    public void DenyAnimation()
    {
        _animator.SetTrigger("denied");
    }
}