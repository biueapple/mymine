using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float health;
    public float Health { get { return health; } set {  health = Mathf.Clamp(value, 0, maxHealth); } }
    [SerializeField]
    private float healthRestoreRate;

    [SerializeField]
    private float chaseRange;
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Cover[] avaliableCovers;

    private Material material;

    private Transform bestCoverSpot;

    private BehaviorTreeNode topNode;

    private void Start()
    {
        Health = maxHealth;
        material = GetComponent<MeshRenderer>().material;
        ConstructBehaviourTree();
    }

    private void Update()
    {
        topNode.Evaluate();
        if(topNode.NodeState == NodeState.FALIERE)
        {
            SetColor(Color.red);
        }
        Health += Time.deltaTime * healthRestoreRate;
    }

    private void OnMouseDown()
    {
        Health -= 3;
    }

    private void ConstructBehaviourTree()
    {
        IsCovereAvaliableNode coverAvaliableNode = new (avaliableCovers, player, this);
        GoToCoverNode goToCoverNode = new (this);
        HealthNode healthNode = new(this, 5);
        IsCoveredNode isCoveredNode = new(player, transform);
        ChaseNode chaseNode = new(player, this);
        RangeNode chaseRangeNode = new(chaseRange, player, transform);
        RangeNode attackRangeNode = new(attackRange, player, transform);
        AttackNode attackNode = new(transform, this);

        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        BehaviorTreeSequence goToCoverSequence = new(new List<BehaviorTreeNode> { coverAvaliableNode, goToCoverNode });
        BehaviorTreeSelector findCoverSelector = new(new List<BehaviorTreeNode> { goToCoverSequence, chaseSequence });
        BehaviorTreeSelector tryToTakeCoverSelector = new(new List<BehaviorTreeNode> { isCoveredNode, findCoverSelector });
        BehaviorTreeSequence mainCoverSequence = new(new List<BehaviorTreeNode> { healthNode, tryToTakeCoverSelector });

        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { mainCoverSequence, attackSequence, chaseSequence });
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
