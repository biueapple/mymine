using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 이동을 위한 네비게이션을 사용하지만 이미 만든게 있으니 나중에 그걸로 수정
public class RunNode : BehaviorTreeNode
{
    private Transform target;
    private EnemyAI ai;

    public RunNode(Transform target, EnemyAI origin)
    {
        this.target = target;
        ai = origin;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(target.position, ai.transform.position);
        if(distance > 0.2f)
        {
            Vector3 direction = ai.transform.position - target.position;
            direction.y = 0; // Y축 회전을 제거

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            ai.transform.rotation = targetRotation;
            //움직이게
            ai.transform.Translate((ai.transform.position - target.position).normalized * Time.deltaTime, Space.World);
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}
