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
        Worm worm = new Worm(length, size.x, size.y, size.z, dir);
        
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

    //움직일 수 있는 방향과 확률
    public List<(Vector3Int, int)> dir = new ();
    //나의 범위 안에 vector값들
    public List<Vector3Int> range = new();
    //나의 범위의 벽의 위치들
    public List<Vector3Int> wall = new ();
    //나의 경로
    public List<Vector3Int> path = new() ;
    //나의 경로의 범위값들
    public List<Vector3Int> pathRange = new();
    //나의 경로의 벽의 값들
    public List<Vector3Int> pathWall = new() ;

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
        for(int i = 0; i < path.Count; i++)
        {
            for(int r = 0; r < range.Count; r++)
            {
                if (!pathRange.Contains(path[i] + range[r]))
                {
                    pathRange.Add(path[i] + range[r]);
                }
                if (pathWall.Contains(path[i] + range[r]))
                {
                    pathWall.Remove(path[i] + range[r]);
                }
            }

            for (int w = 0; w < wall.Count; w++)
            {
                if (!pathWall.Contains(path[i] + wall[w]) && !pathRange.Contains(path[i] + wall[w]))
                {
                    pathWall.Add(path[i] + wall[w]);
                } 
            }
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

    private void Init()
    {
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

        Vector3Int position;
        for(int x = -w; x < w + 1; x++)
        {
            for(int y = -h;  y < h + 1; y++)
            {
                position = new Vector3Int(x, y, -d);
                if (!wall.Contains(position))
                    wall.Add(position);

                position = new Vector3Int(x, y, d); 
                if (!wall.Contains(position))
                    wall.Add(position);
            }
        }

        for (int z = -d; z < d + 1; z++)
        {
            for (int y = -h; y < h + 1; y++)
            {
                position = new Vector3Int(-w, y, z);
                if (!wall.Contains(position))
                    wall.Add(position);

                position = new Vector3Int(w, y, z);
                if (!wall.Contains(position))
                    wall.Add(position);
            }
        }

        for (int x = -w; x < w + 1; x++)
        {
            for (int z = -d; z < d + 1; z++)
            {
                position = new Vector3Int(-w, -h, z);
                if (!wall.Contains(position))
                    wall.Add(position);

                position = new Vector3Int(w, -h, z);
                if (!wall.Contains(position))
                    wall.Add(position);
            }
        }
    }
}