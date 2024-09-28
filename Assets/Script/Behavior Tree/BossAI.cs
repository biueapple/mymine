using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossAI : EnemyAI
{
    protected override void ConstructBehaviourTree()
    {
        ChaseNode chaseNode = new(player, colleague.Select(c => c.transform).ToArray(), this, boss.transform, 3, 90, 0.5f);
        RangeNode chaseRangeNode = new(chaseRange, player, transform);
        RangeNode attackRangeNode = new(attackInRnage, player, transform);
        AttackNode attackNode = new(transform, this);

        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { attackSequence, chaseSequence });
    }
}
