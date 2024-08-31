using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dot_Poison
{
    //�̰͸� �����ϸ� �������� ��ó�� new �Ҷ� �ִ°Ŵϱ�
    private readonly List<DotInfomation> list;
    [NonSerialized]
    private readonly Stat master;
    [NonSerialized]
    private Coroutine coroutine;
    public Dot_Poison(Stat master, DotInfomation dotInfomation)
    {
        this.master = master;
        list = new();
        Dot(dotInfomation);
    }

    public void Dot(DotInfomation infomation)
    {
        list.Add(infomation);
        if (coroutine == null)
            coroutine = master.StartCoroutine(Duration());
    }

    private void Damage()
    {
        List<Stat> stats = list.GroupBy(di => di.Agent)
                       .Select(group => group.Key)
                       .ToList();
        for (int i = 0; i < list.Count; i++)
        {
            AttackInformation attackInformation = new (stats[i], AttackType.NONE);
            attackInformation.Additional.Add(new (list.Where(x => x.Agent == stats[i]).Sum(x => x.Damage), DamageType.AP, false));
            master.Be_Attacked(attackInformation);
        }
    }

    private IEnumerator Duration()
    {
        while (list.Count > 0)
        {
            yield return new WaitForSeconds(master.DotTime);
            Damage();
            list.ForEach(x => x.Duration -= 1);
            list.Remove(list.Find(x => x.Duration <= 0));
        }
    }
}
