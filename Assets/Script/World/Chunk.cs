using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ûũ�� ������� �� ûũ�� ä��� �׸��� ���� �ƴ϶�
//������� �� �ڽſ��� �ܺο��� �ο��� ��ϵ��� ����Ʈ�� ������ �ִٰ� (�� ûũ���� ������� ������ ���� �Ѿ���� ��쳪 ������ ûũ�� �������� ���)
//ûũ�� ���������� �����Ҷ� �� ����Ʈ�� �����ؼ� ���̿��� �̿��� ûũ�� ä��� ��
public class Chunk : MonoBehaviour
{
    public bool Create { get { return map == null ? false : true; } }

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    //ûũ�� �׸��� ����ϴ� ������
    int vertexIndex = 0;
    public readonly List<Vector3> vertices = new();
    public readonly List<int> triangles = new();
    readonly List<Vector2> uvs = new();

    //�� ûũ�� ��ġ
    private Vector3Int position;
    public Vector3Int Position { get { return position; } }

    //��
    private int[,,] map;
    private Biome biome;
    public Biome Biome { get { return biome; } }

    public Matrix4x4 R = Matrix4x4.identity;

    public List<(Vector3Int, int)> wait;

    public void Init(Vector3Int position, Biome biome)
    {
        this.position = position;
        transform.position = position;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = World.Instance.Material;

        //map = new int[BlockInfo.ChunkWidth, BlockInfo.ChunkHeight, BlockInfo.ChunkWidth];
        this.biome = biome;
        wait = new();
        //biome.CreateBaseMap(this, ref map);
    }

    public int EditBlock(Vector3Int index, int id, bool draw)
    {
        if (index.x < 0 || index.x >= map.GetLength(0))
            return 0;
        if (index.y < 0 || index.y >= map.GetLength(1))
            return 0;
        if (index.z < 0 || index.z >= map.GetLength(2))
            return 0;
        int ex = map[index.x, index.y, index.z];
        map[index.x, index.y, index.z] = id;
        
        if( draw )
            Draw();
        
        return ex;
    }

    //�׸���
    public void Draw()
    {
        ClearMesh();
        CreateMeshData();
        CreateMesh();
    }

    public void Active()
    {
        if(map == null)
        {
            map = new int[BlockInfo.ChunkWidth, BlockInfo.ChunkHeight, BlockInfo.ChunkWidth];
            biome.CreateBaseMap(this, ref map);
            biome.CreateWait(this, ref map);
            biome.CreateUnderground(this, ref map);
            biome.CreateTreeMap(this, ref map);
            biome.CreateCave(this, ref map);
        }
        gameObject.SetActive(true);
    }
    
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    //ûũ�� �ٽ� �׸��� ������ ����ϴ� �������� �ʱ�ȭ �ؾ��� �ȱ׷��� �Ȱ����� ������ �׸��鼭 index�� �ʹ� �������ٰ� ������ ���� ��
    void ClearMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        vertexIndex = 0;
    }

    //mesh ����� (map�� �������� mesh�� �׸�)
    void CreateMeshData()
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int z = 0; z < map.GetLength(2); z++)
                {
                    AddBlockMeshToChunk(new Vector3Int(x, y, z));
                }
            }
        }
    }

    //����� �׸��� �ʿ��� 6���� �ﰢ���� �׸���
    void AddBlockMeshToChunk(Vector3Int pos)
    {
        int blockID = map[pos.x, pos.y, pos.z];
        if (blockID == 0)
            return;

        Block block = GameManager.Instance.GetBlock(blockID);

        for (int p = 0; p < BlockInfo.voxelTris.GetLength(0); p++)
        {
            if (World.Instance.WorldBlockPositionTransparent(position + pos + BlockInfo.faceChecks[p]))
            {
                //�� ���� �׸��� ���� 4���� ���
                vertices.Add(pos + R.MultiplyPoint(BlockInfo.voxelVerts[BlockInfo.voxelTris[p, 0]]));
                vertices.Add(pos + R.MultiplyPoint(BlockInfo.voxelVerts[BlockInfo.voxelTris[p, 1]]));
                vertices.Add(pos + R.MultiplyPoint(BlockInfo.voxelVerts[BlockInfo.voxelTris[p, 2]]));
                vertices.Add(pos + R.MultiplyPoint(BlockInfo.voxelVerts[BlockInfo.voxelTris[p, 3]]));

                //�ؽ��� �����
                AddTexture(block.GetTextureID(p));

                //�ð�������� �׸��� �ٱ��� ���̰� �ݽð�� ���ʸ��� �׸��°�
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);
                //������ 4���� �����ϱ� +4����
                vertexIndex += 4;
            }
        }
    }

    //�ؽ��İ� �ϴϴϱ� id�� �޾Ƽ� ���߿� ���� ã�Ƽ� uv �־��ֱ�
    void AddTexture(int textureID)
    {
        float y = textureID / BlockInfo.TextureAtlasSizeInBlocks;
        float x = textureID - (y * BlockInfo.TextureAtlasSizeInBlocks);

        x *= BlockInfo.NormalizedBlockTextureSize;
        y *= BlockInfo.NormalizedBlockTextureSize;

        y = 1f - y - BlockInfo.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + BlockInfo.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + BlockInfo.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + BlockInfo.NormalizedBlockTextureSize, y + BlockInfo.NormalizedBlockTextureSize));
    }

    //mesh �׸��� (CreateMeshData -> AddBlockMeshToChunk ���� �����鿡�� �Ҵ��� mesh�����͵��� ������Ʈ�� �ְ� �׸��� ����)
    void CreateMesh()
    {
        Mesh mesh = new()
        {
            //����
            vertices = vertices.ToArray(),
            //������ ����� �ﰢ���� �׸�
            triangles = triangles.ToArray(),
            //�ؽ���
            uv = uvs.ToArray()
        };

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    public int IndexToBlockID(Vector3Int index)
    {
        if (index.x >= map.GetLength(0) || index.x < 0)
            return 0;
        else if (index.y >= map.GetLength(1) || index.y < 0)
            return 0;
        else if(index.z >= map.GetLength(2) || index.z < 0)
            return 0;
        return map[index.x, index.y, index.z];
    }


    public int GetHeight(int worldX,int WorldZ)
    {
        return biome.Height(worldX, WorldZ); 
    }
}
