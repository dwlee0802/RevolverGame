using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * To do list:
 * Rewards choice should not pick the same ingredient more than once
 * Card crafting UI should update deck after confirm button is pressed
 * Implement ingredient count limit & appropriate UI
 * Status effects. The ones that are confirmed first: Stun (cant do anything), Elemental Lock (element does not decay), Vulnerability (x2 damage) 
 */
public class GameManager : MonoBehaviour
{
    public List<UnitScript> enemiesOnScreen;
    public UIManager uIManager;

    [SerializeField]
    GameState state;

    public PlayerScript playerScript;

    [Header("Unit Prefabs")]
    [SerializeField]
    GameObject ratPrefab;
    [SerializeField]
    List<Ingredient> ratAttackCardIngredients;

    [Header("Ingredients")]
    [SerializeField]
    List<Ingredient> ingredientDB;
    List<Ingredient> rewardOptions;

    private void Start()
    {

    }

    private void Update()
    {
        //main gameplay loop
        //check if all enemies are defeated
        //if true, switch to end of combat ui
        //start next room if player presses next room button
        if(enemiesOnScreen.Count <= 0 && state == GameState.Combat)
        {
            Debug.Log("Room cleared");

            uIManager.EnterEndOfCombatMenu();

            state = GameState.EndOfCombat;

            playerScript.ReturnHandToDeck();

            //show rewards menu
            GenerateRewardOptions(3);
            uIManager.UpdateRewardSelectionUI(rewardOptions);

        }
    }

    public GameState GetGameState()
    {
        return state;
    }

    //load appropriate canvases and instantiate enemies, put their references in the onscreen enemy list, assign appropriate references
    public void LoadNextRoom()
    {
        uIManager.EnterCombatMenu();
        state = GameState.Combat;

        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        int count = Random.Range(1, 5);
        for (int i = 0; i < count; i++)
        {
            UnitScript newentry = Instantiate(ratPrefab).GetComponent<UnitScript>();

            newentry.uiManager = uIManager;
            newentry.gameManager = this;
            Card attackCard = new Card(ratAttackCardIngredients);
            newentry.attackCard = attackCard;
            newentry.currentTarget = playerScript;
            newentry.transform.position = new Vector3(2 + i/2 * 2f, i%2 * 2f, 0);

            enemiesOnScreen.Add(newentry);
        }
    }

    public void ChooseCombatReward(int choice)
    {
        playerScript.AddInventoryIngredient(rewardOptions[choice]);
        uIManager.HideRewardSelectionUI();
    }

    //pick a random ingredient to get for clearing rooms
    //parameter is how many options. default to 3
    void GenerateRewardOptions(int count = 3)
    {
        List<Ingredient> output = new List<Ingredient>();

        for(int i = 0; i < count; i++)
        {
            int randomint = Random.Range(0, ingredientDB.Count);
            output.Add(ingredientDB[randomint]);
        }

        rewardOptions = output;
    }
}

public enum GameState
{
    Combat,
    EndOfCombat
}
