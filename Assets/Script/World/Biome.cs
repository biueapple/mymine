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

    [Header("������ �ִ� �������� (���� �̱���)")]
    [SerializeField] 
    private bool cave;

    [Header("���� �������� �켱�� {��ĥ��� �����ִ°� ������}")]

    [Header("������ ���� ������")]
    [SerializeField] 
    private TreePlacement[] treePlacements;

    [Header("���ӿ� Ȯ�������� �ִ� ��ϵ�")]
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
                    if (y > yHeight)
                        break;

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

    //���� �ɱ�
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
    [Header("���� Ȯ���� ��������")]
    [Range(0.1f, 0.9f)]
    public float probability;
    [Header("Tree Noise")]
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

    public List<BlockOrder> CreateTree(Vector3Int world)
    {
        List<BlockOrder> list = new ();
        int height = Random.Range(minHeight, maxHeight);
        //��������ġ�� ���� ���̴�
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
    public int depth;
    [Header("Noise")]
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

    public BlockOrder CreateUnderground(Vector3Int world)
    {
        return new BlockOrder(world, type);
    }
}

public class BlockOrder
{
    //�̸��� world�� ������ ������������ ����Ѵٰ� ����ҷ���
    public Vector3Int world;
    //���� ������� 
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