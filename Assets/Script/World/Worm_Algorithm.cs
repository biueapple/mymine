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
            if (instance == null)
                instance = new Worm_Algorithm();
            return instance;
        } 
    }

    public Worm Start(Vector3Int position, List<(Vector3Int, int)> dir)
    {
        Worm worm = new Worm(10, 2, 2, 2, dir);
        
        worm.Start();

        worm.PathRange(position);

        worm.Wall();

        return worm;
    }
    
}

public class Worm
{
    //size
    public int length;
    public int w;
    public int h;
    public int d;

    //움직일 수 있는 방향과 확률
    public List<(Vector3Int, int)> dir = new ();
    //나의 범위 안에 vector값들
    public List<Vector3Int> range = new();
    //나의 경로
    public List<Vector3Int> path = new() ;
    //나의 경로의 범위값들
    public List<Vector3Int> pathRange = new();
    //나의 경로의 벽의 값들
    public List<Vector3Int> wall = new() ;

    public Worm(int length, int w, int h, int d, List<(Vector3Int, int)> values)
    {
        this.length = length;
        this.w = w;
        this.h = h;
        this.d = d;
        dir = values;
        Init();
    }

    //경로 찾기
    public void Start()
    {
        Vector3Int dir;
        path.Add(new Vector3Int(0, 0, 0));
        while (length > 0)
        {
            dir = Direction();

            path.Add(path[^1] + dir);

            length--;
        }
    }

    //경로에서 자신의 범위만큼 땅파기
    public void PathRange(Vector3Int position)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Range(position + path[i]);
        }
    }

    //경로에서 벽에 해당하는 곳 찾기
    public void Wall()
    {
        for (int i = 0; i < pathRange.Count; i++)
        {
            WallCheck(pathRange[i]);
        }
    }

    //움직일 방향을 리턴해줌
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

    private void Range(Vector3Int vector)
    {
        for (int i = 0; i < range.Count; i++)
        {
            if(!pathRange.Contains(range[i] + vector))
                pathRange.Add(range[i] + vector);
        }
    }

    private void Init()
    {
        for(int x = -w + 1; x < w; x++)
        {
            for (int y = -h + 1; y < h; y++)
            {
                for (int z = -d + 1; z < d; z++)
                {
                    range.Add(new Vector3Int (x, y, z));
                }
            }
        }
    }

    private void WallCheck(Vector3Int position)
    {
        //위 아래 앞 뒤 왼쪽 오른쪽
        Vector3Int vector = new Vector3Int (0, 1, 0);
        if(position.y + vector.y < 0)
        {
            if(!wall.Contains(position + vector))
                wall.Add(position + vector);
        }

        vector = new Vector3Int(0, -1, 0);
        if (position.y + vector.y < 0)
        {
            if (!wall.Contains(position + vector))
                wall.Add(position + vector);
        }

        vector = new Vector3Int(0, 0, 1);
        if (position.y + vector.y < 0)
        {
            if (!wall.Contains(position + vector))
                wall.Add(position + vector);
        }

        vector = new Vector3Int(0, 0, -1);
        if (position.y + vector.y < 0)
        {
            if (!wall.Contains(position + vector))
                wall.Add(position + vector);
        }

        vector = new Vector3Int(-1, 0, 0);
        if (position.y + vector.y < 0)
        {
            if (!wall.Contains(position + vector))
                wall.Add(position + vector);
        }

        vector = new Vector3Int(1, 0, 0);
        if (position.y + vector.y < 0)
        {
            if (!wall.Contains(position + vector))
                wall.Add(position + vector);
        }
    }
}