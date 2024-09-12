using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pathfinder
{
    private readonly Unit unit;
    [SerializeField]
    private List<Vector3> points;
    public List<Vector3> Points { get { return points; } }

    public Pathfinder(Unit unit)
    {
        this.unit = unit;
        points = new ();
    }

    public void Finding(Vector3 target)
    {
        List<NODE> close = new ();
        List<NODE> open = new();
        points.Clear ();

        open.Add(new NODE((unit.transform.position), -1));
        //12곳만 확인
        //y값이 똑같은 주위와
        //y값이 1 높은 주위

        int test = 0;

        while (true)
        {
            test++;
            if (test >= 100)
                break;

            if (open.Count == 0)
            {
                Debug.Log("길을 찾지 못함");
                break;
            }

            //open 에서 가장 가까운 길을 찾아 close에 넣기
            close.Add(open[0]);
            open.RemoveAt(0);

            //도착인지 확인
            if (Vector3.Distance(close[^1].position , target) < 1f)
            {
                Debug.Log("도착 : " + close[^1].position + " , " + target);
                close[^1].position = target;
                break;
            }

            //주위에 갈수 있는 길 확인해서 open에 넣기
            Vector3 dir;
            Vector3 down = new Vector3(0, -1, 0);
            //앞
            dir = new Vector3Int(0, 0, 1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if(!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                {
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
                }
            }
            //뒤
            dir = new Vector3Int(0, 0, -1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //오른쪽
            dir = new Vector3Int(1, 0, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //왼쪽
            dir = new Vector3Int(-1, 0, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //앞 위
            dir = new Vector3Int(0, 1, 1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //뒤 위
            dir = new Vector3Int(0, 1, -1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //오른쪽 위
            dir = new Vector3Int(1, 1, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //왼쪽 위
            dir = new Vector3Int(-1, 1, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //앞 아래
            dir = new Vector3Int(0, -1, 1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //뒤 아래
            dir = new Vector3Int(0, -1, -1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //오른쪽 아래
            dir = new Vector3Int(1, -1, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //왼쪽 아래
            dir = new Vector3Int(-1, -1, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }

            //리스트 정렬
            open = open.OrderBy(v => Vector3.Distance(v.position, target)).ToList();
        }

        int temp = close.Count - 1;

        int test2 = 0;
        //final 정리
        while(true)
        {
            test2++;
            if (test2 >= 30)
                break;



            points.Add(close[temp].position);
            temp = close[temp].pre;
            if(temp == -1)
            {
                break;
            }
        }

        points.Reverse();
        Debug.Log("길찾기 종료");
    }

    private bool Empty(Vector3 position)
    {
        //남은 거리
        float distanceX = unit.Width * 2;
        //최종 추가되는 값
        float width = -unit.Width;
        //현재 얼마나 추가됬는지
        float measureX;

        //x축
        while (true)
        {
            float distanceY = unit.Height;
            float height = 0;
            float measureY;
            //y축
            while (true)
            {
                float distanceZ = unit.Depth * 2;
                float depth = -unit.Depth;
                float measureZ;
                //z축
                while (true)
                {
                    //충돌계산
                    if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y + height, position.z + depth)))
                    {
                        return false;
                    }
                    //남은 거리가 없다면
                    if (distanceZ <= 0)
                    {
                        break;
                    }
                    //남은 거리 줄이기
                    measureZ = Mathf.Min(1, distanceZ);
                    depth += measureZ;
                    distanceZ -= measureZ;
                }

                if(distanceY <= 0)
                {
                    break;
                }

                measureY = Mathf.Min(1, distanceY);
                height += measureY;
                distanceY -= measureY;
            }
            
            if (distanceX <= 0)
            { 
                break; 
            }

            measureX = Mathf.Min(1, distanceX);
            width += measureX;
            distanceX -= measureX;
        }
        return true;
    }

    public class NODE
    {
        public Vector3 position;
        public int pre;
        public NODE(Vector3 vector, int pre) 
        {
            position = vector;
            this.pre = pre;
        }

    }
}
