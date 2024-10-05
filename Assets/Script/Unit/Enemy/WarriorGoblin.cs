using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//숨을 곳을 찾으며 전진 (방법을 생각해봤는데 일단은 팀원중에 방패가 있다면 그걸 사용하고 없다면 그냥 전진 하는걸로) 어느정도 가까워 졌다면 이젠 플레이어에게 가는 방향으로
//가까워 지면 공격
//체력이 적어지면 도망
//도망을 가다가 숨어야 하나?
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


    //동료중 숨길 수 있는 아군을 찾는 노드는 ShieldColleagueNode 시퀸스중 가장 앞에 놓으면 될듯
    //숨기
    //  체력 확인 (HealthNode)
    //  이미 숨었는지 확인 (Selector => IsCoveredNode, 찾기 Selector)
    //      숨을곳 찾기 (밑에 두개를 Selector)
    //          숨을곳으로 이동 (Sequence => IsCovereAvaliableNode {숨을 곳을 찾는 노드}, GoToCoverNode)
    //          없다면 그냥 쫓기
    //공격
    //  공격 사정거리 안인지 (Sequence => RangeNode{Max}, RangeNode{Min}, attackNode)
    //쫓기
    // 사정거리 안인지 (Selector => RangeNode)
    //      밖이라면 서로 응집 (FlockNode)
    //      안이라면 추격 (ChaseNode)
    //          너무 가깝다면 멀어지기 (해당되지 않음)
    new void Start()
    {
        base.Start();

        FlockingMoveNode flockingMoveNode = new(this, rotationSpeed);

        //추격
        //첫번째 방법 그냥 쫓아가기
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, flockingMoveNode);

        //아직 거리가 숨어서 이동해야 할 거리임
        RangeNode shieldRange = new(attackDistance, GameManager.Instance.Players[0].transform, transform);
        //팀원중에 날 숨겨줄 방패가 있는지
        ShieldColleagueNode shieldColleagueNode = new(colleague);
        //팀원에게 이동
        ShieldColleagueMoveNode shieldColleagueMoveNode = new(shieldColleagueNode, this, flockingMoveNode);
        //두번째 방법 팀원 뒤에 숨어서 이동하기
        BehaviorTreeSequence shieldColleagueSequence = new(new List<BehaviorTreeNode> { new BehaviorTreeInverter(shieldRange), shieldColleagueNode, shieldColleagueMoveNode });

        //보유중인 추격방법 2가지중 가능한것을 실행
        BehaviorTreeSelector chaseWay = new(new List<BehaviorTreeNode> { shieldColleagueSequence, chaseNode });

        //플레이어가 쫓을 거리인지 판단하는 노드
        RangeNode chaseInRange = new(chaseRange, GameManager.Instance.Players[0].transform, transform);
        //플레이어 사거리가 추격 가능한 사거리라면 추격 방법을 선택해서 실행
        BehaviorTreeSequence chaseSequence = new(new List<BehaviorTreeNode> { chaseInRange, chaseWay });


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
        //숨을 수 있는 곳 중에서 가장 좋은 자리를 찾는 노드
        IsCovereAvaliableNode coverAvaliableNode = new(avaliableCovers, GameManager.Instance.Players[0].transform, this);
        //숨을곳으로 이동하는 노드
        GoToCoverNode goToCoverNode = new(this, flockingMoveNode);
        //가장 좋은 숨을 곳을 찾아 이동하는 시퀸스
        BehaviorTreeSequence goToCoverSequence = new(new List<BehaviorTreeNode> { coverAvaliableNode, goToCoverNode });
        //숨을 곳이 있다면 그곳을 찾아 이동하고 없다면 플레이어를 쫓도록 하는 셀렉터
        BehaviorTreeSelector findCoverSelector = new(new List<BehaviorTreeNode> { goToCoverSequence, chaseSelector });

        //현재 숨어있는지 확인하는 노드
        IsCoveredNode isCoveredNode = new(GameManager.Instance.Players[0].transform, transform);
        //이미 숨어있는지를 체크하고 없다면 찾는 셀렉터
        BehaviorTreeSelector tryToTakeCoverSelector = new(new List<BehaviorTreeNode> { isCoveredNode, findCoverSelector });

        //체력 상황을 체크하는 노드
        HealthNode healthNode = new(this, 5);
        //체력이 적은지 확인 후 숨어야 하는 곳을 찾는 셀렉터
        BehaviorTreeSequence mainCoverSequence = new(new List<BehaviorTreeNode> { healthNode, tryToTakeCoverSelector });

        //숨어야 하는지 체크 후 적을 공격 가능한지 체크 후 플레이어를 쫓음 만약 플레이어가 감지 밖이라면 할 행동도 포함
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
