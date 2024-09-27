using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� �� ���̿� ���𰡰� ���θ��� �ִ°� (�����°�)
public class IsCoveredNode : BehaviorTreeNode
{
    private readonly Transform target;
    private readonly Transform origin;

    public IsCoveredNode(Transform target, Transform origin)
    {
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        //target���� ray�� ���� �װ��� target�̸� ���𰡰� �� ���̿� �ִٴ� ���̴� ����
        //�ƴϸ� ����
        if(Physics.Raycast(origin.position, target.position - origin.position, out RaycastHit hit))
        {
            if(hit.collider.transform != target)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FALIERE;
    }
}
