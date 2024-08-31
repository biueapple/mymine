using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ݽ� ���� ���ݱ��� delay�� �ɸ��� 
//������ ���ݸ���̿��ٸ� ���� ���ݸ���� ������ timelimit���� ��޷��� ó������ ���ư��� �ϵ��� �����
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

    //�����Ҷ����� 0��° ��Ǻ���
    //Ÿ�Ӹ���Ʈ�� �ѱ��� ���� �ð��ȿ� �ٽ� �õ��ϸ� ���� ��������
    //�׷��� �ʴٸ� �ٽ� 0������
    public void Attack()
    {
        if (delayCoroutine == null)
        {
            IAttackMotion motion = GetMotion();
            
            if (motion != null)
            {
                //���� �õ�
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

    //���� �ε��� �ʱ�ȭ
    private void Initialization()
    {
        index = 0;
    }

    //���� �� ��޷����ϴ� �ð�
    private IEnumerator DelayCalculate(float delay)
    {
        //���� ���ݱ��� ��޷��� �ϴ� �ð�
        yield return new WaitForSeconds(delay);
        //���� ���� �غ�
        //NextMotion();
        //Ÿ�̸� ����
        //timelimitCoroutine = GameManager.Instance.StartCoroutine(TimelimitCalculate(motions[index].Timelimit * stat.AttackSpeed));
        delayCoroutine = null;
    }

    //���� �� ��޷��� �ϴ� �ð�
    private IEnumerator TimelimitCalculate(float limit)
    {
        //�����ð� �Ŀ� ���� �ε����� �ʱ�ȭ
        yield return new WaitForSeconds(limit);
        //���� �ε��� �ʱ�ȭ
        Initialization();
        //���� �������� �ڷ�ƾ�� ����
        timelimitCoroutine = null;
        //���� ���� ǥ��
        delayCoroutine = null;
    }
}
