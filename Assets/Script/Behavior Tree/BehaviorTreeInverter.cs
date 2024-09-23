using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��尡 �������� �������� �Ǵ��� ���ڷ���Ʈ ���ִ� ����� (���� �����ȿ� ��밡 ������ �����̴� ��带 ���� �ȿ� ������ ������ ���� ��ȯ)
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
