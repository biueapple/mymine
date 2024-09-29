using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveNode : BehaviorTreeNode
{
    //자신들의 동료들과 보스의 영향을 받아 위치를 정해야 함
    //동료들
    protected readonly Transform[] colleague;
    //자신
    protected readonly Enemy flock;

    //가까워 지면 안되는 거리
    protected readonly float avoidance;

    protected readonly float rotationSpeed;

    protected Vector3 desiredVelocity;
    protected readonly float momentum;

    protected readonly float power;

    public MoveNode(Transform[] colleague, Enemy flock, float avoidance, float rotationSpeed, float momentum, float power)
    {
        this.colleague = colleague;
        this.flock = flock;
        this.avoidance = avoidance;
        this.rotationSpeed = rotationSpeed;
        this.momentum = momentum;
        this.power = power;
    }

    //최우선은 일단 동료와 겹치지 않는가
    protected Vector3 Avoidance(ref float magnitude)
    {
        //참고할 동료가 없다면 움직임에 영향을 주지 않기 위해 zero 리턴
        if (colleague.Length == 0)
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
    protected void Move(Vector3 velocity)
    {
        //인자로 전달받은 velocity가 얼마의 비율로 힘이 들어갈지
        float mg = 1 ;
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

        velocity.y = 0; // Y축 회전을 제거
        if (velocity != Vector3.zero)
        {
            //velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = targetRotation;

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //움직이게
            flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
        }
        //desiredVelocity = velocity;
    }
}

/*플레이어를 찾아가는 방법, 몸이 가는 방향을 바라보는 기능은 AutoMove에 있음
                enemy.Pathfinder.Finding(target.transform.position);
                enemy.AutoMove.SetTartget(enemy.Pathfinder.Points);

머리만 플레이어를 바라보도록, 몸은 가는 방향을 바라봐야 하니까
Vector3 directionToTarget;
    void LookAtTargetWithinAngle()
    {
        // 캐릭터와 타겟 간의 방향을 계산
        directionToTarget = target.transform.position - enemy.Head.position;
        directionToTarget.y = 0; // 수평 방향만 고려 (y 축 제외)

        // 두 벡터(캐릭터 forward 방향과 타겟 방향) 사이의 각도 계산
        float angleToTarget = Vector3.Angle(enemy.transform.forward, directionToTarget);

        // 각도가 지정된 시야 각도(viewAngle) 내에 있으면 타겟을 바라봄
        if (angleToTarget <= 45)
        {
            // 타겟을 바라보도록 머리 회전
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            //enemy.Head.rotation = Quaternion.Slerp(enemy.Head.rotation, targetRotation, Time.deltaTime * 5f); // 부드럽게 회전
            enemy.Head.rotation = targetRotation;
        }
    }
 */