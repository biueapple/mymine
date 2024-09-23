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

    [Header("y축 크기")]
    [SerializeField]
    protected float height;
    public float Height { get { return height; } }
    [Header("x축 크기")]
    [SerializeField]
    protected float width;
    public float Width { get { return width; } }
    [Header("z축 크기")]
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
    //유닛이 직접 대미지를 받아 반짝이는 경우는 도트뎀밖에 없음 그 외의 모든 대미지는 substance가 받기때문
    //그래서 그 도트대미지를 표햔할때 반짝이는것으로 표현하지 않고 다른 방법으로 표현한다면
    //여기 반짝이는 함수는 필요가 없어짐
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
