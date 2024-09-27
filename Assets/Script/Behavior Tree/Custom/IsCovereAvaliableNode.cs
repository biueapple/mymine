using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//숨을 곳을 찾았는가
public class IsCovereAvaliableNode : BehaviorTreeNode
{
    //벽을 의미
    private readonly Cover[] avaliableCovers;
    private readonly Transform target;
    private readonly EnemyAI ai;

    public IsCovereAvaliableNode(Cover[] covers, Transform target, EnemyAI ai)
    {
        avaliableCovers = covers;
        this.target = target;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Transform bestSpot = FindBestCoverSpot();
        ai.SetBestCoverSpot(bestSpot);
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FALIERE;
    }

    private Transform FindBestCoverSpot()
    {
        float minAngle = 90;
        Transform bestSpot = null;
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

    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        if(ai.GetBestCoverSpot() != null)
        {
            if(CheckIfSpotIsValid(ai.GetBestCoverSpot()))
            {
                return ai.GetBestCoverSpot();
            }
        }
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for(int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = target.transform.position - avaliableSpots[i].position;
            if (CheckIfSpotIsValid(avaliableSpots[i]))
            {
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
