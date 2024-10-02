using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//숨을 곳을 찾았는가
public class IsCovereAvaliableNode : BehaviorTreeNode
{
    //숨을 수 있는 후보들
    private readonly Cover[] avaliableCovers;
    //상대방 (플레이어)
    private readonly Transform target;
    //숨을 객체
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

    //나와 타겟의 각도를 계산해서 가장 좋은 숨을 곳을 찾음
    private Transform FindBestCoverSpot()
    {
        float minAngle = 90;
        Transform bestSpot = null;
        //후보들 중에서 가장 좋은 각도를 찾아서 리턴함
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

    //이 후보가 베스트 입니다.
    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        //이미 숨을 후보가 존재한다면
        if(ai.BestCoverSpot != null)
        {
            //그거 아직 괜찮은지 확인해
            if(CheckIfSpotIsValid(ai.BestCoverSpot))
            {
                return ai.BestCoverSpot;
            }
        }
        
        //하나의 후보는 하나의 숨을 스팟을 소유하지 않을 수 있으니 모두 체크해야 함
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for(int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = target.transform.position - avaliableSpots[i].position;
            //숨어지는거 맞음?
            if (CheckIfSpotIsValid(avaliableSpots[i]))
            {
                //각도 확인해
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

    //이 후보에 숨으면 가려지는거 맞음?
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
