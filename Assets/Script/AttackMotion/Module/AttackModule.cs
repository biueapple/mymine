using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격시 다음 공격까지 delay가 걸리고 
//마지막 공격모션이였다면 다음 공격모션이 없으니 timelimit까지 기달려서 처음으로 돌아가야 하도록 만들기
//delay -> motion -> positioning
public class AttackModule
{
    private readonly Stat stat;
    private readonly IAttackMotion[] motions;
    public IAttackMotion Motion { get { return motions[index]; } }
    private int index = 0;
    public int Index { get { return index; } }
    private Coroutine timelimitCoroutine = null;
    private Coroutine delayCoroutine = null;

    public AttackModule(int count, Stat stat)
    {
        motions = new IAttackMotion[count];
        this.stat = stat;
    }

    public void MotionAdd(IAttackMotion motion)
    {
        for(int i = 0; i < motions.Length; i++)
        {
            if (motions[i] == null)
            {
                motions[i] = motion;
                return;
            }
        }
    }

    public void MotionAdd(IAttackMotion motion, int index)
    {
        if(index < 0 || index >= motions.Length)
            return;
        motions[index] = motion;
    }

    //공격할때마다 0번째 모션부터
    //타임리미트를 넘기지 않은 시간안에 다시 시도하면 다음 공격으로
    //그렇지 않다면 다시 0번부터
    public void Attack()
    {
        if (delayCoroutine == null)
        {
            IAttackMotion motion = GetMotion();
            
            if (motion != null)
            {
                //공격 시도
                motion.Motion();
                delayCoroutine = GameManager.Instance.StartCoroutine(DelayCalculate(motion.Delay * stat.AttackSpeed));
            }
            index++;
        }
        
    }

    private IAttackMotion GetMotion()
    {
        for(int i = 0; i < motions.Length  ;i++ )
        {
            if (motions[(i + index) % motions.Length] != null)
            {
                index = (i + index) % motions.Length;
                return motions[index];
            }
        }
        return null;
    }

    //공격 인덱스 초기화
    private void Initialization()
    {
        index = 0;
    }

    //공격 후 기달려야하는 시간
    private IEnumerator DelayCalculate(float delay)
    {
        //다음 공격까지 기달려야 하는 시간
        yield return new WaitForSeconds(delay);
        //다음 공격 준비
        //NextMotion();
        //타이머 시작
        //timelimitCoroutine = GameManager.Instance.StartCoroutine(TimelimitCalculate(motions[index].Timelimit * stat.AttackSpeed));
        delayCoroutine = null;
    }

    //공격 전 기달려야 하는 시간
    private IEnumerator TimelimitCalculate(float limit)
    {
        //일정시간 후에 공격 인덱스를 초기화
        yield return new WaitForSeconds(limit);
        //공격 인덱스 초기화
        Initialization();
        //현재 실행중인 코루틴은 없음
        timelimitCoroutine = null;
        //공격 가능 표시
        delayCoroutine = null;
    }
}
