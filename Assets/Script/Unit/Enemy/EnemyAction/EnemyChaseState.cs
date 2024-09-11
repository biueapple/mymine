using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ �߰��ϴ� ����
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
        //�Ÿ��� ��������� ���� �����ؾ��ϴ� ���� ��ȭ
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
            //�Ÿ��� �ʹ� �־����� ���� �������� ���� ��ȭ
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
