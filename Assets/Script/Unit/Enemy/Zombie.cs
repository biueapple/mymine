using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    AutoMove autoMove;
    JumpInputMove jumpSystem;

    AttackModule attackModule;
    private Animator animator;
    public Animator Animator { get { return animator; } }

    //근처에
    //  적이 있다면
    //      적을 향해 이동
    //          충분히 가까우면 멈춰서 공격
    //  없다면
    //      주위 랜덤한 포인트로 이동
    //
    // Start is called before the first frame update
    void Start()
    {
        autoMove = new (this);
        jumpSystem = new (moveSystem.Machine);

        moveSystem.AddMoveMode(autoMove);
        moveSystem.AddMoveMode(new AutoJump(jumpSystem, this)); 
        
        moveSystem.AddMoveMode(new GravityForce(moveSystem.Machine));

        moveSystem.AddMoveMode(new WorldMovePhysicsShift(moveSystem.Machine, GetComponent<Unit>()));

        //distanceDetection = new DistanceDetection(transform, 10);

        //pathfinder = new Pathfinder(this);


        animator = GetComponent<Animator>();

        attackModule = new AttackModule(1, STAT);
        attackModule.MotionAdd(new ZombieAttack(this));
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
        //if(target != null)
        //{
        //    if(Vector3.Distance(transform.position, target.transform.position) <= 1 && state != null)
        //    {
        //        StopCoroutine(state);
        //        state = null;
        //        StartCoroutine(Attack());
        //    }
        //    if(autoMove.Points != null && autoMove.Points.Count > 0)
        //    {
        //        Vector3 direction = autoMove.Points[0] - transform.position;
        //        direction.y = 0; // Y축 회전을 제거
        //        if(direction != Vector3.zero)
        //        {
        //            Quaternion targetRotation = Quaternion.LookRotation(direction);
        //            transform.rotation = targetRotation;
        //        }
        //    }
        //}
    }

    //private IEnumerator Sensing()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(2); 
    //        target = distanceDetection.Sensing(GameManager.Instance.Players);

    //        if (target != null)
    //        {
    //            pathfinder.Finding(target.transform.position);
    //            autoMove.SetTartget(pathfinder.Points);
    //        }
    //        else
    //        {
    //            //주위 랜덤한 위치로 이동
    //        }
    //    }
    //}

    //private IEnumerator Attack()
    //{
    //    autoMove.SetTartget(null);
    //    yield return new WaitForSeconds(1.2f);
    //    attackModule.Attack();
    //    yield return new WaitForSeconds(0.1f);
    //    state = StartCoroutine(Sensing());
    //}
}
