using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goblin : Enemy , ICoveredEnemy, IFlockingEnemy
{
    private readonly Animator animator;
    public Animator Animator { get { return animator; } }

    public override Transform Head => substances[1].transform;

    public override Transform Body => substances[0].transform;

    [SerializeField]
    protected Cover[] avaliableCovers;

    protected Transform bestCoverSpot;
    public Transform BestCoverSpot { get { return bestCoverSpot; } set { bestCoverSpot = value; } }



    //�ڽ��� ����� ������ ����
    [SerializeField]
    protected Enemy[] colleague;
    public Enemy[] Colleague => colleague;
    [SerializeField]
    protected Enemy boss;
    public Enemy Boss => boss;

    [SerializeField]
    protected float avoiddance = 3;
    public float Avoidance => avoiddance;

    [SerializeField]
    protected float avoidPower = 1;
    public float AvoidPower => avoidPower;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        FlockingMoveNode flockingMoveNode = new (this, rotationSpeed);

        //���� �� �ִ� �� �߿��� ���� ���� �ڸ��� ã�� ���
        IsCovereAvaliableNode coverAvaliableNode = new(avaliableCovers, GameManager.Instance.Players[0].transform, this);
        //���������� �̵��ϴ� ���
        GoToCoverNode goToCoverNode = new(this, flockingMoveNode);
        //ü�� ��Ȳ�� üũ�ϴ� ���
        HealthNode healthNode = new(this, 5);
        //���� �����ִ��� Ȯ���ϴ� ���
        IsCoveredNode isCoveredNode = new(GameManager.Instance.Players[0].transform, transform);
        //�÷��̾ �Ѿư��� ���
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, flockingMoveNode);

        //�÷��̾ �Ѵ� ���� �ȿ� �ִ��� Ȯ���ϴ� ���
        RangeNode chaseRangeNode = new(chaseRange, GameManager.Instance.Players[0].transform, transform);
        //��ǥ�� ���� �� ���� ��ġ�� �ʰ� ���� ��ó�� �̵��ϴ� ���
        FlockNode flockNode = new(boss != null ? boss.transform : null, 3, flockingMoveNode, this);

        //���� �Ÿ��� üũ�ϴ� ���
        RangeNode attackInRangeNode = new(attackInRnage, GameManager.Instance.Players[0].transform, transform);
        //�ʹ� ������� üũ�ϴ� ���
        RangeNode minInRangeNode = new(minRange, GameManager.Instance.Players[0].transform, transform);

        //�������� ���
        RunNode runNode = new(GameManager.Instance.Players[0].transform, this, flockingMoveNode);
        //�����ϴ� ���
        AttackNode attackNode = new(this, GameManager.Instance.Players[0], flockingMoveNode);

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

    // Update is called once per frame
    void Update()
    {
        topNode.Evaluate();
        if (topNode.NodeState == NodeState.FALIERE)
        {
            SetColor(Color.red);
        }
    }

    protected override void Dead()
    {
        gameObject.SetActive(false);
    }
}
