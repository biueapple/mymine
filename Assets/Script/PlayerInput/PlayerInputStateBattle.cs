using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInputStateBattle : IPlayerInputSystem
{
    public IPlayerInputSystem Mode { get => this; 
        //set { player.PlayerInput = value; }
    }
    private readonly Player player;
    private readonly EquipmentSystem equipmentSystem;
    private AttackModule attackModule;
    public AttackModule AttackModule { get => attackModule; }

    private readonly AttackMotionTree attackMotionTree;
    public AttackMotionTree AttackMotionTree { get  => attackMotionTree; }

    private readonly AttackMotionInterface attackmotionInterface;
    public AttackMotionInterface AttackMotionInterace { get { return attackmotionInterface; } }

    private readonly Rect rect;

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

        rect.width = Screen.width;
        rect.height = Screen.height;

        sight = new Sight(player.Camera, rect);
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

    readonly Sight sight;
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

        //적을 찾아서 화면에 표시해주기
        sight.SightOn();
        //현재 보이는 것들중에서 적만 가지고
        List<Enemy> enemyList = new ();
        for(int i = 0; i < sight.Values.Count; i++)
        {
            if (sight.Values[i].enemy.Unit is Enemy enemy)
            {
                if(!enemyList.Contains(enemy))
                    enemyList.Add(enemy);
            }
        }

        //새 리스트에 포함되지 않은 녀석은 화면 밖으로 나간 녀석
        for(int i = MonsterInformation.Instance.List.Count - 1; i >= 0; i--)
        {
            if (!enemyList.Contains(MonsterInformation.Instance.List[i].enemy))
            {
                MonsterInformation.Instance.CloseMonsterInfo(MonsterInformation.Instance.List[i].enemy);
            }
        }
        //이미 보여주고 있는 리스트에 없다면 화면 안으로 들어온 녀석
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!MonsterInformation.Instance.List.Select(x => x.enemy).Contains(enemyList[i]))
            {
                MonsterInformation.Instance.OpenMonsterInfo(enemyList[i]);
            }
        }
        for(int i = 0; i < MonsterInformation.Instance.List.Count; i++)
        {
            MonsterInformation.Instance.List[i].rect.position = player.Camera.WorldToScreenPoint(MonsterInformation.Instance.List[i].enemy.transform.position + new Vector3(0, MonsterInformation.Instance.List[i].enemy.Height + 0.1f, 0));
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

    public void Enter()
    {

    }

    public void Exit()
    {
        //만들어진 ui들 삭제
        MonsterInformation.Instance.CloseMonsterInfo();
    }
}
