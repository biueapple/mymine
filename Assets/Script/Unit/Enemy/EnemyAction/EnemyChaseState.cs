using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어를 추격하는 상태
public class EnemyChaseState : EnemyState
{
    private Enemy enemy;
    private Player target;
    private Coroutine coroutine;

    public EnemyChaseState(Enemy enemy, Player target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public override void Enter()
    {
        coroutine = GameManager.Instance.StartCoroutine(Sensing());
    }

    public override void Exit()
    {
        GameManager.Instance.StopCoroutine(coroutine);
    }

    public override void Update()
    {
        //거리가 가까워지면 적을 공격해야하니 상태 변화
        if(Vector3.Distance(enemy.transform.position, target.transform.position) <= 1)
        {
            enemy.ChangeState(new EnemyAttackState(enemy));
        }
    }

    private IEnumerator Sensing()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            //거리가 너무 멀어지면 적을 놓쳤으니 상태 변화
            if (Vector3.Distance(enemy.transform.position, target.transform.position) >= 10)
            {
                enemy.ChangeState(new EnemyPatrolState(enemy));
            }
            else
            {
                enemy.Pathfinder.Finding(target.transform.position);
                enemy.AutoMove.SetTartget(enemy.Pathfinder.Points);
            }
        }
    }
}
