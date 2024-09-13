using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ͱ� ��θ� ���� �̵��ϴ� ���
public class EnemyPatrolState : EnemyState
{
    private Enemy enemy;
    private Coroutine sensing;
    private Coroutine path;
    //������ �ֱ��
    //������ ��ġ�� �ϳ� �� �װ����� �̵�

    public EnemyPatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        sensing = GameManager.Instance.StartCoroutine(Sensing());
        path = GameManager.Instance.StartCoroutine(PathPoint());
        Debug.Log("���� ����");
    }

    public override void Exit()
    {
        if (sensing != null)
            GameManager.Instance.StopCoroutine(sensing);
        if(path != null)
            GameManager.Instance.StopCoroutine(path);
        Debug.Log("���� ����");
    }

    public override void Update()
    {

    }

    //���� �ֱ�� �ֺ� �÷��̾ ã��
    private IEnumerator Sensing()
    {
        while (true)
        {
            Player target = enemy.DistanceDetection.Sensing(GameManager.Instance.Players);

            if (target != null)
            {
                //���� ��ȭ
                enemy.ChangeState(new EnemyChaseState(enemy, target));
            }
            yield return new WaitForSeconds(2);
        }
    }

    //���� �ֱ�� �ֺ� ��ġ�� �̵�
    private IEnumerator PathPoint()
    {
        while(true)
        {
            //������ �� �� �ִ� ĭ�� ã�� ��ã�� ��ο� �־��ֱ�

            int x = 0;
            int y = 0;
            int z = 0;
            //�ϴ� 10���� �ݺ�
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
