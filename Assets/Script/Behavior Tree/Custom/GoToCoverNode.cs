using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� ���� �׺���̼��� ��������� �̹� ����� ������ ���߿� �װɷ� ����
public class GoToCoverNode : BehaviorTreeNode
{
    private EnemyAI ai;

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
            //�̵�
            ai.transform.Translate((coverSpot.position - ai.transform.position).normalized * Time.deltaTime);
            return NodeState.RUNNING;
        }
        else
        {
            //����
            return NodeState.SUCCESS;
        }
    }
}
