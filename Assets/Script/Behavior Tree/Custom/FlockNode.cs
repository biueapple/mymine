using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockNode : BehaviorTreeNode
{
    //������ ��������� �ϴ� �Ÿ�
    private readonly float cohesion;
    private readonly Transform boss;
    private readonly Enemy ai;
    private readonly MoveNode moveNode;
    public FlockNode(Transform boss, float cohesion, MoveNode moveNode, Enemy ai)
    {
        this.cohesion = cohesion;
        this.boss = boss;
        this.moveNode = moveNode;
        this.ai = ai;
    }

    //Flock�˰����� �����ؼ� �������� ���ϱ� �ٸ� �ڽŰ� ������ ������ ���ľ� �ϸ� ��ü������ ������ �༱ �ȵ�
    //���� ��� �ڽ��� ��ġ�� �ٲ�� �ص� �ڽ��� ��ġ�� ����� ��ġ�� ��ġ�� �ڽŰ� �� ���Ḹ �ڸ��� �ٲٸ� �Ǵ� ������ ��ü������ �������� ���ܼ� �ȵ�

    //�ڽ��� ���� �翡 �ִ°�
    public Vector3 Cohesion()
    {
        if(boss == null)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        if(Vector3.SqrMagnitude(ai.transform.position - boss.transform.position) > cohesion * cohesion)
        {
            velocity = (boss.transform.position - ai.transform.position).normalized;
        }

        return velocity;
    }
    //������ ���� �������� �̵��ϴ� ��

    //�ϴ� ������ ��� ���� ����̱⿡ �׻� �۵���
    public override NodeState Evaluate()
    {
        ai.SetColor(Color.cyan);

        Vector3 velocity = Vector3.zero;
        
        velocity += Cohesion();

        moveNode.Move(velocity);
        
        return NodeState.RUNNING;
    }
}
