using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dot_Burn
{
    //이것만 저장하면 나머지는 어처피 new 할때 넣는거니까
    private readonly List<DotInfomation> list;
    [NonSerialized]
    private readonly Stat master;
    [NonSerialized]
    private readonly MoveSystem moveSystem;
    [NonSerialized]
    private readonly JumpInputMove jump;
    public Dot_Burn(Stat master, DotInfomation dotInfomation)
    {
        this.master = master;
        moveSystem = master.GetComponent<MoveSystem>();
        list = new();
        jump = moveSystem.FindMoveMode<JumpInputMove>();
        Dot(dotInfomation);
    }

    public void Dot(DotInfomation infomation)
    {
        list.Add(infomation);
        master.StartCoroutine(Duration());
    }

    private void Damage()
    {
        List<Stat> stats = list.GroupBy(di => di.Agent)
                       .Select(group => group.Key)
                       .ToList();
        for (int i = 0; i < stats.Count; i++)
        {
            AttackInformation attackInformation = new (stats[i], AttackType.DOT);
            attackInformation.Additional.Add(new (list.Where(x => x.Agent == stats[i]).Sum(x => x.Damage), DamageType.AD, false));
            master.Be_Attacked(attackInformation);
        }
    }

    private IEnumerator Duration()
    {
        if (moveSystem != null)
            moveSystem.RemoveMoveMode(jump);
        while (list.Count > 0)
        {
            yield return new WaitForSeconds(master.DotTime);
            Damage();
            list.ForEach(x => x.Duration -= 1);
            list.Remove(list.Find(x => x.Duration <= 0));
        }
        if (moveSystem != null)
            moveSystem.AddMoveMode(jump);
    }
}
