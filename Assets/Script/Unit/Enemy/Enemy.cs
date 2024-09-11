using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    //죽었을때 떨굴 아이템
    protected int itemID;

    protected EnemyState state;
    public EnemyState State { get { return state; } }

    public void ChangeState(EnemyState _state)
    {
        if(state != null)
            state.Exit();

        state = _state;

        if (state != null)
            state.Enter();
    }

    public override float Hit(AttackInformation attackInformation)
    {
        float damage = base.Hit(attackInformation);
        if(stat.HP <= 0)
        {
            //아이템 드랍

            //비활성화
        }

        return damage;
    }
}
