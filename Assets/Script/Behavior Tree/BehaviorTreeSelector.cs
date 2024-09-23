using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ڽ��� ������ �ִ� ��� ������ �ϳ��� �����ص� ������
public class BehaviorTreeSelector : BehaviorTreeNode
{
    protected List<BehaviorTreeNode> nodes = new();

    public BehaviorTreeSelector(List<BehaviorTreeNode> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (BehaviorTreeNode node in nodes)
        {
            switch(node.Evaluate())
            {
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    return _nodeState;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                case NodeState.FALIERE:
                default:
                    break;
            }
        }
        _nodeState = NodeState.FALIERE;
        return _nodeState;
    }
}
