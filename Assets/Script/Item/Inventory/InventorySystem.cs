using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private Storage inventoryStorage;
    [SerializeField]
    private Storage hotkeyStorage;
    [SerializeField]
    private DynamicInterface inventory;
    public DynamicInterface Inventory { get { return inventory; } }
    [SerializeField]
    private HotkeyInterface hotkey;
    public HotkeyInterface HotkeyInterface { get { return hotkey; } }
    [SerializeField]
    private KeyCode _keyCode = KeyCode.I;
    public KeyCode KeyCode { get { return _keyCode; } set { _keyCode = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
        inventoryStorage = new Storage(20, true, true);
        hotkeyStorage = new Storage(9, true, true);
        //hotkey.ModelPosition(player.Hand);
        hotkey.Interlock(hotkeyStorage);
        inventory.Interlock(inventoryStorage);
        //_storage.SavePath = "/inventory";
        //_storage.Load();
        hotkey.Choice(0);

        int amount = inventoryStorage.Acquire(GameManager.Instance.GetItem(6), 1);
        if (amount == 0)
        {
            Debug.Log("아이템 생성");
        }
        else
        {
            Debug.Log(amount + "만큼 아이템 생성 실패");
        }

        amount = inventoryStorage.Acquire(GameManager.Instance.GetItem(1), 10);
        if (amount == 0)
        {
            Debug.Log("아이템 생성");
        }
        else
        {
            Debug.Log(amount + "만큼 아이템 생성 실패");
        }

        amount = inventoryStorage.Acquire(GameManager.Instance.GetItem(2), 10);
        if (amount == 0)
        {
            Debug.Log("아이템 생성");
        }
        else
        {
            Debug.Log(amount + "만큼 아이템 생성 실패");
        }

        amount = inventoryStorage.Acquire(GameManager.Instance.GetItem(7), 1);
        if (amount == 0)
        {
            Debug.Log("아이템 생성");
        }
        else
        {
            Debug.Log(amount + "만큼 아이템 생성 실패");
        }

        amount = inventoryStorage.Acquire(GameManager.Instance.GetItem(11), 10);
        if (amount == 0)
        {
            Debug.Log("아이템 생성");
        }
        else
        {
            Debug.Log(amount + "만큼 아이템 생성 실패");
        }

        amount = inventoryStorage.Acquire(GameManager.Instance.GetItem(12), 1);
        if (amount == 0)
        {
            Debug.Log("아이템 생성");
        }
        else
        {
            Debug.Log(amount + "만큼 아이템 생성 실패");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            //클릭중인 아이템을 처리해야함 던지던가
            if (!inventory.gameObject.activeSelf)
            {
                UIManager.Instance.OpenUI(inventory.gameObject);
            }
            else
            {
                UIManager.Instance.CloseUI(inventory.gameObject);
                SlotManager.Instance.ItemCatchState.Another();
            }
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                hotkey.Change(-1);
            }
            else
            {
                hotkey.Change(1);
            }
        }
    }

    public int Acquisition(ItemSculpture itemSculpture)
    {
        return inventoryStorage.Acquire(itemSculpture.Item, itemSculpture.Amount);
    }

    public void Collecting(ItemSlot criteria)
    {
        if (criteria == null || criteria.Item == null)
            return;

        for (int i = 0; i < inventoryStorage.Slots.Length; i++)
        {
            if (inventoryStorage.Slots[i].Item?.ItemID == criteria.Item.ItemID)
            {
                criteria.Update(criteria.Item, criteria.Amount + inventoryStorage.Slots[i].Amount);
                inventoryStorage.Slots[i].Update(null, 0);
            }
        }
        for (int i = 0; i < hotkeyStorage.Slots.Length; i++)
        {
            if (hotkeyStorage.Slots[i].Item?.ItemID == criteria.Item.ItemID)
            {
                criteria.Update(criteria.Item, criteria.Amount + hotkeyStorage.Slots[i].Amount);
                hotkeyStorage.Slots[i].Update(null, 0);
            }
        }
    }

    //test
    public void Throw(int id, int amount)
    {
        FlowManager.Instance.DropItem(transform.position + transform.forward + Vector3.up * 1.5f, id, amount, transform.forward);
    }

    private void OnApplicationQuit()
    {
        //_storage.Save();
    }
}
