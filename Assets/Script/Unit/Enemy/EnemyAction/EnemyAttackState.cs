using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격하는 상태
public class EnemyAttackState : EnemyState
{
    //여기는 공격을 할지 결정하는게 아닌 이미 공격이 결정된 상태에서 시작하는거임
    //바로 공격하고 그에 따른 판정
    //후에 어떤 상태가 되어야 하는지만 판단하면 끝

    private Enemy enemy;
    private Coroutine coroutine;

    public EnemyAttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        //enemy가 가지고 있는 attackmodule을 이용해서 공격
        //공격에 대한 판정도 attackmodule이 해줌
        //후에 다음 어떤 상태가 되어야 하는지 판단
        coroutine = GameManager.Instance.StartCoroutine(Attack());
    }

    public override void Exit()
    {
        GameManager.Instance.StopCoroutine(coroutine);
    }

    public override void Update()
    {

    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.2f);
        Debug.Log("몬스터의 공격");
        enemy.AttackModule.Attack();
        yield return new WaitForSeconds(0.1f);
        //다음 상태로
        enemy.ChangeState(new EnemyPatrolState(enemy));
    }
}
