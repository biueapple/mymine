using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGoblin : Enemy, IFlockingEnemy
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

        FlockingMoveNode flockingMoveNode = new(this, rotationSpeed);

        //��븦 �Ѿư��� ���
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, flockingMoveNode);
        //����� �Ÿ��� �Ѿư��� ���� ������ üũ�ϴ� ���
        RangeNode chaseRangeNode = new(chaseRange, GameManager.Instance.Players[0].transform, transform);
        //�������ϱ� �Ÿ� üũ�� �����̸� �Ѿư��� ������
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });

        //���� �Ÿ��� üũ�ϴ� ���
        RangeNode attackRangeNode = new(attackInRnage, GameManager.Instance.Players[0].transform, transform);
        //�����ϴ� ���
        AttackNode attackNode = new(this, GameManager.Instance.Players[0], flockingMoveNode);
        //�������ϱ� �����ϴ� ���� üũ�� �����̸� �����ϴ� ������
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        //���� �������� �ѱ� �������߿� �����ϴ°� ������
        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { attackSequence, chaseSequence });
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
