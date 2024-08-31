using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRayDistanceDetection : ISensing
{
    //기준 (보통 자신을 의미)
    readonly Transform criteria;
    //감지할 거리
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
