using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class ChaseNode : BehaviorTreeNode
{
    private readonly MoveNode moveNode;
    private readonly Player target;
    private readonly Enemy ai;

    public ChaseNode(Player target, Enemy flock, MoveNode moveNode)
    {
        this.target = target;
        ai = flock;
        this.moveNode = moveNode;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);
        //�ʹ� ������ �ȵǴϱ�
        float distance = Vector3.Distance(target.transform.position, ai.transform.position);
        if(distance > 0.2f)
        {
            //����
            Vector3 direction = target.transform.position - ai.transform.position;
            //�̵�
            moveNode.Move(direction, target);

            return NodeState.RUNNING;
        }
        else
        {
            //�ȿ����̰�
            return NodeState.SUCCESS;
        }
    }
}
