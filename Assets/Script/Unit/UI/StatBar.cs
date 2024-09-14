using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������ �޾Ƽ� �ϳ��� ������ �ٸ� ǥ���ϴ� ��ũ��Ʈ
public class StatBar : MonoBehaviour
{
    [SerializeField]
    private Stat stat;
    public Stat Stat { get { return stat; } set { stat = value; } }
    [SerializeField]
    private Image bar;
    [SerializeField]
    private Attribute_Property property;
    public Attribute_Property Property { get { return property; } set { property = value; } }

    public void Interlock(Stat stat)
    {
        this.stat = stat;
    }

    private void Start()
    {
        if(bar != null)
        {
            bar.fillAmount = 0;
        }
    }

    private void Update()
    {
        if(stat != null && bar != null)
        {
            bar.fillAmount = stat.NowAttribute(property) / stat.GetAttribute(property);
        }
    }
}
