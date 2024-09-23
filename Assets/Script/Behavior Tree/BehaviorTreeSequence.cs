using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ڽ��� ������ �ִ� ��� ������ �ϳ��� �����ϸ� ������
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
