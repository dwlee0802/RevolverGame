using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    [Header("Unit Action Parameters")]
    [SerializeField]
    State state = State.Idle;

    [SerializeField]
    UnitScript currentTarget;

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
    float lastBaseAttackTime = -1;

    private void Awake()
    {
        unitSpriteRenderer = GetComponent<SpriteRenderer>();
        timeHolder = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
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
    }

    public void Attack()
    {
        //attack is still on cooldown
        if(lastBaseAttackTime > 0 && Time.timeSinceLevelLoad - lastBaseAttackTime < attack_Cooldown)
        {
            Debug.Log("Attack not ready yet");
            return;
        }

        state = State.PreAttack;
        timeHolder = Time.timeSinceLevelLoad;
        lastBaseAttackTime = Time.timeSinceLevelLoad;
    }

    public void ReceiveHit()
    {
        //if hit is successful
        if(true)
        {
            state = State.Hit;
            timeHolder = Time.timeSinceLevelLoad;
            Debug.Log(transform.name + " was hit!");
        }
        //dodged attack
        else
        {
            state = State.Dodge;
            timeHolder = Time.timeSinceLevelLoad;
            Debug.Log(transform.name + " dodged the attack!");
        }
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