using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldColleagueMoveNode : BehaviorTreeNode
{
    private readonly ShieldColleagueNode shieldColleagueNode;
    //상대방 (플레이어)
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

        //숨는 장소 인터페이스를 소지하면서 장소도 존재한다면 이동
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
            //이동 완료
            return NodeState.SUCCESS;
        }
    }

    //나와 타겟의 각도를 계산해서 가장 좋은 숨을 곳을 찾음
    private Transform FindBestCoverSpot()
    {
        float minAngle = 90;
        Transform bestSpot = null;
        //후보들 중에서 가장 좋은 각도를 찾아서 리턴함
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

    //이 후보가 베스트 입니다.
    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        //이미 숨을 후보가 존재한다면
        if (coveredEnemy.BestCoverSpot != null)
        {
            //그거 아직 괜찮은지 확인해
            if (CheckIfSpotIsValid(coveredEnemy.BestCoverSpot))
            {
                return coveredEnemy.BestCoverSpot;
            }
        }

        //하나의 후보는 하나의 숨을 스팟을 소유하지 않을 수 있으니 모두 체크해야 함
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for (int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = player.transform.position - avaliableSpots[i].position;
            //숨어지는거 맞음?
            if (CheckIfSpotIsValid(avaliableSpots[i]))
            {
                //각도 확인해
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

    //이 후보에 숨으면 가려지는거 맞음?
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
