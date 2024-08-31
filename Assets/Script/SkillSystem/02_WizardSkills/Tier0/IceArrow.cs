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

        name = "얼음화살";
        level = 0;
        maxLevel = 5;
        cooltime = 4;

        projectile = Resources.Load<ProjectileObject>("Projectile/IceArrow");
        icon = Resources.Load<Sprite>("Skill/ADB");

        //몇개를 발사할 것인가
        ProjectileObject[] projectileObjects = new ProjectileObject[3];
        for (int i = 0; i < projectileObjects.Length; i++)
        {
            ProjectileObject projectileObject = GameObject.Instantiate(projectile);
            //substance와 부딫히고 삭제되는 판정
            DestroyToProjectile destroy = new(player, projectileObject.gameObject);
            //월드에 존재하는 블록과 충돌하면 삭제되는 판정
            WorldCollisionToProjectile collisionToProjectile = new(projectileObject, destroy);
            //움직이는 효과
            MoveToProjectile move = new(projectileObject.transform, 5, collisionToProjectile);
            //대미지를 넣는 효과
            AttackInformation attackInformation = new(stat, AttackType.SPECIAL);
            attackInformation.Additional.Add(new(10, DamageType.AP, false));
            DamageToProjectile damage = new(player, move, attackInformation);
            //얼리는 효과를 만들고
            IceEffectToProjectile iceEffect = new(player, damage);

            projectileObject.Projectile = iceEffect;
            projectileObject.gameObject.SetActive(false);
            projectileObjects[i] = projectileObject;
        }
        //몇명에게 발사할 것인가
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
