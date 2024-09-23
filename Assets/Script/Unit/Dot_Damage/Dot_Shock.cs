using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//stat을 사용해 대미지를 넣을것인가 unit을 이용해 대미지를 넣을것인가
//그냥 stat을 사용하고 맞았을때 반짝인다던가 하는 이펙트같은것들은 callback을 이용하는게 좋을듯 함
[Serializable]
public class Dot_Shock
{
    //이것만 저장하면 나머지는 어처피 new 할때 넣는거니까
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
        //가해자를 중복없이 저장
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
