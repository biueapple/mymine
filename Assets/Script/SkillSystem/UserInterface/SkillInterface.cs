using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInterface : MonoBehaviour
{
    protected SkillSlotUI[] skillSlots;
    public SkillSlotUI[] SkillSlots { get { return skillSlots; } }

    //ui의 시작지점
    [SerializeField]
    protected int X_START;
    [SerializeField]
    protected int Y_START;
    //ui간의 얼마만큼 떨어져 있는지 한줄에 몇개가 들어가는지
    [SerializeField]
    protected int X_SPACE_BETWWEN_ITEM;
    [SerializeField]
    protected int NUMBER_OF_COLUMN;
    [SerializeField]
    protected int Y_SPACE_BETWWEN_ITEMS;

    protected Transform highlight;

    protected int select;
    public int Select { get { return select; } 
        set
        {
            select = value;
            if (select < 0)
                select = skillSlots.Length - 1;
            else if(select >= skillSlots.Length)
                select = 0;
        }
    }
    public SkillSlotUI SelectSlot { get { return skillSlots[select]; } }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                select--;
            }
            else
            {
                select++;
            }

            if (select < 0)
                select = skillSlots.Length - 1;
            else if (select >= skillSlots.Length)
                select = 0;

            highlight.transform.localPosition = new Vector3(X_START + X_SPACE_BETWWEN_ITEM * select, 0, 0);
        }
    }

    public void CreateSlot(int length)
    {
        highlight = transform.GetChild(0);
        SkillSlotUI slot = Resources.Load<SkillSlotUI>("UI/Slot/SkillSlotUI");
        skillSlots = new SkillSlotUI[length];
        for (int i = 0; i < length; i++)
        {
            //만들고
            skillSlots[i] = Instantiate(slot, transform);
            //위치정하고
            skillSlots[i].GetComponent<RectTransform>().localPosition = GetPosition(i);
            //초기화
            skillSlots[i].Init();
        }
    }

    //ui포지션을 계산해줌
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWWEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWWEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0);
    }
}
