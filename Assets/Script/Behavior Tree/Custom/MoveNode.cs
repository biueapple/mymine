using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveNode : BehaviorTreeNode
{
    //�ڽŵ��� ������ ������ ������ �޾� ��ġ�� ���ؾ� ��
    //�����
    protected readonly Transform[] colleague;
    //�ڽ�
    protected readonly EnemyAI flock;
    //����
    protected readonly Transform boss;

    //����� ���� �ȵǴ� �Ÿ�
    protected readonly float avoidance;

    protected readonly float rotationSpeed;

    protected Vector3 desiredVelocity;
    protected readonly float momentum;

    public MoveNode(Transform[] colleague, EnemyAI flock, Transform boss, float avoidance, float rotationSpeed, float momentum)
    {
        this.colleague = colleague;
        this.flock = flock;
        this.boss = boss;
        this.avoidance = avoidance;
        this.rotationSpeed = rotationSpeed;
        this.momentum = momentum;
    }

    //�ֿ켱�� �ϴ� ����� ��ġ�� �ʴ°�
    protected Vector3 Avoidance(ref float magnitude)
    {
        //������ ���ᰡ ���ٸ� �����ӿ� ������ ���� �ʱ� ���� zero ����
        if (colleague.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        foreach (var f in colleague)
        {
            if (f != flock)
            {
                magnitude = Vector3.Magnitude(flock.transform.position - f.transform.position);
                magnitude /= avoidance;
                if (magnitude < 1)
                {
                    velocity += (flock.transform.position - f.transform.position).normalized;
                }
            }
        }

        //���ʿ� ���÷� ����ؼ� ����� ũ�⸸�� ����
        return velocity.normalized;
    }

    //������ ������ �Է�
    protected void Move(Vector3 velocity)
    {
        //���ڷ� ���޹��� velocity�� ���� ������ ���� ����
        float mg = 1;
        Vector3 avoid = Avoidance(ref mg);
        velocity *= mg;
        if(avoid == Vector3.zero)
        {
            velocity += avoid;
        }
        else
        {
            velocity = avoid;
        }

        velocity.y = 0; // Y�� ȸ���� ����
        if (velocity != Vector3.zero)
        {
            velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = targetRotation;

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //�����̰�
            flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
        }
        desiredVelocity = velocity;
    }
}
