using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : UnitScript
{
    protected override void Start()
    {
    }

    protected override void Update()
    {
        base.Update();
        base.Attack();

        if(base.healthPoints <= 0)
        {
            base.OnDeath();
            OnDeath();
        }
    }

    protected override void OnDeath()
    {
        gameManager.enemiesOnScreen.Remove(this);
        Destroy(this.gameObject);
    }
}
