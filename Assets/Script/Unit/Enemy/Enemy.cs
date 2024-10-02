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

    protected AttackModule attackModule;
    public AttackModule AttackModule { get { return attackModule; } }

    //
    protected Pathfinder pathfinder;
    public Pathfinder Pathfinder { get { return pathfinder; } }
    [SerializeField]
    protected AutoMove autoMove;
    public AutoMove AutoMove { get { return autoMove; } }
    protected DistanceDetection distanceDetection;
    public DistanceDetection DistanceDetection { get {  return distanceDetection; } }

    JumpInputMove jumpSystem;

    public abstract Transform Head { get; }
    public abstract Transform Body { get; }


    protected BehaviorTreeNode topNode;

    [SerializeField]
    protected float chaseRange;
    [SerializeField]
    protected float minRange;
    [SerializeField]
    protected float attackInRnage;

    [SerializeField]
    protected float rotationSpeed;
    protected void Start()
    {
        autoMove = new(this, substances[0].transform);
        jumpSystem = new(moveSystem.Machine);

        moveSystem.AddMoveMode(autoMove);
        moveSystem.AddMoveMode(new AutoJump(jumpSystem, this));

        moveSystem.AddMoveMode(new GravityForce(moveSystem.Machine));

        moveSystem.AddMoveMode(new WorldMovePhysicsShift(moveSystem.Machine, GetComponent<Unit>()));

        distanceDetection = new DistanceDetection(transform, 10);

        pathfinder = new Pathfinder(this);

        attackModule = new AttackModule(1, STAT);
    }

    public override float Hit(AttackInformation attackInformation)
    {
        float damage = base.Hit(attackInformation);
        if(stat.HP <= 0)
        {
            //아이템 드랍
            if(itemID.Length > 0)
            {
                FlowManager.Instance.DropItem(transform.position, itemID[Random.Range(0, itemID.Length)], Random.Range(0, amount + 1), Vector3.up * 2);
            }
            
            //비활성화
            Dead();
        }

        return damage;
    }

    protected abstract void Dead();

    public void SetColor(Color color)
    {
        for(int i = 0; i < substances.Length; i++)
        {
            substances[i].GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
