using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Zombie : Enemy
{
    private Animator animator;
    public Animator Animator { get { return animator; } }

    public override Transform Head => substances[1].transform;

    public override Transform Body => substances[0].transform;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        attackModule.MotionAdd(new ZombieAttack(this));

        animator = GetComponent<Animator>();

        MoveNode moveNode = new MoveNode(this, rotationSpeed);

        //��븦 �Ѿư��� ���
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, moveNode);
        //����� �Ÿ��� �Ѿư��� ���� ������ üũ�ϴ� ���
        RangeNode chaseRangeNode = new(chaseRange, GameManager.Instance.Players[0].transform, transform);
        //�������ϱ� �Ÿ� üũ�� �����̸� �Ѿư��� ������
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });

        //���� �Ÿ��� üũ�ϴ� ���
        RangeNode attackRangeNode = new(attackInRnage, GameManager.Instance.Players[0].transform, transform);
        //�����ϴ� ���
        AttackNode attackNode = new(this, GameManager.Instance.Players[0], moveNode);
        //�������ϱ� �����ϴ� ���� üũ�� �����̸� �����ϴ� ������
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        //���� �������� �ѱ� �������߿� �����ϴ°� ������
        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { attackSequence, chaseSequence });
    }

    // Update is called once per frame
    void Update()
    {
        //����
        topNode.Evaluate();
        //��� ��尡 ������
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
