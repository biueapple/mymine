using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : IAttackMotion
{
    //����� (�ִϸ��̼� ��� �� �ʿ��Ұ� ���Ƽ�)
    private readonly Zombie zombie;

    private readonly string name;
    public string Name { get { return name; } }

    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    //������ ��������
    private readonly Vector3 position;
    public Vector3 Position { get { return position + zombie.transform.forward; } }

    //������ ũ��
    private readonly Vector3 range;
    public Vector3 Range { get { return range; } }

    private readonly float timelimit;
    public float Timelimit { get { return timelimit; } }

    private readonly float delay;
    public float Delay { get { return delay; } }

    private readonly float multiple;
    public float Multiple { get { return multiple; } }

    public ZombieAttack(Zombie zombie)
    {
        this.zombie = zombie;
        name = "���� ������";
        icon = Resources.Load<Sprite>("Item/Sword");
        position = new Vector3(0, 0.75f, 1);
        range = new Vector3(1, 1.5f, 1);
        timelimit = 1;
        delay = 1f;
        multiple = 0.9f;
    }

    public void Motion()
    {
        Collider[] colliders = Physics.OverlapBox(zombie.transform.position + position + zombie.transform.forward, range * 0.5f, zombie.transform.rotation);
        List<Stat> stat = new();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform.TryGetComponent(out Substance substance))
            {
                if (substance.Unit == zombie)
                    continue;

                if (!stat.Contains(substance.Unit.STAT))
                    stat.Add(substance.Unit.STAT);

                AttackInformation information = new(zombie.STAT, AttackType.NOMAL);
                if (zombie.STAT.Critical)
                    information.Additional.Add(new((zombie.STAT.AD + (zombie.STAT.AD * zombie.STAT.CriticalMultiplier)) * multiple, DamageType.AD, true));
                else
                    information.Additional.Add(new(zombie.STAT.AD * multiple, DamageType.AD, false));

                substance.Hit(information);
            }
        }
        if (zombie.Animator != null)
        {
            zombie.Animator.SetTrigger("AttackTrigger");
            Debug.Log("�ִϸ��̼� �۵�");
        }
            
        zombie.STAT.AttackTryCall(stat.ToArray());
        SoundManager.Instance.CreateSound(zombie.transform.position + position + zombie.transform.forward, SoundManager.SoundClips.Sowrd);
    }
}
