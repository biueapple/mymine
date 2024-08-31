using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Charge : ISkill, IActiveSkill
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

    private readonly int duration;
    public int Duration => duration;

    private readonly MoveSystem moveSystem;
    public MoveSystem MoveSystem => moveSystem;

    public float Figure
    {
        get
        {
            return coefficients.ToList().Sum(x => x.Calculate(level, stat));
        }
    }

    private readonly AttributePiece piece;

    public Charge(Stat stat, MoveSystem moveSystem)
    {
        this.stat = stat;

        name = "돌진";
        level = 0;
        maxLevel = 5;
        cooltime = 4;
        duration = 5;
        coefficients = new Coefficient[2];
        coefficients[0] = new Coefficient(0, 5, Attribute_Property.HP, 0.1f);
        coefficients[1] = new Coefficient(0, 0, Attribute_Property.DEFENCE, 0.1f);
        damageType = DamageType.AD;
        description = $"앞으로 돌진하며 {duration}초동안 방어력이 {duration}초동안 <color=Yellow>{Figure}</color> 증가합니다. ";
        detailDescription =
            $"앞으로 돌진하며 " +
            $"<color=White>{coefficients[0].LevelFigure} * {level}" +
            $"</color> + <color=Green>{coefficients[0].Value(stat)} * {coefficients[0].CoefficientFigure}</color>" +
            $"<color=Orange>{coefficients[1].Value(stat)} * {coefficients[1].CoefficientFigure}</color>" +
            $" 만큼의 방어력이 {duration}초간 추가됩니다.";
        piece = new AttributePiece(Attribute_Property.DEFENCE, Figure);
        this.moveSystem = moveSystem;
        icon = Resources.Load<Sprite>("Skill/ADB");
    }

    public void Use()
    {
        stat.AddStat(piece);
        moveSystem.AddExternalForcesGround((stat.transform.forward + Vector3.up) * 5);
        GameManager.Instance.StartCoroutine(TimerCoroutine());
        stat.SkillTryCall();
        cooltimer = cooltime;
        GameManager.Instance.StartCoroutine(CooltimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(duration);
        stat.TakeStat(piece);
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

    }
}
