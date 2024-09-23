using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� �� ���̿� ���𰡰� ���θ��� �ִ°� (�����°�)
public class IsCoveredNode : BehaviorTreeNode
{
    private Transform target;
    private Transform origin;

    public IsCoveredNode(Transform target, Transform origin)
    {
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        //target���� ray�� ���� �װ��� target�̸� ���𰡰� �� ���̿� �ִٴ� ���̴� ����
        //�ƴϸ� ����
        return NodeState.SUCCESS;
    }
}
