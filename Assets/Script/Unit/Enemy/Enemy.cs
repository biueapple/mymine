using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    //�׾����� ���� ������
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
            //������ ���

            //��Ȱ��ȭ
        }

        return damage;
    }
}
