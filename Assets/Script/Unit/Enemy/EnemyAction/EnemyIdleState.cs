using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���¿� �ִ� ���
public class EnemyIdleState : EnemyState
{
    private readonly Enemy enemy;
    public EnemyIdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        GameManager.Instance.StartCoroutine(Wait());
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        enemy.ChangeState(new EnemyPatrolState(enemy));
    }
}
