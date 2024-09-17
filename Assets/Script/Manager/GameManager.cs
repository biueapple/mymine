using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    { 
        get
        { 
            if (instance == null)
            {
                instance = new GameObject ("GameManager").AddComponent<GameManager>();
            }
            return instance; 
        }
    }

    public enum ITEM_ENUM
    {
        Bone = 1,
        Fish,
        Stone,
        Sword,
        Dirt,
        Workbench,
        WoodShovels,
        Wood,
        Plank,
        WoodenStick,
        Coal,
    }

    public enum BLCOK_ENUM
    {
        Stone = 1,
        Dirt,
        Bedrock,
        Workbench,
        ArableDirt,
        Grass,
        Wood,
        Leaf,
        Plank,
        CoalOre,

    }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        AddItem(new Item_Bone(1));
        AddItem(new Item_Fish(2));
        AddItem(new Item_Stone(3));
        AddItem(new Item_Sword(4));
        AddItem(new Item_Dirt(5));
        AddItem(new Item_Workbench(6));
        AddItem(new Item_WoodShovels(7));
        AddItem(new Item_Wood(8));
        AddItem(new Item_Plank(9));
        AddItem(new Item_WoodenStick(10));
        AddItem(new Item_Coal(11));
        AddItem(new Item_Furnace(12));

        AddBlock(new Stone(1));
        AddBlock(new Dirt(2));
        AddBlock(new Bedrock(3));
        AddBlock(new Workbench(4));
        AddBlock(new ArableDirt(5));
        AddBlock(new Grass(6));
        AddBlock(new Wood(7));
        AddBlock(new leaf(8));
        AddBlock(new Plank(9));
        AddBlock(new CoalOre(10));
        AddBlock(new Furnace(11));

        DontDestroyOnLoad(instance);
    }

    public Unit[] Units
    {
        get
        {
            return Unit.AllUnits.ToArray();
        }
    }

    public Player[] Players
    {
        get
        {
            return Units.OfType<Player>().ToArray();
        }
    }

    public Substance[] Substances
    {
        get
        {
            return FindObjectsOfType<Substance>();
        }
    }

    private readonly Dictionary<int, Item> allItem = new ();
    public Item GetItem(int id)
    {
        allItem.TryGetValue(id, out Item item);
        return item;
    }
    private void AddItem(Item item)
    {
        allItem.Add(item.ItemID, item);
    }

    private readonly Dictionary<int, Block> allBlock = new ();
    public IMineral[] Minerals { 
        get 
        {
            return allBlock.Values.OfType<IMineral>().ToArray();
        } 
    }
    public Block GetBlock(int id)
    {
        allBlock.TryGetValue(id, out Block block);
        return block;
    }
    private void AddBlock(Block block)
    {
        allBlock.Add(block.ID, block);
    }

    public (Vector3Int[], int) Mineral(int y, int yHeight)
    {
        IMineral[] minerals = Minerals.Where(s => s.Possibility(y, yHeight)).ToArray();
        float pro = Random.Range(0f, 1f);
        for(int i = 0; i < minerals.Length; i++)
        {
            if (minerals[i].Probability >= pro)
            {
                Vector3Int[] vectors = minerals[i].ShapeDir(new Vector3Int());
                if (minerals[i] is Block b)
                {
                    return (vectors, b.ID);
                }
            }
            pro -= minerals[i].Probability;
        }
        return (null, 0);
    }

    //

    public void Initialization()
    {
        //�� ����
        SceneManager.LoadScene(1);
        //�÷��̾� ����

        //���� ����
        //���尡 �÷��̾� ���� (��� ���� �����ϸ� ���� ������)
    }

    //

    /// <summary>
    /// ������ � ��ġ�� �� �� �ִ���
    /// </summary>
    /// <param name="unit">����</param>
    /// <param name="position">��ġ</param>
    /// <returns></returns>
    public bool Empty(Unit unit, Vector3 position)
    {
        //���� �Ÿ�
        float distanceX = unit.Width * 2;
        //���� �߰��Ǵ� ��
        float width = -unit.Width;
        //���� �󸶳� �߰������
        float measureX;

        //x��
        while (true)
        {
            float distanceY = unit.Height;
            float height = 0;
            float measureY;
            //y��
            while (true)
            {
                float distanceZ = unit.Depth * 2;
                float depth = -unit.Depth;
                float measureZ;
                //z��
                while (true)
                {
                    //�浹���
                    if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y + height, position.z + depth)))
                    {
                        return false;
                    }
                    //���� �Ÿ��� ���ٸ�
                    if (distanceZ <= 0)
                    {
                        break;
                    }
                    //���� �Ÿ� ���̱�
                    measureZ = Mathf.Min(1, distanceZ);
                    depth += measureZ;
                    distanceZ -= measureZ;
                }

                if (distanceY <= 0)
                {
                    break;
                }

                measureY = Mathf.Min(1, distanceY);
                height += measureY;
                distanceY -= measureY;
            }

            if (distanceX <= 0)
            {
                break;
            }

            measureX = Mathf.Min(1, distanceX);
            width += measureX;
            distanceX -= measureX;
        }
        return true;
    }


    //
    public Vector3Int Vector3Int(Vector3 vector)
    {
        return new Vector3Int(Mathf.FloorToInt(vector.x + 0.5f), (int)vector.y, Mathf.FloorToInt(vector.z + 0.5f));
    }
    public Vector3Int ChunkIndex(Vector3 position)
    {
        return new(Mathf.FloorToInt((float)position.x / BlockInfo.ChunkWidth), Mathf.FloorToInt((float)position.y / BlockInfo.ChunkHeight), Mathf.FloorToInt((float)position.z / BlockInfo.ChunkWidth));
    }
    public float NormalizeAngle(float angle)
    {
        angle %= 360; // ������ 360�� ���� ���� ����

        if (angle < -180)
        {
            angle += 360; // -180�� �̸��� ���, 180���� ���� 180�� ���� ���� �̵�
        }
        else if (angle > 180)
        {
            angle -= 360; // 180�� �ʰ��� ���, 180���� �� 180�� ���� ���� �̵�
        }

        return angle;
    }
}
