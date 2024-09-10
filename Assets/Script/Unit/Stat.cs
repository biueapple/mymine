using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;


[Serializable]
public class StatAttributes
{
    public float hp;
    public float mp;
    public float barrier;
    public float naturalHP;
    public float naturalMP;
    public float naturalBarrier;
    public float defence;
    public float resistance;
    public float ad;
    public float ap;
    public float attackSpeed;
    public float criticalChance;
    public float criticalMultiplier;
    public float criticalDefence;
    public float defencePenetration;
    public float defencePenetrationPer;
    public float resistancePenetration;
    public float resistancePenetrationPer;
    public float speed;
    public StatAttributes() { }
    public StatAttributes(float hp, float mp, float ba, float nhp, float nmp,float nba,  float de, float re, float ad,float ap, float ats,float cc, float cm,float cd, float dp, float dpp, float rp, float rpp, float s) 
    {
        this.hp = hp;
        this.mp = mp;
        barrier = ba;
        naturalHP = nhp;
        naturalMP = nmp;
        naturalBarrier = nba;
        defence = de;
        resistance = re;
        this.ad = ad;
        this.ap = ap;
        attackSpeed = ats;
        criticalChance = cc;
        criticalMultiplier = cm;
        criticalDefence = cd;
        defencePenetration = dp;
        defencePenetrationPer = dpp;
        resistancePenetration = rp;
        resistancePenetrationPer = rpp;
        speed = s;
    }
    public StatAttributes(StatAttributes stat)
    {
        hp = stat.hp;
        mp = stat.mp;
        barrier = stat.barrier;
        naturalHP = stat.naturalHP;
        naturalMP = stat.naturalMP;
        naturalBarrier = stat.naturalBarrier;
        defence = stat.defence;
        resistance = stat.resistance;
        ad = stat.ad;
        ap = stat.ap;
        attackSpeed = stat.attackSpeed;
        criticalChance = stat.criticalChance;
        criticalMultiplier = stat.criticalMultiplier;
        criticalDefence = stat.criticalDefence;
        defencePenetration = stat.defencePenetration;
        defencePenetrationPer= stat.defencePenetrationPer;
        resistancePenetration = stat.resistancePenetration;
        resistancePenetrationPer = stat.resistancePenetrationPer;
        speed = stat.speed;
    }
    public void Init(StatAttributes stat)
    {
        hp = stat.hp;
        mp = stat.mp;
        barrier = stat.barrier;
        naturalHP = stat.naturalHP;
        naturalMP = stat.naturalMP;
        naturalBarrier = stat.naturalBarrier;
        defence = stat.defence;
        resistance = stat.resistance;
        ad = stat.ad;
        ap = stat.ap;
        attackSpeed = stat.attackSpeed;
        criticalChance = stat.criticalChance;
        criticalMultiplier = stat.criticalMultiplier;
        criticalDefence = stat.criticalDefence;
        defencePenetration = stat.defencePenetration;
        defencePenetrationPer = stat.defencePenetrationPer;
        resistancePenetration = stat.resistancePenetration;
        resistancePenetrationPer = stat.resistancePenetrationPer;
        speed = stat.speed;
    }
    public float Value(Attribute_Property type)
    {
        return type switch
        {
            Attribute_Property.HP => hp,
            Attribute_Property.MP => mp,
            Attribute_Property.BARRIER => barrier,
            Attribute_Property.NATURALHP => naturalHP,
            Attribute_Property.NATURALMP => naturalMP,
            Attribute_Property.NATURALARRIER => naturalBarrier,
            Attribute_Property.DEFENCE => defence,
            Attribute_Property.RESISTANCE => resistance,
            Attribute_Property.AD => ad,
            Attribute_Property.AP => ap,
            Attribute_Property.ATTACKSPEED => attackSpeed,
            Attribute_Property.CRITICALCHANCE => criticalChance,
            Attribute_Property.CRITICALMULTIPLIER => criticalMultiplier,
            Attribute_Property.CRITICALDEFENCE => criticalDefence,
            Attribute_Property.DEFENCEPENETRATION => defencePenetration,
            Attribute_Property.DEFENCEPENETRATIONPER => defencePenetrationPer,
            Attribute_Property.RESISTANCEPENETRATION => resistancePenetration,
            Attribute_Property.RESISTANCEPENETRATIONPER => resistancePenetrationPer,
            Attribute_Property.SPEED => speed,
            _ => 0,
        };
    }
}

//shield
//Barrier
//protective

//현재 최대 체력
//오리지날 최대 체력
//오른 최대 체력

//현재 공격력
//오리지날 공격력
//오른 공격력

//버프들 디버프들

//공격 오리지날 대미지
//공격 타입
//대미지 타입
//추가 대미지 리스트 (item , skill)


//시간 유지 효과
//횟수 유지 효과 (시간도 포함)

//공격 전에 효과
//공격 후의 효과
//횟수는 호출과 상관없이 공격 시도에 따라 변화해야 함
//공격 시도에 따른 횟수 차감

/*
 * 누군가를 공격해서 발동하는 효과가
 * 횟수와 타이머를 보유할지
 * 효과를 생성하는 클래스가
 * 보유해서 체크하고 remove를 해줘야 하는지
 * 데코레이터 패턴으로
 * 효과 클래스를 만들고 거기에 조건을 달고있는 클래스를
 * 데코레이트 해서 콜백으로 전달하는 방식  (조건을)
 * 근데 인터페이스가 무슨 역할을 해야하는지 모르겠는데
 */

//애초에 ui부터 신경을 쓴게 문제
//직업에 따라 배울 수 있는 스킬 (스킬트리) 가 존재하고
//그것은 유닛이 가지고 있는거지 스킬트리 ui가 스킬을 소유하고
//플레이어에게 넣어주는 방식이 아님
//처음부터 다시 시작해야 할듯
//한번의 공격으로 여러명을 동시타격하는 근접공격의 경우
//스킬에서 어디까지가 한번의 공격인지 판단할 수가 없음
//여기서 구분을 해줘야함
//기존의 콜백으로는 어떤 스킬이 몇번 발동하면 사라지는지 몇초 유지되는지를 모르기에
//새로운 클래스를 만들어서 바꿔야함 새로운 대미지 클래스를 만들어서 Func를 사용할 필요성이 사라짐
//기존의 Action들은 아이템효과로 쓰일듯 하고
//이번에 만든 Effect클래스들은 스킬의 효과로 쓰일듯 함

public class AttackInformation
{
    private readonly Stat agent;
    public Stat Agent { get => agent; }
    private readonly AttackType attackType;
    public AttackType AttackType { get => attackType; }
    private readonly List<DamageInfomation> additional;
    public List<DamageInfomation> Additional { get => additional; }
    public AttackInformation(Stat agent, AttackType attackType)
    {
        this.agent = agent;
        this.attackType = attackType;
        additional = new();
    }
}

public class DamageInfomation
{
    public DamageInfomation(float figure, DamageType damageType, bool critical)
    {
        this.figure = figure;
        type = damageType;
        this.critical = critical;
    }
    private float figure;
    public float Figure { get => figure; set => figure = value; }
    private DamageType type;
    public DamageType DamageType { get => type; set => type = value; }
    private bool critical;
    public bool Critical { get => critical; set => critical = value; }
}

[Serializable]
public class DotInfomation
{
    private readonly Stat agent;
    public Stat Agent { get => agent; }
    private readonly float damage;
    public float Damage { get => damage; }
    private int duration;
    public int Duration { get => duration; set => duration = value; }

    public DotInfomation(Stat agent, float damage, int duration)
    {
        this.agent = agent;
        this.damage = damage;
        this.duration = duration;
    }
}


public enum ETeam
{
    Player = 1,
    Ally = 2,
    Enemy = 4,
    Neutral = 8,

}


[Serializable]
public class Stat : MonoBehaviour
{
    [SerializeField]
    private float hp;
    [SerializeField]
    private float mp;
    [SerializeField]
    private float barrier;

    [SerializeField]
    //기본
    private StatAttributes original = new ();
    [SerializeField]
    //상승
    private StatAttributes elevated = new ();

    //체력
    public float HP { get { return hp; } }

    public float MAXHP { get { return original.hp + elevated.hp; } }
    //체력 자연 회복량
    public float NaturalHP { get { return original.naturalHP + elevated.naturalHP; } }

    //마나
    public float MP { get { return mp; } }
    public float MAXMP { get { return original.mp + elevated.mp; } }
    public float NaturalMP { get { return original.naturalMP + elevated.naturalMP; } }

    //배리어
    public float Barrier { get => barrier; }
    public float MAXBarrier { get { return original.barrier + elevated.barrier; } }
    public float NaturalBarrier { get => original.naturalBarrier + elevated.naturalBarrier; }

    //방어력 100이 100퍼
    public float Defence { get { return original.defence + elevated.defence; } }

    //저항력 100이 100퍼
    public float Resistance { get { return original.resistance + elevated.resistance; } }

    //공격력
    public float AD { get { return original.ad + elevated.ad; } }

    //주문력
    public float AP { get { return original.ap + elevated.ap; } }

    //공격속도
    public float AttackSpeed { get { return 1 / ((original.attackSpeed + elevated.attackSpeed) * 0.7f) ; } }

    //치명타 확률 100이 100퍼
    public float CriticalChance { get { return original.criticalChance + elevated.criticalChance; } }

    //크리티컬이 터졌는가
    public bool Critical { get { if (UnityEngine.Random.Range(0f, 1f) <= CriticalChance * 0.01f) { return true; } else { return false; } } }

    //치명타 배율 1이 100퍼센트 추가 대미지
    public float CriticalMultiplier { get { return original.criticalMultiplier + elevated.criticalMultiplier; } }

    //치명타 방어력 100이 100퍼
    public float CriticalDefence { get { return original.criticalDefence + elevated.criticalDefence; } }

    //방어 관통력
    public float DefencePenetration { get { return original.defencePenetration + elevated.defencePenetration; } }

    //방어 관통력 퍼센트 100이 100퍼센트
    public float DefencePenetrationPer { get { return original.defencePenetrationPer + elevated.defencePenetrationPer; } }

    //저항 관통력
    public float ResistancePenetration { get { return original.resistancePenetration + elevated.resistancePenetration; } }

    //저항 관통력 퍼센트 100이 100퍼센트
    public float ResistancePenetrationPer { get { return original.resistancePenetrationPer + elevated.resistancePenetrationPer; } }

    //쿨타임 감소
    public float CooltimeReduction { get => 0.1f; }

    //이동속도
    public float Speed { get { return original.speed + elevated.speed; } }

    //private List<Buff> buffs = new List<Buff>();
    //public int buffCount { get { return buffs.Count; }}
    //private Coroutine buffCoroutine = null;

    //private List<Debuff> debuffs = new List<Debuff>();
    //public int debuffCount { get { return debuffs.Count; }}
    //private Coroutine debuffCoroutine = null;

    //Natural_Recovery_HP
    [NonSerialized] private Coroutine nrhp = null;
    //Natural_Recovery_MP
    [NonSerialized] private Coroutine nrmp = null;
    //Natural_Recovery_Barrier
    [NonSerialized] private Coroutine nrba = null;

    [SerializeField]
    protected ETeam team;
    public ETeam Team { get { return team; } }

    //감전 도트 
    [SerializeField]
    private Dot_Poison dot_Poison = null;
    //감전 도트 
    [SerializeField]
    private Dot_Shock dot_Shock = null;
    //감전 도트 
    [SerializeField]
    private Dot_Burn dot_Burn = null;
    //감전 도트 
    [SerializeField]
    private Dot_Bleeding dot_Bleeding = null;

    //도트뎀이나 자연 회복이 몇초에 한번 일어날지
    [SerializeField]
    private float dotTime = 2;
    public float DotTime { get { return dotTime; } }
    [SerializeField]
    private float naturalTime = 2;

    //대미지 증가 콜백
    [NonSerialized]
    private readonly List<IDamageIncreaseCallback> damageIncreaseCallbacks = new ();
    public List<IDamageIncreaseCallback> DamageIncreaseCallback { get => damageIncreaseCallbacks; }
    public void DamageIncreaseCall(AttackInformation attackInformation)
    { 
        for (int i = damageIncreaseCallbacks.Count - 1; i >= 0; i--)
            damageIncreaseCallbacks[i].DamageIncrease(attackInformation);
    } 
    //대미지를 띄우는 것도 여기에 넣기
    [NonSerialized]
    private readonly List<IDamageAfterCallback> damageAfterCallbacks = new ();
    public List<IDamageAfterCallback> DamageAfterCallbacks { get => damageAfterCallbacks; }
    public void DamageAfterCall(AttackInformation attackInformation, float damage)
    {
        for (int i = damageAfterCallbacks.Count - 1; i >= 0; i--)
            damageAfterCallbacks[i].DamageAfterCall(attackInformation, damage);
    }
    [NonSerialized]
    private readonly List<IHitBeforeCallback> hitBeforeCallbacks = new ();
    public List<IHitBeforeCallback> HitBeforeCallbacks { get => hitBeforeCallbacks; }
    public void HitBeforeCall(AttackInformation attackInformation)
    {
        for (int i = hitBeforeCallbacks.Count - 1; i >= 0; i--)
            hitBeforeCallbacks[i].HitBefore(attackInformation);

    }
    [NonSerialized]
    private readonly List<IHitAfterCallback> hitAfterCallbacks = new();
    public List<IHitAfterCallback> HitAfterCallbacks { get => hitAfterCallbacks; }
    public void HitAfterCall(AttackInformation attackInformation)
    {
        for (int i = hitAfterCallbacks.Count - 1; i >= 0; i--)
            hitAfterCallbacks[i].HitAfter(attackInformation);
    }

    //공격 시도에 대한 콜백
    private readonly List<IAttackTryCallback> attackTryCallbacks = new();
    public List<IAttackTryCallback> AttackTryCallbacks { get { return attackTryCallbacks; } }
    public void AttackTryCall(Stat[] stats)
    {
        for (int i = attackTryCallbacks.Count - 1; i >= 0; i--)
            attackTryCallbacks[i].AttackTry(stats);
    }
    //스킬 시도에 대한 콜백
    private readonly List<ISkillTryCallback> skillTryCallbacks = new();
    public List<ISkillTryCallback> SkillTryCallbacks { get => skillTryCallbacks; }
    public void SkillTryCall()
    {
        for (int i = skillTryCallbacks.Count - 1; i >= 0; i--)
            skillTryCallbacks[i].SkillTry();
    }

    private void Awake()
    {
        //Load();
        //dot 대미지들도 load해야함
    }

    private void Start()
    {
        hp = original.hp + elevated.hp;
        mp = original.mp + elevated.mp;
        barrier = original.barrier + elevated.barrier;
    }

    private void Update()
    {

    }

    private void OnApplicationQuit()
    {
        //Save();
    }

    //저장 경로 : C:\Users\사용자명\AppData\LocalLow\DefaultCompany\My InventorySystem
    public void Save()
    {
        //try
        //{
        //    IFormatter formatter = new BinaryFormatter();
        //    Stream stream = new FileStream(string.Concat(Application.persistentDataPath, "/Skills"), FileMode.Create, FileAccess.Write);
        //    formatter.Serialize(stream, callbackController.Skills);
        //    stream.Close();
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError($"예외 발생: {ex.Message}");
        //}
    }

    //불러오기 경로 : C:\Users\사용자명\AppData\LocalLow\DefaultCompany\My InventorySystem
    public void Load()
    {
        //if (File.Exists(string.Concat(Application.persistentDataPath, "/Skills")))
        //{
        //    IFormatter formatter = new BinaryFormatter();
        //    Stream stream = new FileStream(string.Concat(Application.persistentDataPath, "/Skills"), FileMode.Open, FileAccess.Read);
        //    List<ISkill> newContainer = (List<ISkill>)formatter.Deserialize(stream);
        //    for(int i = 0; i < newContainer.Count; i++)
        //    {
        //        callbackController.AcquisitionSkill(newContainer[i]);
        //    }
        //    for (int i = 0; i < callbackController.Skills.Count; i++)
        //    {
        //        callbackController.Skills[i].Stat = this;
        //    }
        //    stream.Close();
        //}
    }

    //
    //밑에 함수와는 다른점이 hp mp barriar 밖에 없음
    public float GetAttribute(Attribute_Property attribute)
    {
        return attribute switch
        {
            Attribute_Property.HP => MAXHP,
            Attribute_Property.MP => MAXMP,
            Attribute_Property.BARRIER => MAXBarrier,
            Attribute_Property.DEFENCE => Defence,
            Attribute_Property.RESISTANCE => Resistance,
            Attribute_Property.AD => AD,
            Attribute_Property.AP => AP,
            Attribute_Property.ATTACKSPEED => AttackSpeed,
            Attribute_Property.CRITICALCHANCE => CriticalChance,
            Attribute_Property.CRITICALDEFENCE => CriticalDefence,
            Attribute_Property.CRITICALMULTIPLIER => CriticalMultiplier,
            Attribute_Property.DEFENCEPENETRATION => DefencePenetration,
            Attribute_Property.DEFENCEPENETRATIONPER => DefencePenetrationPer,
            Attribute_Property.RESISTANCEPENETRATION => ResistancePenetration,
            Attribute_Property.RESISTANCEPENETRATIONPER => ResistancePenetrationPer,
            Attribute_Property.SPEED => Speed,
            _ => 0,
        };
    }

    public float NowAttribute(Attribute_Property attribute)
    {
        return attribute switch
        {
            Attribute_Property.HP => hp,
            Attribute_Property.MP => mp,
            Attribute_Property.BARRIER => barrier,
            Attribute_Property.DEFENCE => Defence,
            Attribute_Property.RESISTANCE => Resistance,
            Attribute_Property.AD => AD,
            Attribute_Property.AP => AP,
            Attribute_Property.ATTACKSPEED => AttackSpeed,
            Attribute_Property.CRITICALCHANCE => CriticalChance,
            Attribute_Property.CRITICALDEFENCE => CriticalDefence,
            Attribute_Property.CRITICALMULTIPLIER => CriticalMultiplier,
            Attribute_Property.DEFENCEPENETRATION => DefencePenetration,
            Attribute_Property.DEFENCEPENETRATIONPER => DefencePenetrationPer,
            Attribute_Property.RESISTANCEPENETRATION => ResistancePenetration,
            Attribute_Property.RESISTANCEPENETRATIONPER => ResistancePenetrationPer,
            Attribute_Property.SPEED => Speed,
            _ => 0,
        };
    }

    //자연 회복, max보다 많아지면 끝남
    private IEnumerator Natural_Recovery_HP()
    {
        while (hp < MAXHP)
        {
            hp += NaturalHP;
            if (hp > MAXHP)
                hp = MAXHP;

            yield return naturalTime;
        }
    }
    private IEnumerator Natural_Recovery_MP()
    {
        while (mp < MAXMP)
        {
            mp += NaturalMP;
            if(mp > MAXMP)
                mp = MAXMP;

            yield return naturalTime;
        }
    }
    private IEnumerator Natural_Recovery_Barrier()
    {
        while(barrier < MAXBarrier)
        {
            barrier += NaturalBarrier;
            if(barrier > MAXBarrier)
                barrier = MAXBarrier;

            yield return naturalTime;
        }
    }


    public void AddStat(AttributePiece piece)
    {
        switch (piece.Property)
        {
            case Attribute_Property.HP:
                elevated.hp += piece.Value;
                hp += piece.Value;
                break;
            case Attribute_Property.MP:
                elevated.mp += piece.Value;
                mp += piece.Value;
                break;
            case Attribute_Property.BARRIER:
                elevated.barrier += piece.Value;
                barrier += piece.Value;
                break;
            case Attribute_Property.DEFENCE:
                elevated.defence += piece.Value;
                break;
            case Attribute_Property.RESISTANCE:
                elevated.resistance += piece.Value;
                break;
            case Attribute_Property.AD:
                elevated.ad += piece.Value;
                break;
            case Attribute_Property.AP:
                elevated.ap += piece.Value;
                break;
            case Attribute_Property.ATTACKSPEED:
                elevated.attackSpeed += piece.Value;
                break;
            case Attribute_Property.CRITICALCHANCE:
                elevated.criticalChance += piece.Value;
                break;
            case Attribute_Property.CRITICALMULTIPLIER:
                elevated.criticalMultiplier += piece.Value;
                break;
            case Attribute_Property.SPEED:
                elevated.speed += piece.Value;
                break;
        }
    }
    public void TakeStat(AttributePiece piece)
    {
        switch (piece.Property)
        {
            case Attribute_Property.HP:
                elevated.hp -= piece.Value;
                if (hp > MAXHP)
                    hp = MAXHP;
                break;
            case Attribute_Property.MP:
                elevated.mp -= piece.Value;
                if (MP > MAXMP)
                    mp = MAXMP;
                break;
            case Attribute_Property.BARRIER:
                elevated.barrier -= piece.Value;
                if(barrier > MAXBarrier)
                    barrier = MAXBarrier;
                break;
            case Attribute_Property.DEFENCE:
                elevated.defence -= piece.Value;
                break;
            case Attribute_Property.RESISTANCE:
                elevated.resistance -= piece.Value;
                break;
            case Attribute_Property.AD:
                elevated.ad -= piece.Value;
                break;
            case Attribute_Property.AP:
                elevated.ap -= piece.Value;
                break;
            case Attribute_Property.ATTACKSPEED:
                elevated.attackSpeed -= piece.Value;
                break;
            case Attribute_Property.CRITICALCHANCE:
                elevated.criticalChance -= piece.Value;
                break;
            case Attribute_Property.CRITICALMULTIPLIER:
                elevated.criticalMultiplier -= piece.Value;
                break;
            case Attribute_Property.SPEED:
                elevated.speed -= piece.Value;
                break;
        }
    }

    public void MinusHp(float figure)
    {
        hp -= figure;
        On_Natural_Recovery_HP();
    }

    public void MinusMp(float figure)
    {
        mp -= figure;
        if (mp < 0)
            mp = 0;
        On_Natural_Recovery_MP();
    }
    public float BarrierAbsorption(float damage)
    {
        float remainingDamage = Mathf.Max(0, damage - barrier);
        barrier -= damage - remainingDamage;
        return remainingDamage;
    }

    public void RecoveryHP(float figure)
    {
        float heal = hp - Mathf.Min(MAXHP, hp + figure);
        hp += heal;
    }

    public void RecoveryMP(float figure)
    {
        float heal = mp - Mathf.Min(MAXMP, mp + figure);
        mp += heal;
    }

    public void On_Natural_Recovery_HP()
    {
        if (nrhp == null)
        {
            nrhp = StartCoroutine(Natural_Recovery_HP());
        }
        else { }
    }
    public void On_Natural_Recovery_MP()
    {
        if (nrmp == null)
        {
            nrmp = StartCoroutine(Natural_Recovery_MP());
        }
        else { }
    }
    public void On_Natural_Recovery_Barrier()
    {
        if (nrba == null)
        {
            nrba = StartCoroutine(Natural_Recovery_Barrier());
        }
        else { }
    }

    //((대미지 + (치명타 - 치명타 방어력)) - 방어력)
    /// <summary>
    /// ad 대미지를 방어력에 맞게 조정해줌
    /// </summary>
    /// <param name="figure">대미지</param>
    /// <param name="penetration">방어구 관통력 (고정) </param>
    /// <param name="per">방어구 관통력 (퍼센트)</param>
    /// <returns>방어력과 관통력을 거쳐간 대미지</returns>
    public float Halved_AD(float figure, float penetration, float per)
    {
        float defence = Defence - penetration;
        defence -= defence * per * 0.01f;
        if (defence > 0)
            return (100 / (100 + defence) * figure);
        else
            return figure;
    }
    /// <summary>
    /// ap 대미지를 방어력에 맞게 조정해줌
    /// </summary>
    /// <param name="figure">대미지</param>
    /// <param name="penetration">저항력 관통력 (고정) </param>
    /// <param name="per">저항력 관통력 (퍼센트)</param>
    /// <returns>저항력과 관통력을 거쳐간 대미지</returns>
    public float Halved_AP(float figure, float penetration, float per)
    {
        float resistance = Resistance - penetration;
        resistance -= resistance * per * 0.01f;
        if (resistance > 0)
            return (100 / (100 + resistance) * figure);
        else
            return figure;
    }
    public float Halved_Critical(float figure)
    {
        return (100 / (100 + (CriticalDefence * 0.01f)) * figure);
    }

    /// <summary>
    /// 누군가에게 공격 받았을 때
    /// </summary>
    /// <param name="figure">대매지</param>
    /// <param name="penetration">관통력</param>
    /// <param name="per">관통력 (퍼센트)</param>
    private float Be_Attacked_AD(float figure, float penetration, float per)
    {
        float damage = Halved_AD(figure, penetration, per);
        MinusHp(damage);

        return damage;
    }
    /// <summary>
    /// 누군가에게 공격 받았을 때
    /// </summary>
    /// <param name="figure">대매지</param>
    /// <param name="penetration">관통력</param>
    /// <param name="per">관통력 (퍼센트)</param>
    private float Be_Attacked_AP(float figure, float penetration, float per)
    {
        float damage = Halved_AP(figure, penetration, per);
        MinusHp(damage);

        return damage;
    }
    /// <summary>
    /// 누군가에게 공격 받았을 때
    /// </summary>
    /// <param name="figure">대매지</param>
    private float Be_Attacked_TRUE(float figure)
    {
        MinusHp(figure);

        return figure;
    }

    public float Be_Attacked(AttackInformation attackInformation)
    {
        //여기서 콜백 호출
        HitBeforeCall(attackInformation);

        attackInformation.Agent.DamageIncreaseCall(attackInformation);

        float all = 0;

        for(int i = 0; i < attackInformation.Additional.Count; i++)
        {
            if (attackInformation.Additional[i].Critical && attackInformation.Additional[i].DamageType != DamageType.TRUE)
            {
                attackInformation.Additional[i].Figure = Halved_Critical(attackInformation.Additional[i].Figure);
            }
            switch(attackInformation.Additional[i].DamageType)
            {
                case DamageType.AD:
                    all += Be_Attacked_AD(attackInformation.Additional[i].Figure, attackInformation.Agent != null ? attackInformation.Agent.DefencePenetration : 0, attackInformation.Agent != null ? attackInformation.Agent.DefencePenetrationPer : 0);
                    break;
                case DamageType.AP:
                    all += Be_Attacked_AP(attackInformation.Additional[i].Figure, attackInformation.Agent != null ? attackInformation.Agent.ResistancePenetration : 0, attackInformation.Agent != null ? attackInformation.Agent.ResistancePenetrationPer : 0);
                    break;
                case DamageType.TRUE:
                    all += Be_Attacked_TRUE(attackInformation.Additional[i].Figure);
                    break;
                default:

                    break;
            }
        }

        //여기서 콜백 호출
        attackInformation.Agent.DamageAfterCall(attackInformation, all);

        HitAfterCall(attackInformation);
        return all;
    }

    //도트에게만 적용되는 callback이 있다면 enum을 새로 만들고 만들기로 애초에 be attack을 호출하는건 똑같이 하고 뒤에 인자 enum만 바꾸기로
    //도트데미지
    public void Be_Attacked_Poison(DotInfomation dot)
    {
        if (dot_Poison == null)
            dot_Poison = new Dot_Poison(this, dot);
        else
            dot_Poison.Dot(dot);
    }
    //도트데미지
    public void Be_Attacked_Shock(DotInfomation dot)
    {
        if (dot_Shock == null)
            dot_Shock = new Dot_Shock(this, dot);
        else
            dot_Shock.Dot(dot);
    }
    //도트데미지
    public void Be_Attacked_Burn(DotInfomation dot)
    {
        if (dot_Burn == null)
            dot_Burn = new Dot_Burn(this, dot);
        else
            dot_Burn.Dot(dot);
    }
    //도트데미지
    public void Be_Attacked_Bleeding(DotInfomation dot)
    {
        if (dot_Bleeding == null)
            dot_Bleeding = new Dot_Bleeding(this, dot);
        else
            dot_Bleeding.Dot(dot);
    }
}