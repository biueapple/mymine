using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth;
    [SerializeField]
    protected float health;
    public float Health { get { return health; } set {  health = Mathf.Clamp(value, 0, maxHealth); } }
    [SerializeField]
    protected float healthRestoreRate;

    [SerializeField]
    protected float chaseRange;
    [SerializeField]
    protected float minRange;
    [SerializeField]
    protected float attackInRnage;

    [SerializeField]
    protected Transform player;
    [SerializeField]
    protected Cover[] avaliableCovers;

    protected Material material;

    protected Transform bestCoverSpot;

    protected BehaviorTreeNode topNode;

    protected void Start()
    {
        Health = maxHealth;
        material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        ConstructBehaviourTree();
    }

    protected void Update()
    {
        topNode.Evaluate();
        if(topNode.NodeState == NodeState.FALIERE)
        {
            SetColor(Color.red);
        }
        //Health += Time.deltaTime * healthRestoreRate;
    }

    protected void OnMouseDown()
    {
        Health -= 3;
    }

    protected virtual void ConstructBehaviourTree()
    {
        IsCovereAvaliableNode coverAvaliableNode = new (avaliableCovers, player, this);
        GoToCoverNode goToCoverNode = new (this);
        HealthNode healthNode = new(this, 5);
        IsCoveredNode isCoveredNode = new(player, transform);
        ChaseNode chaseNode = new(player, this);
        RangeNode chaseRangeNode = new(chaseRange, player, transform);
        RangeNode attackInRangeNode = new(attackInRnage, player, transform);
        RangeNode minInRangeNode = new RangeNode(minRange, player, transform);
        BehaviorTreeInverter minRangeNode = new(minInRangeNode);
        RunNode runNode = new(player, this);
        AttackNode attackNode = new(transform, this);

        //apart 멀어지다
        BehaviorTreeSequence apartSequence = new(new List<BehaviorTreeNode> { minInRangeNode, runNode });
        //Approach 다가가다
        BehaviorTreeSelector approachSelector = new(new List<BehaviorTreeNode> { apartSequence, chaseNode });


        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, approachSelector });

        BehaviorTreeSequence attackOutSequence = new(new List<BehaviorTreeNode> { minRangeNode, attackNode });

        BehaviorTreeSequence attackInSequence = new(new List<BehaviorTreeNode> { attackInRangeNode, attackOutSequence });

        BehaviorTreeSequence goToCoverSequence = new(new List<BehaviorTreeNode> { coverAvaliableNode, goToCoverNode });
        BehaviorTreeSelector findCoverSelector = new(new List<BehaviorTreeNode> { goToCoverSequence, chaseSequence });
        BehaviorTreeSelector tryToTakeCoverSelector = new(new List<BehaviorTreeNode> { isCoveredNode, findCoverSelector });
        BehaviorTreeSequence mainCoverSequence = new(new List<BehaviorTreeNode> { healthNode, tryToTakeCoverSelector });

        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { mainCoverSequence, attackInSequence, chaseSequence });
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform transform)
    {
        this.bestCoverSpot = transform;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }
}
