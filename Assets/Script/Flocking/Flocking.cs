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
            //함수로 생성된 값은 크기가 다양함 최대치를 정해주기 위한 작업
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

    //flocks들의 평균 위치값
    public Vector3 Cohesion(Flock[] flocks, Flock flock)
    {
        //참고할 flocks가 없다면 아 함수는 움직임에 영향을 주지 않기 위해 zero를 리턴함
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;

        //모든 포지션을 더하고 n/1하면 월드 좌표 기준 평균값이 나옴
        foreach (var f in flocks)
        {
            if (f != flock)
            {
                velocity += f.transform.position;
            }
        }
        velocity /= flocks.Length;

        //월드 좌표기준에서 자신의 좌표를 빼면 로컬기준 방향과 길이가 남음
        return (velocity - flock.transform.position).normalized;
    }

    //flocks들의 평균 위치값
    public Vector3 SteeredCohesion(Flock[] flocks, Flock flock)
    {
        //참고할 flocks가 없다면 아 함수는 움직임에 영향을 주지 않기 위해 zero를 리턴함
        if (flocks.Length == 0 || Vector3.SqrMagnitude(flock.transform.position - flocks[0].transform.position) < cohesionRange * cohesionRange)
            return Vector3.zero;

        Vector3 vector = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        //모든 포지션을 더하고 n/1하면 월드 좌표 기준 평균값이 나옴
        foreach (var f in flocks)
        {
            if (f != flock)
            {
                velocity += f.transform.position;
            }
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
        //참고할 flocks가 없다면 움직임에 영향을 주지 않기 위해
        if (flocks.Length == 0)
            return Vector3.zero;

        Vector3 velocity = Vector3.zero;

        //모든 flocks들의 forward를 더하고 나누면 역시 평균적으로 바라보는 방향이 나옴
        foreach (var f in flocks)
        {
            if (f != flock)
            {
                velocity += f.transform.forward;
            }
        }
        velocity /= flocks.Length;

        //방향이기에 이미 로컬기준이라 값 변경 불필요
        return velocity.normalized;
    }

    //flocks들의 평균적으로 향하고 있는 방향
    public Vector3 AlignmentT(Flock[] flocks, Flock flock)
    {
        //참고할 flocks가 없다면 움직임에 영향을 주지 않기 위해
        if (flocks.Length == 0)
            return Vector3.zero;

        //방향이기에 이미 로컬기준이라 값 변경 불필요
        return flocks[0].transform.forward;
    }

    //너무 가까운 flocks들에게서 반대방향의 평균
    public Vector3 Avoidance(Flock[] flocks, Flock flock)
    {
        //참고할 flocks가 없다면 움직임에 영향을 주지 않기 위해 zero 리턴
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

        //애초에 로컬로 계산해서 방향과 크기만이 있음
        return velocity.normalized;
    }
}

