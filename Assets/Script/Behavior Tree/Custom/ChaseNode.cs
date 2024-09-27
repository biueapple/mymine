using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class ChaseNode : BehaviorTreeNode
{
    private readonly Transform target;
    private readonly EnemyAI ai;

    public ChaseNode(Transform target, EnemyAI origin)
    {
        this.target = target;
        ai = origin;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(target.position, ai.transform.position);
        if(distance > 0.2f)
        {
            Vector3 direction = target.position - ai.transform.position;
            direction.y = 0; // Y�� ȸ���� ����

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            ai.transform.rotation = targetRotation;
            //�����̰�
            ai.transform.Translate((target.position - ai.transform.position).normalized * Time.deltaTime, Space.World);
            return NodeState.RUNNING;
        }
        else
        {
            //�ȿ����̰�
            return NodeState.SUCCESS;
        }
    }
}
