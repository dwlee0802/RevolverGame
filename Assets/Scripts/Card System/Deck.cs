using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    /*
    List Cards - Holds the cards that are included in this deck
    public void AddCard(Card card) - Adds card to this deck
    public List<Cards> DrawCards(int amount) - Draws cards from this deck at random
     */
    public int temp_deck_size = 12;
    static int elementCount = Card.elementCount;

    public List<Card> cards;

    public Deck()
    {
        cards = new List<Card>();
    }
    
    public void ShuffleDeck()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(0, cards.Count);

            Card temp = cards[i];

            cards[i] = cards[randomIndex];

            cards[randomIndex] = temp;
        }
    }

    public void AddNewCard(Card card)
    {
        if(temp_deck_size > 0)
        {
            cards.Add(card);
            temp_deck_size--;
        }
    }

    public void RemoveCard(int index)
    {
        cards.RemoveAt(index);
    }

    public List<Card> DrawCards(int amount)
    {
        List<Card> output = new List<Card>();

        if(cards.Count == 0)
        {
            return output;
        }

        for(int i = 0; i < amount; i++)
        {
            output.Add(cards[0]);
            cards.RemoveAt(0);
        }

        return output;
    }
}

public enum Element
{
    Red,
    Blue,
    Black,
    White
}