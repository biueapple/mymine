using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private static readonly List<Unit> allUnits = new ();
    public static List<Unit> AllUnits { get => allUnits; }

    protected Stat stat;
    public Stat STAT { get { return stat; } }
    
    protected MoveSystem moveSystem;
    public MoveSystem MoveSystem => moveSystem;
    
    [SerializeField]
    protected Substance[] substances;
    public Substance[] Substances { get => substances; }

    [Header("y�� ũ��")]
    [SerializeField]
    protected float height;
    public float Height { get { return height; } }
    [Header("x�� ũ��")]
    [SerializeField]
    protected float width;
    public float Width { get { return width; } }
    [Header("z�� ũ��")]
    [SerializeField]
    protected float depth;
    public float Depth { get { return depth; } }

    [SerializeField]
    protected int level;
    public int Level { get { return level; } }  

    // Start is called before the first frame update
    protected virtual void  Awake()
    {
        allUnits.Add(this);
        stat = GetComponent<Stat>();
        moveSystem = GetComponent<MoveSystem>();
        //originals = models.ToList().Select(x => x.transform.GetComponent<Renderer>().material.GetColor("_Color")).ToArray();
    }

    protected virtual void OnDestroy()
    {
        AllUnits.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual float Hit(AttackInformation attackInformation)
    {
        return stat.Be_Attacked(attackInformation);
    }

    public virtual float Hit(AttackInformation attackInformation, Vector3 dir, float power)
    {
        Pushed(dir, power);
        return Hit(attackInformation);
    }

    public virtual float Hit(AttackInformation attackInformation, Color color)
    {
        Twinkle(color);
        return Hit(attackInformation);
    }

    public virtual float Hit(AttackInformation attackInformation, Vector3 dir, float power, Color color)
    {
        Pushed(dir, power);
        Twinkle(color);
        return Hit(attackInformation);
    }

    //
    public virtual void Pushed(Vector3 dir, float power)
    {
        if (moveSystem != null)
        {
            moveSystem.AddExternalForces(dir * power);
        }
    }
    //������ ���� ������� �޾� ��¦�̴� ���� ��Ʈ���ۿ� ���� �� ���� ��� ������� substance�� �ޱ⶧��
    //�׷��� �� ��Ʈ������� ǥ�h�Ҷ� ��¦�̴°����� ǥ������ �ʰ� �ٸ� ������� ǥ���Ѵٸ�
    //���� ��¦�̴� �Լ��� �ʿ䰡 ������
    public virtual void Twinkle(Color color)
    {
        substances.ToList().ForEach(x => x.Twinkle(color)); 
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(0,height * 0.5f,0), new Vector3(Width * 2, height, depth * 2));
    }
}
