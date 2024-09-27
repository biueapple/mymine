using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : BehaviorTreeNode
{
    private readonly float range;
    private readonly Transform target;
    private readonly Transform origin;

    public RangeNode(float range, Transform target, Transform origin)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, origin.position);
        return distance <= range ? NodeState.SUCCESS : NodeState.FALIERE;
    }
}
