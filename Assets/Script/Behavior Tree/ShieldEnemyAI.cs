using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShieldEnemyAI : EnemyAI
{
    protected override void ConstructBehaviourTree()
    {
        ChaseNode chaseNode = new(player, colleague.Select(c => c.transform).ToArray(), this, boss.transform, 3, 90, 0.5f);
        RangeNode chaseRangeNode = new(chaseRange, player, transform);
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });

        FlockNode flockNode = new(colleague.Select(c => c.transform).ToArray(), this, boss.transform, 2, 3, 90, 0.5f);
        BehaviorTreeSequence tooFar = new(new List<BehaviorTreeNode> { new BehaviorTreeInverter(chaseRangeNode), flockNode });

        RangeNode attackRangeNode = new(attackInRnage, player, transform);
        AttackNode attackNode = new(transform, this);

        BehaviorTreeSelector chaseSelector = new(new List<BehaviorTreeNode>() { chaseSequence, tooFar });
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { attackSequence, chaseSelector });
    }
}
