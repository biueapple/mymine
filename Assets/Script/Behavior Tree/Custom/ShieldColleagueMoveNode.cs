using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldColleagueMoveNode : BehaviorTreeNode
{
    private readonly ShieldColleagueNode shieldColleagueNode;
    //���� (�÷��̾�)
    private readonly Player player;
    private readonly Enemy ai;
    private readonly ICoveredEnemy coveredEnemy;
    private readonly MoveNode moveNode;

    public ShieldColleagueMoveNode(ShieldColleagueNode shieldColleagueNode, Enemy ai, MoveNode moveNode)
    {
        this.shieldColleagueNode = shieldColleagueNode;
        this.ai = ai;
        coveredEnemy = ai as ICoveredEnemy;
        this.moveNode = moveNode;
    }

    public override NodeState Evaluate()
    {
        if(shieldColleagueNode == null || shieldColleagueNode.Covers == null)
            return NodeState.FALIERE;

        if(shieldColleagueNode.Covers.Count == 0)
            return NodeState.FALIERE;

        if(coveredEnemy == null)
            return NodeState.FALIERE;

        coveredEnemy.BestCoverSpot = FindBestCoverSpot();
        if (coveredEnemy.BestCoverSpot == null)
            return NodeState.FALIERE;

        //���� ��� �������̽��� �����ϸ鼭 ��ҵ� �����Ѵٸ� �̵�
        ai.SetColor(Color.blue);
        float distance = Vector3.Distance(coveredEnemy.BestCoverSpot.position, ai.transform.position);
        if (distance > 0.2f)
        {
            Vector3 direction = coveredEnemy.BestCoverSpot.position - ai.transform.position;

            moveNode.Move(direction);

            return NodeState.RUNNING;
        }
        else
        {
            //�̵� �Ϸ�
            return NodeState.SUCCESS;
        }
    }

    //���� Ÿ���� ������ ����ؼ� ���� ���� ���� ���� ã��
    private Transform FindBestCoverSpot()
    {
        float minAngle = 90;
        Transform bestSpot = null;
        //�ĺ��� �߿��� ���� ���� ������ ã�Ƽ� ������
        for (int i = 0; i < shieldColleagueNode.Covers.Count; i++)
        {
            Transform bestSpotInCover = FindBestSpotInCover(shieldColleagueNode.Covers[i], ref minAngle);
            if (bestSpotInCover != null)
            {
                bestSpot = bestSpotInCover;
            }
        }
        return bestSpot;
    }

    //�� �ĺ��� ����Ʈ �Դϴ�.
    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        //�̹� ���� �ĺ��� �����Ѵٸ�
        if (coveredEnemy.BestCoverSpot != null)
        {
            //�װ� ���� �������� Ȯ����
            if (CheckIfSpotIsValid(coveredEnemy.BestCoverSpot))
            {
                return coveredEnemy.BestCoverSpot;
            }
        }

        //�ϳ��� �ĺ��� �ϳ��� ���� ������ �������� ���� �� ������ ��� üũ�ؾ� ��
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for (int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = player.transform.position - avaliableSpots[i].position;
            //�������°� ����?
            if (CheckIfSpotIsValid(avaliableSpots[i]))
            {
                //���� Ȯ����
                float angle = Vector3.Angle(avaliableSpots[i].forward, direction);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = avaliableSpots[i];
                }
            }
        }
        return bestSpot;
    }

    //�� �ĺ��� ������ �������°� ����?
    private bool CheckIfSpotIsValid(Transform spot)
    {
        Vector3 direction = player.transform.position - spot.position;
        if (Physics.Raycast(spot.position, direction, out RaycastHit hit))
        {
            if (hit.collider.transform != player)
            {
                return true;
            }
        }
        return false;
    }
}
