using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject CardPrefab;
    public void InstantiateCards(CardData[] cards)
    {
        foreach (var card in cards)
        {
            var cardComponent = Instantiate(CardPrefab).GetComponent<Card>();
            cardComponent.CardData = card;
            cardComponent.UpdateUI();
        }
    }
}
