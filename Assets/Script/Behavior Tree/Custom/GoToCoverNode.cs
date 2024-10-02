using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class GoToCoverNode : BehaviorTreeNode
{
    //�̵��� �� �� ��ü
    private readonly Enemy ai;
    //�� ��ü�� ���� ��� �������̽�
    private readonly ICoveredEnemy coveredEnemy;
    private readonly MoveNode moveNode;
    public GoToCoverNode(Enemy flock, MoveNode moveNode)
    {
        ai = flock;
        coveredEnemy = flock as ICoveredEnemy;
        this.moveNode = moveNode;
    }

    public override NodeState Evaluate()
    {
        //���� ��� �������̽��� �������� �ʴٸ� ������ ���ٴ� ���� �翬�� ����
        if (coveredEnemy == null)
            return NodeState.FALIERE;

        //���� ��� �������̽��� �������� ���� ��Ұ� ���ٸ� ����
        Transform coverSpot = coveredEnemy.BestCoverSpot;
        if (coverSpot == null)
            return NodeState.FALIERE;

        //���� ��� �������̽��� �����ϸ鼭 ��ҵ� �����Ѵٸ� �̵�
        ai.SetColor(Color.blue);
        float distance = Vector3.Distance(coverSpot.position, ai.transform.position);
        if(distance > 0.2f)
        {
            Vector3 direction = coverSpot.position - ai.transform.position;

            moveNode.Move(direction);

            return NodeState.RUNNING;
        }
        else
        {
            //�̵� �Ϸ�
            return NodeState.SUCCESS;
        }
    }
}
