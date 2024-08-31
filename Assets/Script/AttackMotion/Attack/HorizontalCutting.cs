using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HorizontalCutting : IAttackMotion
{
    //����� (�ִϸ��̼� ��� �� �ʿ��Ұ� ���Ƽ�)
    private readonly Player user;

    private readonly string name;
    public string Name { get { return name; } } 

    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    //������ ��������
    private readonly Vector3 position;
    public Vector3 Position { get { return position + user.transform.forward; } }

    //������ ũ��
    private readonly Vector3 range;
    public Vector3 Range { get { return range; } }

    private readonly float timelimit;
    public float Timelimit { get { return timelimit; } }

    private readonly float delay;
    public float Delay { get { return delay; } }

    private readonly float multiple;
    public float Multiple { get { return multiple; } }

    public HorizontalCutting(Player user)
    {
        this.user = user;
        name = "Ⱦ����";
        icon = Resources.Load<Sprite>("Item/Sword");
        position = new Vector3(0, user.Hand.localPosition.y, 0);
        range = new Vector3(3, 1, 1);
        timelimit = 1;
        delay = 1f;
        multiple = 0.9f;
    }

    public void Motion()
    {
        Collider[] colliders = Physics.OverlapBox(user.transform.position + position + user.transform.forward, range * 0.5f, user.transform.rotation);
        List<Stat> stat = new (); 
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].transform.TryGetComponent(out Substance substance))
            {
                if (substance.Unit == user)
                    continue;

                if (!stat.Contains(substance.Unit.STAT))
                    stat.Add(substance.Unit.STAT);

                AttackInformation information = new (user.STAT, AttackType.NOMAL);
                if(user.STAT.Critical)
                    information.Additional.Add(new ((user.STAT.AD + (user.STAT.AD * user.STAT.CriticalMultiplier)) * multiple, DamageType.AD, true));
                else
                    information.Additional.Add(new (user.STAT.AD * multiple, DamageType.AD, false));

                substance.Hit(information);
            }
        }
        if(user.Animator != null)
            user.Animator.SetTrigger("HorizontalCutting");
        user.STAT.AttackTryCall(stat.ToArray());
        SoundManager.Instance.CreateSound(user.transform.position + position + user.transform.forward, SoundManager.SoundClips.Sowrd);
    }
}
