using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }

    public GameState GetGameState()
    {
        return state;
    }

    //pick a random ingredient to get for clearing rooms
    //parameter is how many options. default to 3
    public List<Ingredient> GenerateRoomClearRewardOptions(int count = 3)
    {
        List<Ingredient> output = new List<Ingredient>();

        return output;
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
}

public enum GameState
{
    Combat,
    EndOfCombat
}
