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
        //12���� Ȯ��
        //y���� �Ȱ��� ������
        //y���� 1 ���� ����

        int test = 0;

        while (true)
        {
            test++;
            if (test >= 100)
                break;

            if (open.Count == 0)
            {
                Debug.Log("���� ã�� ����");
                break;
            }

            //open ���� ���� ����� ���� ã�� close�� �ֱ�
            close.Add(open[0]);
            open.RemoveAt(0);

            //�������� Ȯ��
            if (Vector3.Distance(close[^1].position , target) < 1f)
            {
                Debug.Log("���� : " + close[^1].position + " , " + target);
                close[^1].position = target;
                break;
            }

            //������ ���� �ִ� �� Ȯ���ؼ� open�� �ֱ�
            Vector3 dir;
            Vector3 down = new Vector3(0, -1, 0);
            //��
            dir = new Vector3Int(0, 0, 1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if(!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                {
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
                }
            }
            //��
            dir = new Vector3Int(0, 0, -1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //������
            dir = new Vector3Int(1, 0, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //����
            dir = new Vector3Int(-1, 0, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //�� ��
            dir = new Vector3Int(0, 1, 1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //�� ��
            dir = new Vector3Int(0, 1, -1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //������ ��
            dir = new Vector3Int(1, 1, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //���� ��
            dir = new Vector3Int(-1, 1, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //�� �Ʒ�
            dir = new Vector3Int(0, -1, 1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //�� �Ʒ�
            dir = new Vector3Int(0, -1, -1);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //������ �Ʒ�
            dir = new Vector3Int(1, -1, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }
            //���� �Ʒ�
            dir = new Vector3Int(-1, -1, 0);
            if (Empty(close[^1].position + dir) && !Empty(close[^1].position + dir + down))
            {
                if (!open.Any(node => node.position == close[^1].position + dir) && !close.Any(node => node.position == close[^1].position + dir))
                    open.Add(new NODE(close[^1].position + dir, close.Count - 1));
            }

            //����Ʈ ����
            open = open.OrderBy(v => Vector3.Distance(v.position, target)).ToList();
        }

        int temp = close.Count - 1;

        int test2 = 0;
        //final ����
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
        Debug.Log("��ã�� ����");
    }

    private bool Empty(Vector3 position)
    {
        //���� �Ÿ�
        float distanceX = unit.Width * 2;
        //���� �߰��Ǵ� ��
        float width = -unit.Width;
        //���� �󸶳� �߰������
        float measureX;

        //x��
        while (true)
        {
            float distanceY = unit.Height;
            float height = 0;
            float measureY;
            //y��
            while (true)
            {
                float distanceZ = unit.Depth * 2;
                float depth = -unit.Depth;
                float measureZ;
                //z��
                while (true)
                {
                    //�浹���
                    if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y + height, position.z + depth)))
                    {
                        return false;
                    }
                    //���� �Ÿ��� ���ٸ�
                    if (distanceZ <= 0)
                    {
                        break;
                    }
                    //���� �Ÿ� ���̱�
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
