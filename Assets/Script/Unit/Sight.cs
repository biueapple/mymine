using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//�þ߳��� �󸶳� �ִ��� ã�� Ŭ����
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
        //��� �����߿� ���� enemy�ΰ͸�
        Substance[] substances = GameManager.Instance.Substances.Where(x => x.Unit.STAT.Team == ETeam.Enemy).ToArray();
        preliminary.Clear();
        //���� �ȿ� �ִ���
        for (int i = 0; i < substances.Length; i++)
        {
            Vector3 screenPosition = camera.WorldToScreenPoint(substances[i].transform.position);
            if (screenPosition.z > 0 && rect.Contains(screenPosition))
            {
                float distance = Vector2.Distance(screenPosition, new Vector2(rect.x, rect.y));
                preliminary.Add((substances[i], distance));
            }
        }

        //�������� �߾ӿ� ���� ����� ������ ����
        preliminary.OrderBy(e => e.distance).Select(e => e.enemy).ToList();
    }
}
