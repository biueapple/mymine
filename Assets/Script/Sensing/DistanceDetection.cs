using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDetection : ISensing
{
    //���� (���� �ڽ��� �ǹ�)
    readonly Transform criteria;
    //������ �Ÿ�
    readonly float distance;
    public DistanceDetection(Transform Criteria, float Distance) 
    {
        criteria = Criteria;
        distance = Distance;
    }

    public bool Sensing(Transform[] values)
    {
        for(int i = 0; i < values.Length; i++)
        {
            if(Vector3.Distance(criteria.position, values[i].position) < distance)
                return true;
        }
        return false;
    }
    public T Sensing<T>(T[] values) where T : Component
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (Vector3.Distance(criteria.position, values[i].transform.position) < distance)
                return values[i].GetComponent<T>();
        }
        return default;
    }
}