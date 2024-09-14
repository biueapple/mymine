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

    //움직일 수 있는 방향과 확률
    public List<(Vector3Int, int)> dir = new ();
    //나의 범위 안에 vector값들
    public List<Vector3Int> range = new();
    //나의 범위의 벽의 위치들
    public List<(Vector3Int position, Vector3Int dir)> wall = new ();
    //나의 경로
    public List<Vector3Int> path = new() ;
    //나의 경로의 범위값들
    public List<Vector3Int> pathRange = new();
    //나의 경로의 벽의 값들
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
        //벌레가 지나간 길
        for(int i = 0; i < path.Count; i++)
        {
            //벌레가 지나간 길을 벌레의 범위만큼
            for(int r = 0; r < range.Count; r++)
            {
                //중복 방지
                if (!pathRange.Contains(path[i] + range[r]))
                {
                    //벌레의 범위 안에 있는 위치
                    pathRange.Add(path[i] + range[r]);
                }
                //벌레가 지나간길 + 범위에 해당되면 벽이 될 수 없기에 만약 벽위치 list에 있다면 지우기
                if (pathWall.Select(x => x.position).Contains(path[i] + range[r]))
                {
                    //pathWall.Remove(path[i] + range[r]);
                    pathWall.Remove(pathWall.Find(x => x.position == path[i] + range[r]));
                }
            }

            //벽 위치들을 넣기
            for (int w = 0; w < wall.Count; w++)
            {
                //중복방지 && 벌레의 길에 해당되면 벽이 아님
                if (!pathWall.Select(x => x.position).Contains(path[i] + wall[w].position) && !pathRange.Contains(path[i] + wall[w].position))
                {
                    pathWall.Add((path[i] + wall[w].position, wall[w].dir));
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

    //범위 설정
    private void Init()
    {
        //벌레의 크기에 해당되는 범위들을 설정
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

        //벌레의 크기에 해당되는 벽 위치들 설정
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