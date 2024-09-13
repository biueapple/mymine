using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터가 경로를 따라 이동하는 경우
public class EnemyPatrolState : EnemyState
{
    private Enemy enemy;
    private Coroutine sensing;
    private Coroutine path;
    //일정한 주기로
    //주위의 위치를 하나 찍어서 그곳으로 이동

    public EnemyPatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        sensing = GameManager.Instance.StartCoroutine(Sensing());
        path = GameManager.Instance.StartCoroutine(PathPoint());
        Debug.Log("순찰 개시");
    }

    public override void Exit()
    {
        if (sensing != null)
            GameManager.Instance.StopCoroutine(sensing);
        if(path != null)
            GameManager.Instance.StopCoroutine(path);
        Debug.Log("순찰 종료");
    }

    public override void Update()
    {

    }

    //일정 주기로 주변 플레이어를 찾음
    private IEnumerator Sensing()
    {
        while (true)
        {
            Player target = enemy.DistanceDetection.Sensing(GameManager.Instance.Players);

            if (target != null)
            {
                //상태 변화
                enemy.ChangeState(new EnemyChaseState(enemy, target));
            }
            yield return new WaitForSeconds(2);
        }
    }

    //일정 주기로 주변 위치로 이동
    private IEnumerator PathPoint()
    {
        while(true)
        {
            //주위에 갈 수 있는 칸을 찾아 길찾기 경로에 넣어주기

            int x = 0;
            int y = 0;
            int z = 0;
            //일단 10번만 반복
            for (int i = 0; i < 10; i++)
            {
                x = Random.Range(-4, 5);
                z = Random.Range(-4, 5);
                y = BlockCheck(x, z);
                if (y != -10)
                {
                    break;
                }
            }

            enemy.Pathfinder.Finding(enemy.transform.position + new Vector3(x, y, z));
            enemy.AutoMove.SetTartget(enemy.Pathfinder.Points);

            yield return new WaitForSeconds(5);
        }
    }

    private int BlockCheck(int x, int z)
    {
        for (int y = (int)enemy.transform.position.y - 5; y < (int)enemy.transform.position.y + 5; y++)
        {
            if (GameManager.Instance.Empty(enemy, enemy.transform.position + new Vector3(x, y, z)))
            {
                return y;
            }
        }
        return -10;
    }
}
