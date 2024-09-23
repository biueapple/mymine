using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적을 상대로 숨는것을 성공했느냐
public class IsCovereAvaliableNode : BehaviorTreeNode
{
    //벽을 의미
    private Cover[] avaliableCovers;
    private Transform target;
    private EnemyAI ai;

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
        RaycastHit hit;
        Vector3 direction = target.position - spot.position;
        if(Physics.Raycast(spot.position, direction, out hit))
        {
            if(hit.collider.transform != target)
            {
                return true;
            }
        }
        return false;
    }
}
