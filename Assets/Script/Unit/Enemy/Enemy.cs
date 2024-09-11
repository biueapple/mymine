using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Unit
{
    //죽었을때 떨굴 아이템
    [SerializeField]
    protected int[] itemID;

    [SerializeField]
    [Range(1, 3)]
    protected int amount;

    protected EnemyState state;
    public EnemyState State { get { return state; } }

    protected AttackModule attackModule;
    public AttackModule AttackModule { get { return attackModule; } }

    //
    protected Pathfinder pathfinder;
    public Pathfinder Pathfinder { get { return pathfinder; } }
    protected AutoMove autoMove;
    public AutoMove AutoMove { get { return autoMove; } }
    protected DistanceDetection distanceDetection;
    public DistanceDetection DistanceDetection { get {  return distanceDetection; } }

    JumpInputMove jumpSystem;
    protected void Start()
    {
        autoMove = new(this);
        jumpSystem = new(moveSystem.Machine);

        moveSystem.AddMoveMode(autoMove);
        moveSystem.AddMoveMode(new AutoJump(jumpSystem, this));

        moveSystem.AddMoveMode(new GravityForce(moveSystem.Machine));

        moveSystem.AddMoveMode(new WorldMovePhysicsShift(moveSystem.Machine, GetComponent<Unit>()));

        distanceDetection = new DistanceDetection(transform, 10);

        pathfinder = new Pathfinder(this);

        attackModule = new AttackModule(1, STAT);
    }

    public void ChangeState(EnemyState _state)
    {
        if(state != null)
            state.Exit();

        state = _state;

        if (state != null)
            state.Enter();
    }

    public override float Hit(AttackInformation attackInformation)
    {
        float damage = base.Hit(attackInformation);
        if(stat.HP <= 0)
        {
            //아이템 드랍
            FlowManager.Instance.DropItem(transform.position, itemID[Random.Range(0, itemID.Length)], Random.Range(0, amount + 1), Vector3.up * 2);
            //비활성화
            Dead();
        }

        return damage;
    }

    protected abstract void Dead();
}
