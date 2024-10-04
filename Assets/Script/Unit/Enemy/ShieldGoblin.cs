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



    //자신의 동료와 리더를 참조
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

        //상대를 쫓아가는 노드
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, flockingMoveNode);
        //상대의 거리가 쫓아가는 범위 안인지 체크하는 노드
        RangeNode chaseRangeNode = new(chaseRange, GameManager.Instance.Players[0].transform, transform);
        //시퀸스니까 거리 체크가 성공이면 쫓아가는 시퀸스
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseRangeNode, chaseNode });

        //공격 거리를 체크하는 노드
        RangeNode attackRangeNode = new(attackInRnage, GameManager.Instance.Players[0].transform, transform);
        //공격하는 노드
        AttackNode attackNode = new(this, GameManager.Instance.Players[0], flockingMoveNode);
        //시퀸스니까 공격하는 범위 체크가 성공이면 공격하는 시퀸스
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRangeNode, attackNode });

        //공격 시퀸스와 쫓기 시퀸스중에 성공하는거 실행함
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
