using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlockNode : BehaviorTreeNode
{
    //�ڽŵ��� ������ ������ ������ �޾� ��ġ�� ���ؾ� ��
    //�����
    private readonly Transform[] colleague;
    //�ڽ�
    private readonly EnemyAI flock;
    //����
    private readonly Transform boss;
    //����� ���� �ȵǴ� �Ÿ�
    private readonly float avoidance;
    //������ ��������� �ϴ� �Ÿ�
    private readonly float cohesion;

    private readonly float rotationSpeed;

    private Vector3 desiredVelocity;
    private readonly float momentum;
    public FlockNode(Transform[] colleague, EnemyAI flock, Transform boss, float avoidance, float cohesion, float rotationSpeed, float momentum)
    {
        this.colleague = colleague;
        this.flock = flock;
        this.boss = boss;
        this.avoidance = avoidance;
        this.cohesion = cohesion;
        this.rotationSpeed = rotationSpeed;
        this.momentum = momentum;
    }

    //Flock�˰����� �����ؼ� �������� ���ϱ� �ٸ� �ڽŰ� ������ ������ ���ľ� �ϸ� ��ü������ ������ �༱ �ȵ�
    //���� ��� �ڽ��� ��ġ�� �ٲ�� �ص� �ڽ��� ��ġ�� ����� ��ġ�� ��ġ�� �ڽŰ� �� ���Ḹ �ڸ��� �ٲٸ� �Ǵ� ������ ��ü������ �������� ���ܼ� �ȵ�

    //�ֿ켱�� �ϴ� ����� ��ġ�� �ʴ°�
    private Vector3 Avoidance()
    {
        //������ ���ᰡ ���ٸ� �����ӿ� ������ ���� �ʱ� ���� zero ����
        if (colleague.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        foreach (var f in colleague)
        {
            if (f != flock)
            {
                if (Vector3.SqrMagnitude(flock.transform.position - f.transform.position) < avoidance * avoidance)
                {
                    velocity += (flock.transform.position - f.transform.position).normalized;
                }
            }
        }

        //���ʿ� ���÷� ����ؼ� ����� ũ�⸸�� ����
        return velocity.normalized;
    }
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
        flock.SetColor(Color.cyan);
        Vector3 velocity = Vector3.zero;
        
        velocity += Cohesion();

        velocity += Avoidance();

        velocity.y = 0; // Y�� ȸ���� ����
        if(velocity != Vector3.zero)
        {
            velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            flock.transform.rotation = targetRotation;

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //Vector3 smoothVelocity = Vector3.zero;
            //flock.transform.position = Vector3.SmoothDamp(flock.transform.position, flock.transform.position + velocity * power, ref smoothVelocity, smoothTime);
            //�����̰�
            flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
            desiredVelocity = velocity;
        }
        return NodeState.RUNNING;
    }
}
