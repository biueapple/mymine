using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputStateNomal : IPlayerInputSystem
{
    public IPlayerInputSystem Mode { get => this; }

    private readonly Player player;
    private World.BlockLaycast blockLaycast;
    //인벤토리 가지고 있어야 함
    private readonly InventorySystem inventorySystem;
    private float broken = 1;
    private readonly GameObject brokenPre;

    public PlayerInputStateNomal(Player player, InventorySystem inventorySystem)
    {
        this.player = player;
        this.inventorySystem = inventorySystem;
        brokenPre = GameObject.Instantiate(Resources.Load<GameObject>("World/Broken"));
        brokenPre.SetActive(false);
    }

    public void LeftClick()
    {
        //블록 체크
        World.BlockLaycast laycast = World.Instance.WorldRaycast(player.Camera.transform.position, player.Camera.transform.forward, 8);
        if (laycast != null)
        {
            if (laycast.Equals(blockLaycast))
            {
                broken -= blockLaycast.block.HardnessFigure(inventorySystem.HotkeyInterface.SelectSlot.Item);
                brokenPre.SetActive(true);
                brokenPre.transform.position = blockLaycast.positionToInt + new Vector3(0.5f, 0.5f, 0.5f);
                brokenPre.GetComponent<Renderer>().material.SetFloat("_clip", broken);
                //블록 파괴
                if (broken <= 0)
                {
                    World.Instance.WorldPostionBroken(blockLaycast.positionToInt);
                }
                return;
            }
        }
        blockLaycast = laycast;
        broken = 1;
        brokenPre.SetActive(false);
    }

    public void RightClick() { }
    public void LeftDown() { }
    public void LeftUp()
    {
        broken = 1;
        brokenPre.SetActive(false);
    }
    public void RightDown()
    {
        //ui가 켜져있다면 입력을 받으면 안됨
        if(UIManager.Instance.ActiveUI.Count == 0)
        {
            World.BlockLaycast laycast = World.Instance.WorldRaycast(player.Camera.transform.position, player.Camera.transform.forward, 8);

            if (laycast != null)
            {
                if (laycast.block is IBlockOpenUI openUI)
                {
                    openUI.OpenUI(player, laycast);
                }
                else
                {
                    if(laycast.block is IBlockActive active)
                    {
                        active.Active(inventorySystem.HotkeyInterface.SelectSlot.Item, laycast);
                        inventorySystem.HotkeyInterface.SelectSlot.AfterUpdate?.Invoke(inventorySystem.HotkeyInterface.SelectSlot);
                    }
                    if (inventorySystem.HotkeyInterface.SelectSlot.Item is IInstall install)
                    {
                        if (World.Instance.WorldPositionInstall(laycast.lastPositionToInt, install.BlockID))
                        {
                            inventorySystem.HotkeyInterface.SelectSlot.Update(inventorySystem.HotkeyInterface.SelectSlot.Item, inventorySystem.HotkeyInterface.SelectSlot.Amount - 1);
                        }
                    }
                }
            }
        }
    }
    public void RightUp() { }
    
    public void Update()
    {
        
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        broken = 1;
        brokenPre.SetActive(false);
    }
}
