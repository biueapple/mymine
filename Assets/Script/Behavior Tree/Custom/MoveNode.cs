using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode
{
    ////�ڽŵ��� ������ ������ ������ �޾� ��ġ�� ���ؾ� ��
    ////�����
    //protected readonly Transform[] colleague;
    //�ڽ�
    protected readonly Enemy flock;

    ////����� ���� �ȵǴ� �Ÿ�
    //protected readonly float avoidance;

    protected readonly float rotationSpeed;

    //protected Vector3 desiredVelocity;
    ////���� ��������� ���� ����
    //protected readonly float momentum;

    //protected readonly float power;

    public MoveNode(Enemy flock, float rotationSpeed)
    {
        this.flock = flock;
        this.rotationSpeed = rotationSpeed;
    }

    //������ ������ �Է�
    public virtual void Move(Vector3 velocity, Player target = null)
    {
        ////���ڷ� ���޹��� velocity�� ���� ������ ���� ����
        //float mg = 1 ;
        //Vector3 avoid = Avoidance(ref mg);
        //velocity = velocity.normalized * mg;
        ////if(avoid == Vector3.zero)
        ////{
        //    velocity += avoid * power;
        ////}
        ////else
        ////{
        ////    velocity = avoid;
        ////}

        velocity.y = 0; // Y�� ȸ���� ����
        if (velocity != Vector3.zero)
        {
            //velocity = Vector3.Lerp(desiredVelocity, velocity, momentum * Time.deltaTime);

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = targetRotation;

            //Quaternion targetRotation = Quaternion.LookRotation(velocity);
            //flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            LookAtVelocity(velocity);
            LookAtTargetWithinAngle(target);

            //�����̰�
            flock.transform.Translate(velocity.normalized * Time.deltaTime, Space.World);
        }
        //desiredVelocity = velocity;
    }

    //������ �ָ� ��ü�� ȸ��
    public void LookAtVelocity(Vector3 velocity)
    {
        Quaternion targetRotation = Quaternion.LookRotation(velocity);
        flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    //Ÿ���� �ָ� ��ü�� ȸ��
    public void LookAtTarget(Player target)
    {
        if (flock == null || target == null)
            return;

        // ĳ���Ϳ� Ÿ�� ���� ������ ���
        Vector3 directionToTarget = target.transform.position + new Vector3(0, target.Height, 0) - flock.transform.position;
        directionToTarget.y = 0; // ���� ���⸸ ��� (y �� ����)

        // Ÿ���� �ٶ󺸵��� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        flock.transform.rotation = Quaternion.RotateTowards(flock.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    //�Ӹ��� �÷��̾ �ٶ󺸵���, ���� ���� ������ �ٶ���� �ϴϱ�
    public void LookAtTargetWithinAngle(Player target)
    {
        if (flock.Head == null || target == null)
            return;

        // ĳ���Ϳ� Ÿ�� ���� ������ ���
        Vector3 directionToTarget = target.transform.position + new Vector3(0, target.Height, 0) - flock.Head.position;
        //directionToTarget.y = 0; // ���� ���⸸ ��� (y �� ����)

        // �� ����(ĳ���� forward ����� Ÿ�� ����) ������ ���� ���
        float angleToTarget = Vector3.Angle(flock.transform.forward, directionToTarget);

        // ������ ������ �þ� ����(viewAngle) ���� ������ Ÿ���� �ٶ�
        if (angleToTarget <= 45)
        {
            // Ÿ���� �ٶ󺸵��� �Ӹ� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            //enemy.Head.rotation = Quaternion.Slerp(enemy.Head.rotation, targetRotation, Time.deltaTime * 5f); // �ε巴�� ȸ��
            flock.Head.rotation = targetRotation;
        }
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