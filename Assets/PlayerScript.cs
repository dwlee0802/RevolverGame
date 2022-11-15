using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Action Parameters")]
    [SerializeField]
    float baseAttack_Cooldown;
    [SerializeField]
    bool baseAttack_OnCooldown = false;
    [SerializeField]
    float baseAttack_PreDelay;
    [SerializeField]
    float baseAttack_MidDelay;
    [SerializeField]
    float baseAttack_AfterDelay;

    [SerializeField]
    protected int currentActionPower = 1;

    [SerializeField]
    protected bool actionInProcess = false;

    [SerializeField]
    GameObject currentTarget;

    [Header("UI Elements")]
    [SerializeField]
    RectTransform baseAttack_CooldownUI;
    [SerializeField]
    RectTransform specialAttack_CooldownUI;

    [Header("GameWorld Elements")]
    //update this list when loading scene and when an enemy is downed.
    [SerializeField]
    public List<GameObject> enemiesOnScreen = null;

    SpriteRenderer tempSprite;

    // Start is called before the first frame update
    void Start()
    {
        tempSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LockOnTarget();
    }

    public void BaseAttack()
    {
        if(actionInProcess == false && baseAttack_OnCooldown == false)
        {
            StartCoroutine(BaseAttackProcess());
        }
    }

    IEnumerator BaseAttackProcess()
    {
        actionInProcess = true;

        currentActionPower = 0;

        baseAttack_CooldownUI.gameObject.SetActive(true);

        Debug.Log("Getting ready");

        tempSprite.color = Color.yellow;

        yield return new WaitForSeconds(baseAttack_PreDelay);

        Debug.Log("Bang");

        tempSprite.color = Color.red;


        yield return new WaitForSeconds(baseAttack_MidDelay);

        Debug.Log("finishing");

        tempSprite.color = Color.yellow;

        yield return new WaitForSeconds(baseAttack_AfterDelay);

        Debug.Log("back to default state");

        tempSprite.color = Color.white;

        currentActionPower = 1;

        actionInProcess = false;

        baseAttack_OnCooldown = true;

        yield return new WaitForSeconds(baseAttack_Cooldown);

        baseAttack_CooldownUI.gameObject.SetActive(false);

        baseAttack_OnCooldown = false;
    }

    //get the closest enemy from mouse cursor
    void LockOnTarget()
    {
        if (enemiesOnScreen.Count == 0)
        {
            currentTarget = null;
            return;
        }
        else
        {
            float closestDist = Vector2.Distance(enemiesOnScreen[0].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) );
            currentTarget = enemiesOnScreen[0];

            foreach (var enemy in enemiesOnScreen)
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

        foreach(var enemy in enemiesOnScreen)
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

    public void ReceiveAttack()
    {
        if (currentActionPower < 1)
        {
            StopAllCoroutines();
            Debug.Log("Attack hit on " + transform.name);

            //temporary
            GetComponent<SpriteRenderer>().color = Color.white;

            actionInProcess = false;
        }
    }
}

public enum PlayerAction
{
    baseAttack,
    specialAttack
}
