using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 네비게이션을 사용하지만 다른게 있으니 나중에 그걸로 수정
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
