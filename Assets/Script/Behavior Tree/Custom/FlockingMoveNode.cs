using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingMoveNode : MoveNode
{
    //자신들의 동료들과 보스의 영향을 받아 위치를 정해야 함
    //동료들
    protected readonly Transform[] colleague;

    //가까워 지면 안되는 거리
    protected readonly float avoidance;

    protected Vector3 desiredVelocity;
    //현재 사용중이지 않은 변수
    protected readonly float momentum;

    protected readonly float power;

    public FlockingMoveNode(Transform[] colleague, Enemy flock, float avoidance, float rotationSpeed, float momentum, float power) : base(flock, rotationSpeed)
    {
        this.colleague = colleague;
        this.avoidance = avoidance;
        this.momentum = momentum;
        this.power = power;
    }

    //최우선은 일단 동료와 겹치지 않는가
    protected Vector3 Avoidance(ref float magnitude)
    {
        //참고할 동료가 없다면 움직임에 영향을 주지 않기 위해 zero 리턴
        if (colleague == null || colleague.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        foreach (var f in colleague)
        {
            if (f != flock)
            {
                magnitude = Vector3.Magnitude(flock.transform.position - f.transform.position);
                magnitude /= avoidance;
                if (magnitude < 1)
                {
                    velocity += (flock.transform.position - f.transform.position).normalized;
                }
            }
        }

        //애초에 로컬로 계산해서 방향과 크기만이 있음
        return velocity.normalized;
    }

    //움직일 방향을 입력
    public override void Move(Vector3 velocity, Player target = null)
    {
        //인자로 전달받은 velocity가 얼마의 비율로 힘이 들어갈지
        float mg = 1;
        Vector3 avoid = Avoidance(ref mg);
        velocity = velocity.normalized * mg;
        //if(avoid == Vector3.zero)
        //{
        velocity += avoid * power;
        //}
        //else
        //{
        //    velocity = avoid;
        //}

        base.Move(velocity, target);
        //velocity.y = 0; // Y축 회전을 제거
        //if (velocity != Vector3.zero)
        //{
        //    //velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

        //    //Quaternion targetRotation = Quaternion.LookRotation(velocity);
        //    //flock.transform.rotation = targetRotation;

        //    //Quaternion targetRotation = Quaternion.LookRotation(velocity);
        //    //flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //    LookAtVelocity(velocity);
        //    LookAtTargetWithinAngle(target);

        //    //움직이게
        //    flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
        //}
        ////desiredVelocity = velocity;
    }
}
