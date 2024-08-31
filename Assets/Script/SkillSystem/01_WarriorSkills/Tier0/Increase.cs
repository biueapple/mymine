using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increase : ISkill, IAttackTryCallback, IActiveSkill, IDamageIncreaseCallback
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

    public float Damage
    {
        get
        {
            return coefficients[0].Calculate(level, stat) + coefficients[1].Calculate(level, stat);
        }
    }

    public Increase(Stat stat)
    {
        this.stat = stat;

        name = "추가공격";
        level = 0;
        maxLevel = 5;
        cooltime = 4;
        coefficients = new Coefficient[2];
        coefficients[0] = new Coefficient(10, 10, Attribute_Property.AP, 0.5f);
        coefficients[1] = new Coefficient(10, 10, Attribute_Property.AD, 0.5f);
        damageType = DamageType.AD;
        description = $"다음 기본공격에 <color=Orange>{Damage}</color> 만큼의 대미지가 추가됩니다";
        detailDescription =
            $"다음 기본공격에 <color=White>{coefficients[0].Figure}</color> + " +
            $"<color=White>{coefficients[0].LevelFigure} * {level}" +
            $"</color> + <color=Blue>{coefficients[0].Value(stat)} * {coefficients[0].CoefficientFigure}</color>" +
            $"<color=White>{coefficients[1].Figure}</color> + " +
            $"<color=White>{coefficients[1].LevelFigure} * {level}</color> + " +
            $"<color=Orange>{coefficients[1].Value(stat)} * {coefficients[1].CoefficientFigure}</color>" +
            $" 만큼의 대미지가 추가됩니다.";
        icon = Resources.Load<Sprite>("Skill/ADB");
    }

    //맞추지 못해도 사용되는 스킬
    public void AttackTry(Stat[] stats)
    {
        //사용 종료
        stat.DamageIncreaseCallback.Remove(this);
        stat.AttackTryCallbacks.Remove(this);
        GameManager.Instance.StartCoroutine(CooltimerCoroutine());
    }

    public void Use()
    {
        stat.DamageIncreaseCallback.Add(this);
        stat.AttackTryCallbacks.Add(this);
        stat.SkillTryCall();
        cooltimer = cooltime;
    }

    public void DamageIncrease(AttackInformation attackInformation)
    {
        attackInformation.Additional.Add(new DamageInfomation(Damage, damageType, false));
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
        //사용 종료
        stat.DamageIncreaseCallback.Remove(this);
        stat.AttackTryCallbacks.Remove(this);
    }
}
