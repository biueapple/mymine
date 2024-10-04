using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingMoveNode : MoveNode
{
    //�ڽŵ��� ������ ������ ������ �޾� ��ġ�� ���ؾ� ��
    protected readonly IFlockingEnemy flockingEnemy;
    //�����
    //protected readonly Transform[] colleague;

    //����� ���� �ȵǴ� �Ÿ�
    //protected readonly float avoidance;

    //protected readonly float power;

    //���� ��������� ���� ������ 
    //protected Vector3 desiredVelocity;
    //protected readonly float momentum;

    public FlockingMoveNode(Enemy flock, float rotationSpeed) : base(flock, rotationSpeed)
    {
        flockingEnemy = flock as IFlockingEnemy;
    }

    //�ֿ켱�� �ϴ� ����� ��ġ�� �ʴ°�
    protected Vector3 Avoidance(ref float magnitude)
    {
        if (flockingEnemy == null)
            return Vector3.zero;

        //������ ���ᰡ ���ٸ� �����ӿ� ������ ���� �ʱ� ���� zero ����
        if (flockingEnemy.Colleague == null || flockingEnemy.Colleague.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        foreach (var f in flockingEnemy.Colleague)
        {
            if (f != flock)
            {
                magnitude = Vector3.Magnitude(flock.transform.position - f.transform.position);
                magnitude /= flockingEnemy.Avoidance;
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
    public override void Move(Vector3 velocity, Player target = null)
    {
        if (flockingEnemy == null)
            return;

        //���ڷ� ���޹��� velocity�� ���� ������ ���� ����
        float mg = 1;
        Vector3 avoid = Avoidance(ref mg);
        velocity = velocity.normalized * mg;
        //if(avoid == Vector3.zero)
        //{
        velocity += avoid * flockingEnemy.AvoidPower;
        //}
        //else
        //{
        //    velocity = avoid;
        //}

        base.Move(velocity, target);
        //velocity.y = 0; // Y�� ȸ���� ����
        //if (velocity != Vector3.zero)
        //{
        //    //velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

        //    //Quaternion targetRotation = Quaternion.LookRotation(velocity);
        //    //flock.transform.rotation = targetRotation;

        //    //Quaternion targetRotation = Quaternion.LookRotation(velocity);
        //    //flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //    LookAtVelocity(velocity);
        //    LookAtTargetWithinAngle(target);

        //    //�����̰�
        //    flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
        //}
        ////desiredVelocity = velocity;
    }
}
