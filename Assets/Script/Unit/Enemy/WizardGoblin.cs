using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WizardGoblin : Enemy, IFlockingEnemy
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



    //ü���� ���� ���Ḧ ã�� ȸ���� �����ִ� ���� ���
    //������ ü���� Ȯ��
    //  ü���� ���ٸ� ���� �� �ִٸ� ����
    //  ü���� ���ٸ� ���� ���Ḧ Ȯ��
    //      ü���� ���� ���ᰡ �ִٸ� �� ��ó�� �̵�
    //          ü�� ȸ��

    //(������ ü���� ���� ������� ü�µ� ����� ����)
    //�� �ڿ� ����
    //  �÷��̾�� ����� �������� ray�� ���� 10�Ÿ���ŭ ������ ���� ����� ������ �̵�

    //����
    //  ���Ÿ� ���� ����

    //�߰�
    //  10�� �Ÿ���ŭ ������ �̵�
    new void Start()
    {
        base.Start();

        FlockingMoveNode flockingMoveNode = new(this, rotationSpeed);


        //�߰�
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, flockingMoveNode);
        //�÷��̾ ���� �Ÿ����� �Ǵ��ϴ� ���
        RangeNode chaseInRange = new(chaseRange, GameManager.Instance.Players[0].transform, transform);

        //�÷��̾� ��Ÿ��� �߰� ������ ��Ÿ���� �߰� ����� �����ؼ� ����
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseInRange, chaseNode });

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



        //ȸ��
        HealthNode[] colleagueHealths = new HealthNode[colleague.Length];
        List<BehaviorTreeNode> treeNodes = new List<BehaviorTreeNode>();
        for (int i = 0; i < colleagueHealths.Length; i++)
        {
            colleagueHealths[i] = new HealthNode(colleague[i], 5);
            treeNodes.Add(new BehaviorTreeInverter(colleagueHealths[i]));
        }
        //��� ������� ü���� Ȯ���ؼ� 5���� ���� �Ʊ��� �ִٸ� ������ ������ ���������� inverter �߱⿡ ���и� ������ �Ѹ��̶� ���� ģ���� �ִٸ� ���и� ������ �ٽ� �������� inverter�ؼ� ����� �ٲ� ����
        BehaviorTreeSequence colleagueHealthSequence = new(treeNodes);
        //ü���� ȸ����Ű�� ���

        //��� ������� ü���� Ȯ���ؼ� �ϳ��� 5���� ������ ���и� �����ϴ� �������� inverter�� ������ �����ϵ��� ��ȯ �׸��� �ι�° ��忡 ȸ����Ű�� ��带 ����Ѵٴ� ����
        BehaviorTreeSequence HealthSequence = new(new List<BehaviorTreeNode> { new BehaviorTreeInverter(colleagueHealthSequence)/*, ���⿡ ȸ����Ű�� ��尡 �ʿ���*/ });
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
