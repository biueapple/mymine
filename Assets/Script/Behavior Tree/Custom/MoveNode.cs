using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveNode : BehaviorTreeNode
{
    //�ڽŵ��� ������ ������ ������ �޾� ��ġ�� ���ؾ� ��
    //�����
    protected readonly Transform[] colleague;
    //�ڽ�
    protected readonly Enemy flock;

    //����� ���� �ȵǴ� �Ÿ�
    protected readonly float avoidance;

    protected readonly float rotationSpeed;

    protected Vector3 desiredVelocity;
    protected readonly float momentum;

    protected readonly float power;

    public MoveNode(Transform[] colleague, Enemy flock, float avoidance, float rotationSpeed, float momentum, float power)
    {
        this.colleague = colleague;
        this.flock = flock;
        this.avoidance = avoidance;
        this.rotationSpeed = rotationSpeed;
        this.momentum = momentum;
        this.power = power;
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
        float mg = 1 ;
        Vector3 avoid = Avoidance(ref mg);
        velocity = velocity.normalized * mg;
        //if(avoid == Vector3.zero)
        //{
            velocity += avoid * power;
        //}
        //else
        //{
        //    velocity = avoid;
        //}

        velocity.y = 0; // Y�� ȸ���� ����
        if (velocity != Vector3.zero)
        {
            //velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = targetRotation;

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //�����̰�
            flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
        }
        //desiredVelocity = velocity;
    }
}

/*�÷��̾ ã�ư��� ���, ���� ���� ������ �ٶ󺸴� ����� AutoMove�� ����
                enemy.Pathfinder.Finding(target.transform.position);
                enemy.AutoMove.SetTartget(enemy.Pathfinder.Points);

�Ӹ��� �÷��̾ �ٶ󺸵���, ���� ���� ������ �ٶ���� �ϴϱ�
Vector3 directionToTarget;
    void LookAtTargetWithinAngle()
    {
        // ĳ���Ϳ� Ÿ�� ���� ������ ���
        directionToTarget = target.transform.position - enemy.Head.position;
        directionToTarget.y = 0; // ���� ���⸸ ��� (y �� ����)

        // �� ����(ĳ���� forward ����� Ÿ�� ����) ������ ���� ���
        float angleToTarget = Vector3.Angle(enemy.transform.forward, directionToTarget);

        // ������ ������ �þ� ����(viewAngle) ���� ������ Ÿ���� �ٶ�
        if (angleToTarget <= 45)
        {
            // Ÿ���� �ٶ󺸵��� �Ӹ� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            //enemy.Head.rotation = Quaternion.Slerp(enemy.Head.rotation, targetRotation, Time.deltaTime * 5f); // �ε巴�� ȸ��
            enemy.Head.rotation = targetRotation;
        }
    }
 */