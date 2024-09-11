using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ͱ� ��θ� ���� �̵��ϴ� ���
public class EnemyPatrolState : EnemyState
{
    private Enemy enemy;
    private Pathfinder pathfinder;
    private DistanceDetection distanceDetection;
    private Player target;
    private AutoMove autoMove;
    private Coroutine coroutine;
    //������ �ֱ��
    //������ ��ġ�� �ϳ� �� �װ����� �̵�

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
            //���� ��ȭ
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
                //���� ��ȭ
                enemy.ChangeState(new EnemyChaseState(enemy, pathfinder, target, autoMove));
            }
        }
    }
}
