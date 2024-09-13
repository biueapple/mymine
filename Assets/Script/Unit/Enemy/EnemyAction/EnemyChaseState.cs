using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어를 추격하는 상태
public class EnemyChaseState : EnemyState
{
    private readonly Enemy enemy;
    private readonly Player target;
    private Coroutine coroutine;

    public EnemyChaseState(Enemy enemy, Player target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public override void Enter()
    {
        coroutine = GameManager.Instance.StartCoroutine(Sensing());
        Debug.Log("추격 개시");
    }

    public override void Exit()
    {
        GameManager.Instance.StopCoroutine(coroutine);
        Debug.Log("추격 종료");
    }

    public override void Update()
    {
        //거리가 가까워지면 적을 공격해야하니 상태 변화
        if(Vector3.Distance(enemy.transform.position, target.transform.position) <= 1)
        {
            enemy.ChangeState(new EnemyAttackState(enemy));
        }
        LookAtTargetWithinAngle();
    }

    private IEnumerator Sensing()
    {
        while (true)
        {
            //거리가 너무 멀어지면 적을 놓쳤으니 상태 변화
            if (Vector3.Distance(enemy.transform.position, target.transform.position) >= 10)
            {
                enemy.ChangeState(new EnemyPatrolState(enemy));
            }
            else
            {
                enemy.Pathfinder.Finding(target.transform.position);
                enemy.AutoMove.SetTartget(enemy.Pathfinder.Points);
            }
            yield return new WaitForSeconds(2);
        }
    }

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
}
