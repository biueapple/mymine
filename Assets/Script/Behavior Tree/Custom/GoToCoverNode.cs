using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 이동을 위한 네비게이션을 사용하지만 이미 만든게 있으니 나중에 그걸로 수정
public class GoToCoverNode : BehaviorTreeNode
{
    private EnemyAI ai;

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
            //이동
            ai.transform.Translate((coverSpot.position - ai.transform.position).normalized * Time.deltaTime);
            return NodeState.RUNNING;
        }
        else
        {
            //멈춤
            return NodeState.SUCCESS;
        }
    }
}
