using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터가 경로를 따라 이동하는 경우
public class EnemyPatrolState : EnemyState
{
    private Enemy enemy;
    private Player target;
    private Coroutine coroutine;
    //일정한 주기로
    //주위의 위치를 하나 찍어서 그곳으로 이동

    public EnemyPatrolState(Enemy enemy)
    {
        this.enemy = enemy;
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

    }

    private IEnumerator Sensing()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            target = enemy.DistanceDetection.Sensing(GameManager.Instance.Players);

            if (target != null)
            {
                //상태 변화
                enemy.ChangeState(new EnemyChaseState(enemy, target));
            }
        }
    }
}
