using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
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
