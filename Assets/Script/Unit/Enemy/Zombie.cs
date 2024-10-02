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

        //상대를 쫓아가는 노드
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, moveNode);
        //상대의 거리가 쫓아가는 범위 안인지 체크하는 노드
        RangeNode chaseRangeNode = new(chaseRange, GameManager.Instance.Players[0].transform, transform);
        //시퀸스니까 거리 체크가 성공이면 쫓아가는 시퀸스
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });

        //공격 거리를 체크하는 노드
        RangeNode attackRangeNode = new(attackInRnage, GameManager.Instance.Players[0].transform, transform);
        //공격하는 노드
        AttackNode attackNode = new(this, GameManager.Instance.Players[0], moveNode);
        //시퀸스니까 공격하는 범위 체크가 성공이면 공격하는 시퀸스
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        //공격 시퀸스와 쫓기 시퀸스중에 성공하는거 실행함
        topNode = new BehaviorTreeSelector(new List<BehaviorTreeNode> { attackSequence, chaseSequence });
    }

    // Update is called once per frame
    void Update()
    {
        //실행
        topNode.Evaluate();
        //모든 노드가 실패임
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
