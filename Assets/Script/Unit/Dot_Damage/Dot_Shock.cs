using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//stat�� ����� ������� �������ΰ� unit�� �̿��� ������� �������ΰ�
//�׳� stat�� ����ϰ� �¾����� ��¦�δٴ��� �ϴ� ����Ʈ�����͵��� callback�� �̿��ϴ°� ������ ��
[Serializable]
public class Dot_Shock
{
    //�̰͸� �����ϸ� �������� ��ó�� new �Ҷ� �ִ°Ŵϱ�
    private readonly List<DotInfomation> list;
    [NonSerialized]
    private readonly Stat master;
    [NonSerialized]
    private readonly MoveSystem moveSystem;
    public Dot_Shock(Stat master, DotInfomation dotInfomation)
    {
        this.master = master;
        moveSystem = master.GetComponent<MoveSystem>();
        list = new();
        Dot(dotInfomation);
    }

    public void Dot(DotInfomation infomation)
    {
        list.Add(infomation);
        master.StartCoroutine(Duration());
    }

    private void Damage()
    {
        //�����ڸ� �ߺ����� ����
        List<Stat> stats = list.GroupBy(di => di.Agent)
                       .Select(group => group.Key)
                       .ToList();
        for (int i = 0; i < stats.Count; i++)
        {
            AttackInformation attackInformation = new (stats[i], AttackType.DOT);
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
            if(moveSystem != null)
                moveSystem.ShackleTimer(0.2f);
        }
    }
}
