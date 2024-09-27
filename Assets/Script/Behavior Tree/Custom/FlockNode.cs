using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlockNode : BehaviorTreeNode
{
    //자신들의 동료들과 보스의 영향을 받아 위치를 정해야 함
    //동료들
    private readonly Transform[] colleague;
    //자신
    private readonly EnemyAI flock;
    //보스
    private readonly Transform boss;
    //가까워 지면 안되는 거리
    private readonly float avoidance;
    //보스와 가까워져야 하는 거리
    private readonly float cohesion;

    private readonly float rotationSpeed;

    private Vector3 desiredVelocity;
    private readonly float momentum;
    public FlockNode(Transform[] colleague, EnemyAI flock, Transform boss, float avoidance, float cohesion, float rotationSpeed, float momentum)
    {
        this.colleague = colleague;
        this.flock = flock;
        this.boss = boss;
        this.avoidance = avoidance;
        this.cohesion = cohesion;
        this.rotationSpeed = rotationSpeed;
        this.momentum = momentum;
    }

    //Flock알고리즘을 참고해서 포지션을 정하기 다만 자신과 주위만 영향을 끼쳐야 하며 전체적으로 영향을 줘선 안됨
    //예를 들면 자신의 위치를 바꿔야 해도 자신의 위치만 동료와 위치가 겹치면 자신과 그 동료만 자리를 바꾸면 되는 일이지 전체적으로 움직임이 생겨선 안됨

    //최우선은 일단 동료와 겹치지 않는가
    private Vector3 Avoidance()
    {
        //참고할 동료가 없다면 움직임에 영향을 주지 않기 위해 zero 리턴
        if (colleague.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        foreach (var f in colleague)
        {
            if (f != flock)
            {
                if (Vector3.SqrMagnitude(flock.transform.position - f.transform.position) < avoidance * avoidance)
                {
                    velocity += (flock.transform.position - f.transform.position).normalized;
                }
            }
        }

        //애초에 로컬로 계산해서 방향과 크기만이 있음
        return velocity.normalized;
    }
    //자신이 보스 곁에 있는가
    public Vector3 Cohesion()
    {
        if(boss == null)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        if(Vector3.SqrMagnitude(flock.transform.position - boss.transform.position) > cohesion * cohesion)
        {
            velocity = (boss.transform.position - flock.transform.position).normalized;
        }

        return velocity;
    }
    //보스가 가는 방향으로 이동하는 가

    //일단 할일이 없어서 오는 노드이기에 항상 작동중
    public override NodeState Evaluate()
    {
        flock.SetColor(Color.cyan);
        Vector3 velocity = Vector3.zero;
        
        velocity += Cohesion();

        velocity += Avoidance();

        velocity.y = 0; // Y축 회전을 제거
        if(velocity != Vector3.zero)
        {
            velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            flock.transform.rotation = targetRotation;

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //Vector3 smoothVelocity = Vector3.zero;
            //flock.transform.position = Vector3.SmoothDamp(flock.transform.position, flock.transform.position + velocity * power, ref smoothVelocity, smoothTime);
            //움직이게
            flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
            desiredVelocity = velocity;
        }
        return NodeState.RUNNING;
    }
}
