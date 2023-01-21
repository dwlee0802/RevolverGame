using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : UnitScript
{
    [Header("PlayerUnit")]
    public Deck deck;
    public Card[] potions = new Card[3];
    List<Card> hand;
    [SerializeField]
    public List<Ingredient> ingredientInventory = new List<Ingredient>();

    [Header("Starting Cards")]
    [SerializeField]
    List<Ingredient> card1Ingredients;
    [SerializeField]
    List<Ingredient> card2Ingredients;
    [SerializeField]
    List<Ingredient> card3Ingredients;
    [SerializeField]
    List<Ingredient> card4Ingredients;
    [SerializeField]
    List<Ingredient> card5Ingredients;
    [SerializeField]
    List<Ingredient> card6Ingredients;

    protected override void Start()
    {
        deck = new Deck();
        hand = new List<Card>();

        uiManager.UpdatePlayerInfo(this);
        uiManager.UpdateInventoryUI(ingredientInventory);

        //testing cards
        deck.AddNewCard(new Card(card1Ingredients));
        deck.AddNewCard(new Card(card2Ingredients));
        deck.AddNewCard(new Card(card3Ingredients));
        deck.AddNewCard(new Card(card4Ingredients));
        deck.AddNewCard(new Card(card5Ingredients));
        deck.AddNewCard(new Card(card6Ingredients));
        deck.ShuffleDeck();

        //Debug.Log(deck.cards.Count);

        uiManager.UpdateDeckUI(deck);
        uiManager.UpdateUnusedCardsUI(deck.temp_deck_size);

    }



    protected override void Update()
    {
        LockOnTarget();

        //In combat controls
        if (gameManager.GetGameState() == GameState.Combat)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
        
        base.Update();

        if (gameManager.GetGameState() == GameState.Combat)
        {
            if (hand.Count == 0)
            {
                DrawCards();
            }
        }


        uiManager.UpdateEnemyInfo(currentTarget);
    }

    //get the closest enemy from mouse cursor
    void LockOnTarget()
    {
        if (gameManager.enemiesOnScreen == null || gameManager.enemiesOnScreen.Count == 0)
        {
            currentTarget = null;
            return;
        }
        else
        {
            float closestDist = Vector2.Distance(gameManager.enemiesOnScreen[0].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) );
            currentTarget = gameManager.enemiesOnScreen[0];

            foreach (var enemy in gameManager.enemiesOnScreen)
            {
                float thisdist = Vector2.Distance(enemy.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //calculate distance
                if (thisdist < closestDist)
                {
                    closestDist = thisdist;
                    currentTarget = enemy;
                }
            }
        }

        foreach(var enemy in gameManager.enemiesOnScreen)
        {
            if(enemy == currentTarget)
            {
                enemy.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                enemy.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    //draws cards from the deck
    public void DrawCards()
    {
        hand = deck.DrawCards(6);

        uiManager.UpdateCardPanel(hand);

        //Debug.Log("Drew cards");

        /*
        foreach(Card card in hand)
        {
            Debug.Log(card);
        }
        */
    }

    //used cards go first in first out back into the deck
    public override bool Attack()
    {
        attackCard = hand[0];

        if(base.Attack())
        {
            deck.cards.Add(hand[0]);

            hand.RemoveAt(0);

            uiManager.UpdateCardPanel(hand);

            return true;
        }
        else
        {
            return false;
        }
    }

    //cards that are left in the hand when reloaded are shuffled and went in
    public void Reload()
    {
        ReturnHandToDeck();

        DrawCards();
    }

    public void ReturnHandToDeck()
    {
        //shuffle cards in hand
        for (int i = 0; i < hand.Count; i++)
        {
            Card temp = hand[i];
            int randnum = Random.Range(0, hand.Count);
            hand[i] = hand[randnum];
            hand[randnum] = temp;
        }

        foreach (var card in hand)
        {
            deck.cards.Add(card);
        }

        hand = new List<Card>();
    }

    public void Debug_GetHit()
    {
        base.ReceiveHit(new Card());
    }

    public void RemoveInventoryIngredient(Ingredient target)
    {
        Debug.Log("remove " + target.ingredientName);
        ingredientInventory.Remove(target);
    }

    public void AddInventoryIngredient(Ingredient target)
    {
        Debug.Log("add " + target.ingredientName);
        ingredientInventory.Add(target);
    }

    public int GetInventoryCount()
    {
        return ingredientInventory.Count;
    }

    public override void ReceiveHit(Card card = null)
    {
        base.ReceiveHit(card);

        uiManager.UpdatePlayerInfo(this);
    }
}