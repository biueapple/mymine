using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField]
    private Storage equipStorage;
    public Storage EquipStorage { get {return equipStorage; }  }
    [SerializeField]
    private StaticInterface staticInterface;
    public StaticInterface StaticInterface { get { return staticInterface; } }

    private Player player;
    private Stat stat;

    //변경가능
    public ItemSlot WeaponSlot { get { return equipStorage.Slots[4]; } }
    public IWeapon Weapon { get { if (equipStorage.Slots[4].Item == null) return null; return equipStorage.Slots[4].Item as IWeapon; } }
    public Item WeaponItem { get { return equipStorage.Slots[4].Item; } }

    [SerializeField]
    private AttackMotionSlot[] attackMotionSlots;
    public AttackMotionSlot[] AttackMotionSlots { get { return attackMotionSlots; } }

    private void Awake()
    {
        EquipItemSlot[] equipItemSlots = new EquipItemSlot[staticInterface.Slots.Length];
        for(int i = 0; i < equipItemSlots.Length; i++)
        {
            if(staticInterface.Slots[i] is EquipItemSlotUI equipItemSlotUI)
                equipItemSlots[i] = new EquipItemSlot(equipItemSlotUI.Parts, equipItemSlotUI.Input, equipItemSlotUI.Output);
        }
        equipStorage = new Storage(equipItemSlots);

        staticInterface.Interlock(equipStorage);

        player = GetComponent<Player>();
        stat = player.STAT;

        for(int i = 0; i < staticInterface.Slots.Length; i++) 
        {
            if (staticInterface.Slots[i].ItemSlot is EquipItemSlot equip)
            {
                equip.AfterEquip += WearEqiup;
                equip.BeforeEquip += OffEquip;
            }
        }

        for(int i = 0; i < attackMotionSlots.Length; i++)
        {
            attackMotionSlots[i].Init(player, i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(player.PlayerSetting.equip))
        {
            if(staticInterface.gameObject.activeSelf)
            {
                UIManager.Instance.CloseUI(staticInterface.gameObject);
                SlotManager.Instance.ItemCatchState.Another();
            }
            else
            {
                UIManager.Instance.OpenUI(staticInterface.gameObject);
            }
        }
    }

    private void WearEqiup(EquipItemSlot equipItemSlot)
    {
        if(equipItemSlot.Item != null)
        {
            if (equipItemSlot.Item is IEquipment equipment)
            {
                for (int i = 0; i < equipment.AttributePieces.Length; i++)
                {
                    stat.AddStat(equipment.AttributePieces[i]);
                }
            }
        }  
    }
    private void OffEquip(EquipItemSlot equipItemSlot)
    {
        if (equipItemSlot.Item != null)
        {
            if (equipItemSlot.Item is IEquipment equipment)
            {
                for (int i = 0; i < equipment.AttributePieces.Length; i++)
                {
                    stat.TakeStat(equipment.AttributePieces[i]);
                }
            }
        }
    }
}
