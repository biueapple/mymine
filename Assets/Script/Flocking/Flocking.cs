using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    public Flock[] flock;
    public float cohesionPower; 
    public float alignmentPower;
    public float avoidancePower;
    public float cohesionRange;
    public float avoidanceRange;
    public float smoothTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < flock.Length; i++)
        {
            Vector3 velocity = Vector3.zero;
            Vector3 vector;

            vector = SteeredCohesion(flock, flock[i]) * cohesionPower;
            //�Լ��� ������ ���� ũ�Ⱑ �پ��� �ִ�ġ�� �����ֱ� ���� �۾�
            if (vector != Vector3.zero)
            {
                velocity += vector;
            }

            //vector = AlignmentT(flock, flock[i]) * alignmentPower;
            //if (vector != Vector3.zero)
            //{
            //    velocity += vector;
            //}

            vector = Avoidance(flock, flock[i]) * avoidancePower;
            if (vector != Vector3.zero)
            {
                velocity += vector;
            }

            velocity = Time.deltaTime * 3 * velocity.normalized;

            if(velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity);
                flock[i].transform.rotation = targetRotation;
            }
            
            flock[i].Move(velocity);
        }
    }

    //flocks���� ��� ��ġ��
    public Vector3 Cohesion(Flock[] flocks, Flock flock)
    {
        //������ flocks�� ���ٸ� �� �Լ��� �����ӿ� ������ ���� �ʱ� ���� zero�� ������
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;

        //��� �������� ���ϰ� n/1�ϸ� ���� ��ǥ ���� ��հ��� ����
        foreach (var f in flocks)
        {
            if (f != flock)
            {
                velocity += f.transform.position;
            }
        }
        velocity /= flocks.Length;

        //���� ��ǥ���ؿ��� �ڽ��� ��ǥ�� ���� ���ñ��� ����� ���̰� ����
        return (velocity - flock.transform.position).normalized;
    }

    //flocks���� ��� ��ġ��
    public Vector3 SteeredCohesion(Flock[] flocks, Flock flock)
    {
        //������ flocks�� ���ٸ� �� �Լ��� �����ӿ� ������ ���� �ʱ� ���� zero�� ������
        if (flocks.Length == 0 || Vector3.SqrMagnitude(flock.transform.position - flocks[0].transform.position) < cohesionRange * cohesionRange)
            return Vector3.zero;

        Vector3 vector = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        //��� �������� ���ϰ� n/1�ϸ� ���� ��ǥ ���� ��հ��� ����
        foreach (var f in flocks)
        {
            if (f != flock)
            {
                velocity += f.transform.position;
            }
        }
        velocity /= flocks.Length;

        //���� ��ǥ���ؿ��� �ڽ��� ��ǥ�� ���� ���ñ��� ����� ���̰� ����
        velocity -= flock.transform.position;
        velocity = Vector3.SmoothDamp(flock.transform.forward, velocity, ref vector, smoothTime);
        return velocity;
    }

    //flocks���� ��������� ���ϰ� �ִ� ����
    public Vector3 Alignment(Flock[] flocks, Flock flock)
    {
        //������ flocks�� ���ٸ� �����ӿ� ������ ���� �ʱ� ����
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;

        //��� flocks���� forward�� ���ϰ� ������ ���� ��������� �ٶ󺸴� ������ ����
        foreach (var f in flocks)
        {
            if (f != flock)
            {
                velocity += f.transform.forward;
            }
        }
        velocity /= flocks.Length;

        //�����̱⿡ �̹� ���ñ����̶� �� ���� ���ʿ�
        return velocity.normalized;
    }

    //flocks���� ��������� ���ϰ� �ִ� ����
    public Vector3 AlignmentT(Flock[] flocks, Flock flock)
    {
        //������ flocks�� ���ٸ� �����ӿ� ������ ���� �ʱ� ����
        if (flocks.Length == 0)
            return Vector3.zero;

        //�����̱⿡ �̹� ���ñ����̶� �� ���� ���ʿ�
        return flocks[0].transform.forward;
    }

    //�ʹ� ����� flocks�鿡�Լ� �ݴ������ ���
    public Vector3 Avoidance(Flock[] flocks, Flock flock)
    {
        //������ flocks�� ���ٸ� �����ӿ� ������ ���� �ʱ� ���� zero ����
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        foreach (var f in flocks)
        {
            if (f != flock)
            {
                float distance = Vector3.Distance(flock.transform.position, f.transform.position);
                if (distance < avoidanceRange)
                {
                    velocity += (flock.transform.position - f.transform.position).normalized / distance;
                }
            }
        }

        //���ʿ� ���÷� ����ؼ� ����� ũ�⸸�� ����
        return velocity.normalized;
    }
}

