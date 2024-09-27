using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상대방과 나 사이에 무언가가 가로막고 있는가 (숨었는가)
public class IsCoveredNode : BehaviorTreeNode
{
    private readonly Transform target;
    private readonly Transform origin;

    public IsCoveredNode(Transform target, Transform origin)
    {
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        //target에게 ray를 쏴서 그것이 target이면 무언가가 둘 사이에 있다는 것이니 성공
        //아니면 실패
        if(Physics.Raycast(origin.position, target.position - origin.position, out RaycastHit hit))
        {
            if(hit.collider.transform != target)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FALIERE;
    }
}
