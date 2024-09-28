using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class GoToCoverNode : MoveNode
{
    private readonly EnemyAI ai;

    public GoToCoverNode(Transform[] colleague, EnemyAI flock, Transform boss, float avoidance, float rotationSpeed, float momentum) : base(colleague, flock, boss, avoidance, rotationSpeed, momentum)
    {
        ai = flock;
    }

    public override NodeState Evaluate()
    {
        Transform coverSpot = ai.GetBestCoverSpot();
        if (coverSpot == null)
            return NodeState.FALIERE;

        ai.SetColor(Color.blue);
        float distance = Vector3.Distance(coverSpot.position, ai.transform.position);
        if(distance > 0.2f)
        {
            Vector3 direction = coverSpot.position - ai.transform.position;

            Move(direction);

            return NodeState.RUNNING;
        }
        else
        {
            //����
            return NodeState.SUCCESS;
        }
    }
}
