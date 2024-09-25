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
            ////함수로 생성된 값은 크기가 다양함 최대치를 정해주기 위한 작업
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

    //flocks들의 평균 위치값
    public Vector3 Cohesion(Flock[] flocks, Flock flock)
    {
        //참고할 flocks가 없다면 아 함수는 움직임에 영향을 주지 않기 위해 zero를 리턴함
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;

        //모든 포지션을 더하고 n/1하면 월드 좌표 기준 평균값이 나옴
        for (int i = 0; i < flocks.Length; i++)
        {
            velocity += flocks[i].transform.position;
        }
        velocity /= flocks.Length;

        //월드 좌표기준에서 자신의 좌표를 빼면 로컬기준 방향과 길이가 남음
        return velocity - flock.transform.position;
    }

    //flocks들의 평균 위치값
    public Vector3 SteeredCohesion(Flock[] flocks, Flock flock)
    {
        //참고할 flocks가 없다면 아 함수는 움직임에 영향을 주지 않기 위해 zero를 리턴함
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 vector = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        //모든 포지션을 더하고 n/1하면 월드 좌표 기준 평균값이 나옴
        for (int i = 0; i < flocks.Length; i++)
        {
            velocity += flocks[i].transform.position;
        }
        velocity /= flocks.Length;

        //월드 좌표기준에서 자신의 좌표를 빼면 로컬기준 방향과 길이가 남음
        velocity -= flock.transform.position;
        velocity = Vector3.SmoothDamp(flock.transform.forward, velocity, ref vector, smoothTime);
        return velocity;
    }

    //flocks들의 평균적으로 향하고 있는 방향
    public Vector3 Alignment(Flock[] flocks, Flock flock)
    {
        //참고할 flocks가 없다면 움직임에 영향을 주지 않기 위해 바라보던 방향을 그대로 리턴함
        if (flocks.Length == 0)
            return flock.transform.forward;

        Vector3 velocity = Vector3.zero;

        //모든 flocks들의 forward를 더하고 나누면 역시 평균적으로 바라보는 방향이 나옴
        for (int i = 0; i < flocks.Length; i++)
        {
            velocity += flocks[i].transform.forward;
        }
        velocity /= flocks.Length;

        //방향이기에 이미 로컬기준이라 값 변경 불필요
        return velocity;
    }

    //너무 가까운 flocks들에게서 반대방향의 평균
    public Vector3 Avoidance(Flock[] flocks, Flock flock)
    {
        //참고할 flocks가 없다면 움직임에 영향을 주지 않기 위해 zero 리턴
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;
        int avoid = 0;
        //
        for (int i = 0; i < flocks.Length; i++)
        {
            //너무 가까움을 의미 벡터의 길이(Magnitude)는 모든 xyz값을 더한 후 나눔으로서 나옴 (정확히는 다르지만 대충)
            //SqrMagnitude는 나누지 않기에 연산이 빠름 대신 값이 큼 (비교대상을 제곱하면 똑같아짐)
            if (Vector3.SqrMagnitude(flocks[i].transform.position - flock.transform.position) <= avoidanceRange)
            {
                //너무 가까운 flock의 수를 하나 더하고
                avoid++;
                //반대 방향으로 값을 더해줌 위에 if는 제대로 된 상대 좌표 계산이고 velocity에 더해지는 값은 반대로 된 값임
                //위에 if는 길이만 재는 것이기에 velocity 계산할때처럼 반대로 해도 상관은 없음 어처피 계산중에 -는 사라져서 음수가 나올 수 없음
                velocity += (flock.transform.position - flocks[i].transform.position);
            }
        }
        //너무 가까운 친구가 0명이면 나누기에서 버그가 나니까
        if(avoid > 0)
            velocity /= avoid;

        //애초에 로컬로 계산해서 방향과 크기만이 있음
        return velocity;
    }

    Flock[] RemoveAt(Flock[] array, int index)
    {
        return array.Where((source, idx) => idx != index).ToArray();
    }
}

