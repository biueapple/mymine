using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ϴ� ����
public class EnemyAttackState : EnemyState
{
    //����� ������ ���� �����ϴ°� �ƴ� �̹� ������ ������ ���¿��� �����ϴ°���
    //�ٷ� �����ϰ� �׿� ���� ����
    //�Ŀ� � ���°� �Ǿ�� �ϴ����� �Ǵ��ϸ� ��

    private Enemy enemy;
    private Coroutine coroutine;

    public EnemyAttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        //enemy�� ������ �ִ� attackmodule�� �̿��ؼ� ����
        //���ݿ� ���� ������ attackmodule�� ����
        //�Ŀ� ���� � ���°� �Ǿ�� �ϴ��� �Ǵ�
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
        Debug.Log("������ ����");
        enemy.AttackModule.Attack();
        yield return new WaitForSeconds(0.1f);
        //���� ���·�
        enemy.ChangeState(new EnemyPatrolState(enemy));
    }
}
