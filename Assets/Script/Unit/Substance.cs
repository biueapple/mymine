using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Substance : MonoBehaviour
{
    [SerializeField]
    private Unit unit;
    public Unit Unit { get { return unit; } }
    [SerializeField]
    private float total;
    public float Total { get { return total; } }
    [Range(0f, 2f)]
    [SerializeField]
    private float decrease = 1;
    public float Decrease { get { return decrease; } }
    private Color original;
    private Coroutine twinkle;

    private void Start()
    {
        original = transform.GetComponent<Renderer>().material.GetColor("_Color");
    }
    //원래는 대미지를 받는 판정을 unit에서 했는데 여기서 대미지를 받는 판정을 하기로 함 (unit이지만 대미지 판정을 받지 않을 수 있고 하나의 유닛이지만 여러곳에서 대미지를 받을 수 있음)
    //색깔변화도 여기서 하자
    public void Hit(AttackInformation attackInformation)
    {
        for(int i = 0; i < attackInformation.Additional.Count; i++)
        {
            attackInformation.Additional[i].Figure = attackInformation.Additional[i].Figure * decrease;
        }
        
        if (attackInformation.Agent != null)
        {
            total += unit.Hit(attackInformation, (unit.transform.position - attackInformation.Agent.transform.position).normalized, attackInformation.Additional.Sum(x => x.Figure) * 0.1f);
            Twinkle(Color.red);
        }
        else
        {
            total += unit.Hit(attackInformation);
            Twinkle(Color.red);
        }
    }

    //반짝이는 함수가 동시에 여러번 호출되면 original color가 덮어써져서 오류가 남
    //그래서 original을 밖으로 빼고 이미 호출중인 반짝이는 캔슬 후 호출
    public void Twinkle(Color color)
    {
        if (twinkle != null)
        {
            StopCoroutine(twinkle);
            transform.GetComponent<Renderer>().material.SetColor("_Color", original);
        }
        twinkle = StartCoroutine(TwinkleCoroutine(0.2f, color));
    }
    private IEnumerator TwinkleCoroutine(float timer, Color color)
    {
        transform.GetComponent<Renderer>().material.SetColor("_Color", color);
        yield return new WaitForSeconds(timer);
        transform.GetComponent<Renderer>().material.SetColor("_Color", original);
        twinkle = null;
    }
}
