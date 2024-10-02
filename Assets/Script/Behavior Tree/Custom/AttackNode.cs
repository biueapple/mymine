using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �׺���̼��� ��������� �ٸ��� ������ ���߿� �װɷ� ����
public class AttackNode : BehaviorTreeNode
{
    private readonly MoveNode moveNode;
    private readonly Enemy ai;
    private readonly Player target;

    public AttackNode(Enemy flock, Player target, MoveNode moveNode)
    {
        ai = flock;
        this.target = target;
        this.moveNode = moveNode;
    }

    //��븦 �ٶ󺸵��� ���� �ʿ�
    public override NodeState Evaluate()
    {
        ai.SetColor(Color.green);

        //��븦 �ٶ󺸴� �Լ� ��ü��
        moveNode.LookAtTarget(target);
        //��븦 �ٶ󺸴� �Լ� �Ӹ���
        moveNode.LookAtTargetWithinAngle(target);
        //����
        ai.AttackModule.Attack();

        return NodeState.RUNNING;
    }
}
