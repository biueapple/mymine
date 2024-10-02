using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 네비게이션을 사용하지만 다른게 있으니 나중에 그걸로 수정
public class AttackNode : BehaviorTreeNode
{
    private readonly MoveNode moveNode;
    private readonly Enemy ai;
    private readonly Player target;

    public AttackNode(Enemy flock, Player target, MoveNode moveNode)
    {
        ai = flock;
        this.target = target;
        this.moveNode = moveNode;
    }

    //상대를 바라보도록 변경 필요
    public override NodeState Evaluate()
    {
        ai.SetColor(Color.green);

        //상대를 바라보는 함수 전체가
        moveNode.LookAtTarget(target);
        //상대를 바라보는 함수 머리만
        moveNode.LookAtTargetWithinAngle(target);
        //공격
        ai.AttackModule.Attack();

        return NodeState.RUNNING;
    }
}
