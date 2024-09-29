using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �׺���̼��� ��������� �ٸ��� ������ ���߿� �װɷ� ����
public class AttackNode : MoveNode
{
    private EnemyAI ai;

    public AttackNode(Transform[] colleague, EnemyAI flock, Transform boss, float avoidance, float rotationSpeed, float momentum, float power) : base(colleague, flock, boss, avoidance, rotationSpeed, momentum, power)
    {
        this.ai = flock;
    }

    //��븦 �ٶ󺸵��� ���� �ʿ�
    public override NodeState Evaluate()
    {
        ai.SetColor(Color.green);

        Move(Vector3.zero);

        return NodeState.RUNNING;
    }
}
