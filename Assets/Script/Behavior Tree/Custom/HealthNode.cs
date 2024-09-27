using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : BehaviorTreeNode
{
    private readonly EnemyAI ai;
    private readonly float threshold;

    public HealthNode(EnemyAI ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        //ai�� ü���� threshold ���� ������ ���� �ƴϸ� ����
        return ai.Health <= threshold ? NodeState.SUCCESS : NodeState.FALIERE;
    }
}
