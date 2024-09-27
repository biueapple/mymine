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
        //ai의 체력이 threshold 보다 낮으면 성공 아니면 실패
        return ai.Health <= threshold ? NodeState.SUCCESS : NodeState.FALIERE;
    }
}
