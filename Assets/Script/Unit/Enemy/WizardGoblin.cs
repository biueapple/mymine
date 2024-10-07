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

    //이 거리부터는 플레이어를 향해 이동
    [SerializeField]
    protected float attackDistance;

    //자신의 동료와 리더를 참조
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



    //체력이 적은 동료를 찾고 회복을 시켜주는 보스 고블린
    //본인의 체력을 확인
    //  체력이 적다면 숨을 수 있다면 숨기
    //  체력이 많다면 주위 동료를 확인
    //      체력이 적은 동료가 있다면 그 근처로 이동
    //          체력 회복

    //(본인의 체력이 많고 동료들의 체력도 충분한 상태)
    //팀 뒤에 숨기
    //  플레이어에서 동료들 방향으로 ray를 쏴서 10거리만큼 갔을때 가장 가까운 곳으로 이동

    //공격
    //  원거리 마법 공격

    //추격
    //  10의 거리만큼 가까이 이동
    new void Start()
    {
        base.Start();

        FlockingMoveNode flockingMoveNode = new(this, rotationSpeed);


        //추격
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, flockingMoveNode);
        //플레이어가 쫓을 거리인지 판단하는 노드
        RangeNode chaseInRange = new(chaseRange, GameManager.Instance.Players[0].transform, transform);

        //플레이어 사거리가 추격 가능한 사거리라면 추격 방법을 선택해서 실행
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseInRange, chaseNode });

        //서로 응집
        FlockNode flockNode = new(boss != null ? boss.transform : null, 3, flockingMoveNode, this);

        //플레이어를 쫓아가거나 서로 응집하거나
        BehaviorTreeSelector chaseSelector = new(new List<BehaviorTreeNode> { chaseSequence, flockNode });


        //공격
        //사거리 체크
        RangeNode attackRange = new(attackInRnage, GameManager.Instance.Players[0].transform, transform);
        //공격
        AttackNode attackNode = new(this, GameManager.Instance.Players[0], flockingMoveNode);
        BehaviorTreeSequence attackSequence = new(new List<BehaviorTreeNode> { attackRange, attackNode });

        //숨기



        //회복
        HealthNode[] colleagueHealths = new HealthNode[colleague.Length];
        List<BehaviorTreeNode> treeNodes = new List<BehaviorTreeNode>();
        for (int i = 0; i < colleagueHealths.Length; i++)
        {
            colleagueHealths[i] = new HealthNode(colleague[i], 5);
            treeNodes.Add(new BehaviorTreeInverter(colleagueHealths[i]));
        }
        //모든 동료들의 체력을 확인해서 5보다 낮은 아군이 있다면 원래는 성공을 리턴하지만 inverter 했기에 실패를 리턴함 한명이라도 낮은 친구가 있다면 실패를 리턴함 다시 시퀸스를 inverter해서 결과를 바꿀 예정
        BehaviorTreeSequence colleagueHealthSequence = new(treeNodes);
        //체력을 회복시키는 노드

        //모든 동료들의 체력을 확인해서 하나라도 5보다 낮으면 실패를 리턴하는 시퀸스를 inverter해 성공을 리턴하도록 변환 그리고 두번째 노드에 회복시키는 노드를 사용한다는 느낌
        BehaviorTreeSequence HealthSequence = new(new List<BehaviorTreeNode> { new BehaviorTreeInverter(colleagueHealthSequence)/*, 여기에 회복시키는 노드가 필요함*/ });
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
