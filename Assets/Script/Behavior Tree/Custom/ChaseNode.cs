using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 이동을 위한 네비게이션을 사용하지만 이미 만든게 있으니 나중에 그걸로 수정
public class ChaseNode : BehaviorTreeNode
{
    private readonly MoveNode moveNode;
    private readonly Player target;
    private readonly Enemy ai;

    public ChaseNode(Player target, Enemy flock, MoveNode moveNode)
    {
        this.target = target;
        ai = flock;
        this.moveNode = moveNode;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);
        //너무 가까우면 안되니까
        float distance = Vector3.Distance(target.transform.position, ai.transform.position);
        if(distance > 0.2f)
        {
            //방향
            Vector3 direction = target.transform.position - ai.transform.position;
            //이동
            moveNode.Move(direction, target);

            return NodeState.RUNNING;
        }
        else
        {
            //안움직이게
            return NodeState.SUCCESS;
        }
    }
}
