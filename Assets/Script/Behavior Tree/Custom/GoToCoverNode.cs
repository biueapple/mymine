using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class GoToCoverNode : BehaviorTreeNode
{
    private readonly EnemyAI ai;

    public GoToCoverNode(EnemyAI origin)
    {
        ai = origin;
    }

    public override NodeState Evaluate()
    {
        Transform coverSpot = ai.GetBestCoverSpot();
        if (coverSpot == null)
            return NodeState.FALIERE;

        ai.SetColor(Color.blue);
        float distance = Vector3.Distance(coverSpot.position, ai.transform.position);
        if(distance > 0.2f)
        {
            Vector3 direction = coverSpot.position - ai.transform.position;
            direction.y = 0; // Y�� ȸ���� ����

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            ai.transform.rotation = targetRotation;
            //�̵�
            ai.transform.Translate((coverSpot.position - ai.transform.position).normalized * Time.deltaTime, Space.World);
            return NodeState.RUNNING;
        }
        else
        {
            //����
            return NodeState.SUCCESS;
        }
    }
}
