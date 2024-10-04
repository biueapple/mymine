using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 이동을 위한 네비게이션을 사용하지만 이미 만든게 있으니 나중에 그걸로 수정
public class RunNode : BehaviorTreeNode
{
    private readonly Transform target;
    private readonly Enemy ai;
    private readonly MoveNode moveNode;
    public RunNode(Transform target, Enemy flock, MoveNode moveNode)
    {
        this.target = target;
        ai = flock;
        this.moveNode = moveNode;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);

        Vector3 direction = ai.transform.position - target.position;

        moveNode.Move(direction);

        return NodeState.RUNNING;
    }
}
