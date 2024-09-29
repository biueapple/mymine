using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 네비게이션을 사용하지만 다른게 있으니 나중에 그걸로 수정
public class AttackNode : MoveNode
{
    private Enemy ai;

    public AttackNode(Transform[] colleague, Enemy flock, float avoidance, float rotationSpeed, float momentum, float power) : base(colleague, flock, avoidance, rotationSpeed, momentum, power)
    {
        ai = flock;
    }

    //상대를 바라보도록 변경 필요
    public override NodeState Evaluate()
    {
        //ai.SetColor(Color.green);

        Move(Vector3.zero);

        return NodeState.RUNNING;
    }
}
