using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 이동을 위한 네비게이션을 사용하지만 이미 만든게 있으니 나중에 그걸로 수정
public class ChaseNode : MoveNode
{
    private readonly Transform target;
    private readonly EnemyAI ai;

    public ChaseNode(Transform target, Transform[] colleague, EnemyAI flock, Transform boss, float avoidance, float rotationSpeed, float momentum, float power) : base(colleague, flock, boss, avoidance, rotationSpeed, momentum, power)
    {
        this.target = target;
        ai = flock;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(target.position, ai.transform.position);
        if(distance > 0.2f)
        {
            Vector3 direction = target.position - ai.transform.position;

            Move(direction);

            return NodeState.RUNNING;
        }
        else
        {
            //안움직이게
            return NodeState.SUCCESS;
        }
    }
}
