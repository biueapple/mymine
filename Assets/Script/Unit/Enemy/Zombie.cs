using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private Animator animator;
    public Animator Animator { get { return animator; } }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        attackModule.MotionAdd(new ZombieAttack(this));

        animator = GetComponent<Animator>();
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

    //// Update is called once per frame
    //void Update()
    //{
    //    state.Update();
    //    //if(target != null)
    //    //{
    //    //    if(Vector3.Distance(transform.position, target.transform.position) <= 1 && state != null)
    //    //    {
    //    //        StopCoroutine(state);
    //    //        state = null;
    //    //        StartCoroutine(Attack());
    //    //    }
    //    //    if(autoMove.Points != null && autoMove.Points.Count > 0)
    //    //    {
    //    //        Vector3 direction = autoMove.Points[0] - transform.position;
    //    //        direction.y = 0; // Y축 회전을 제거
    //    //        if(direction != Vector3.zero)
    //    //        {
    //    //            Quaternion targetRotation = Quaternion.LookRotation(direction);
    //    //            transform.rotation = targetRotation;
    //    //        }
    //    //    }
    //    //}
    //}

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
