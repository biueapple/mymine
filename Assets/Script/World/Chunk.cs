using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//청크는 만들어질 때 청크를 채우고 그리는 것이 아니라
//만들어질 때 자신에게 외부에서 부여된 블록들을 리스트로 가지고 있다가 (옆 청크에서 만들어진 나무의 잎이 넘어오는 경우나 동굴이 청크를 지나가는 경우)
//청크를 실질적으로 생성할때 그 리스트를 포함해서 바이옴을 이용해 청크를 채우는 것
public class Chunk : MonoBehaviour
{
    public bool Create { get { return map == null ? false : true; } }

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    //청크를 그릴때 사용하는 정보들
    int vertexIndex = 0;
    public readonly List<Vector3> vertices = new();
    public readonly List<int> triangles = new();
    readonly List<Vector2> uvs = new();

    //이 청크의 위치
    private Vector3Int position;
    public Vector3Int Position { get { return position; } }

    //맵
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

    //그리기
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

    //청크를 다시 그릴때 기존에 사용하던 변수들을 초기화 해야함 안그러면 똑같은걸 여러번 그리면서 index가 너무 높아진다고 에러도 나고 함
    void ClearMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        vertexIndex = 0;
    }

    //mesh 만들기 (map을 바탕으로 mesh를 그림)
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

    //블록을 그릴때 필요한 6개의 삼각형을 그리기
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
                //한 면을 그릴때 점은 4개로 충분
                vertices.Add(pos + R.MultiplyPoint(BlockInfo.voxelVerts[BlockInfo.voxelTris[p, 0]]));
                vertices.Add(pos + R.MultiplyPoint(BlockInfo.voxelVerts[BlockInfo.voxelTris[p, 1]]));
                vertices.Add(pos + R.MultiplyPoint(BlockInfo.voxelVerts[BlockInfo.voxelTris[p, 2]]));
                vertices.Add(pos + R.MultiplyPoint(BlockInfo.voxelVerts[BlockInfo.voxelTris[p, 3]]));

                //텍스쳐 씌우기
                AddTexture(block.GetTextureID(p));

                //시계방향으로 그리면 바깥쪽 면이고 반시계는 안쪽면을 그리는것
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);
                //정점은 4개씩 넣으니까 +4해줌
                vertexIndex += 4;
            }
        }
    }

    //텍스쳐가 하니니까 id를 받아서 그중에 뭔지 찾아서 uv 넣어주기
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

    //mesh 그리기 (CreateMeshData -> AddBlockMeshToChunk 에서 변수들에게 할당한 mesh데이터들을 컴포넌트에 넣고 그리는 과정)
    void CreateMesh()
    {
        Mesh mesh = new()
        {
            //정점
            vertices = vertices.ToArray(),
            //정점을 사용해 삼각형을 그림
            triangles = triangles.ToArray(),
            //텍스쳐
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
