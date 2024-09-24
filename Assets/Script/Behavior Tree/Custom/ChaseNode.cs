using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class ChaseNode : BehaviorTreeNode
{
    private Transform target;
    private EnemyAI ai;

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
            //�����̰�
            ai.transform.Translate((target.position - ai.transform.position).normalized * Time.deltaTime);
            return NodeState.RUNNING;
        }
        else
        {
            //�ȿ����̰�
            return NodeState.SUCCESS;
        }
    }
}
