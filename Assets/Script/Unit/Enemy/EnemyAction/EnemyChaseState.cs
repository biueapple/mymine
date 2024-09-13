using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ �߰��ϴ� ����
public class EnemyChaseState : EnemyState
{
    private readonly Enemy enemy;
    private readonly Player target;
    private Coroutine coroutine;

    public EnemyChaseState(Enemy enemy, Player target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public override void Enter()
    {
        coroutine = GameManager.Instance.StartCoroutine(Sensing());
        Debug.Log("�߰� ����");
    }

    public override void Exit()
    {
        GameManager.Instance.StopCoroutine(coroutine);
        Debug.Log("�߰� ����");
    }

    public override void Update()
    {
        //�Ÿ��� ��������� ���� �����ؾ��ϴ� ���� ��ȭ
        if(Vector3.Distance(enemy.transform.position, target.transform.position) <= 1)
        {
            enemy.ChangeState(new EnemyAttackState(enemy));
        }
        LookAtTargetWithinAngle();
    }

    private IEnumerator Sensing()
    {
        while (true)
        {
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
            yield return new WaitForSeconds(2);
        }
    }

    Vector3 directionToTarget;
    void LookAtTargetWithinAngle()
    {
        // ĳ���Ϳ� Ÿ�� ���� ������ ���
        directionToTarget = target.transform.position - enemy.Head.position;
        directionToTarget.y = 0; // ���� ���⸸ ��� (y �� ����)

        // �� ����(ĳ���� forward ����� Ÿ�� ����) ������ ���� ���
        float angleToTarget = Vector3.Angle(enemy.transform.forward, directionToTarget);

        // ������ ������ �þ� ����(viewAngle) ���� ������ Ÿ���� �ٶ�
        if (angleToTarget <= 45)
        {
            // Ÿ���� �ٶ󺸵��� �Ӹ� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            //enemy.Head.rotation = Quaternion.Slerp(enemy.Head.rotation, targetRotation, Time.deltaTime * 5f); // �ε巴�� ȸ��
            enemy.Head.rotation = targetRotation;
        }
    }
}
