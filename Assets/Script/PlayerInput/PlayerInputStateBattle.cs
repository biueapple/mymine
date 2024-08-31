using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputStateBattle : IPlayerInputSystem
{
    public IPlayerInputSystem Mode { get => this; set { player.PlayerInput = value; } }
    private readonly Player player;
    private readonly EquipmentSystem equipmentSystem;
    private AttackModule attackModule;
    public AttackModule AttackModule { get => attackModule; }

    private AttackMotionTree attackMotionTree;
    public AttackMotionTree AttackMotionTree { get  => attackMotionTree; }

    private AttackMotionInterface attackmotionInterface;
    public AttackMotionInterface AttackMotionInterace { get { return attackmotionInterface; } }

    public PlayerInputStateBattle(Player player, EquipmentSystem equipmentSystem, AttackMotionInterface attackMotionInterface)
    {
        this.player = player;
        this.equipmentSystem = equipmentSystem;
        if (equipmentSystem.weapon != null)
        {
            Debug.Log(equipmentSystem.weapon);
            attackModule = new AttackModule(equipmentSystem.weapon.MotionCount, player.STAT);
            EquipToMotion(equipmentSystem.weaponSlot);
        }
        equipmentSystem.weaponSlot.AfterUpdate += EquipToMotion;

        attackMotionTree = new AttackMotionTree(player);
        attackmotionInterface = attackMotionInterface;
        attackmotionInterface.Init(attackMotionTree);
    }

    public void LeftClick()
    {
        
    }

    public void LeftDown()
    {
        if(UIManager.Instance.ActiveUI.Count == 0)
        {
            attackModule?.Attack();
        }
    }

    public void LeftUp()
    {

    }

    public void RightClick()
    {

    }

    public void RightDown()
    {
        //오른손 공격? 왼손공격? 스킬?
    }

    public void RightUp()
    {

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(attackmotionInterface.gameObject.activeSelf)
            {
                UIManager.Instance.CloseUI(attackmotionInterface.gameObject);
                SlotManager.Instance.MotionCatchState.Another();
            }
            else
            {
                UIManager.Instance.OpenUI(attackmotionInterface.gameObject);
            }
        }
    }


    //장착한 무기가 바뀌면 모션이 바꿔야 하니까 (weaponitem에 callback으로 넣어줄 용도)
    private void EquipToMotion(ItemSlot slot)
    {
        if (slot.Item != null)
        {
            if(slot.Item is IWeapon weapon)
            {
                attackModule = new AttackModule(weapon.MotionCount, player.STAT);
                for(int i = 0; i < equipmentSystem.AttackMotionSlots.Length; i++)
                {
                    if (i < weapon.MotionCount)
                        equipmentSystem.AttackMotionSlots[i].gameObject.SetActive(true);
                    else
                        equipmentSystem.AttackMotionSlots[i].gameObject.SetActive(false);
                }
                
                return;
            }
        }
        for (int i = 0; i < equipmentSystem.AttackMotionSlots.Length; i++)
        {
            equipmentSystem.AttackMotionSlots[i].gameObject.SetActive(false);
        }
        attackModule = null;
    }
}
