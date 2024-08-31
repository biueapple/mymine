using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRayDistanceDetection : ISensing
{
    //���� (���� �ڽ��� �ǹ�)
    readonly Transform criteria;
    //������ �Ÿ�
    readonly float distance;

    public WorldRayDistanceDetection(Transform Criteria, float Distance)
    {
        criteria = Criteria;
        distance = Distance;
    }

    public bool Sensing(Transform[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (Vector3.Distance(criteria.position, values[i].position) < distance)
            {
                if(World.Instance.WorldRaycast(criteria.position, (values[i].position - criteria.position).normalized, distance) == null)
                    return true;
            }
        }
        return false;
    }

    public T Sensing<T>(T[] values) where T : Component
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (Vector3.Distance(criteria.position, values[i].transform.position) < distance)
            {
                if (World.Instance.WorldRaycast(criteria.position, (values[i].transform.position - criteria.position).normalized, distance) == null)
                    return values[i].GetComponent<T>();
            }
        }
        return default;
    }

}
