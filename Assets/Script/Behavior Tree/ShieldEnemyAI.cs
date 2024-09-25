using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyAI : EnemyAI
{
    protected override void ConstructBehaviourTree()
    {
        ChaseNode chaseNode = new(player, this);
        RangeNode chaseRangeNode = new(chaseRange, player, transform);
        RangeNode attackRangeNode = new(attackInRnage, player, transform);
        AttackNode attackNode = new(transform, this);

        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { attackSequence, chaseSequence });
    }
}
