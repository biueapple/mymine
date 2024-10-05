using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���� ã���� ���� (����� �����غôµ� �ϴ��� �����߿� ���а� �ִٸ� �װ� ����ϰ� ���ٸ� �׳� ���� �ϴ°ɷ�) ������� ����� ���ٸ� ���� �÷��̾�� ���� ��������
//����� ���� ����
//ü���� �������� ����
//������ ���ٰ� ����� �ϳ�?
public class WarriorGoblin : Enemy, ICoveredEnemy, IFlockingEnemy
{
    private readonly Animator animator;
    public Animator Animator { get { return animator; } }

    public override Transform Head => substances[1].transform;

    public override Transform Body => substances[0].transform;

    [SerializeField]
    protected Cover[] avaliableCovers;

    protected Transform bestCoverSpot;
    public Transform BestCoverSpot { get { return bestCoverSpot; } set { bestCoverSpot = value; } }

    //�� �Ÿ����ʹ� �÷��̾ ���� �̵�
    [SerializeField]
    protected float attackDistance;

    //�ڽ��� ����� ������ ����
    [SerializeField]
    protected Enemy[] colleague;
    public Enemy[] Colleague => colleague;
    [SerializeField]
    protected Enemy boss;
    public Enemy Boss => boss;

    [SerializeField]
    protected float avoidance = 3;
    public float Avoidance => avoidance;

    [SerializeField]
    protected float avoidPower = 1;
    public float AvoidPower => avoidPower;


    //������ ���� �� �ִ� �Ʊ��� ã�� ���� ShieldColleagueNode �������� ���� �տ� ������ �ɵ�
    //����
    //  ü�� Ȯ�� (HealthNode)
    //  �̹� �������� Ȯ�� (Selector => IsCoveredNode, ã�� Selector)
    //      ������ ã�� (�ؿ� �ΰ��� Selector)
    //          ���������� �̵� (Sequence => IsCovereAvaliableNode {���� ���� ã�� ���}, GoToCoverNode)
    //          ���ٸ� �׳� �ѱ�
    //����
    //  ���� �����Ÿ� ������ (Sequence => RangeNode{Max}, RangeNode{Min}, attackNode)
    //�ѱ�
    // �����Ÿ� ������ (Selector => RangeNode)
    //      ���̶�� ���� ���� (FlockNode)
    //      ���̶�� �߰� (ChaseNode)
    //          �ʹ� �����ٸ� �־����� (�ش���� ����)
    new void Start()
    {
        base.Start();

        FlockingMoveNode flockingMoveNode = new(this, rotationSpeed);

        //�߰�
        //ù��° ��� �׳� �Ѿư���
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, flockingMoveNode);

        //���� �Ÿ��� ��� �̵��ؾ� �� �Ÿ���
        RangeNode shieldRange = new(attackDistance, GameManager.Instance.Players[0].transform, transform);
        //�����߿� �� ������ ���а� �ִ���
        ShieldColleagueNode shieldColleagueNode = new(colleague);
        //�������� �̵�
        ShieldColleagueMoveNode shieldColleagueMoveNode = new(shieldColleagueNode, this, flockingMoveNode);
        //�ι�° ��� ���� �ڿ� ��� �̵��ϱ�
        BehaviorTreeSequence shieldColleagueSequence = new(new List<BehaviorTreeNode> { new BehaviorTreeInverter(shieldRange), shieldColleagueNode, shieldColleagueMoveNode });

        //�������� �߰ݹ�� 2������ �����Ѱ��� ����
        BehaviorTreeSelector chaseWay = new(new List<BehaviorTreeNode> { shieldColleagueSequence, chaseNode });

        //�÷��̾ ���� �Ÿ����� �Ǵ��ϴ� ���
        RangeNode chaseInRange = new(chaseRange, GameManager.Instance.Players[0].transform, transform);
        //�÷��̾� ��Ÿ��� �߰� ������ ��Ÿ���� �߰� ����� �����ؼ� ����
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseInRange, chaseWay });


        //���� ����
        FlockNode flockNode = new(boss != null ? boss.transform : null, 3, flockingMoveNode, this);

        //�÷��̾ �Ѿư��ų� ���� �����ϰų�
        BehaviorTreeSelector chaseSelector = new(new List<BehaviorTreeNode> { chaseSequence, flockNode });

        //����
        //��Ÿ� üũ
        RangeNode attackRange = new(attackInRnage, GameManager.Instance.Players[0].transform, transform);
        //����
        AttackNode attackNode = new(this, GameManager.Instance.Players[0], flockingMoveNode);
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRange, attackNode });

        //����
        //���� �� �ִ� �� �߿��� ���� ���� �ڸ��� ã�� ���
        IsCovereAvaliableNode coverAvaliableNode = new(avaliableCovers, GameManager.Instance.Players[0].transform, this);
        //���������� �̵��ϴ� ���
        GoToCoverNode goToCoverNode = new(this, flockingMoveNode);
        //���� ���� ���� ���� ã�� �̵��ϴ� ������
        BehaviorTreeSequence goToCoverSequence = new(new List<BehaviorTreeNode> { coverAvaliableNode, goToCoverNode });
        //���� ���� �ִٸ� �װ��� ã�� �̵��ϰ� ���ٸ� �÷��̾ �ѵ��� �ϴ� ������
        BehaviorTreeSelector findCoverSelector = new(new List<BehaviorTreeNode> { goToCoverSequence, chaseSelector });

        //���� �����ִ��� Ȯ���ϴ� ���
        IsCoveredNode isCoveredNode = new(GameManager.Instance.Players[0].transform, transform);
        //�̹� �����ִ����� üũ�ϰ� ���ٸ� ã�� ������
        BehaviorTreeSelector tryToTakeCoverSelector = new(new List<BehaviorTreeNode> { isCoveredNode, findCoverSelector });

        //ü�� ��Ȳ�� üũ�ϴ� ���
        HealthNode healthNode = new(this, 5);
        //ü���� ������ Ȯ�� �� ����� �ϴ� ���� ã�� ������
        BehaviorTreeSequence mainCoverSequence = new(new List<BehaviorTreeNode> { healthNode, tryToTakeCoverSelector });

        //����� �ϴ��� üũ �� ���� ���� �������� üũ �� �÷��̾ ���� ���� �÷��̾ ���� ���̶�� �� �ൿ�� ����
        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { mainCoverSequence, attackSequence, chaseSelector });
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
