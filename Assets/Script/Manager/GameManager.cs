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
    public Block GetBlock(int id)
    {
        allBlock.TryGetValue(id, out Block block);
        return block;
    }
    private void AddBlock(Block block)
    {
        allBlock.Add(block.ID, block);
    }

    //

    public void Initialization()
    {
        //씬 변경
        SceneManager.LoadScene(1);
        //플레이어 생성

        //월드 생성
        //월드가 플레이어 감지 (없어도 월드 생성하면 감지 시작함)
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
        angle %= 360; // 각도를 360도 범위 내로 줄임

        if (angle < -180)
        {
            angle += 360; // -180도 미만일 경우, 180도를 더해 180도 범위 내로 이동
        }
        else if (angle > 180)
        {
            angle -= 360; // 180도 초과일 경우, 180도를 빼 180도 범위 내로 이동
        }

        return angle;
    }
}
