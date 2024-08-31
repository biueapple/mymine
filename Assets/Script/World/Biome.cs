using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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

    [Header("동굴이 있는 지형인지 (아직 미구현)")]
    [SerializeField] 
    private bool cave;

    [Header("위에 있을수록 우선적 {겹칠경우 위에있는게 생성됨}")]

    [Header("나무에 대한 정보들")]
    [SerializeField] 
    private TreePlacement[] treePlacements;

    [Header("땅속에 확률적으로 있는 블록들")]
    [SerializeField] 
    private Underground[] undergrounds;


    public int Height(int x, int z)
    {
        //return (int)(Noise.Get2DPerlin(new Vector2Int(x, z), offsetHeight, scaleHeight) * nomalHeight + surfaceHeight);
        return (int)GetNoiseValue(x, z) ;
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
        return (total / maxValue) * BlockInfo.ChunkHeight;
    }

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
                    if (y > yHeight)
                        break;

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
                }
            }
        }
    }

    //public void CreateUnderground(Chunk chunk)
    //{
    //    List<BlockOrder> list = new List<BlockOrder>();

    //    for (int x = 0; x < BlockInfo.ChunkWidth; x++)
    //    {
    //        for (int z = 0; z < BlockInfo.ChunkWidth; z++)
    //        {
    //            int yHeight = Height(chunk.Position.x + x, chunk.Position.z + z);
    //            for (int y = 0; y < BlockInfo.ChunkHeight; y++)
    //            {
    //                for (int i = undergrounds.Length - 1; i >= 0; i--)
    //                {
    //                    if (undergrounds[i].MakeUnderground(x + chunk.Position.x, y, z + chunk.Position.z, yHeight))
    //                    {
    //                        list.Add(undergrounds[i].CreateUnderground(new Vector3Int(x + chunk.Position.x, y, z + chunk.Position.z)));
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    chunk.World.EditBlock(list);
    //}

    //나무 심기
    public void CreateTreeMap(Chunk chunk)
    {
        for (int x = 0; x < BlockInfo.ChunkWidth; x++)
        {
            for (int z = 0; z < BlockInfo.ChunkWidth; z++)
            {
                int yHeight = Height(chunk.Position.x + x, chunk.Position.z + z);
                for (int i = treePlacements.Length - 1; i >= 0; i--)
                {
                    if (treePlacements[i].MakeTree(x + chunk.Position.x, z + chunk.Position.z))
                    {
                        World.Instance.WorldPositionEdit(treePlacements[i].CreateTree(new Vector3Int(chunk.Position.x + x, yHeight, chunk.Position.z + z)));
                    }
                }
            }
        }
    }
}


[System.Serializable]
public class TreePlacement
{
    [Header("무슨 확률로 존재할지")]
    [Range(0.1f, 0.9f)]
    public float probability;
    [Header("Tree Noise")]
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

    public List<BlockOrder> CreateTree(Vector3Int world)
    {
        List<BlockOrder> list = new ();
        int height = Random.Range(minHeight, maxHeight);
        //나무가설치될 땅은 흙이다
        list.Add(new BlockOrder(world.x, world.y, world.z, groundType));

        for (int i = 1; i < height; i++)
        {
            list.Add(new BlockOrder(world.x, world.y + i, world.z, woodType));
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
                    list.Add(new BlockOrder(world.x + x, world.y + y + height, world.z + z, leavesType));
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
                list.Add(new BlockOrder(world.x + x, world.y + height, world.z + z, leavesType));
            }
        }
        height += 1;
        list.Add(new BlockOrder(world.x + 0, world.y + height, world.z + 1, leavesType));
        list.Add(new BlockOrder(world.x + 1, world.y + height, world.z + 0, leavesType));
        list.Add(new BlockOrder(world.x + 0, world.y + height, world.z + 0, leavesType));
        list.Add(new BlockOrder(world.x + -1, world.y + height, world.z + 0, leavesType));
        list.Add(new BlockOrder(world.x + 0, world.y + height, world.z + -1, leavesType));

        return list;
    }
}


//땅속 지형 
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
    public int depth;
    [Header("Noise")]
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

    public BlockOrder CreateUnderground(Vector3Int world)
    {
        return new BlockOrder(world, type);
    }
}

public class BlockOrder
{
    //이름이 world인 이유는 월드포지션을 줘야한다고 기억할려고
    public Vector3Int world;
    //무슨 블록인지 
    public byte type;
    public BlockOrder(int x, int y, int z, byte type)
    {
        world = new Vector3Int(x, y, z);
        this.type = type;
    }
    public BlockOrder(Vector3Int pos, byte type)
    {
        world = pos;
        this.type = type;
    }
}