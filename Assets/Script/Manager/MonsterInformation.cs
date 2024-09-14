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
            if(instance == null)
                instance = new MonsterInformation();
            return instance;
        }
    }

    public MonsterInformation()
    {
        infomation = Resources.Load<RectTransform>("UI/Unit/MonsterInfo");
    }

    private RectTransform infomation;
    private List<(Enemy enemy, RectTransform rect)> list = new ();  
    public List<(Enemy enemy, RectTransform rect)> List { get { return list; } }

    //적을 넘기면 그 정보를 ui에 띄워줌
    public void OpenMonsterInfo(Enemy enemy)
    {
        RectTransform rectTransform = ObjectPooling.Instance.CreateObject(infomation.gameObject, UIManager.Instance.Canvas.transform, Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, enemy.Height,0) ), Quaternion.identity).GetComponent<RectTransform>();
        rectTransform.GetChild(0).GetComponent<Text>().text = enemy.ToString();
        rectTransform.GetChild(1).GetComponent<StatBar>().Stat = enemy.STAT;
        list.Add((enemy, rectTransform));
    }

    public void CloseMonsterInfo(Enemy enemy)
    {
        (Enemy, RectTransform) index = list.Find(x => x.Item1 == enemy);
        if(index.Item2 != null)
        {
            ObjectPooling.Instance.DestroyObject(index.Item2.gameObject);
            list.Remove(index);
        }    
    }
}
