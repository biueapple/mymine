using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode
{
    ////자신들의 동료들과 보스의 영향을 받아 위치를 정해야 함
    ////동료들
    //protected readonly Transform[] colleague;
    //자신
    protected readonly Enemy flock;

    ////가까워 지면 안되는 거리
    //protected readonly float avoidance;

    protected readonly float rotationSpeed;

    //protected Vector3 desiredVelocity;
    ////현재 사용중이지 않은 변수
    //protected readonly float momentum;

    //protected readonly float power;

    public MoveNode(Enemy flock, float rotationSpeed)
    {
        this.flock = flock;
        this.rotationSpeed = rotationSpeed;
    }

    //움직일 방향을 입력
    public virtual void Move(Vector3 velocity, Player target = null)
    {
        ////인자로 전달받은 velocity가 얼마의 비율로 힘이 들어갈지
        //float mg = 1 ;
        //Vector3 avoid = Avoidance(ref mg);
        //velocity = velocity.normalized * mg;
        ////if(avoid == Vector3.zero)
        ////{
        //    velocity += avoid * power;
        ////}
        ////else
        ////{
        ////    velocity = avoid;
        ////}

        velocity.y = 0; // Y축 회전을 제거
        if (velocity != Vector3.zero)
        {
            //velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = targetRotation;

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            LookAtVelocity(velocity);
            LookAtTargetWithinAngle(target);

            //움직이게
            flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
        }
        //desiredVelocity = velocity;
    }

    //방향을 주면 전체를 회전
    public void LookAtVelocity(Vector3 velocity)
    {
        Quaternion targetRotation = Quaternion.LookRotation(velocity);
        flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    //타겟을 주면 전체를 회전
    public void LookAtTarget(Player target)
    {
        if (flock == null || target == null)
            return;

        // 캐릭터와 타겟 간의 방향을 계산
        Vector3 directionToTarget = target.transform.position + new Vector3(0, target.Height, 0) - flock.transform.position;
        directionToTarget.y = 0; // 수평 방향만 고려 (y 축 제외)

        // 타겟을 바라보도록 회전
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    //머리만 플레이어를 바라보도록, 몸은 가는 방향을 바라봐야 하니까
    public void LookAtTargetWithinAngle(Player target)
    {
        if (flock.Head == null || target == null)
            return;

        // 캐릭터와 타겟 간의 방향을 계산
        Vector3 directionToTarget = target.transform.position + new Vector3(0, target.Height, 0) - flock.Head.position;
        //directionToTarget.y = 0; // 수평 방향만 고려 (y 축 제외)

        // 두 벡터(캐릭터 forward 방향과 타겟 방향) 사이의 각도 계산
        float angleToTarget = Vector3.Angle(flock.transform.forward, directionToTarget);

        // 각도가 지정된 시야 각도(viewAngle) 내에 있으면 타겟을 바라봄
        if (angleToTarget <= 45)
        {
            // 타겟을 바라보도록 머리 회전
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            //enemy.Head.rotation = Quaternion.Slerp(enemy.Head.rotation, targetRotation, Time.deltaTime * 5f); // 부드럽게 회전
            flock.Head.rotation = targetRotation;
        }
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