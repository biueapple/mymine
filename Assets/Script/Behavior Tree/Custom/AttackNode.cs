using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �׺���̼��� ��������� �ٸ��� ������ ���߿� �װɷ� ����
public class AttackNode : MoveNode
{
    private readonly Enemy ai;
    private readonly Player target;

    public AttackNode(Enemy flock, Player target, float rotationSpeed) : base(null, flock, 0, rotationSpeed, 0 , 0)
    {
        ai = flock;
        this.target = target;
    }

    //��븦 �ٶ󺸵��� ���� �ʿ�
    public override NodeState Evaluate()
    {
        ai.SetColor(Color.green);

        //��븦 �ٶ󺸴� �Լ� ��ü��
        LookAtTarget(target);
        //��븦 �ٶ󺸴� �Լ� �Ӹ���
        LookAtTargetWithinAngle(target);
        //����
        ai.AttackModule.Attack();

        return NodeState.RUNNING;
    }
}
