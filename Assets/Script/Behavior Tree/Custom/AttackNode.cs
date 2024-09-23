using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �׺���̼��� ��������� �ٸ��� ������ ���߿� �װɷ� ����
public class AttackNode : BehaviorTreeNode
{
    private Transform origin;
    private EnemyAI ai;

    public AttackNode(Transform origin, EnemyAI ai)
    {
        this.origin = origin;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.green);
        return NodeState.RUNNING;
    }
}
