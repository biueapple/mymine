using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 이동을 위한 네비게이션을 사용하지만 이미 만든게 있으니 나중에 그걸로 수정
public class GoToCoverNode : BehaviorTreeNode
{
    //이동을 할 적 객체
    private readonly Enemy ai;
    //적 객체의 숨는 장소 인터페이스
    private readonly ICoveredEnemy coveredEnemy;
    private readonly MoveNode moveNode;
    public GoToCoverNode(Enemy flock, MoveNode moveNode)
    {
        ai = flock;
        coveredEnemy = flock as ICoveredEnemy;
        this.moveNode = moveNode;
    }

    public override NodeState Evaluate()
    {
        //숨는 장소 인터페이스가 존재하지 않다면 숨으러 간다는 것은 당연히 실패
        if (coveredEnemy == null)
            return NodeState.FALIERE;

        //숨는 장소 인터페이스를 가졌더라도 숨을 장소가 없다면 실패
        Transform coverSpot = coveredEnemy.BestCoverSpot;
        if (coverSpot == null)
            return NodeState.FALIERE;

        //숨는 장소 인터페이스를 소지하면서 장소도 존재한다면 이동
        ai.SetColor(Color.blue);
        float distance = Vector3.Distance(coverSpot.position, ai.transform.position);
        if(distance > 0.2f)
        {
            Vector3 direction = coverSpot.position - ai.transform.position;

            moveNode.Move(direction);

            return NodeState.RUNNING;
        }
        else
        {
            //이동 완료
            return NodeState.SUCCESS;
        }
    }
}
