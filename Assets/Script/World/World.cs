using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class World : MonoBehaviour
{
    private static World instance;
    public static World Instance
    {
        get
        {
            if(instance == null)
                instance = new GameObject("World").AddComponent<World>();
            instance.transform.position = Vector3.zero;
            return instance;
        }
    }

    private Material material;
    public Material Material { 
        get 
        {
            if (material == null)
                material = Resources.Load<Material>("Material/WorldMaterial");
            return material; 
        } 
    }

    [SerializeField]
    private Biome[] biomes;
    public Biome[] Biomes
    {
        get
        {
            if (biomes == null)
                biomes = Resources.LoadAll<Biome>("Biome");
            return biomes;
        }
    }

    //��� ûũ��
    private readonly Dictionary<Vector3Int, Chunk> chunks = new ();
    private Vector3Int playerChunkIndex = new ();
    //Ȱ��ȭ�� ûũ��
    private List<Chunk> activeChunks = new ();
    public Chunk CreateChunk(Vector3Int index)
    {
        chunks.TryGetValue(index, out Chunk chunk);
        if (chunk == null)
        {
            chunk = new GameObject("Chunk {" + index.x + ", " + index.y + ", " + index.z + "}").AddComponent<Chunk>();
            chunk.transform.parent = transform;
            chunk.Init(index * BlockInfo.ChunkWidth, Biomes[0]);
            chunks.Add(index, chunk);
        }
        return chunk;
    }

    private Coroutine sensing;
    private void Start()
    {
        StartCoroutine(CreateWorld());
    }

    private IEnumerator CreateWorld()
    {
        Vector3Int index = GetChunkIndex(GameManager.Instance.Players[0].transform.position);
        var list = GetChunkRangeIfCreate(index, 2);

        list.ForEach(x => x.Active());

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Draw();
            yield return null;
        }
        activeChunks.AddRange(list);
        GameManager.Instance.Players.ToList().ForEach(x => x.transform.position = new Vector3(transform.position.x, Height((int)transform.position.x, (int)transform.position.z) + 3, transform.position.z));
        sensing = StartCoroutine(PlayerSensing());
    }

    private IEnumerator PlayerSensing()
    {
        while(true)
        {
            Vector3Int index = GetChunkIndex(GameManager.Instance.Players[0].transform.position);
            if (playerChunkIndex != index)
            {
                playerChunkIndex = index;
                ActiveChunkFind();
            }
            yield return new WaitForSeconds(1);
        }
    }

    //�÷��̾��� ��ġ ����
    //������ ��ġ�� �ƴ϶�� 
    //������ ûũ ����
    //���� ���� ûũ�� �ִٸ� ����
    //������ ����Ʈ���� ���°� ��Ȱ��ȭ

    private void ActiveChunkFind()
    {
        Player player = GameManager.Instance.Players[0];
        //�÷��̾�� �ش�� ûũ�� �ε����� ã��
        Vector3Int positionInt = new(Mathf.FloorToInt(player.transform.position.x), Mathf.FloorToInt(player.transform.position.y), Mathf.FloorToInt(player.transform.position.z));
        Vector3Int chunkIndex = new(positionInt.x / BlockInfo.ChunkWidth, positionInt.y / BlockInfo.ChunkHeight, positionInt.z / BlockInfo.ChunkWidth);
        //�� ûũ�� ������ �ش�Ǵ� ���� ã��
        List<Chunk> active = GetChunkRangeIfCreate(chunkIndex, 2);


        //ûũ�� ���� �׸��� ��輱�� �ִ� �̹� �׷��� �ִ� ûũ�� �� ûũ���� �׷��� ���� �ʴٴ� ������ �߻��߱⿡
        //�� ûũ�� �׸��� ��輱�� �ִ� �ٸ� ûũ���� �׷���� ��
        //���� Ȱ��ȭ�� �ĺ����� ����Ʈ
        List<Chunk> draw = new();

        //������ ��� ûũ�� ��Ȱ��ȭ
        for (int j = 0; j < activeChunks.Count; j++)
        {
            if (!active.Contains(activeChunks[j]))
            {
                activeChunks[j].Deactive();
            }
        }

        //���� �ȿ� �ִ� ûũ���� Ȱ��ȭ �ؾ��ϴµ� �ϴ� �ĺ����� �ø�
        for(int i = 0; i < active.Count; i++)
        {
            if (!activeChunks.Contains(active[i]))
            {
                draw.Add(active[i]);
                //active[i].Active();
                //active[i].Draw();
            }
        }

        //�ĺ��� �ø� ����Ʈ�� 6���� Ȯ���ϸ鼭 �̹� Ȱ��ȭ�� ûũ�� �ִٸ� �׸��� �̹� ����Ʈ�� �� ���� �ʴٸ� �߰�������
        //for���� �ݴ�� ���� ������ �߰��� ����Ʈ�� Ȯ������ �ʱ� ����
        for(int i = draw.Count - 1; i >= 0; i--)
        {
            chunkIndex = new(draw[i].Position.x / BlockInfo.ChunkWidth, draw[i].Position.y / BlockInfo.ChunkHeight, draw[i].Position.z / BlockInfo.ChunkWidth);
            for (int p = 0; p < BlockInfo.faceChecks.Length; p++)
            {
                if(chunks.TryGetValue(chunkIndex + BlockInfo.faceChecks[p], out Chunk value))
                {
                    if(value.Create && !draw.Contains(value))
                    {
                        draw.Add(value);
                    }
                }
            }
        }

        //�׸� �ĺ����� ���� ����Ʈ�� �־����� �׸���
        for(int i = 0; i < draw.Count; i++)
        {
            draw[i].Active();
            draw[i].Draw(); 
        }

        //ûũ Ȱ��ȭ
        activeChunks = active;
    }

    /// <summary>
    /// center�� �������� range��ŭ �������ִ� ûũ���� ����Ʈ�� ��������
    /// </summary>
    /// <param name="index">�߽� ûũ�� �ε��� (world��ġ�� �ƴ�)</param>
    /// <param name="range">����</param>
    /// <returns></returns>
    public List<Chunk> GetChunkRangeIfCreate(Vector3Int index, int range)
    {
        List<Chunk> list = new ();
        for(int x = -range; x <= range; x++)
        {
            for(int z = -range; z <= range; z++)
            {
                if(chunks.TryGetValue(index + new Vector3Int(x,0,z), out Chunk value))
                {
                    list.Add(value);
                }
                else
                {
                    Chunk chunk = CreateChunk(index + new Vector3Int(x, 0, z));
                    list.Add(chunk);
                }
            }
        }
        return list;
    }

    public bool WorldBlockPositionTransparent(Vector3Int position)
    {
        Vector3Int chunkIndex = new(Mathf.FloorToInt((float)position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)position.z / BlockInfo.ChunkWidth));
        
        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            if(chunk.Create)
            {
                position -= chunk.Position;
                int index = chunk.IndexToBlockID(position);

                if (index > 0)
                {
                    if (GameManager.Instance.GetBlock(index).Transparent)
                        return true;

                    return false;
                }
                //ûũ ���� ����� ����
                return true;
            }
        }

        //ûũ �ٱ� ���� ���� ������ (�׸��� ����)
        return false;
    }

    public bool WorldBlockPositionSolid(Vector3 position)
    {
        Vector3Int positionInt = new (Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));
        Vector3Int chunkIndex = new (Mathf.FloorToInt(position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt(position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt(position.z / BlockInfo.ChunkWidth));
        
        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            if (chunk.Create)
            {
                positionInt -= chunk.Position;
                int index = chunk.IndexToBlockID(positionInt);
                if (index > 0)
                {
                    if (GameManager.Instance.GetBlock(index).IsSolid)
                    {
                        return true;
                    }

                    return false;
                }
            }
        }
        return false;
    }

    public Block WorldPositionToBlock(Vector3 position)
    {
        Vector3Int positionInt = new (Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));
        Vector3Int chunkIndex = new(Mathf.FloorToInt(position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt(position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt(position.z / BlockInfo.ChunkWidth));

        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            if (chunk.Create)
            {
                positionInt -= chunk.Position;
                int index = chunk.IndexToBlockID(positionInt);
                if (index > 0)
                {
                    return GameManager.Instance.GetBlock(index);
                }
            }
        }
        return null;
    }

    public bool WorldPostionBroken(Vector3Int position)
    {
        //������������ ûũ�� �ε����� ��ȯ�ϴ� ���� (-��ǥ���� int�� ���� ���� �߸������⿡ float���� ����� ����� �� {[int = -2 / 16 = 0] , [float -2.0 / 16 = -1]})
        Vector3Int chunkIndex = new(Mathf.FloorToInt((float)position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)position.z / BlockInfo.ChunkWidth));

        //���� ûũ�� �ִٸ�
        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            if(chunk.Create)
            {
                //���� �������� ûũ���������� �ٲ��� (-2 13 -5 => 13 13 10)
                Vector3Int Chunkposition = position - chunk.Position;

                //�� ��ġ�� ���� ����� �ִ���
                int index = chunk.EditBlock(Chunkposition, 0, true);
                if (index > 0)
                {
                    //�� ����� ����ϴ� ������ id�� ��������
                    int itemID = GameManager.Instance.GetBlock(index).ItemID;

                    //������id�� 0�� ���⿡ 
                    if (itemID > 0)
                    {
                        FlowManager.Instance.DropItem(position + new Vector3(0.5f, 0.5f, 0.5f), itemID, 1, Vector3.zero);
                    }
                }

                //�ٲ� ����� ������ �ٸ� ûũ�� ��ġ�� �پ��ִ��� üũ
                for (int i = 0; i < BlockInfo.faceChecks.Length; i++)
                {
                    //��ġ + ������ ��ġ���� �ٸ�ûũ�� ���ð��
                    if (chunks.TryGetValue(GetChunkIndex(position + BlockInfo.faceChecks[i]), out Chunk ex))
                    {
                        //�� ûũ���� �ٽ� �׷��� ��
                        if (!(chunk.Position == ex.Position))
                        {
                            ex.Draw();
                        }
                    }
                }

                return true;
            }
        }
        return false;
    }

    public bool WorldPositionInstall(Vector3Int position, int id)
    {
        //������������ ûũ�� �ε����� ��ȯ�ϴ� ���� (-��ǥ���� int�� ���� ���� �߸������⿡ float���� ����� ����� �� {[int = -2 / 16 = 0] , [float -2.0 / 16 = -1]})
        Vector3Int chunkIndex = new(Mathf.FloorToInt((float)position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)position.z / BlockInfo.ChunkWidth));

        //���� ûũ�� �ִٸ�
        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            if(chunk.Create)
            {
                //���� �������� ûũ���������� �ٲ��� (-2 13 -5 => 13 13 10)
                Vector3Int Chunkposition = position - chunk.Position;

                //�� ��ġ�� ���� ����� �ִ���
                int index = chunk.IndexToBlockID(Chunkposition);
                if (index == 0)
                {
                    chunk.EditBlock(chunkIndex, id, true);
                }

                //�ٲ� ����� ������ �ٸ� ûũ�� ��ġ�� �پ��ִ��� üũ
                for (int i = 0; i < BlockInfo.faceChecks.Length; i++)
                {
                    //��ġ + ������ ��ġ���� �ٸ�ûũ�� ���ð��
                    if (chunks.TryGetValue(GetChunkIndex(position + BlockInfo.faceChecks[i]), out Chunk ex))
                    {
                        //�� ûũ���� �ٽ� �׷��� ��
                        if (!(chunk.Position == ex.Position))
                        {
                            ex.Draw();
                        }
                    }
                }

                return true;
            }
        }
        return false;
    }

    public bool WorldPositionEdit(Vector3Int position, int id)
    {
        //������������ ûũ�� �ε����� ��ȯ�ϴ� ���� (-��ǥ���� int�� ���� ���� �߸������⿡ float���� ����� ����� �� {[int = -2 / 16 = 0] , [float -2.0 / 16 = -1]})
        Vector3Int chunkIndex = new(Mathf.FloorToInt((float)position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)position.z / BlockInfo.ChunkWidth));

        //���� ûũ�� �ִٸ�
        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            //���� �������� ûũ���������� �ٲ��� (-2 13 -5 => 13 13 10)
            Vector3Int Chunkposition = position - chunk.Position;

            if (chunk.Create)
            {
                //�� ��ġ�� ���� ����� �ִ���
                chunk.EditBlock(Chunkposition, id, true);

                //�ٲ� ����� ������ �ٸ� ûũ�� ��ġ�� �پ��ִ��� üũ
                for (int i = 0; i < BlockInfo.faceChecks.Length; i++)
                {
                    //��ġ + ������ ��ġ���� �ٸ�ûũ�� ���ð��
                    if (chunks.TryGetValue(GetChunkIndex(position + BlockInfo.faceChecks[i]), out Chunk ex))
                    {
                        //�� ûũ���� �ٽ� �׷��� ��
                        if (!(chunk.Position == ex.Position))
                        {
                            ex.Draw();
                        }
                    }
                }

                return true;
            }
            else
            {
                chunk.wait.Add((Chunkposition, id));
            }
        }
        return false;
    }

    public void BiomeCallEdit(List<BlockOrder> orders)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            //����� �ö� ���̿��� orders�� local���� world�� �ٲ��� �Ŀ� ������

            //������������ ûũ�� �ε����� ��ȯ�ϴ� ���� (-��ǥ���� int�� ���� ���� �߸������⿡ float���� ����� ����� �� {[int = -2 / 16 = 0] , [float -2.0 / 16 = -1]})
            Vector3Int chunkIndex = new(Mathf.FloorToInt((float)orders[i].local.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)orders[i].local.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)orders[i].local.z / BlockInfo.ChunkWidth));

            //���� �������� ûũ���������� �ٲ��� (-2 13 -5 => 13 13 10)
            Vector3Int Chunkposition;

            //���� ûũ�� �ִٸ�
            if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
            {
                //���� �������� ûũ���������� �ٲ��� (-2 13 -5 => 13 13 10)
                Chunkposition = orders[i].local - chunk.Position;

                if(chunk.Create)
                {
                    chunk.EditBlock(Chunkposition, orders[i].type, false);
                }
                else
                {
                    chunk.wait.Add((Chunkposition, orders[i].type));
                }
            }
            else
            {
                Chunk c = CreateChunk(chunkIndex);

                //���� �������� ûũ���������� �ٲ��� (-2 13 -5 => 13 13 10)
                Chunkposition = orders[i].local - c.Position;

                c.wait.Add((Chunkposition, orders[i].type));
            }
        }
    }

    //raycast�� ����� �Լ� (collider�� �ȽἭ ������ raycast�� ������� ����)
    public BlockLaycast WorldRaycast(Vector3 postion, Vector3 dir, float distance, float checkIncrement = 0.1f)
    {
        BlockLaycast lay;
        //step�� checkIncrement�� �þ�鼭 �װ��� ����� �ִ��� Ȯ���ؾ���
        float step = checkIncrement;
        //���������� Ȯ���غ� �����ġ
        Vector3 lastPos = postion;

        //�÷��̾��� ��Ÿ��� ����� �ʴ� ��
        while (step < distance)
        {
            //ī�޶���� step��ŭ �������� ������ �Ÿ���
            Vector3 pos = postion + (dir * step);

            //lastPos == pos �ΰ�� continue �ҷ� �ߴµ� lastPos�� Vector3Int �� pos �� Vector3�� �񱳰� �ȵų�

            //����� �ִ°�?
            Block scriptable = WorldPositionToBlock(pos);
            if (scriptable != null)
            {
                //�ִٸ� �� ����� ��ġ�� �ı��� ����� ��ġ��
                lay = new()
                {
                    position = pos,
                    positionToInt = new(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)),
                    lastPosition = lastPos,
                    lastPositionToInt = new(Mathf.FloorToInt(lastPos.x), Mathf.FloorToInt(lastPos.y), Mathf.FloorToInt(lastPos.z)),
                    block = scriptable
                };
                //�� ���� ���������� Ȯ���ߴ� ����� ��ġ�� ����� ��ġ�� ��ġ
                return lay;
            }
            //Ȯ���� ��ġ�� ������ ��ġ�� �־����
            lastPos = new(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
            step += checkIncrement;
        }

        return null;
    }

    public Vector3Int GetChunkIndex(Vector3 worldPosition)
    {
        return new(Mathf.FloorToInt(worldPosition.x / BlockInfo.ChunkWidth), Mathf.FloorToInt(worldPosition.y / BlockInfo.ChunkHeight), Mathf.FloorToInt(worldPosition.z / BlockInfo.ChunkWidth));
    }
    public Vector3Int GetChunkIndex(Vector3Int worldPosition)
    {
        return new(Mathf.FloorToInt((float)worldPosition.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)worldPosition.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)worldPosition.z / BlockInfo.ChunkWidth));
    }

    public class BlockLaycast
    {
        public Vector3 position;
        public Vector3Int positionToInt;
        public Vector3 lastPosition;
        public Vector3Int lastPositionToInt;
        public Block block;
        public bool Equals(BlockLaycast laycast)
        {
            if (laycast == null)
                return false;
            if (positionToInt == laycast.positionToInt && lastPositionToInt == laycast.lastPositionToInt && block.ID == laycast.block.ID)
                return true; 
            return false;
        }
    }

    public int Height(int x,int z)
    {
        Vector3Int positionInt = new(x, 0, z);
        Vector3Int chunkIndex = new(positionInt.x / BlockInfo.ChunkWidth, 0, positionInt.z / BlockInfo.ChunkWidth);

        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            return chunk.GetHeight(x, z);
        }
        return 0;
    }
}

////��ϸ��� �ڽ��� �����ϴ� vertices�� triangles�� ������ �־�� �浹ó���� ������ (PointInPolyhedron ��� ��)
////������ �׿� ���� vertices�� triangles�� ������ �־�� ��
////��� �鿡 ��������(���� �ٸ� ����� ���� �׷��� �ϴ���)�� ���� ������ ������ �־�� �� 
////�ڽ��� �ٸ� ��Ͽ� � ������ ��ġ�� �ٸ� ��Ͽ��� ������ �������� ���� ������ ������ �־�� �� (����� �Ӹ��κ��� �ٸ� �κа� ������� �׷��� ��, �ݺ���� ���κ��� ����ϰ� ������� �׷��� ��)
//public class WorldBlockInfomation
//{
//    private byte blockID;
//    public byte BlockID => blockID;
//    private Vector3Int rotation;
//    public Vector3Int Rotation => rotation;
//    //ûũ����
//    //ûũ�� �׸��� ����ϴ� ������
//    int vertexIndex = 0;
//    private readonly List<Vector3> vertices;
//    public List<Vector3> Vertices => vertices;
//    private readonly List<int> triangles;
//    public List<int> Triangles => triangles;
//    private readonly List<Vector2> uvs;
//    public List<Vector2> UVS => uvs;

//    public WorldBlockInfomation(byte id, Vector3Int rotation, List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
//    {
//        blockID = id;
//        this.rotation = rotation;
//        this.vertices = vertices;
//        this.triangles = triangles;
//        this.uvs = uvs;
//    }

//    public void AddAngle(Vector3Int angle)
//    {
//        rotation = new Vector3Int((rotation.x + angle.x) % 360, (rotation.y + angle.y) % 360, (rotation.z + angle.z) % 360);
//        SetVertices();
//    }
//    public void SetAngle(Vector3Int angle)
//    {
//        rotation = angle;
//        SetVertices();
//    }
//    private void SetVertices()
//    {
//        Matrix4x4 R = Matrix4x4.Rotate(Quaternion.Euler(rotation));
//        for(int i = 0; i < vertices.Count; i++)
//        {
//            vertices[i] = R.MultiplyPoint(vertices[i]);
//        }
//    }

//    public void ClearMesh()
//    {
//        vertices.Clear();
//        triangles.Clear();
//        uvs.Clear();
//        vertexIndex = 0;
//    }
//}