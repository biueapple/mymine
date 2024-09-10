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

    //모든 청크들
    private readonly Dictionary<Vector3Int, Chunk> chunks = new ();
    private Vector3Int playerChunkIndex = new ();
    //활성화된 청크들
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

    //플레이어의 위치 감지
    //본래의 위치가 아니라면 
    //주위의 청크 감지
    //만약 없는 청크가 있다면 생성
    //기존의 리스트에세 없는것 비활성화

    private void ActiveChunkFind()
    {
        Player player = GameManager.Instance.Players[0];
        //플레이어에게 해당된 청크의 인덱스를 찾기
        Vector3Int positionInt = new(Mathf.FloorToInt(player.transform.position.x), Mathf.FloorToInt(player.transform.position.y), Mathf.FloorToInt(player.transform.position.z));
        Vector3Int chunkIndex = new(positionInt.x / BlockInfo.ChunkWidth, positionInt.y / BlockInfo.ChunkHeight, positionInt.z / BlockInfo.ChunkWidth);
        //그 청크의 범위에 해당되는 범위 찾기
        List<Chunk> active = GetChunkRangeIfCreate(chunkIndex, 2);


        //청크를 새로 그릴때 경계선에 있는 이미 그려져 있는 청크의 새 청크면이 그려져 있지 않다는 문제가 발생했기에
        //새 청크를 그릴때 경계선에 있는 다른 청크들을 그려줘야 함
        //새로 활성화될 후보들의 리스트
        List<Chunk> draw = new();

        //범위를 벗어난 청크는 비활성화
        for (int j = 0; j < activeChunks.Count; j++)
        {
            if (!active.Contains(activeChunks[j]))
            {
                activeChunks[j].Deactive();
            }
        }

        //범위 안에 있는 청크들을 활성화 해야하는데 일단 후보에만 올림
        for(int i = 0; i < active.Count; i++)
        {
            if (!activeChunks.Contains(active[i]))
            {
                draw.Add(active[i]);
                //active[i].Active();
                //active[i].Draw();
            }
        }

        //후보에 올린 리스트의 6면을 확인하면서 이미 활성화된 청크가 있다면 그리고 이미 리스트에 들어가 있지 않다면 추가해주자
        //for문을 반대로 돌린 이유는 추가된 리스트는 확인하지 않기 위해
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

        //그릴 후보들을 전부 리스트에 넣었으니 그리기
        for(int i = 0; i < draw.Count; i++)
        {
            draw[i].Active();
            draw[i].Draw(); 
        }

        //청크 활성화
        activeChunks = active;
    }

    /// <summary>
    /// center를 기준으로 range만큼 떨어져있는 청크들을 리스트로 리턴해줌
    /// </summary>
    /// <param name="index">중심 청크의 인덱스 (world위치가 아님)</param>
    /// <param name="range">범위</param>
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
                //청크 내의 공기는 투명
                return true;
            }
        }

        //청크 바깥 없는 존재 불투명 (그리지 않음)
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
        //월드포지션을 청크의 인덱스로 변환하는 과정 (-좌표에선 int로 계산시 값이 잘못나오기에 float으로 계산을 해줘야 함 {[int = -2 / 16 = 0] , [float -2.0 / 16 = -1]})
        Vector3Int chunkIndex = new(Mathf.FloorToInt((float)position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)position.z / BlockInfo.ChunkWidth));

        //만약 청크가 있다면
        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            if(chunk.Create)
            {
                //월드 포지션을 청크포지션으로 바꿔줌 (-2 13 -5 => 13 13 10)
                Vector3Int Chunkposition = position - chunk.Position;

                //그 위치에 무슨 블록이 있는지
                int index = chunk.EditBlock(Chunkposition, 0, true);
                if (index > 0)
                {
                    //그 블록이 드랍하는 아이템 id는 무엇인지
                    int itemID = GameManager.Instance.GetBlock(index).ItemID;

                    //아이템id가 0은 없기에 
                    if (itemID > 0)
                    {
                        FlowManager.Instance.DropItem(position + new Vector3(0.5f, 0.5f, 0.5f), itemID, 1, Vector3.zero);
                    }
                }

                //바뀐 블록의 모든면이 다른 청크의 위치와 붙어있는지 체크
                for (int i = 0; i < BlockInfo.faceChecks.Length; i++)
                {
                    //위치 + 모든면의 위치값이 다른청크가 나올경우
                    if (chunks.TryGetValue(GetChunkIndex(position + BlockInfo.faceChecks[i]), out Chunk ex))
                    {
                        //그 청크또한 다시 그려야 함
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
        //월드포지션을 청크의 인덱스로 변환하는 과정 (-좌표에선 int로 계산시 값이 잘못나오기에 float으로 계산을 해줘야 함 {[int = -2 / 16 = 0] , [float -2.0 / 16 = -1]})
        Vector3Int chunkIndex = new(Mathf.FloorToInt((float)position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)position.z / BlockInfo.ChunkWidth));

        //만약 청크가 있다면
        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            if(chunk.Create)
            {
                //월드 포지션을 청크포지션으로 바꿔줌 (-2 13 -5 => 13 13 10)
                Vector3Int Chunkposition = position - chunk.Position;

                //그 위치에 무슨 블록이 있는지
                int index = chunk.IndexToBlockID(Chunkposition);
                if (index == 0)
                {
                    chunk.EditBlock(chunkIndex, id, true);
                }

                //바뀐 블록의 모든면이 다른 청크의 위치와 붙어있는지 체크
                for (int i = 0; i < BlockInfo.faceChecks.Length; i++)
                {
                    //위치 + 모든면의 위치값이 다른청크가 나올경우
                    if (chunks.TryGetValue(GetChunkIndex(position + BlockInfo.faceChecks[i]), out Chunk ex))
                    {
                        //그 청크또한 다시 그려야 함
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
        //월드포지션을 청크의 인덱스로 변환하는 과정 (-좌표에선 int로 계산시 값이 잘못나오기에 float으로 계산을 해줘야 함 {[int = -2 / 16 = 0] , [float -2.0 / 16 = -1]})
        Vector3Int chunkIndex = new(Mathf.FloorToInt((float)position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)position.z / BlockInfo.ChunkWidth));

        //만약 청크가 있다면
        if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
        {
            //월드 포지션을 청크포지션으로 바꿔줌 (-2 13 -5 => 13 13 10)
            Vector3Int Chunkposition = position - chunk.Position;

            if (chunk.Create)
            {
                //그 위치에 무슨 블록이 있는지
                chunk.EditBlock(Chunkposition, id, true);

                //바뀐 블록의 모든면이 다른 청크의 위치와 붙어있는지 체크
                for (int i = 0; i < BlockInfo.faceChecks.Length; i++)
                {
                    //위치 + 모든면의 위치값이 다른청크가 나올경우
                    if (chunks.TryGetValue(GetChunkIndex(position + BlockInfo.faceChecks[i]), out Chunk ex))
                    {
                        //그 청크또한 다시 그려야 함
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
            //여기로 올때 바이옴이 orders의 local값을 world로 바꿔준 후에 보내줌

            //월드포지션을 청크의 인덱스로 변환하는 과정 (-좌표에선 int로 계산시 값이 잘못나오기에 float으로 계산을 해줘야 함 {[int = -2 / 16 = 0] , [float -2.0 / 16 = -1]})
            Vector3Int chunkIndex = new(Mathf.FloorToInt((float)orders[i].local.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)orders[i].local.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)orders[i].local.z / BlockInfo.ChunkWidth));

            //월드 포지션을 청크포지션으로 바꿔줌 (-2 13 -5 => 13 13 10)
            Vector3Int Chunkposition;

            //만약 청크가 있다면
            if (chunks.TryGetValue(chunkIndex, out Chunk chunk))
            {
                //월드 포지션을 청크포지션으로 바꿔줌 (-2 13 -5 => 13 13 10)
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

                //월드 포지션을 청크포지션으로 바꿔줌 (-2 13 -5 => 13 13 10)
                Chunkposition = orders[i].local - c.Position;

                c.wait.Add((Chunkposition, orders[i].type));
            }
        }
    }

    //raycast를 대신할 함수 (collider를 안써서 기존의 raycast를 사용하지 못함)
    public BlockLaycast WorldRaycast(Vector3 postion, Vector3 dir, float distance, float checkIncrement = 0.1f)
    {
        BlockLaycast lay;
        //step은 checkIncrement씩 늘어나면서 그곳에 블록이 있는지 확인해야함
        float step = checkIncrement;
        //마지막으로 확인해본 블록위치
        Vector3 lastPos = postion;

        //플레이어의 사거리를 벋어나지 않는 한
        while (step < distance)
        {
            //카메라부터 step만큼 정면으로 떨어진 거리에
            Vector3 pos = postion + (dir * step);

            //lastPos == pos 인경우 continue 할려 했는데 lastPos는 Vector3Int 고 pos 는 Vector3라서 비교가 안돼네

            //블록이 있는가?
            Block scriptable = WorldPositionToBlock(pos);
            if (scriptable != null)
            {
                //있다면 그 블록의 위치가 파괴될 블록의 위치고
                lay = new()
                {
                    position = pos,
                    positionToInt = new(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)),
                    lastPosition = lastPos,
                    lastPositionToInt = new(Mathf.FloorToInt(lastPos.x), Mathf.FloorToInt(lastPos.y), Mathf.FloorToInt(lastPos.z)),
                    block = scriptable
                };
                //그 전에 마지막으로 확인했던 블록의 위치가 블록이 설치될 위치
                return lay;
            }
            //확인한 위치를 마지막 위치에 넣어놓기
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

////블록마다 자신을 구성하는 vertices와 triangles을 가지고 있어야 충돌처리가 빨라짐 (PointInPolyhedron 사용 시)
////각도와 그에 따른 vertices와 triangles를 가지고 있어야 함
////어느 면에 투명한지(그쪽 다른 블록의 면을 그려야 하는지)에 대한 정보도 가지고 있어야 함 
////자신의 다른 블록에 어떤 영향을 끼치고 다른 블록에게 영향을 받을지에 대한 정보도 가지고 있어야 함 (계단의 머리부분은 다른 부분과 상관없이 그려야 함, 반블록의 윗부분은 윗블록과 상관없이 그려야 함)
//public class WorldBlockInfomation
//{
//    private byte blockID;
//    public byte BlockID => blockID;
//    private Vector3Int rotation;
//    public Vector3Int Rotation => rotation;
//    //청크단위
//    //청크를 그릴때 사용하는 정보들
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