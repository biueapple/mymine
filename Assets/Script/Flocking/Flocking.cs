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

            //vector = SteeredCohesion(RemoveAt(flock, i), flock[i]) * cohesionPower;
            ////�Լ��� ������ ���� ũ�Ⱑ �پ��� �ִ�ġ�� �����ֱ� ���� �۾�
            //if (vector != Vector3.zero)
            //{
            //    if (vector.sqrMagnitude > cohesionPower * cohesionPower)
            //    {
            //        vector.Normalize();
            //        vector *= cohesionPower;
            //    }
            //    velocity += vector;
            //}

            vector = Alignment(RemoveAt(flock, i), flock[i]) * alignmentPower;
            if (vector != Vector3.zero)
            {
                if (vector.sqrMagnitude >= alignmentPower * alignmentPower)
                {
                    vector.Normalize();
                    vector *= alignmentPower;
                }
                velocity += vector;
            }

            //vector = Avoidance(RemoveAt(flock, i), flock[i]) * avoidancePower;
            //if (vector != Vector3.zero)
            //{
            //    if (vector.sqrMagnitude >= avoidancePower * avoidancePower)
            //    {
            //        vector.Normalize();
            //        vector *= avoidancePower;
            //    }
            //    velocity += vector;
            //}

            velocity = Time.deltaTime * 3 * velocity.normalized;

            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            flock[i].transform.rotation = targetRotation;
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
        for (int i = 0; i < flocks.Length; i++)
        {
            velocity += flocks[i].transform.position;
        }
        velocity /= flocks.Length;

        //���� ��ǥ���ؿ��� �ڽ��� ��ǥ�� ���� ���ñ��� ����� ���̰� ����
        return velocity - flock.transform.position;
    }

    //flocks���� ��� ��ġ��
    public Vector3 SteeredCohesion(Flock[] flocks, Flock flock)
    {
        //������ flocks�� ���ٸ� �� �Լ��� �����ӿ� ������ ���� �ʱ� ���� zero�� ������
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 vector = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        //��� �������� ���ϰ� n/1�ϸ� ���� ��ǥ ���� ��հ��� ����
        for (int i = 0; i < flocks.Length; i++)
        {
            velocity += flocks[i].transform.position;
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
        //������ flocks�� ���ٸ� �����ӿ� ������ ���� �ʱ� ���� �ٶ󺸴� ������ �״�� ������
        if (flocks.Length == 0)
            return flock.transform.forward;

        Vector3 velocity = Vector3.zero;

        //��� flocks���� forward�� ���ϰ� ������ ���� ��������� �ٶ󺸴� ������ ����
        for (int i = 0; i < flocks.Length; i++)
        {
            velocity += flocks[i].transform.forward;
        }
        velocity /= flocks.Length;

        //�����̱⿡ �̹� ���ñ����̶� �� ���� ���ʿ�
        return velocity;
    }

    //�ʹ� ����� flocks�鿡�Լ� �ݴ������ ���
    public Vector3 Avoidance(Flock[] flocks, Flock flock)
    {
        //������ flocks�� ���ٸ� �����ӿ� ������ ���� �ʱ� ���� zero ����
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        int avoid = 0;
        //
        for (int i = 0; i < flocks.Length; i++)
        {
            //�ʹ� ������� �ǹ� ������ ����(Magnitude)�� ��� xyz���� ���� �� �������μ� ���� (��Ȯ���� �ٸ����� ����)
            //SqrMagnitude�� ������ �ʱ⿡ ������ ���� ��� ���� ŭ (�񱳴���� �����ϸ� �Ȱ�����)
            if (Vector3.SqrMagnitude(flocks[i].transform.position - flock.transform.position) <= avoidanceRange)
            {
                //�ʹ� ����� flock�� ���� �ϳ� ���ϰ�
                avoid++;
                //�ݴ� �������� ���� ������ ���� if�� ����� �� ��� ��ǥ ����̰� velocity�� �������� ���� �ݴ�� �� ����
                //���� if�� ���̸� ��� ���̱⿡ velocity ����Ҷ�ó�� �ݴ�� �ص� ����� ���� ��ó�� ����߿� -�� ������� ������ ���� �� ����
                velocity += (flock.transform.position - flocks[i].transform.position);
            }
        }
        //�ʹ� ����� ģ���� 0���̸� �����⿡�� ���װ� ���ϱ�
        if(avoid > 0)
            velocity /= avoid;

        //���ʿ� ���÷� ����ؼ� ����� ũ�⸸�� ����
        return velocity;
    }

    Flock[] RemoveAt(Flock[] array, int index)
    {
        return array.Where((source, idx) => idx != index).ToArray();
    }
}

