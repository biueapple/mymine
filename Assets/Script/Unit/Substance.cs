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
    //������ ������� �޴� ������ unit���� �ߴµ� ���⼭ ������� �޴� ������ �ϱ�� �� (unit������ ����� ������ ���� ���� �� �ְ� �ϳ��� ���������� ���������� ������� ���� �� ����)
    //����ȭ�� ���⼭ ����
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

    //��¦�̴� �Լ��� ���ÿ� ������ ȣ��Ǹ� original color�� ��������� ������ ��
    //�׷��� original�� ������ ���� �̹� ȣ������ ��¦�̴� ĵ�� �� ȣ��
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
