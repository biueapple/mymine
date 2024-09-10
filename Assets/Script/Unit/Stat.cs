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

//���� �ִ� ü��
//�������� �ִ� ü��
//���� �ִ� ü��

//���� ���ݷ�
//�������� ���ݷ�
//���� ���ݷ�

//������ �������

//���� �������� �����
//���� Ÿ��
//����� Ÿ��
//�߰� ����� ����Ʈ (item , skill)


//�ð� ���� ȿ��
//Ƚ�� ���� ȿ�� (�ð��� ����)

//���� ���� ȿ��
//���� ���� ȿ��
//Ƚ���� ȣ��� ������� ���� �õ��� ���� ��ȭ�ؾ� ��
//���� �õ��� ���� Ƚ�� ����

/*
 * �������� �����ؼ� �ߵ��ϴ� ȿ����
 * Ƚ���� Ÿ�̸Ӹ� ��������
 * ȿ���� �����ϴ� Ŭ������
 * �����ؼ� üũ�ϰ� remove�� ����� �ϴ���
 * ���ڷ����� ��������
 * ȿ�� Ŭ������ ����� �ű⿡ ������ �ް��ִ� Ŭ������
 * ���ڷ���Ʈ �ؼ� �ݹ����� �����ϴ� ���  (������)
 * �ٵ� �������̽��� ���� ������ �ؾ��ϴ��� �𸣰ڴµ�
 */

//���ʿ� ui���� �Ű��� ���� ����
//������ ���� ��� �� �ִ� ��ų (��ųƮ��) �� �����ϰ�
//�װ��� ������ ������ �ִ°��� ��ųƮ�� ui�� ��ų�� �����ϰ�
//�÷��̾�� �־��ִ� ����� �ƴ�
//ó������ �ٽ� �����ؾ� �ҵ�
//�ѹ��� �������� �������� ����Ÿ���ϴ� ���������� ���
//��ų���� �������� �ѹ��� �������� �Ǵ��� ���� ����
//���⼭ ������ �������
//������ �ݹ����δ� � ��ų�� ��� �ߵ��ϸ� ��������� ���� �����Ǵ����� �𸣱⿡
//���ο� Ŭ������ ���� �ٲ���� ���ο� ����� Ŭ������ ���� Func�� ����� �ʿ伺�� �����
//������ Action���� ������ȿ���� ���ϵ� �ϰ�
//�̹��� ���� EffectŬ�������� ��ų�� ȿ���� ���ϵ� ��

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
    //�⺻
    private StatAttributes original = new ();
    [SerializeField]
    //���
    private StatAttributes elevated = new ();

    //ü��
    public float HP { get { return hp; } }

    public float MAXHP { get { return original.hp + elevated.hp; } }
    //ü�� �ڿ� ȸ����
    public float NaturalHP { get { return original.naturalHP + elevated.naturalHP; } }

    //����
    public float MP { get { return mp; } }
    public float MAXMP { get { return original.mp + elevated.mp; } }
    public float NaturalMP { get { return original.naturalMP + elevated.naturalMP; } }

    //�踮��
    public float Barrier { get => barrier; }
    public float MAXBarrier { get { return original.barrier + elevated.barrier; } }
    public float NaturalBarrier { get => original.naturalBarrier + elevated.naturalBarrier; }

    //���� 100�� 100��
    public float Defence { get { return original.defence + elevated.defence; } }

    //���׷� 100�� 100��
    public float Resistance { get { return original.resistance + elevated.resistance; } }

    //���ݷ�
    public float AD { get { return original.ad + elevated.ad; } }

    //�ֹ���
    public float AP { get { return original.ap + elevated.ap; } }

    //���ݼӵ�
    public float AttackSpeed { get { return 1 / ((original.attackSpeed + elevated.attackSpeed) * 0.7f) ; } }

    //ġ��Ÿ Ȯ�� 100�� 100��
    public float CriticalChance { get { return original.criticalChance + elevated.criticalChance; } }

    //ũ��Ƽ���� �����°�
    public bool Critical { get { if (UnityEngine.Random.Range(0f, 1f) <= CriticalChance * 0.01f) { return true; } else { return false; } } }

    //ġ��Ÿ ���� 1�� 100�ۼ�Ʈ �߰� �����
    public float CriticalMultiplier { get { return original.criticalMultiplier + elevated.criticalMultiplier; } }

    //ġ��Ÿ ���� 100�� 100��
    public float CriticalDefence { get { return original.criticalDefence + elevated.criticalDefence; } }

    //��� �����
    public float DefencePenetration { get { return original.defencePenetration + elevated.defencePenetration; } }

    //��� ����� �ۼ�Ʈ 100�� 100�ۼ�Ʈ
    public float DefencePenetrationPer { get { return original.defencePenetrationPer + elevated.defencePenetrationPer; } }

    //���� �����
    public float ResistancePenetration { get { return original.resistancePenetration + elevated.resistancePenetration; } }

    //���� ����� �ۼ�Ʈ 100�� 100�ۼ�Ʈ
    public float ResistancePenetrationPer { get { return original.resistancePenetrationPer + elevated.resistancePenetrationPer; } }

    //��Ÿ�� ����
    public float CooltimeReduction { get => 0.1f; }

    //�̵��ӵ�
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

    //���� ��Ʈ 
    [SerializeField]
    private Dot_Poison dot_Poison = null;
    //���� ��Ʈ 
    [SerializeField]
    private Dot_Shock dot_Shock = null;
    //���� ��Ʈ 
    [SerializeField]
    private Dot_Burn dot_Burn = null;
    //���� ��Ʈ 
    [SerializeField]
    private Dot_Bleeding dot_Bleeding = null;

    //��Ʈ���̳� �ڿ� ȸ���� ���ʿ� �ѹ� �Ͼ��
    [SerializeField]
    private float dotTime = 2;
    public float DotTime { get { return dotTime; } }
    [SerializeField]
    private float naturalTime = 2;

    //����� ���� �ݹ�
    [NonSerialized]
    private readonly List<IDamageIncreaseCallback> damageIncreaseCallbacks = new ();
    public List<IDamageIncreaseCallback> DamageIncreaseCallback { get => damageIncreaseCallbacks; }
    public void DamageIncreaseCall(AttackInformation attackInformation)
    { 
        for (int i = damageIncreaseCallbacks.Count - 1; i >= 0; i--)
            damageIncreaseCallbacks[i].DamageIncrease(attackInformation);
    } 
    //������� ���� �͵� ���⿡ �ֱ�
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

    //���� �õ��� ���� �ݹ�
    private readonly List<IAttackTryCallback> attackTryCallbacks = new();
    public List<IAttackTryCallback> AttackTryCallbacks { get { return attackTryCallbacks; } }
    public void AttackTryCall(Stat[] stats)
    {
        for (int i = attackTryCallbacks.Count - 1; i >= 0; i--)
            attackTryCallbacks[i].AttackTry(stats);
    }
    //��ų �õ��� ���� �ݹ�
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
        //dot ������鵵 load�ؾ���
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

    //���� ��� : C:\Users\����ڸ�\AppData\LocalLow\DefaultCompany\My InventorySystem
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
        //    Debug.LogError($"���� �߻�: {ex.Message}");
        //}
    }

    //�ҷ����� ��� : C:\Users\����ڸ�\AppData\LocalLow\DefaultCompany\My InventorySystem
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
    //�ؿ� �Լ��ʹ� �ٸ����� hp mp barriar �ۿ� ����
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

    //�ڿ� ȸ��, max���� �������� ����
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

    //((����� + (ġ��Ÿ - ġ��Ÿ ����)) - ����)
    /// <summary>
    /// ad ������� ���¿� �°� ��������
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�� ����� (����) </param>
    /// <param name="per">�� ����� (�ۼ�Ʈ)</param>
    /// <returns>���°� ������� ���İ� �����</returns>
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
    /// ap ������� ���¿� �°� ��������
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">���׷� ����� (����) </param>
    /// <param name="per">���׷� ����� (�ۼ�Ʈ)</param>
    /// <returns>���׷°� ������� ���İ� �����</returns>
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
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�����</param>
    /// <param name="per">����� (�ۼ�Ʈ)</param>
    private float Be_Attacked_AD(float figure, float penetration, float per)
    {
        float damage = Halved_AD(figure, penetration, per);
        MinusHp(damage);

        return damage;
    }
    /// <summary>
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�����</param>
    /// <param name="per">����� (�ۼ�Ʈ)</param>
    private float Be_Attacked_AP(float figure, float penetration, float per)
    {
        float damage = Halved_AP(figure, penetration, per);
        MinusHp(damage);

        return damage;
    }
    /// <summary>
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    private float Be_Attacked_TRUE(float figure)
    {
        MinusHp(figure);

        return figure;
    }

    public float Be_Attacked(AttackInformation attackInformation)
    {
        //���⼭ �ݹ� ȣ��
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

        //���⼭ �ݹ� ȣ��
        attackInformation.Agent.DamageAfterCall(attackInformation, all);

        HitAfterCall(attackInformation);
        return all;
    }

    //��Ʈ���Ը� ����Ǵ� callback�� �ִٸ� enum�� ���� ����� ������ ���ʿ� be attack�� ȣ���ϴ°� �Ȱ��� �ϰ� �ڿ� ���� enum�� �ٲٱ��
    //��Ʈ������
    public void Be_Attacked_Poison(DotInfomation dot)
    {
        if (dot_Poison == null)
            dot_Poison = new Dot_Poison(this, dot);
        else
            dot_Poison.Dot(dot);
    }
    //��Ʈ������
    public void Be_Attacked_Shock(DotInfomation dot)
    {
        if (dot_Shock == null)
            dot_Shock = new Dot_Shock(this, dot);
        else
            dot_Shock.Dot(dot);
    }
    //��Ʈ������
    public void Be_Attacked_Burn(DotInfomation dot)
    {
        if (dot_Burn == null)
            dot_Burn = new Dot_Burn(this, dot);
        else
            dot_Burn.Dot(dot);
    }
    //��Ʈ������
    public void Be_Attacked_Bleeding(DotInfomation dot)
    {
        if (dot_Bleeding == null)
            dot_Bleeding = new Dot_Bleeding(this, dot);
        else
            dot_Bleeding.Dot(dot);
    }
}