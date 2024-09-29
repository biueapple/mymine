using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlockNode : MoveNode
{
    //������ ��������� �ϴ� �Ÿ�
    private readonly float cohesion;
    private readonly Transform boss;
    public FlockNode(Transform[] colleague, Enemy flock, Transform boss, float avoidance, float cohesion, float rotationSpeed, float momentum, float power) : base(colleague, flock, avoidance, rotationSpeed, momentum, power)
    {
        this.cohesion = cohesion;
        this.boss = boss;
    }

    //Flock�˰����� �����ؼ� �������� ���ϱ� �ٸ� �ڽŰ� ������ ������ ���ľ� �ϸ� ��ü������ ������ �༱ �ȵ�
    //���� ��� �ڽ��� ��ġ�� �ٲ�� �ص� �ڽ��� ��ġ�� ����� ��ġ�� ��ġ�� �ڽŰ� �� ���Ḹ �ڸ��� �ٲٸ� �Ǵ� ������ ��ü������ �������� ���ܼ� �ȵ�

    //�ڽ��� ���� �翡 �ִ°�
    public Vector3 Cohesion()
    {
        if(boss == null)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        if(Vector3.SqrMagnitude(flock.transform.position - boss.transform.position) > cohesion * cohesion)
        {
            velocity = (boss.transform.position - flock.transform.position).normalized;
        }

        return velocity;
    }
    //������ ���� �������� �̵��ϴ� ��

    //�ϴ� ������ ��� ���� ����̱⿡ �׻� �۵���
    public override NodeState Evaluate()
    {
        //flock.SetColor(Color.cyan);

        Vector3 velocity = Vector3.zero;
        
        velocity += Cohesion();

        Move(velocity);
        
        return NodeState.RUNNING;
    }
}
