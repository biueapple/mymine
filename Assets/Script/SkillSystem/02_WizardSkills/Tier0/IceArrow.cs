using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : ISkill, IActiveSkill
{
    private readonly string name;
    public string Name => name;

    private readonly string description;
    public string Description => description;

    private readonly string detailDescription;
    public string DetailDescription => detailDescription;

    private int level;
    public int Level { get => level; set { level = value; level = level > maxLevel ? maxLevel : level; } }

    private readonly int maxLevel;
    public int MaxLevel => maxLevel;

    private readonly Stat stat;
    public Stat Stat { get => stat; }

    private readonly Sprite icon;
    public Sprite Icon => icon;

    private readonly float cooltime;
    public float Cooltime => cooltime - (cooltime * stat.CooltimeReduction);

    private float cooltimer;
    public float Cooltimer { get { return cooltimer; } }

    private readonly Coefficient[] coefficients;
    public Coefficient[] Coefficients => coefficients;

    private readonly DamageType damageType;
    public DamageType DamageType => damageType;

    private readonly Player player;
    private readonly ProjectileObject projectile;

    private PlayerInputStateAming stateAming;

    public IceArrow(Player player)
    {
        this.player = player;
        stat = player.STAT;

        name = "����ȭ��";
        level = 0;
        maxLevel = 5;
        cooltime = 4;

        projectile = Resources.Load<ProjectileObject>("Projectile/IceArrow");
        icon = Resources.Load<Sprite>("Skill/ADB");

        //��� �߻��� ���ΰ�
        ProjectileObject[] projectileObjects = new ProjectileObject[3];
        for (int i = 0; i < projectileObjects.Length; i++)
        {
            ProjectileObject projectileObject = GameObject.Instantiate(projectile);
            //substance�� �΋H���� �����Ǵ� ����
            DestroyToProjectile destroy = new(player, projectileObject.gameObject);
            //���忡 �����ϴ� ��ϰ� �浹�ϸ� �����Ǵ� ����
            WorldCollisionToProjectile collisionToProjectile = new(projectileObject, destroy);
            //�����̴� ȿ��
            MoveToProjectile move = new(projectileObject.transform, 5, collisionToProjectile);
            //������� �ִ� ȿ��
            AttackInformation attackInformation = new(stat, AttackType.SPECIAL);
            attackInformation.Additional.Add(new(10, DamageType.AP, false));
            DamageToProjectile damage = new(player, move, attackInformation);
            //�󸮴� ȿ���� �����
            IceEffectToProjectile iceEffect = new(player, damage);

            projectileObject.Projectile = iceEffect;
            projectileObject.gameObject.SetActive(false);
            projectileObjects[i] = projectileObject;
        }
        //����� �߻��� ���ΰ�
        stateAming = new PlayerInputStateAming(player.Camera, player, UIManager.Instance.Canvas, projectileObjects, 3);
    }

    public void Use()
    {
        player.Aiming(stateAming);
        cooltimer = cooltime;
        GameManager.Instance.StartCoroutine(CooltimerCoroutine());
    }

    private IEnumerator CooltimerCoroutine()
    {
        while (cooltimer > 0)
        {
            yield return null;
            cooltimer -= Time.deltaTime;
        }
    }

    public void Cancle()
    {
        cooltimer = 0;
        stateAming.RightDown();
    }
}
