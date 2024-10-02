using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���� ã�Ҵ°�
public class IsCovereAvaliableNode : BehaviorTreeNode
{
    //���� �� �ִ� �ĺ���
    private readonly Cover[] avaliableCovers;
    //���� (�÷��̾�)
    private readonly Transform target;
    //���� ��ü
    private readonly ICoveredEnemy ai;

    public IsCovereAvaliableNode(Cover[] covers, Transform target, ICoveredEnemy ai)
    {
        avaliableCovers = covers;
        this.target = target;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Transform bestSpot = FindBestCoverSpot();
        ai.BestCoverSpot = bestSpot;
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FALIERE;
    }

    //���� Ÿ���� ������ ����ؼ� ���� ���� ���� ���� ã��
    private Transform FindBestCoverSpot()
    {
        float minAngle = 90;
        Transform bestSpot = null;
        //�ĺ��� �߿��� ���� ���� ������ ã�Ƽ� ������
        for(int i = 0; i < avaliableCovers.Length; i++)
        {
            Transform bestSpotInCover = FindBestSpotInCover(avaliableCovers[i], ref minAngle);
            if(bestSpotInCover != null)
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
        if(ai.BestCoverSpot != null)
        {
            //�װ� ���� �������� Ȯ����
            if(CheckIfSpotIsValid(ai.BestCoverSpot))
            {
                return ai.BestCoverSpot;
            }
        }
        
        //�ϳ��� �ĺ��� �ϳ��� ���� ������ �������� ���� �� ������ ��� üũ�ؾ� ��
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for(int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = target.transform.position - avaliableSpots[i].position;
            //�������°� ����?
            if (CheckIfSpotIsValid(avaliableSpots[i]))
            {
                //���� Ȯ����
                float angle = Vector3.Angle(avaliableSpots[i].forward, direction);
                if(angle < minAngle)
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
        Vector3 direction = target.position - spot.position;
        if(Physics.Raycast(spot.position, direction, out RaycastHit hit))
        {
            if(hit.collider.transform != target)
            {
                return true;
            }
        }
        return false;
    }
}
