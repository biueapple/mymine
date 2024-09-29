using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossAI : EnemyAI
{
    protected override void ConstructBehaviourTree()
    {
        //ChaseNode chaseNode = new(player, colleague.Select(c => c.transform).ToArray(), this, boss.transform, 3, 90, 0.5f, power);
        //RangeNode chaseRangeNode = new(chaseRange, player, transform);
        //RangeNode attackRangeNode = new(attackInRnage, player, transform);
        //AttackNode attackNode = new(colleague.Select(c => c.transform).ToArray(), this, boss.transform, 3, rotationSpeed, 0.5f, power);

        //BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });
        //BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        //topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { attackSequence, chaseSequence });
    }
}
