using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//노드가 실패인지 성공인지 판단을 데코레이트 해주는 노드임 (일정 범위안에 상대가 있으면 성공이던 노드를 범위 안에 있으면 실패인 노드로 변환)
public class BehaviorTreeInverter : BehaviorTreeNode
{
    protected BehaviorTreeNode node;

    public BehaviorTreeInverter(BehaviorTreeNode node)
    {
        this.node = node;
    }

    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                break;
            case NodeState.SUCCESS:
                _nodeState = NodeState.FALIERE;
                break;
            case NodeState.FALIERE:
                _nodeState = NodeState.SUCCESS;
                break;
            default:
                break;
        }
        return _nodeState;
    }
}
