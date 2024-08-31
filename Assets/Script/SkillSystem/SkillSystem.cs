using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private SkillInterface skillInterface;
    public SkillInterface SkillInterface { get { return skillInterface; } }
    private Player player;

    [SerializeField]
    WarriorTreeInterface warriorTree;

    WarriorTree tree;
    public WarriorTree WarriorTree { get => tree; set { } }

    [SerializeField]
    protected int skillPoint;
    public int SkillPoint { get { return skillPoint; } set { skillPoint = value; } }
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        skillInterface.CreateSlot(9);

        tree = new WarriorTree(player);
        warriorTree.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.PlayerInput == player.StateBattle)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(skillInterface.SkillSlots[0].AntecedentSkill != null)
                {
                    Debug.Log("1번째 스킬 발동");
                    skillInterface.SkillSlots[0].Use();
                }
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (skillInterface.SkillSlots[1].AntecedentSkill != null)
                {
                    Debug.Log("2번째 스킬 발동");
                    skillInterface.SkillSlots[1].Use();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (skillInterface.SkillSlots[2].AntecedentSkill != null)
                {
                    Debug.Log("3번째 스킬 발동");
                    skillInterface.SkillSlots[2].Use();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (skillInterface.SkillSlots[3].AntecedentSkill != null)
                {
                    Debug.Log("4번째 스킬 발동");
                    skillInterface.SkillSlots[3].Use();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (skillInterface.SkillSlots[4].AntecedentSkill != null)
                {
                    Debug.Log("5번째 스킬 발동");
                    skillInterface.SkillSlots[4].Use();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (skillInterface.SkillSlots[5].AntecedentSkill != null)
                {
                    Debug.Log("6번째 스킬 발동");
                    skillInterface.SkillSlots[5].Use();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                if (skillInterface.SkillSlots[6].AntecedentSkill != null)
                {
                    Debug.Log("7번째 스킬 발동");
                    skillInterface.SkillSlots[6].Use();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                if (skillInterface.SkillSlots[7].AntecedentSkill != null)
                {
                    Debug.Log("8번째 스킬 발동");
                    skillInterface.SkillSlots[7].Use();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                if (skillInterface.SkillSlots[8].AntecedentSkill != null)
                {
                    Debug.Log("9번째 스킬 발동");
                    skillInterface.SkillSlots[8].Use();
                }
            }
            else if(Input.GetMouseButtonDown(2))
            {
                if(skillInterface.SelectSlot.AntecedentSkill != null)
                {
                    Debug.Log("선택중인 스킬 발동");
                    skillInterface.SelectSlot.Use();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            if(warriorTree.gameObject.activeSelf)
            {
                UIManager.Instance.CloseUI(warriorTree.gameObject);
            }
            else
            {
                UIManager.Instance.OpenUI(warriorTree.gameObject);
            }
        }
    }
}
