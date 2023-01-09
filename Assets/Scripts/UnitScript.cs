using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    [Header("Unit Stats")]
    [SerializeField]
    protected int healthPoints = 1;

    [SerializeField]
    public float[] elements = new float[Card.elementCount];

    public Card attackCard;

    [Header("Unit Action Parameters")]
    [SerializeField]
    protected State state = State.Idle;

    [SerializeField]
    public UnitScript currentTarget;

    [SerializeField]
    float attack_Cooldown;
    [SerializeField]
    float attack_PreDelay;
    [SerializeField]
    float attack_MidDelay;
    [SerializeField]
    float attack_PostDelay;
    [SerializeField]
    float hitStunTime;
    [SerializeField]
    float DodgeTime;

    SpriteRenderer unitSpriteRenderer;

    [Header("Other")]
    public GameManager gameManager;
    public UIManager uiManager;

    [Header("Sprites")]
    [SerializeField]
    Sprite idleSprite;
    [SerializeField]
    Sprite preAttackSprite;
    [SerializeField]
    Sprite midAttackSprite;
    [SerializeField]
    Sprite postAttackSprite;
    [SerializeField]
    Sprite hitSprite;
    [SerializeField]
    Sprite dodgeSprite;

    float timeHolder;
    float initialCooldowntimeHolder;
    float lastBaseAttackTime = -1;

    private void Awake()
    {
        unitSpriteRenderer = GetComponent<SpriteRenderer>();
        timeHolder = Time.timeSinceLevelLoad;
        initialCooldowntimeHolder = attack_Cooldown;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        //element natural degradation
        for(int i = 0; i < Card.elementCount; i++)
        {
            if(elements[i] < 0)
            {
                elements[i] = 0;
            }

            if(elements[i] != 0)
            {
                elements[i] -= Time.deltaTime;
            }
        }

        switch(state)
        {
            case State.Idle:
                unitSpriteRenderer.sprite = idleSprite;
                break;
            case State.PreAttack:
                unitSpriteRenderer.sprite = preAttackSprite;
                if(Time.timeSinceLevelLoad - timeHolder > attack_PreDelay)
                {
                    state = State.MidAttack;
                    timeHolder = Time.timeSinceLevelLoad;
                }
                break;
            case State.MidAttack:
                unitSpriteRenderer.sprite = midAttackSprite;
                if (Time.timeSinceLevelLoad - timeHolder > attack_MidDelay)
                {
                    state = State.PostAttack;
                    timeHolder = Time.timeSinceLevelLoad;
                    currentTarget.ReceiveHit(attackCard);
                }
                break;
            case State.PostAttack:
                unitSpriteRenderer.sprite = postAttackSprite;
                if (Time.timeSinceLevelLoad - timeHolder > attack_PostDelay)
                {
                    state = State.Idle;
                }
                break;
            case State.Hit:
                unitSpriteRenderer.sprite = hitSprite;
                if (Time.timeSinceLevelLoad - timeHolder > hitStunTime)
                {
                    state = State.Idle;
                }
                break;
            case State.Dodge:
                unitSpriteRenderer.sprite = dodgeSprite;
                if (Time.timeSinceLevelLoad - timeHolder > DodgeTime)
                {
                    state = State.Idle;
                }
                break;
        }

        if(healthPoints <= 0)
        {
            OnDeath();
        }

        if(initialCooldowntimeHolder > 0)
        {
            initialCooldowntimeHolder -= Time.deltaTime;
        }
    }

    public virtual bool Attack()
    {
        if(currentTarget == null)
        {
            Debug.Log("No current target");
            return false;
        }
        //attack is still on cooldown
        if(lastBaseAttackTime > 0 && Time.timeSinceLevelLoad - lastBaseAttackTime < attack_Cooldown)
        {
            //Debug.Log("Attack not ready yet");
            return false;
        }

        if(initialCooldowntimeHolder > 0)
        {
            return false;
        }

        Debug.Log(transform.name + " attacks " + currentTarget.transform.name + " with " + attackCard);
        state = State.PreAttack;
        timeHolder = Time.timeSinceLevelLoad;
        lastBaseAttackTime = Time.timeSinceLevelLoad;

        return true;
    }

    public void ReceiveHit(Card card = null)
    {
        if(card == null)
        {
            return;
        }

        //if hit is successful
        if(true)
        {
            state = State.Hit;
            timeHolder = Time.timeSinceLevelLoad;

            //effects from the card
            for(int i = 0; i < Card.elementCount; i++)
            {
                elements[i] += card.adds[i];

                if(elements[i] < 0)
                {
                    elements[i] = 0;
                }
            }

            int damage = 0;

            for (int i = 0; i < Card.elementCount; i++)
            {
                damage += (int)Mathf.Ceil(elements[i]) * card.dmgPer[i];
            }

            healthPoints -= damage;

            Debug.Log(transform.name + " received " + damage + " damage!");

            if(card.ingredients != null)
            {
                //special effects from card
                foreach (var item in card.ingredients)
                {
                    item.Effect(this);
                }
            }
        }
        //dodged attack
        else
        {
            /*
            state = State.Dodge;
            timeHolder = Time.timeSinceLevelLoad;
            Debug.Log(transform.name + " dodged the attack!");
            */
        }
    }

    protected virtual void OnDeath()
    {
        Debug.Log(transform.name + " has been defeated!");
    }

    public override string ToString()
    {
        string output = "";

        output += transform.name + "\n";
        output += "HP: " + healthPoints + "\n";
        output += "Red: " + (int)Mathf.Ceil(elements[0]) + "\n";
        output += "Blue: " + (int)Mathf.Ceil(elements[1]) + "\n";
        output += "Black: " + (int)Mathf.Ceil(elements[2]) + "\n";
        output += "White: " + (int)Mathf.Ceil(elements[3]);

        return output;
    }
}

public enum State
{
    Idle,
    PreAttack,
    MidAttack,
    PostAttack,
    Hit,
    Dodge
}