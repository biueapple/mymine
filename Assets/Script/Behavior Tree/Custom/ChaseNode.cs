using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class ChaseNode : MoveNode
{
    private readonly Transform target;
    private readonly Enemy ai;

    public ChaseNode(Transform target, Transform[] colleague, Enemy flock, float avoidance, float rotationSpeed, float momentum, float power) : base(colleague, flock, avoidance, rotationSpeed, momentum, power)
    {
        this.target = target;
        ai = flock;
    }

    public override NodeState Evaluate()
    {
        //ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(target.position, ai.transform.position);
        if(distance > 0.2f)
        {
            Vector3 direction = target.position - ai.transform.position;

            Move(direction);

            return NodeState.RUNNING;
        }
        else
        {
            //�ȿ����̰�
            return NodeState.SUCCESS;
        }
    }
}
