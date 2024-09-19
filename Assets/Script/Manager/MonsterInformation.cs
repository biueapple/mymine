using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInformation
{
    private static MonsterInformation instance;
    public static MonsterInformation Instance
    {
        get
        {
            instance ??= new MonsterInformation();
            return instance;
        }
    }

    public MonsterInformation()
    {
        infomation = Resources.Load<RectTransform>("UI/Unit/MonsterInfo");
    }

    private readonly RectTransform infomation;
    private readonly List<(Enemy enemy, RectTransform rect)> list = new ();  
    public List<(Enemy enemy, RectTransform rect)> List { get { return list; } }

    //적을 넘기면 그 정보를 ui에 띄워줌
    public void OpenMonsterInfo(Enemy enemy)
    {
        RectTransform rectTransform = ObjectPooling.Instance.CreateObject(infomation.gameObject, UIManager.Instance.Canvas.transform, Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, enemy.Height + 0.1f,0) ), Quaternion.identity).GetComponent<RectTransform>();
        rectTransform.GetChild(0).GetComponent<Text>().text = enemy.ToString();
        rectTransform.GetChild(1).GetComponent<StatBar>().Stat = enemy.STAT;
        rectTransform.GetChild(2).GetComponent<Text>().text = "LV " + enemy.Level;
        list.Add((enemy, rectTransform));
    }

    public void CloseMonsterInfo(Enemy enemy)
    {
        (Enemy e, RectTransform r) index = list.Find(x => x.enemy == enemy);
        if(index.r != null)
        {
            ObjectPooling.Instance.DestroyObject(index.r.gameObject);
            list.Remove(index);
        }    
    }
    public void CloseMonsterInfo()
    {
        for(int i = 0; i < list.Count; i++)
        {
            ObjectPooling.Instance.DestroyObject(list[i].rect.gameObject);
        }
        list.Clear  ();
    }
}
