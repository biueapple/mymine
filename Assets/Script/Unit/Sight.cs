using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//시야내에 얼마나 있는지 찾는 클래스
public class Sight
{
    private readonly List<(Substance enemy, float distance)> preliminary = new();
    public List<(Substance enemy, float distance)> Values { get {  return preliminary; } }

    private readonly Camera camera;
    private readonly Rect rect;

    public Sight(Camera camera, Rect rect) 
    {
        this.camera = camera;
        this.rect = rect;
    }


    public void SightOn()
    {
        //모든 유닛중에 팀이 enemy인것만
        Substance[] substances = GameManager.Instance.Substances.Where(x => x.Unit.STAT.Team == ETeam.Enemy).ToArray();
        preliminary.Clear();
        //범위 안에 있는지
        for (int i = 0; i < substances.Length; i++)
        {
            Vector3 screenPosition = camera.WorldToScreenPoint(substances[i].transform.position);
            if (screenPosition.z > 0 && rect.Contains(screenPosition))
            {
                float distance = Vector2.Distance(screenPosition, new Vector2(rect.x, rect.y));
                preliminary.Add((substances[i], distance));
            }
        }

        //범위에서 중앙에 가장 가까운 순으로 정렬
        preliminary.OrderBy(e => e.distance).Select(e => e.enemy).ToList();
    }
}
