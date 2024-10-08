using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeAttributes", menuName = "Minecraft/Biome Attribute")]
public class Biome : ScriptableObject
{
    public string biomeName;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    //높이는 2d noise
    //땅속 블록은 3d noise
    //tree 는 2d
    //cave 는 3d
    [Header("가장 겉면(윗면)을 무슨블록이 채울건지")]
    [SerializeField]
    private byte surfaceType;
    [SerializeField]
    private byte depthBlock;
    [Header("가장 겉면(윗면)이 몇칸 차지할건지")]
    [SerializeField]
    private int surfaceDepth;
    [Header("청크를 채울 기본적인 블록")]
    [SerializeField] 
    private byte nomalType;
    //높이를 정하는 기준이 2개인 이유는 noise를 이용한 값으론 표현이 불가능함
    [Header("기본적인 높이 (이 높이를 기준으로 noise + - 어느정도 들어감)")]
    [SerializeField] 
    private int nomalHeight;
    [Header("기본적인 높이 nomal을 적용 후 이만큼 더해서 높이가 정해짐")]
    [SerializeField] 
    private int surfaceHeight;
    [Header("Height Noise")]
    [SerializeField] 
    private float offsetHeight;
    //[MinValue(0.1f)]
    [SerializeField] 
    private float scaleHeight = 0.5f;

    [Header("위에 있을수록 우선적 {겹칠경우 위에있는게 생성됨}")]

    [Header("나무에 대한 정보들")]
    [SerializeField] 
    private TreePlacement[] treePlacements;

    [Header("땅속에 확률적으로 있는 블록들")]
    [SerializeField] 
    private Underground[] undergrounds;

    [SerializeField]
    private CavePlacement cavePlacement;

    public int Height(int x, int z)
    {
        //return (int)(Noise.Get2DPerlin(new Vector2Int(x, z), offsetHeight, scaleHeight) * nomalHeight + surfaceHeight);
        return (int)GetNoiseValue(x, z)/* * nomalHeight + surfaceHeight*/;
    }

    float GetNoiseValue(int x, int z)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency / scale, z * frequency / scale) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }
        return (total / maxValue) * nomalHeight + surfaceHeight;
    }

    //이 모든 과정을 하나의 함수로 만들어서 불필요한 반복 줄이기

    //기본적인 지형을 만들기
    public void CreateBaseMap(Chunk chunk, ref int[,,] map)
    {
        for (int x = 0; x < BlockInfo.ChunkWidth; x++)
        {
            for (int z = 0; z < BlockInfo.ChunkWidth; z++)
            {
                int yHeight = Height(chunk.Position.x + x, chunk.Position.z + z);
                for (int y = 0; y < BlockInfo.ChunkHeight; y++)
                {
                    //air 의 경우 넣어주지 않더라도 0으로 들어가있어서 괜찮음
                    if (y <= yHeight)
                    {

                        //가장 아래에는 배드락
                        if (y < 1)
                            map[x, y, z] = 3;

                        //가장 윗면
                        else if (y >= yHeight)
                            map[x, y, z] = surfaceType;

                        //윗면
                        else if (y >= yHeight - surfaceDepth)
                            map[x, y, z] = depthBlock;

                        //그 이외
                        else
                            map[x, y, z] = nomalType;


                        ////지하 만들기
                        //for (int i = undergrounds.Length - 1; i >= 0; i--)
                        //{
                        //    if (undergrounds[i].MakeUnderground(x + chunk.Position.x, y, z + chunk.Position.z, yHeight))
                        //    {
                        //        map[x, y, z] = undergrounds[i].type;
                        //    }
                        //}
                        //CreateMineral(x, y, z, chunk, ref map, GameManager.Instance.Mineral(y, yHeight, 0.0005f));
                    }
                }

                ////나무 만들기
                //for (int i = treePlacements.Length - 1; i >= 0; i--)
                //{
                //    if (treePlacements[i].MakeTree(x + chunk.Position.x, z + chunk.Position.z))
                //    {
                //        CreateTreeMapWorld(chunk, ref map, treePlacements[i].CreateTree(new Vector3Int(x, yHeight, z)));
                //    }
                //}

                ////동굴 만들기
                //if (cavePlacement != null)
                //{
                //    if (Random.Range(0f, 1f) <= cavePlacement.probability)
                //    {
                //        CreateCave(x, yHeight, z, chunk, ref map);
                //    }
                //}

            }
        }
    }

    public void CreateWait(Chunk chunk, ref int[,,] map)
    {
        for(int i = 0; i < chunk.wait.Count; i++)
        {
            if( (chunk.wait[i].Item1.x >= 0 && chunk.wait[i].Item1.x < BlockInfo.ChunkWidth) &&
                (chunk.wait[i].Item1.y >= 0 && chunk.wait[i].Item1.y < BlockInfo.ChunkHeight) &&
                (chunk.wait[i].Item1.z >= 0 && chunk.wait[i].Item1.z < BlockInfo.ChunkWidth))
            {
                map[chunk.wait[i].Item1.x, chunk.wait[i].Item1.y, chunk.wait[i].Item1.z] = chunk.wait[i].Item2;
            }
        }
        chunk.wait.Clear();
    }

    private void CreateTreeMapWorld(Chunk chunk, ref int[,,] map, List<BlockOrder> orders)
    {
        //여기서 들어오는 orders에는 음수나 길이를 벗어난 위치의 값이 있을 수 있는데 그 경우 다른 청크에 값을 넘겨야 함
        //만약 이미 만들어진 청크라면 값을 변경만 하고 
        //만들어지지 않은 청크라면 wait에 값을 넣어놓기
        for (int i = orders.Count - 1; i >= 0; i--)
        {
            if ((orders[i].local.x >= 0 && orders[i].local.x < BlockInfo.ChunkWidth) &&
               (orders[i].local.y >= 0 && orders[i].local.y < BlockInfo.ChunkHeight) &&
               (orders[i].local.z >= 0 && orders[i].local.z < BlockInfo.ChunkWidth))
            {
                //청크의 범위 안
                map[orders[i].local.x, orders[i].local.y, orders[i].local.z] = orders[i].type;
                //리스트 재활용하려고 사용한 인덱스는 삭제
                orders.RemoveAt(i);
            }
            else
            {
                //청크 범위 밖이면 월드에게 줘야하니 포지션을 월드로 바꿈
                orders[i].local += chunk.Position;
            }
        }

        //청크의 범위 밖
        //월드에게 local 좌표 + 청크 위치 값을 넘겨서 주기
        World.Instance.BiomeCallEdit(orders);
    }

    public void CreateCave(int x, int y, int z, Chunk chunk, ref int[,,] map)
    {
        Vector3Int worldPosition;
        List<BlockOrder> orders = new();
        Worm worm;
        if (cavePlacement.dir.Count == 0)
            worm = Worm_Algorithm.Instance.Start(Worm_Algorithm.Dir, new Vector3Int(2, 2, 2), 100);
        else
            worm = Worm_Algorithm.Instance.Start(cavePlacement.Dir(), cavePlacement.size, cavePlacement.lenght);

        //땅을 파는 단계
        for (int i = 0; i < worm.pathRange.Count; i++)
        {
            if ((worm.pathRange[i].x + x >= 0 && worm.pathRange[i].x + x < BlockInfo.ChunkWidth) &&
                (worm.pathRange[i].y + y >= 0 && worm.pathRange[i].y + y < BlockInfo.ChunkHeight) &&
                (worm.pathRange[i].z + z >= 0 && worm.pathRange[i].z + z < BlockInfo.ChunkWidth))
            {
                //배드락은 뚫고가지 못함
                if (map[worm.pathRange[i].x + x, worm.pathRange[i].y + y, worm.pathRange[i].z + z] != (int)GameManager.BLCOK_ENUM.Bedrock)
                {
                    map[worm.pathRange[i].x + x, worm.pathRange[i].y + y, worm.pathRange[i].z + z] = 0;
                }
            }
            else
            {
                //월드가 알아서 배드락은 예외 해줌
                worldPosition = new Vector3Int(chunk.Position.x + x, y, chunk.Position.z + z);
                orders.Add(new BlockOrder(worldPosition + worm.pathRange[i], 0));
            }
        }

        //광물 만드는 단계 (광물을 만드는 단계는 동굴을 만드는 것과는 별계의 확률로 독자적임)
        for (int i = 0; i < worm.pathWall.Count; i++)
        {
            CreateMineral(x, worm.pathWall[i].position.y + y, z, chunk, ref map, GameManager.Instance.Mineral(worm.pathWall[i].position.y + y, y, 0.01f));
        }


        World.Instance.BiomeCallEdit(orders);
    }

    //광물 생성기
    public void CreateMineral(int x, int y, int z, Chunk chunk, ref int[,,] map, (Vector3Int[] vectors, int id) shape)
    {
        Vector3Int worldPosition;
        List<BlockOrder> orders = new();

        if (shape.vectors != null)
        {
            for (int i = 0; i < shape.vectors.Length; i++)
            {
                //범위를 벗어남
                if(x + shape.vectors[i].x < 0 || x + shape.vectors[i].x >= BlockInfo.ChunkWidth ||
                   y + shape.vectors[i].y < 0 || y + shape.vectors[i].y >= BlockInfo.ChunkHeight ||
                   z + shape.vectors[i].z < 0 || z + shape.vectors[i].z >= BlockInfo.ChunkWidth)
                {
                    //자신의 청크 범위 밖
                    worldPosition = new Vector3Int(chunk.Position.x + x, y, chunk.Position.z + z);
                    orders.Add(new BlockOrder(worldPosition + shape.vectors[i], shape.id));
                }
                else
                {
                    map[x + shape.vectors[i].x, y + shape.vectors[i].y, z + shape.vectors[i].z] = shape.id;
                }
            }
        }
        
        if(orders.Count > 0)
            World.Instance.BiomeCallEdit(orders);
    }
}


[System.Serializable]
public class TreePlacement
{
    [Header("무슨 확률로 존재할지")]
    [Range(0.1f, 0.9f)]
    public float probability;
    [Header("Tree Noise (scale은 0이면 안됨)")]
    //최소 8이상이여야 함 아니면 겹침 (treeZone을 쓰지 않는다면)
    public float scaleTree = 8;
    public float offsetTree;
    [Header("나무의 블록이 무엇으로 이루어 졌는지")]
    public byte woodType;
    //나무가 심어질 수 있는 지역을 먼저 정하고 그 지역안에서 확률적으로 나무를 심는 방법
    public float treeZoneScale = 1.3f;
    [Range(0.1f, 0.9f)]
    public float treeZoneThreshold = 0.6f;

    [Header("나뭇잎이 존재하는지")]
    public bool leaves;
    [Header("나무잎의 블록이 무엇으로 이루어 졌는지")]
    public byte leavesType;
    [Header("나무가 심어지는 땅은 무슨 블록인지")]
    public byte groundType;
    [Header("나무의 크기")]
    public int minHeight;
    public int maxHeight;

    /// <summary>
    /// 월드포지션
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public bool MakeTree(int x, int z)
    {
        //if (Noise.Get2DPerlin(new Vector2Int(x, z), 0, treeZoneScale) < treeZoneThreshold)
        //{
        if (Noise.Get2DPerlin(new Vector2Int(x, z), offsetTree, scaleTree) < probability)
        {
            return true;
        }
        //}
        return false;
    }

    public List<BlockOrder> CreateTree(Vector3Int local)
    {
        List<BlockOrder> list = new ();
        int height = Random.Range(minHeight, maxHeight);
        //나무가설치될 땅은 흙이다
        list.Add(new BlockOrder(local.x, local.y, local.z, groundType));

        for (int i = 1; i < height; i++)
        {
            list.Add(new BlockOrder(local.x, local.y + i, local.z, woodType));
        }
        if (!leaves)
            return list;

        height -= 2;

        for (int y = 0; y < 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                for (int z = -2; z <= 2; z++)
                {
                    if (x == 0 && z == 0)
                        continue;
                    list.Add(new BlockOrder(local.x + x, local.y + y + height, local.z + z, leavesType));
                }
            }
        }

        height += 2;

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;
                list.Add(new BlockOrder(local.x + x, local.y + height, local.z + z, leavesType));
            }
        }
        height += 1;
        list.Add(new BlockOrder(local.x + 0, local.y + height, local.z + 1, leavesType));
        list.Add(new BlockOrder(local.x + 1, local.y + height, local.z + 0, leavesType));
        list.Add(new BlockOrder(local.x + 0, local.y + height, local.z + 0, leavesType));
        list.Add(new BlockOrder(local.x + -1, local.y + height, local.z + 0, leavesType));
        list.Add(new BlockOrder(local.x + 0, local.y + height, local.z + -1, leavesType));

        return list;
    }
}


//땅속 지형 (광물을 생성하는것도 이 클래스가 하려 했지만 안하는걸로 변경)
[System.Serializable]
public class Underground
{
    [Header("무슨 블록이")]
    public byte type;
    [Header("무슨 확률로 존재할지")]
    [Range(0f, 1f)]
    public float probability;
    [Header("어느 깊이에 있을지")]
    public int minHeight;   //가장 아래에 나올 수 있는 좌표
    public int maxHeight;   //가장 위에 나올 수 있는 좌표
    public int depth;       //최소한의 깊이
    [Header("Noise (scale은 0이면 안됨)")]
    public float offset;
    public float scale;

    public bool MakeUnderground(int x, int y, int z, int maxY)
    {
        //지금 높이가 0보다는 크면서 지상으로부터 depth만큼은 아래여야 함
        if (y > 0 && y <= maxY - depth)
        {
            //그러면서 maxHeight minHeight 사이의 높이여야 함
            if (y < maxHeight && y > minHeight)
            {
                if (Noise.Get3DPerlin(new Vector3Int(x, y, z), offset, scale) < probability)
                    return true;
            }
        }

        return false;
    }
}

//땅속 광물들 생성에 관한 클래스
[System.Serializable]
public class CavePlacement
{
    [Header("무슨 확률로 존재할지")]
    [Range(0f, 0.01f)]
    public float probability;
    [Header("동굴의 크기")]
    public Vector3Int size;
    [Header("동굴의 길이")]
    public int lenght;
    [Header("동굴의 모양의 확률")]
    public List<CaveDir> dir;
    public List<(Vector3Int, int)> Dir()
    {
        List<(Vector3Int, int)> values = new();
        for(int i = 0; i < dir.Count; i++)
        {
            values.Add((dir[i].dir, dir[i].probability));
        }
        return values;
    }
}

[System.Serializable]   
public class CaveDir
{
    public Vector3Int dir;
    public int probability;
}

public class BlockOrder
{
    //이름이 world인 이유는 월드포지션을 줘야한다고 기억할려고
    public Vector3Int local;
    //무슨 블록인지 
    public int type;
    public BlockOrder(int x, int y, int z, int type)
    {
        local = new Vector3Int(x, y, z);
        this.type = type;
    }
    public BlockOrder(Vector3Int pos, int type)
    {
        local = pos;
        this.type = type;
    }
}