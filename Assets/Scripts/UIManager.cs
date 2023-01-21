using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Transform cardPanel;
    [SerializeField]
    Transform playerInfoPanel;
    [SerializeField]
    Transform enemyInfoPanel;
    [SerializeField]
    Transform newCardsPopup;
    [SerializeField]
    Transform unusedCardUI;
    [SerializeField]
    Transform deckUI;
    [SerializeField]
    public Transform ingredientsUI;
    [SerializeField]
    public Transform inventoryUI;
    [SerializeField]
    Button confirmButton;
    [SerializeField]
    TMP_Text statsUI;
    [SerializeField]
    Transform craftingMenuCanvas;
    [SerializeField]
    Transform combatCanvas;
    [SerializeField]
    Transform endOfCombatCanvas;
    [SerializeField]
    Transform rewardChoiceButtons;

    [SerializeField]
    GameObject cardEntryPrefab;
    [SerializeField]
    GameObject ingredientEntryPrefab;

    public Transform selectedCardEntry;

    [SerializeField]
    PlayerScript playerScript;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        UpdateStatsUI();

        if (selectedCardEntry == null)
        {
            confirmButton.interactable = false;
        }
        else
        {
            confirmButton.interactable = true;
        }
    }

    public void UpdateCardPanel(List<Card> cards)
    {
        for(int i = 0; i < cardPanel.childCount; i++)
        {
            if(i >= cards.Count)
            {
                cardPanel.GetChild(i).GetComponent<TMP_Text>().text = " ";
            }
            else
            {
                cardPanel.GetChild(i).GetComponent<TMP_Text>().text = cards[i].ToString();
            }
        }
    }

    public void UpdatePlayerInfo(UnitScript unit)
    {
        //need to add action info such as cool downs and such

        if(unit == null)
        {
            playerInfoPanel.GetChild(0).GetComponent<TMP_Text>().text = " ";
        }
        else
        {
            playerInfoPanel.GetChild(0).GetComponent<TMP_Text>().text = unit.ToString();
        }
    }

    public void UpdateEnemyInfo(UnitScript unit)
    {
        if (unit == null)
        {
            enemyInfoPanel.GetChild(0).GetComponent<TMP_Text>().text = " ";
        }
        else
        {
            enemyInfoPanel.GetChild(0).GetComponent<TMP_Text>().text = unit.ToString();
        }
    }

    public void ToggleNewCardsPopup(int code)
    {
        if(code == 0)
        {
            //toggle off
            newCardsPopup.gameObject.SetActive(false);
        }
        else if(code == 1)
        {
            //toggle on
            newCardsPopup.gameObject.SetActive(true);
        }
    }

    public void UpdateUnusedCardsUI(int num)
    {
        unusedCardUI.GetComponent<TMP_Text>().text = "Unused Cards: " + num.ToString();
    }

    //populates the deck ui with card entry prefabs
    public void UpdateDeckUI(Deck deck = null)
    {
        for(int i = 0; i < deckUI.transform.childCount; i++)
        {
            Destroy(deckUI.GetChild(i).gameObject);
        }

        if(deck == null)
        {
            return;
        }

        foreach(var item in deck.cards)
        {
            GameObject newEntry = Instantiate(cardEntryPrefab);
            CardEntryScript entry = newEntry.GetComponent<CardEntryScript>();
            entry.card = item;
            entry.uiManager = this;
            newEntry.transform.GetChild(2).GetComponent<TMP_Text>().text = entry.card.GetIngredientNames();
            newEntry.transform.SetParent(deckUI);
        }
    }

    public void UpdateInventoryUI(List<Ingredient> inven = null)
    {
        for (int i = 0; i < inventoryUI.transform.childCount; i++)
        {
            Destroy(inventoryUI.GetChild(i).gameObject);
        }

        if (inven == null)
        {
            return;
        }

        foreach (var item in inven)
        {
            GameObject newEntry = Instantiate(ingredientEntryPrefab);
            IngredientEntryScript entry = newEntry.GetComponent<IngredientEntryScript>();
            entry.thisIngredient = item;
            entry.uiManager = this;
            newEntry.transform.GetChild(1).GetComponent<TMP_Text>().text += item.ingredientName + " ";
            newEntry.transform.SetParent(inventoryUI);
        }
    }

    public void SelectCard(Card card)
    {
        if(card == null)
        {
            Debug.Log("null card");
        }

        for (int i = 0; i < deckUI.transform.childCount; i++)
        {
            if(deckUI.transform.GetChild(i).GetComponent<CardEntryScript>().card == card)
            {
                deckUI.transform.GetChild(i).GetComponent<Image>().enabled = true;
                selectedCardEntry = deckUI.transform.GetChild(i);
            }
            else
            {
                deckUI.transform.GetChild(i).GetComponent<Image>().enabled = false;
            }
        }

        UpdateIngredientUI(card);
        UpdateInventoryUI(playerScript.ingredientInventory);
    }

    void UpdateIngredientUI(Card card)
    {
        for (int i = 0; i < ingredientsUI.transform.childCount; i++)
        {
            Destroy(ingredientsUI.GetChild(i).gameObject);
        }

        if (card.ingredients == null)
        {
            return;
        }

        foreach (var item in card.ingredients)
        {
            GameObject newEntry = Instantiate(ingredientEntryPrefab);
            IngredientEntryScript entry = newEntry.GetComponent<IngredientEntryScript>();
            entry.thisIngredient = item;
            entry.uiManager = this;
            newEntry.transform.GetChild(1).GetComponent<TMP_Text>().text += item.ingredientName + " ";
            newEntry.transform.SetParent(ingredientsUI);
        }
    }

    //updates scripts as it is on the ui
    public void Confirm()
    {
        Card cardToChange = selectedCardEntry.GetComponent<CardEntryScript>().card;

        List<Ingredient> cardIngredients = new List<Ingredient>();
        List<Ingredient> newinven = new List<Ingredient>();

        for (int i = 0; i < ingredientsUI.childCount; i++)
        {
            cardIngredients.Add(ingredientsUI.GetChild(i).GetComponent<IngredientEntryScript>().thisIngredient);
            ingredientsUI.GetChild(i).GetComponent<Image>().enabled = false;
        }

        for (int i = 0; i < inventoryUI.childCount; i++)
        {
            newinven.Add(inventoryUI.GetChild(i).GetComponent<IngredientEntryScript>().thisIngredient);
            inventoryUI.GetChild(i).GetComponent<Image>().enabled = false;
        }

        playerScript.ingredientInventory = newinven;
        cardToChange.ingredients = cardIngredients;
        cardToChange.RecalculateCard();

        UpdateInventoryUI(playerScript.ingredientInventory);
        UpdateIngredientUI(cardToChange);
    }

    public void UpdateStatsUI()
    {
        string output = "";

        if(selectedCardEntry != null)
        {
            Card card = selectedCardEntry.GetComponent<CardEntryScript>().card;

            output += card.ToString();
        }

        statsUI.text = output;
    }

    public void AddNewEmptyCard()
    {
        playerScript.deck.AddNewCard(new Card());

        UpdateDeckUI(playerScript.deck);
        UpdateUnusedCardsUI(playerScript.deck.temp_deck_size);
    }

    public void ExitCraftingMenu()
    {
        List<Card> newdeckcards = new List<Card>();

        for (int i = 0; i < deckUI.childCount; i++)
        {
            if(deckUI.GetChild(i).GetComponent<CardEntryScript>().card.GetIngredientNames() != "None")
            {
                newdeckcards.Add(deckUI.GetChild(i).GetComponent<CardEntryScript>().card);
                Destroy(deckUI.GetChild(i).gameObject);
            }
        }

        playerScript.deck.cards = newdeckcards;

        craftingMenuCanvas.gameObject.SetActive(false);
        endOfCombatCanvas.gameObject.SetActive(true);
    }

    public void EnterCraftingMenu()
    {
        playerScript.ReturnHandToDeck();

        craftingMenuCanvas.gameObject.SetActive(true);
        endOfCombatCanvas.gameObject.SetActive(false);

        UpdateDeckUI(playerScript.deck);
        UpdateInventoryUI(playerScript.ingredientInventory);
        UpdateUnusedCardsUI(playerScript.deck.temp_deck_size);
    }

    public void EnterEndOfCombatMenu()
    {
        playerScript.ReturnHandToDeck();
        rewardChoiceButtons.gameObject.SetActive(true);
        endOfCombatCanvas.gameObject.SetActive(true);
        combatCanvas.gameObject.SetActive(false);
    }

    public void EnterCombatMenu()
    {
        endOfCombatCanvas.gameObject.SetActive(false);
        combatCanvas.gameObject.SetActive(true);
    }

    public void UpdateRewardSelectionUI(List<Ingredient> options)
    {
        for(int i = 0; i < rewardChoiceButtons.childCount; i++)
        {
            rewardChoiceButtons.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = options[i].ToString();
        }
    }

    public void HideRewardSelectionUI()
    {
        rewardChoiceButtons.gameObject.SetActive(false);
    }
}
