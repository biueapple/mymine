using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 이동을 위한 네비게이션을 사용하지만 이미 만든게 있으니 나중에 그걸로 수정
public class GoToCoverNode : BehaviorTreeNode
{
    private readonly EnemyAI ai;

    public GoToCoverNode(EnemyAI origin)
    {
        ai = origin;
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
            direction.y = 0; // Y축 회전을 제거

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            ai.transform.rotation = targetRotation;
            //이동
            ai.transform.Translate((coverSpot.position - ai.transform.position).normalized * Time.deltaTime, Space.World);
            return NodeState.RUNNING;
        }
        else
        {
            //멈춤
            return NodeState.SUCCESS;
        }
    }
}
