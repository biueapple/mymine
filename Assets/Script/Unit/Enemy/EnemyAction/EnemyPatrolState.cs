using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터가 경로를 따라 이동하는 경우
public class EnemyPatrolState : EnemyState
{
    private Enemy enemy;
    private Pathfinder pathfinder;
    private DistanceDetection distanceDetection;
    private Player target;
    private AutoMove autoMove;
    private Coroutine coroutine;
    //일정한 주기로
    //주위의 위치를 하나 찍어서 그곳으로 이동

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
        target = distanceDetection.Sensing(GameManager.Instance.Players);

        if (target != null)
        {
            pathfinder.Finding(target.transform.position);
            autoMove.SetTartget(pathfinder.Points);
        }
        else
        {
            //상태 변화
            enemy.ChangeState(new EnemyPatrolState());
        }
    }

    private IEnumerator Sensing()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            target = distanceDetection.Sensing(GameManager.Instance.Players);

            if (target != null)
            {
                //상태 변화
                enemy.ChangeState(new EnemyChaseState(enemy, pathfinder, target, autoMove));
            }
        }
    }
}
