using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        ChangeState(new EnemyPatrolState(this));
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
    }


    protected override void Dead()
    {
        state.Exit();
        gameObject.SetActive(false);
    }

}
