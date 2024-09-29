using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : BehaviorTreeNode
{
    private readonly Enemy ai;
    private readonly float threshold;

    public HealthNode(Enemy ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        //ai�� ü���� threshold ���� ������ ���� �ƴϸ� ����
        return ai.STAT.HP <= threshold ? NodeState.SUCCESS : NodeState.FALIERE;
    }
}
