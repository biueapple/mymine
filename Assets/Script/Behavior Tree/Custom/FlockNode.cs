using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockNode : BehaviorTreeNode
{
    //보스와 가까워져야 하는 거리
    private readonly float cohesion;
    private readonly Transform boss;
    private readonly Enemy ai;
    private readonly MoveNode moveNode;
    public FlockNode(Transform boss, float cohesion, MoveNode moveNode, Enemy ai)
    {
        this.cohesion = cohesion;
        this.boss = boss;
        this.moveNode = moveNode;
        this.ai = ai;
    }

    //Flock알고리즘을 참고해서 포지션을 정하기 다만 자신과 주위만 영향을 끼쳐야 하며 전체적으로 영향을 줘선 안됨
    //예를 들면 자신의 위치를 바꿔야 해도 자신의 위치만 동료와 위치가 겹치면 자신과 그 동료만 자리를 바꾸면 되는 일이지 전체적으로 움직임이 생겨선 안됨

    //자신이 보스 곁에 있는가
    public Vector3 Cohesion()
    {
        if(boss == null)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        if(Vector3.SqrMagnitude(ai.transform.position - boss.transform.position) > cohesion * cohesion)
        {
            velocity = (boss.transform.position - ai.transform.position).normalized;
        }

        return velocity;
    }
    //보스가 가는 방향으로 이동하는 가

    //일단 할일이 없어서 오는 노드이기에 항상 작동중
    public override NodeState Evaluate()
    {
        ai.SetColor(Color.cyan);

        Vector3 velocity = Vector3.zero;
        
        velocity += Cohesion();

        moveNode.Move(velocity);
        
        return NodeState.RUNNING;
    }
}
