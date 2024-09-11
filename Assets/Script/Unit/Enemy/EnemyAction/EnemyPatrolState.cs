using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ͱ� ��θ� ���� �̵��ϴ� ���
public class EnemyPatrolState : EnemyState
{
    private Enemy enemy;
    private Player target;
    private Coroutine coroutine;
    //������ �ֱ��
    //������ ��ġ�� �ϳ� �� �װ����� �̵�

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
                //���� ��ȭ
                enemy.ChangeState(new EnemyChaseState(enemy, target));
            }
        }
    }
}
