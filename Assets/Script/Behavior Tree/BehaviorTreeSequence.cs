using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자신의 하위에 있는 모든 노드들중 하나라도 실패하면 실패임
public class BehaviorTreeSequence : BehaviorTreeNode
{
    protected List<BehaviorTreeNode> nodes = new();

    public BehaviorTreeSequence(List<BehaviorTreeNode> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool isAnyNodeRunning = false;
        foreach (BehaviorTreeNode node in nodes)
        {
            switch(node.Evaluate())
            {
                case NodeState.RUNNING:
                    isAnyNodeRunning = true;
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FALIERE:
                    _nodeState = NodeState.FALIERE;
                    return _nodeState;
                default:
                    break;
            }
        }
        _nodeState = isAnyNodeRunning == true ? NodeState.RUNNING : NodeState.SUCCESS;
        return _nodeState;
    }
}
