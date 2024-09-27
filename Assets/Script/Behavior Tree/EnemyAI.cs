using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//enemyAi�� �⺻������ ������ ���� �����̸� �ڽſ��� ������ ���ٸ� ������ ���� �������� �̵��Ѵ�
//���ΰ� �ʹ� �����ٸ� ���θ� �о�� �������� �����δ�.
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

    //�ڽ��� ����� ������ ����
    [SerializeField]
    protected EnemyAI[] colleague;
    [SerializeField]
    protected EnemyAI boss;

    public float rotationSpeed;
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
        FlockNode flockNode = new(colleague.Select(c => c.transform).ToArray(), this, boss.transform, 2, 5, rotationSpeed, 0.5f);

        RangeNode attackInRangeNode = new(attackInRnage, player, transform);
        RangeNode minInRangeNode = new (minRange, player, transform);
        BehaviorTreeInverter minRangeNode = new(minInRangeNode);
        RunNode runNode = new(player, this);
        AttackNode attackNode = new(transform, this);

        //apart �־�����
        BehaviorTreeSequence apartSequence = new(new List<BehaviorTreeNode> { minInRangeNode, runNode });
        //Approach �ٰ�����
        BehaviorTreeSelector approachSelector = new(new List<BehaviorTreeNode> { apartSequence, chaseNode });
        //�ʹ� �־ �� ���� ������ �� �ൿ���� ������� ���⿡ ����Ʈ�� �߰��ϸ� �� ù��° ���� ������ �÷��̾ �ʹ� ���� üũ�ϴ� ���
        //������ ������ �޾� ������ �翡 ������ �ϸ鼭 ������ �ʹ� ������ �ʵ��� �ؾ���
        BehaviorTreeSequence tooFar = new(new List<BehaviorTreeNode> { new BehaviorTreeInverter(chaseRangeNode), flockNode });

        BehaviorTreeSelector chaseSelector = new(new List<BehaviorTreeNode> { tooFar, approachSelector });

        BehaviorTreeSequence attackOutSequence = new(new List<BehaviorTreeNode> { minRangeNode, attackNode });

        BehaviorTreeSequence attackInSequence = new(new List<BehaviorTreeNode> { attackInRangeNode, attackOutSequence });

        BehaviorTreeSequence goToCoverSequence = new(new List<BehaviorTreeNode> { coverAvaliableNode, goToCoverNode });
        BehaviorTreeSelector findCoverSelector = new(new List<BehaviorTreeNode> { goToCoverSequence, chaseSelector });
        BehaviorTreeSelector tryToTakeCoverSelector = new(new List<BehaviorTreeNode> { isCoveredNode, findCoverSelector });
        BehaviorTreeSequence mainCoverSequence = new(new List<BehaviorTreeNode> { healthNode, tryToTakeCoverSelector });

        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { mainCoverSequence, attackInSequence, chaseSelector });
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
