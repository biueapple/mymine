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
        //���� �� �ִ� �� �߿��� ���� ���� �ڸ��� ã�� ���
        IsCovereAvaliableNode coverAvaliableNode = new (avaliableCovers, player, this);
        //���������� �̵��ϴ� ���
        GoToCoverNode goToCoverNode = new (colleague.Select(c => c.transform).ToArray(), this, boss.transform, 3, rotationSpeed, 0.5f);
        //ü�� ��Ȳ�� üũ�ϴ� ���
        HealthNode healthNode = new(this, 5);
        //���� �����ִ��� Ȯ���ϴ� ���
        IsCoveredNode isCoveredNode = new(player, transform);
        //�÷��̾ �Ѿư��� ���
        ChaseNode chaseNode = new(player, colleague.Select(c => c.transform).ToArray(), this, boss.transform, 3, rotationSpeed, 0.5f);

        //�÷��̾ �Ѵ� ���� �ȿ� �ִ��� Ȯ���ϴ� ���
        RangeNode chaseRangeNode = new(chaseRange, player, transform);
        //��ǥ�� ���� �� ���� ��ġ�� �ʰ� ���� ��ó�� �̵��ϴ� ���
        FlockNode flockNode = new(colleague.Select(c => c.transform).ToArray(), this, boss.transform, 2, 3, rotationSpeed, 0.5f);

        //���� �Ÿ��� üũ�ϴ� ���
        RangeNode attackInRangeNode = new(attackInRnage, player, transform);
        //�ʹ� ������� üũ�ϴ� ���
        RangeNode minInRangeNode = new (minRange, player, transform);

        //�������� ���
        RunNode runNode = new(player, colleague.Select(c => c.transform).ToArray(), this, boss.transform, 3, rotationSpeed, 0.5f);
        //�����ϴ� ���
        AttackNode attackNode = new(transform, this);

        //apart �־����� �ʹ� �����ٸ� ���������� �ϴ� ������
        BehaviorTreeSequence apartSequence = new(new List<BehaviorTreeNode> { minInRangeNode, runNode });
        //Approach �ٰ����� ���� �������� �ʹ� ������ �ʾ� ���и� �����Ѵٸ� chaseNode ��带 �����ϴ� ������
        BehaviorTreeSelector approachSelector = new(new List<BehaviorTreeNode> { apartSequence, chaseNode });

        //�ʹ� �־ �� ���� ������ �� �ൿ���� ������� ���⿡ ����Ʈ�� �߰��ϸ� �� ù��° ���� ������ �÷��̾ �ʹ� ���� üũ�ϴ� ���
        //������ ������ �޾� ������ �翡 ������ �ϸ鼭 ������ �ʹ� ������ �ʵ��� �ؾ���
        //�÷��̾ �ʹ� �־ ������ �ʴ� �����϶� ���� ���� �ִ� ��带 �����Ű�� ������
        BehaviorTreeSequence tooFar = new(new List<BehaviorTreeNode> { new BehaviorTreeInverter(chaseRangeNode), flockNode });

        //���� ���� �Ÿ� �ȿ� �ִ��� �������� üũ�� �۵���Ű�� ������
        BehaviorTreeSelector chaseSelector = new(new List<BehaviorTreeNode> { tooFar, approachSelector });

        //���� �Ÿ� �ȿ� �ִٸ� ���� �������� �����ϴ� ������ (���� ���� ��Ÿ� ������ Ȯ���� �� �ʹ� ������ ������ Ȯ���ϴ� ���� ���� ��带 ���� �������� �۵���Ŵ)
        BehaviorTreeSequence attackInSequence = new(new List<BehaviorTreeNode> { attackInRangeNode, new BehaviorTreeInverter(minInRangeNode), attackNode });

        //���� ���� ���� ���� ã�� �̵��ϴ� ������
        BehaviorTreeSequence goToCoverSequence = new(new List<BehaviorTreeNode> { coverAvaliableNode, goToCoverNode });
        //���� ���� �ִٸ� �װ��� ã�� �̵��ϰ� ���ٸ� �÷��̾ �ѵ��� �ϴ� ������
        BehaviorTreeSelector findCoverSelector = new(new List<BehaviorTreeNode> { goToCoverSequence, chaseSelector });
        //�̹� �����ִ����� üũ�ϰ� ���ٸ� ã�� ������
        BehaviorTreeSelector tryToTakeCoverSelector = new(new List<BehaviorTreeNode> { isCoveredNode, findCoverSelector });
        //ü���� ������ Ȯ�� �� ����� �ϴ� ���� ã�� ������
        BehaviorTreeSequence mainCoverSequence = new(new List<BehaviorTreeNode> { healthNode, tryToTakeCoverSelector });

        //����� �ϴ��� üũ �� ���� ���� �������� üũ �� �÷��̾ ���� ���� �÷��̾ ���� ���̶�� �� �ൿ�� ����
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
