using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class RunNode : BehaviorTreeNode
{
    private Transform target;
    private EnemyAI ai;

    public RunNode(Transform target, EnemyAI origin)
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
            Vector3 direction = ai.transform.position - target.position;
            direction.y = 0; // Y�� ȸ���� ����

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            ai.transform.rotation = targetRotation;
            //�����̰�
            ai.transform.Translate((ai.transform.position - target.position).normalized * Time.deltaTime, Space.World);
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}
