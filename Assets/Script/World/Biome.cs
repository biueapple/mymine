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
    //���̴� 2d noise
    //���� ����� 3d noise
    //tree �� 2d
    //cave �� 3d
    [Header("���� �Ѹ�(����)�� ��������� ä�����")]
    [SerializeField]
    private byte surfaceType;
    [SerializeField]
    private byte depthBlock;
    [Header("���� �Ѹ�(����)�� ��ĭ �����Ұ���")]
    [SerializeField]
    private int surfaceDepth;
    [Header("ûũ�� ä�� �⺻���� ���")]
    [SerializeField] 
    private byte nomalType;
    //���̸� ���ϴ� ������ 2���� ������ noise�� �̿��� ������ ǥ���� �Ұ�����
    [Header("�⺻���� ���� (�� ���̸� �������� noise + - ������� ��)")]
    [SerializeField] 
    private int nomalHeight;
    [Header("�⺻���� ���� nomal�� ���� �� �̸�ŭ ���ؼ� ���̰� ������")]
    [SerializeField] 
    private int surfaceHeight;
    [Header("Height Noise")]
    [SerializeField] 
    private float offsetHeight;
    //[MinValue(0.1f)]
    [SerializeField] 
    private float scaleHeight = 0.5f;

    [Header("���� �������� �켱�� {��ĥ��� �����ִ°� ������}")]

    [Header("������ ���� ������")]
    [SerializeField] 
    private TreePlacement[] treePlacements;

    [Header("���ӿ� Ȯ�������� �ִ� ��ϵ�")]
    [SerializeField] 
    private Underground[] undergrounds;

    [SerializeField]
    private CavePlacement cavePlacement;

    private readonly Vector3Int[] plus = new Vector3Int[7]
    {
        new Vector3Int(0,0,0),
        new Vector3Int(1,0,0),
        new Vector3Int(-1,0,0),
        new Vector3Int(0,1,0),
        new Vector3Int(0,-1,0),
        new Vector3Int(0,0,1),
        new Vector3Int(0,0,-1)
    };

    private readonly Vector3Int[] chair = new Vector3Int[6]
    {
        new Vector3Int(0,0,0),
        new Vector3Int(1,0,0),
        new Vector3Int(0,1,0),
        new Vector3Int(1,1,0),
        new Vector3Int(0,0,1),
        new Vector3Int(1,0,1)
    };
    public Vector3Int[] Chair(Vector3Int dir)
    {
        if(dir == Vector3Int.back)
        {
            Vector3Int[] c = new Vector3Int[6];
            for (int i = 0; i < chair.Length; i++)
            {
                c[i] = new Vector3Int(0, 0, -1) + chair[i];
            }
            return c;
        }
        else if(dir == Vector3Int.down)
        {
            Vector3Int[] c = new Vector3Int[6];
            for (int i = 0; i < chair.Length; i++)
            {
                c[i] = new Vector3Int(0, -1, 0) + chair[i];
            }
            return c;
        }
        else if(dir == Vector3Int.left)
        {
            Vector3Int[] c = new Vector3Int[6];
            for (int i = 0; i < chair.Length; i++)
            {
                c[i] = new Vector3Int(-1, 0, 0) + chair[i];
            }
            return c;
        }
        return chair;
    }

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

    //�� ��� ������ �ϳ��� �Լ��� ���� ���ʿ��� �ݺ� ���̱�

    //�⺻���� ������ �����
    public void CreateBaseMap(Chunk chunk, ref int[,,] map)
    {
        for (int x = 0; x < BlockInfo.ChunkWidth; x++)
        {
            for (int z = 0; z < BlockInfo.ChunkWidth; z++)
            {
                int yHeight = Height(chunk.Position.x + x, chunk.Position.z + z);
                for (int y = 0; y < BlockInfo.ChunkHeight; y++)
                {
                    //air �� ��� �־����� �ʴ��� 0���� ���־ ������
                    if (y <= yHeight)
                    {
                        //���� �Ʒ����� ����
                        if (y < 1)
                            map[x, y, z] = 3;

                        //���� ����
                        else if (y >= yHeight)
                            map[x, y, z] = surfaceType;

                        //����
                        else if (y >= yHeight - surfaceDepth)
                            map[x, y, z] = depthBlock;

                        //�� �̿�
                        else
                            map[x, y, z] = nomalType;


                        //���� �����
                        for (int i = undergrounds.Length - 1; i >= 0; i--)
                        {
                            if (undergrounds[i].MakeUnderground(x + chunk.Position.x, y, z + chunk.Position.z, yHeight))
                            {
                                map[x, y, z] = undergrounds[i].type;
                            }
                        }
                    }
                }

                //���� �����
                for (int i = treePlacements.Length - 1; i >= 0; i--)
                {
                    if (treePlacements[i].MakeTree(x + chunk.Position.x, z + chunk.Position.z))
                    {
                        CreateTreeMapWorld(chunk, ref map, treePlacements[i].CreateTree(new Vector3Int(x, yHeight, z)));
                    }
                }

                //���� �����
                if(cavePlacement != null)
                {
                    if(Random.Range(0f, 1f) <= cavePlacement.probability)
                    {
                        CreateCave(x, yHeight, z, chunk, ref map);
                    }
                    //if (Random.Range(0, 1f) > 0.999f)
                    //{
                    //    CreateCave(x, yHeight, z, chunk, ref map);
                    //}
                }
                
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

    //public void CreateUnderground(Chunk chunk, ref int[,,] map)
    //{
    //    //for (int x = 0; x < BlockInfo.ChunkWidth; x++)
    //    //{
    //    //    for (int z = 0; z < BlockInfo.ChunkWidth; z++)
    //    //    {
    //    //        int yHeight = Height(chunk.Position.x + x, chunk.Position.z + z);
    //    //        for (int y = 0; y < BlockInfo.ChunkHeight; y++)
    //    //        {
    //    //            for (int i = undergrounds.Length - 1; i >= 0; i--)
    //    //            {
    //    //                if (undergrounds[i].MakeUnderground(x + chunk.Position.x, y, z + chunk.Position.z, yHeight))
    //    //                {
    //    //                    map[x, y, z] = undergrounds[i].type;
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //}
    //}

    ////���� �ɱ�
    //public void CreateTreeMap(Chunk chunk, ref int[,,] map)
    //{
    //    //for (int x = 0; x < BlockInfo.ChunkWidth; x++)
    //    //{
    //    //    for (int z = 0; z < BlockInfo.ChunkWidth; z++)
    //    //    {
    //    //        int yHeight = Height(chunk.Position.x + x, chunk.Position.z + z);
    //    //        for (int i = treePlacements.Length - 1; i >= 0; i--)
    //    //        {
    //    //            if (treePlacements[i].MakeTree(x + chunk.Position.x, z + chunk.Position.z))
    //    //            {
    //    //                CreateTreeMapWorld(chunk, ref map, treePlacements[i].CreateTree(new Vector3Int(x, yHeight, z)));
    //    //            }
    //    //        }
    //    //    }
    //    //}
    //}

    private void CreateTreeMapWorld(Chunk chunk, ref int[,,] map, List<BlockOrder> orders)
    {
        //���⼭ ������ orders���� ������ ���̸� ��� ��ġ�� ���� ���� �� �ִµ� �� ��� �ٸ� ûũ�� ���� �Ѱܾ� ��
        //���� �̹� ������� ûũ��� ���� ���游 �ϰ� 
        //��������� ���� ûũ��� wait�� ���� �־����
        for (int i = orders.Count - 1; i >= 0; i--)
        {
            if ((orders[i].local.x >= 0 && orders[i].local.x < BlockInfo.ChunkWidth) &&
               (orders[i].local.y >= 0 && orders[i].local.y < BlockInfo.ChunkHeight) &&
               (orders[i].local.z >= 0 && orders[i].local.z < BlockInfo.ChunkWidth))
            {
                //ûũ�� ���� ��
                map[orders[i].local.x, orders[i].local.y, orders[i].local.z] = orders[i].type;
                //����Ʈ ��Ȱ���Ϸ��� ����� �ε����� ����
                orders.RemoveAt(i);
            }
            else
            {
                //ûũ ���� ���̸� ���忡�� ����ϴ� �������� ����� �ٲ�
                orders[i].local += chunk.Position;
            }
        }

        //ûũ�� ���� ��
        //���忡�� local ��ǥ + ûũ ��ġ ���� �Ѱܼ� �ֱ�
        World.Instance.BiomeCallEdit(orders);
    }

    //public void CreateCave(Chunk chunk, ref int[,,] map)
    //{
    //    ////�� ���� �������� ���� �����

    //    ////�ϴ��� �� �������� ������

    //    //Vector3Int worldPosition;
    //    //List<BlockOrder> orders = new();
    //    //for (int x = 0; x < BlockInfo.ChunkWidth; x++)
    //    //{
    //    //    for (int z = 0; z < BlockInfo.ChunkWidth; z++)
    //    //    {
    //    //        if (Random.Range(0, 1f) > 0.999f)
    //    //        {
    //    //            int yHeight = Height(chunk.Position.x + x, chunk.Position.z + z);
    //    //            Worm worm = Worm_Algorithm.Instance.Start(Worm_Algorithm.Dir, new Vector3Int(2, 2, 2), 100);

    //    //            for (int i = 0; i < worm.pathRange.Count; i++)
    //    //            {
    //    //                if ((worm.pathRange[i].x + x >= 0 && worm.pathRange[i].x + x < BlockInfo.ChunkWidth) &&
    //    //                    (worm.pathRange[i].y + yHeight >= 0 && worm.pathRange[i].y + yHeight < BlockInfo.ChunkHeight) &&
    //    //                    (worm.pathRange[i].z + z >= 0 && worm.pathRange[i].z + z < BlockInfo.ChunkWidth))
    //    //                {
    //    //                    map[worm.pathRange[i].x + x, worm.pathRange[i].y + yHeight, worm.pathRange[i].z + z] = 0;
    //    //                }
    //    //                else
    //    //                {
    //    //                    worldPosition = new Vector3Int(chunk.Position.x + x, yHeight, chunk.Position.z + z);
    //    //                    orders.Add(new BlockOrder(worldPosition + worm.pathRange[i], 0));
    //    //                }
    //    //            }

    //    //            for (int i = 0; i < worm.pathWall.Count; i++)
    //    //            {
    //    //                if (Random.Range(0, 1f) > 0.99f)
    //    //                {
    //    //                    //������ ��ġ�� ����� ����
    //    //                    //�ϴ��� chair�� ����

    //    //                    Vector3Int[] c = Chair(worm.pathWall[i].dir);
    //    //                    for (int index = 0; index < c.Length;  index++)
    //    //                    {
    //    //                        //���⵵ �����ؾ� ��
    //    //                        if ((worm.pathWall[i].position.x + x + c[index].x >= 0 && worm.pathWall[i].position.x + x + c[index].x < BlockInfo.ChunkWidth) &&
    //    //                        (worm.pathWall[i].position.y + yHeight + c[index].y >= 0 && worm.pathWall[i].position.y + yHeight + c[index].y < BlockInfo.ChunkHeight) &&
    //    //                        (worm.pathWall[i].position.z + z + c[index].z >= 0 && worm.pathWall[i].position.z + z + c[index].z < BlockInfo.ChunkWidth))
    //    //                        {
    //    //                            //�ڽ��� ûũ ���� ��
    //    //                            map[worm.pathWall[i].position.x + x + c[index].x, worm.pathWall[i].position.y + yHeight + c[index].y, worm.pathWall[i].position.z + z + c[index].z] = (int)GameManager.BLCOK_ENUM.CoalOre;
    //    //                        }
    //    //                        else
    //    //                        {
    //    //                            //�ڽ��� ûũ ���� ��
    //    //                            worldPosition = new Vector3Int(chunk.Position.x + x, yHeight, chunk.Position.z + z);
    //    //                            orders.Add(new BlockOrder(worldPosition + worm.pathWall[i].position + c[index], (int)GameManager.BLCOK_ENUM.CoalOre));
    //    //                        }
    //    //                    }
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //}
    //    //World.Instance.BiomeCallEdit(orders);
    //}

    public void CreateCave(int x, int y, int z, Chunk chunk, ref int[,,] map)
    {
        Vector3Int worldPosition;
        List<BlockOrder> orders = new();
        Worm worm;
        if (cavePlacement.dir.Count == 0)
            worm = Worm_Algorithm.Instance.Start(Worm_Algorithm.Dir, new Vector3Int(2, 2, 2), 100);
        else
            worm = Worm_Algorithm.Instance.Start(cavePlacement.Dir(), cavePlacement.size, cavePlacement.lenght);

        //���� �Ĵ� �ܰ�
        for (int i = 0; i < worm.pathRange.Count; i++)
        {
            if ((worm.pathRange[i].x + x >= 0 && worm.pathRange[i].x + x < BlockInfo.ChunkWidth) &&
                (worm.pathRange[i].y + y >= 0 && worm.pathRange[i].y + y < BlockInfo.ChunkHeight) &&
                (worm.pathRange[i].z + z >= 0 && worm.pathRange[i].z + z < BlockInfo.ChunkWidth))
            {
                //������ �հ��� ����
                if (map[worm.pathRange[i].x + x, worm.pathRange[i].y + y, worm.pathRange[i].z + z] != (int)GameManager.BLCOK_ENUM.Bedrock)
                {
                    map[worm.pathRange[i].x + x, worm.pathRange[i].y + y, worm.pathRange[i].z + z] = 0;
                }
            }
            else
            {
                //���尡 �˾Ƽ� ������ ���� ����
                worldPosition = new Vector3Int(chunk.Position.x + x, y, chunk.Position.z + z);
                orders.Add(new BlockOrder(worldPosition + worm.pathRange[i], 0));
            }
        }

        //���� ����� �ܰ� (������ ����� �ܰ�� ������ ����� �Ͱ��� ������ Ȯ���� ��������)
        for (int i = 0; i < worm.pathWall.Count; i++)
        {
            if (Random.Range(0, 1f) > 0.99f)
            {
                //������ ��ġ�� ����� ����
                //�ϴ��� chair�� ����

                Vector3Int[] c = Chair(worm.pathWall[i].dir);
                for (int index = 0; index < c.Length; index++)
                {
                    //�ڽ��� ûũ ���� ��
                    if ((worm.pathWall[i].position.x + x + c[index].x >= 0 && worm.pathWall[i].position.x + x + c[index].x < BlockInfo.ChunkWidth) &&
                    (worm.pathWall[i].position.y + y + c[index].y >= 0 && worm.pathWall[i].position.y + y + c[index].y < BlockInfo.ChunkHeight) &&
                    (worm.pathWall[i].position.z + z + c[index].z >= 0 && worm.pathWall[i].position.z + z + c[index].z < BlockInfo.ChunkWidth))
                    {
                        //���� ����ó��
                        if (map[worm.pathWall[i].position.x + x + c[index].x, worm.pathWall[i].position.y + y + c[index].y, worm.pathWall[i].position.z + z + c[index].z] != (int)GameManager.BLCOK_ENUM.Bedrock
                            && map[worm.pathWall[i].position.x + x + c[index].x, worm.pathWall[i].position.y + y + c[index].y, worm.pathWall[i].position.z + z + c[index].z] != 0)
                        {
                            map[worm.pathWall[i].position.x + x + c[index].x, worm.pathWall[i].position.y + y + c[index].y, worm.pathWall[i].position.z + z + c[index].z] = (int)GameManager.BLCOK_ENUM.CoalOre;
                        }
                    }
                    else
                    {
                        //���尡 �˾Ƽ� ������ ����ó�� ����
                        //�ڽ��� ûũ ���� ��
                        worldPosition = new Vector3Int(chunk.Position.x + x, y, chunk.Position.z + z);
                        orders.Add(new BlockOrder(worldPosition + worm.pathWall[i].position + c[index], (int)GameManager.BLCOK_ENUM.CoalOre));
                    }
                }
            }
        }


        World.Instance.BiomeCallEdit(orders);
    }

    //public void CreateMineral(Chunk chunk, ref int[,,] map, Worm worm)
    //{
    //    //������� ������ ���� �ֱ�
    //}
}


[System.Serializable]
public class TreePlacement
{
    [Header("���� Ȯ���� ��������")]
    [Range(0.1f, 0.9f)]
    public float probability;
    [Header("Tree Noise (scale�� 0�̸� �ȵ�)")]
    //�ּ� 8�̻��̿��� �� �ƴϸ� ��ħ (treeZone�� ���� �ʴ´ٸ�)
    public float scaleTree = 8;
    public float offsetTree;
    [Header("������ ����� �������� �̷�� ������")]
    public byte woodType;
    //������ �ɾ��� �� �ִ� ������ ���� ���ϰ� �� �����ȿ��� Ȯ�������� ������ �ɴ� ���
    public float treeZoneScale = 1.3f;
    [Range(0.1f, 0.9f)]
    public float treeZoneThreshold = 0.6f;

    [Header("�������� �����ϴ���")]
    public bool leaves;
    [Header("�������� ����� �������� �̷�� ������")]
    public byte leavesType;
    [Header("������ �ɾ����� ���� ���� �������")]
    public byte groundType;
    [Header("������ ũ��")]
    public int minHeight;
    public int maxHeight;

    /// <summary>
    /// ����������
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
        //��������ġ�� ���� ���̴�
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


//���� ���� 
[System.Serializable]
public class Underground
{
    [Header("���� �����")]
    public byte type;
    [Header("���� Ȯ���� ��������")]
    [Range(0f, 1f)]
    public float probability;
    [Header("��� ���̿� ������")]
    public int minHeight;   //���� �Ʒ��� ���� �� �ִ� ��ǥ
    public int maxHeight;   //���� ���� ���� �� �ִ� ��ǥ
    public int depth;       //�ּ����� ����
    [Header("Noise (scale�� 0�̸� �ȵ�)")]
    public float offset;
    public float scale;

    public bool MakeUnderground(int x, int y, int z, int maxY)
    {
        //���� ���̰� 0���ٴ� ũ�鼭 �������κ��� depth��ŭ�� �Ʒ����� ��
        if (y > 0 && y <= maxY - depth)
        {
            //�׷��鼭 maxHeight minHeight ������ ���̿��� ��
            if (y < maxHeight && y > minHeight)
            {
                if (Noise.Get3DPerlin(new Vector3Int(x, y, z), offset, scale) < probability)
                    return true;
            }
        }

        return false;
    }
}

//���� ������ ������ ���� Ŭ����
[System.Serializable]
public class CavePlacement
{
    [Header("���� Ȯ���� ��������")]
    [Range(0f, 0.01f)]
    public float probability;
    [Header("������ ũ��")]
    public Vector3Int size;
    [Header("������ ����")]
    public int lenght;
    [Header("������ ����� Ȯ��")]
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
    //�̸��� world�� ������ ������������ ����Ѵٰ� ����ҷ���
    public Vector3Int local;
    //���� ������� 
    public byte type;
    public BlockOrder(int x, int y, int z, byte type)
    {
        local = new Vector3Int(x, y, z);
        this.type = type;
    }
    public BlockOrder(Vector3Int pos, byte type)
    {
        local = pos;
        this.type = type;
    }
}