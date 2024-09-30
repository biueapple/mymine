using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 이동을 위한 네비게이션을 사용하지만 이미 만든게 있으니 나중에 그걸로 수정
public class GoToCoverNode : MoveNode
{
    private readonly Enemy ai;

    public GoToCoverNode(Transform[] colleague, Enemy flock, float avoidance, float rotationSpeed, float momentum, float power) : base(colleague, flock, avoidance, rotationSpeed, momentum, power)
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
            //멈춤
            return NodeState.SUCCESS;
        }
    }
}
