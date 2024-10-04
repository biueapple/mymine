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

        FlockingMoveNode flockingMoveNode = new (this, rotationSpeed);

        //숨을 수 있는 곳 중에서 가장 좋은 자리를 찾는 노드
        IsCovereAvaliableNode coverAvaliableNode = new(avaliableCovers, GameManager.Instance.Players[0].transform, this);
        //숨을곳으로 이동하는 노드
        GoToCoverNode goToCoverNode = new(this, flockingMoveNode);
        //체력 상황을 체크하는 노드
        HealthNode healthNode = new(this, 5);
        //현재 숨어있는지 확인하는 노드
        IsCoveredNode isCoveredNode = new(GameManager.Instance.Players[0].transform, transform);
        //플레이어를 쫓아가는 노드
        ChaseNode chaseNode = new(GameManager.Instance.Players[0], this, flockingMoveNode);

        //플레이어가 쫓는 범위 안에 있는지 확인하는 노드
        RangeNode chaseRangeNode = new(chaseRange, GameManager.Instance.Players[0].transform, transform);
        //목표가 없을 때 서로 겹치지 않게 보스 근처로 이동하는 노드
        FlockNode flockNode = new(boss != null ? boss.transform : null, 3, flockingMoveNode, this);

        //공격 거리를 체크하는 노드
        RangeNode attackInRangeNode = new(attackInRnage, GameManager.Instance.Players[0].transform, transform);
        //너무 가까운지 체크하는 노드
        RangeNode minInRangeNode = new(minRange, GameManager.Instance.Players[0].transform, transform);

        //도망가는 노드
        RunNode runNode = new(GameManager.Instance.Players[0].transform, this, flockingMoveNode);
        //공격하는 노드
        AttackNode attackNode = new(this, GameManager.Instance.Players[0], flockingMoveNode);

        //apart 멀어지다 너무 가깝다면 도망가도록 하는 시퀸스
        BehaviorTreeSequence apartSequence = new(new List<BehaviorTreeNode> { minInRangeNode, runNode });
        //Approach 다가가다 위에 시퀸스가 너무 가깝지 않아 실패를 리턴한다면 chaseNode 노드를 실행하는 셀렉터
        BehaviorTreeSelector approachSelector = new(new List<BehaviorTreeNode> { apartSequence, chaseNode });

        //너무 멀어서 할 일이 없을때 할 행동들을 순서대로 여기에 리스트로 추가하면 됨 첫번째 노드는 정말로 플레이어가 너무 먼지 체크하는 노드
        //리더의 영향을 받아 리더의 곁에 가도록 하면서 동료들과 너무 가깝지 않도록 해야함
        //플레이어가 너무 멀어서 쫓지도 않는 상태일때 보스 옆에 있는 노드를 실행시키는 시퀸스
        BehaviorTreeSequence tooFar = new(new List<BehaviorTreeNode> { new BehaviorTreeInverter(chaseRangeNode), flockNode });

        //적이 쫓을 거리 안에 있는지 없는지를 체크해 작동시키는 셀렉터
        BehaviorTreeSelector chaseSelector = new(new List<BehaviorTreeNode> { tooFar, approachSelector });

        //적이 거리 안에 있다면 공격 시퀸스를 수행하는 시퀸스 (적이 공격 사거리 안인지 확인한 후 너무 가깝지 않은지 확인하는 노드와 공격 노드를 가진 시퀸스를 작동시킴)
        BehaviorTreeSequence attackInSequence = new(new List<BehaviorTreeNode> { attackInRangeNode, new BehaviorTreeInverter(minInRangeNode), attackNode });

        //가장 좋은 숨을 곳을 찾아 이동하는 시퀸스
        BehaviorTreeSequence goToCoverSequence = new(new List<BehaviorTreeNode> { coverAvaliableNode, goToCoverNode });
        //숨을 곳이 있다면 그곳을 찾아 이동하고 없다면 플레이어를 쫓도록 하는 셀렉터
        BehaviorTreeSelector findCoverSelector = new(new List<BehaviorTreeNode> { goToCoverSequence, chaseSelector });
        //이미 숨어있는지를 체크하고 없다면 찾는 셀렉터
        BehaviorTreeSelector tryToTakeCoverSelector = new(new List<BehaviorTreeNode> { isCoveredNode, findCoverSelector });
        //체력이 적은지 확인 후 숨어야 하는 곳을 찾는 셀렉터
        BehaviorTreeSequence mainCoverSequence = new(new List<BehaviorTreeNode> { healthNode, tryToTakeCoverSelector });

        //숨어야 하는지 체크 후 적을 공격 가능한지 체크 후 플레이어를 쫓음 만약 플레이어가 감지 밖이라면 할 행동도 포함
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
