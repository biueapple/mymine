using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Worm_Algorithm
{
    private static Worm_Algorithm instance;
    public static Worm_Algorithm Instance 
    { get 
        {
            instance ??= new Worm_Algorithm();
            return instance;
        } 
    }

    public static List<(Vector3Int, int)> Dir = new()
    {
            (new Vector3Int(1, 0, 0), 6),
            (new Vector3Int(-1, 0, 0), 6),
            (new Vector3Int(0, 0, 1), 6),
            (new Vector3Int(0, 0, -1), 6),
            (new Vector3Int(0, -1, 0), 8),
            (new Vector3Int(0, 1, 0), 1)
    };

    public Worm Start(List<(Vector3Int, int)> dir, Vector3Int size, int length)
    {
        Worm worm = new (length, size.x, size.y, size.z, dir);
        
        worm.Start();

        return worm;
    }
    
}

[System.Serializable]
public class Worm
{
    //size
    public int length;
    public int w;
    public int h;
    public int d;

    //������ �� �ִ� ����� Ȯ��
    public List<(Vector3Int, int)> dir = new ();
    //���� ���� �ȿ� vector����
    public List<Vector3Int> range = new();
    //���� ������ ���� ��ġ��
    public List<(Vector3Int position, Vector3Int dir)> wall = new ();
    //���� ���
    public List<Vector3Int> path = new() ;
    //���� ����� ��������
    public List<Vector3Int> pathRange = new();
    //���� ����� ���� ����
    public List<(Vector3Int position, Vector3Int dir)> pathWall = new() ;

    public Worm(int length, int w, int h, int d, List<(Vector3Int, int)> values)
    {
        this.length = length;
        this.w = w;
        this.h = h;
        this.d = d;
        dir = values;
        Init();
    }

    //��� ã��
    public void Start()
    {
        Vector3Int dir;
        path.Add(new Vector3Int(0, 0, 0));
        while (length > 0)
        {
            dir = Direction();

            Dig(path[^1] + dir);
        }
        Cutting();
    }

    private void Dig(Vector3Int dir)
    {
        if (!path.Contains(dir))
        {
            path.Add(dir);
            length--;
        }
    }

    private void Cutting()
    {
        //������ ������ ��
        for(int i = 0; i < path.Count; i++)
        {
            //������ ������ ���� ������ ������ŭ
            for(int r = 0; r < range.Count; r++)
            {
                //�ߺ� ����
                if (!pathRange.Contains(path[i] + range[r]))
                {
                    //������ ���� �ȿ� �ִ� ��ġ
                    pathRange.Add(path[i] + range[r]);
                }
                //������ �������� + ������ �ش�Ǹ� ���� �� �� ���⿡ ���� ����ġ list�� �ִٸ� �����
                if (pathWall.Select(x => x.position).Contains(path[i] + range[r]))
                {
                    //pathWall.Remove(path[i] + range[r]);
                    pathWall.Remove(pathWall.Find(x => x.position == path[i] + range[r]));
                }
            }

            //�� ��ġ���� �ֱ�
            for (int w = 0; w < wall.Count; w++)
            {
                //�ߺ����� && ������ �濡 �ش�Ǹ� ���� �ƴ�
                if (!pathWall.Select(x => x.position).Contains(path[i] + wall[w].position) && !pathRange.Contains(path[i] + wall[w].position))
                {
                    pathWall.Add((path[i] + wall[w].position, wall[w].dir));
                } 
            }
        }
    }

    //������ ������ ��������
    private Vector3Int Direction()
    {
        int value = Random.Range(0, dir.Sum(x => x.Item2) + 1);
        float cumulative = 0f;

        foreach (var (direction, probability) in dir)
        {
            cumulative += probability;
            if (value < cumulative)
            {
                return direction;
            }
        }

        return Vector3Int.zero;
    }

    //���� ����
    private void Init()
    {
        //������ ũ�⿡ �ش�Ǵ� �������� ����
        for (int x = -w + 1; x < w; x++)
        {
            for (int y = -h + 1; y < h; y++)
            {
                for (int z = -d + 1; z < d; z++)
                {
                    range.Add(new Vector3Int(x, y, z));
                }
            }
        }

        //������ ũ�⿡ �ش�Ǵ� �� ��ġ�� ����
        Vector3Int position;
        Vector3Int dir;
        for(int x = -w; x < w + 1; x++)
        {
            for(int y = -h;  y < h + 1; y++)
            {
                position = new Vector3Int(x, y, -d);
                dir = new Vector3Int(0, 0, -1);
                if (!wall.Contains((position, dir)))
                    wall.Add((position, dir));

                position = new Vector3Int(x, y, d);
                dir = new Vector3Int(0, 0, 1);
                if (!wall.Contains((position, dir)))
                    wall.Add((position, dir));
            }
        }

        for (int z = -d; z < d + 1; z++)
        {
            for (int y = -h; y < h + 1; y++)
            {
                position = new Vector3Int(-w, y, z);
                dir = new Vector3Int(-1, 0, 0);
                if (!wall.Contains((position, dir)))
                    wall.Add((position, dir));

                position = new Vector3Int(w, y, z);
                dir = new Vector3Int(1, 0, 0);
                if (!wall.Contains((position, dir)))
                    wall.Add((position, dir));
            }
        }

        for (int x = -w; x < w + 1; x++)
        {
            for (int z = -d; z < d + 1; z++)
            {
                position = new Vector3Int(-w, -h, z);
                dir = new Vector3Int(0, -1, 0);
                if (!wall.Contains((position, dir)))
                    wall.Add((position, dir));

                position = new Vector3Int(w, -h, z);
                dir = new Vector3Int(0, 1, 0);
                if (!wall.Contains((position, dir)))
                    wall.Add((position, dir));
            }
        }
    }
}