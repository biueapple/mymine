using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//스탯을 받아서 하나의 스탯의 바를 표현하는 스크립트
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
