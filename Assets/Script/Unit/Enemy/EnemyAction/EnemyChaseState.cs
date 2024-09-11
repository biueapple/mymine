using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어를 추격하는 상태
public class EnemyChaseState : EnemyState
{
    private Enemy enemy;
    private Pathfinder pathfinder;
    private Player target;
    private AutoMove autoMove;
    private Coroutine coroutine;

    public EnemyChaseState(Enemy enemy, Pathfinder pathfinder, Player target, AutoMove autoMove)
    {
        this.enemy = enemy;
        this.pathfinder = pathfinder;
        this.target = target;
        this.autoMove = autoMove;
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
        //거리가 너무 멀어지면 적을 놓쳤으니 상태 변화
    }

    private IEnumerator Sensing()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if(Vector3.Distance(enemy.transform.position, target.transform.position) >= 10)
            {
                enemy.ChangeState(new EnemyPatrolState());
            }
            else
            {
                pathfinder.Finding(target.transform.position);
                autoMove.SetTartget(pathfinder.Points);
            }
        }
    }
}
