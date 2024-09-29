using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �׺���̼��� ��������� �ٸ��� ������ ���߿� �װɷ� ����
public class AttackNode : MoveNode
{
    private Enemy ai;

    public AttackNode(Transform[] colleague, Enemy flock, float avoidance, float rotationSpeed, float momentum, float power) : base(colleague, flock, avoidance, rotationSpeed, momentum, power)
    {
        ai = flock;
    }

    //��븦 �ٶ󺸵��� ���� �ʿ�
    public override NodeState Evaluate()
    {
        //ai.SetColor(Color.green);

        Move(Vector3.zero);

        return NodeState.RUNNING;
    }
}
