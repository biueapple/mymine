using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Explanation
{
    private readonly Transform parent;
    private RectTransform overview;
    private RectTransform description;
    private RectTransform[] attribute;
    private RectTransform consumeEx;

    public Explanation()
    {
        parent = new GameObject("ExplanationParent").transform;
        parent.parent = UIManager.Instance.Canvas.transform;

        OffExplanSlotUI();
    }

    public void OffExplanSlotUI()
    {
        if (overview != null)
            ObjectPooling.Instance.DestroyObject(overview.gameObject);
        if (description != null)
            ObjectPooling.Instance.DestroyObject(description.gameObject);
        if (attribute != null)
        {
            for (int i = 0; i < attribute.Length; i++)
            {
                ObjectPooling.Instance.DestroyObject(attribute[i].gameObject);
            }
        }
        if (consumeEx != null)
            ObjectPooling.Instance.DestroyObject(consumeEx.gameObject);
    }

    public void ViewItemSlotUI(ItemSlotUI itemSlotUI)
    {
        parent.transform.position = itemSlotUI.transform.position;
        float height = 0;

        overview = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Overview"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        overview.localPosition = Vector3.zero;
        Text itemName = overview.GetChild(0).GetComponent<Text>();
        Text itemRating = overview.GetChild(1).GetComponent<Text>();
        Text itemAmount = overview.GetChild(2).GetComponent<Text>();

        itemName.text = itemSlotUI.ItemSlot.Item.Name;
        itemRating.text = itemSlotUI.ItemSlot.Item.Item_Rating switch
        {
            Item_Rating.COMMON => "<color=silver>",
            Item_Rating.UNCOMMON => "<color=black>",
            Item_Rating.RARE => "<color=purple>",
            Item_Rating.EPIC => "<color=magenta>",
            Item_Rating.UNIQUE => "<color=orange>",
            Item_Rating.LEGEND => "<color=red>",
            _ => "<color=blue>", // 기본값
        };
        itemRating.text += itemSlotUI.ItemSlot.Item.Item_Rating.ToString() + "</color>";

        itemAmount.text = itemSlotUI.ItemSlot.Amount.ToString();

        height -= overview.sizeDelta.y * 0.5f;

        //여기에 장비아이템의 경우 추가되는 항목 있음
        if (itemSlotUI.ItemSlot.Item is IEquipment equipment)
        {
            attribute = new RectTransform[equipment.AttributePieces.Length];
            Text attributeText;
            for (int i = 0; i < equipment.AttributePieces.Length;  i++)
            {
                attribute[i] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
                attributeText = attribute[i].GetChild(0).GetComponent<Text>();
                attributeText.text = equipment.AttributePieces[i].Property.ToString() + " : " + equipment.AttributePieces[i].Value.ToString();
                height -= attribute[i].sizeDelta.y * 0.5f;
                attribute[i].localPosition = new Vector3(0, height, 0);
                height -= attribute[i].sizeDelta.y * 0.5f;
            }
        }

        if(itemSlotUI.ItemSlot.Item is IConsume consume)
        {
            consumeEx = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/ConsumeEx"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
            Text text = consumeEx.GetChild(0).GetComponent<Text>();
            text.text = consume.MaxDurability + " / " + consume.Durability;
            height -= consumeEx.sizeDelta.y * 0.5f;
            consumeEx.localPosition = new Vector3(0, height, 0);
            height -= consumeEx.sizeDelta.y * 0.5f;
        }

        description = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Description"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        Text itemDescription = description.GetChild(0).GetComponent<Text>();

        itemDescription.text = itemSlotUI.ItemSlot.Item.Description;
        description.sizeDelta = new Vector2(description.sizeDelta.x, itemDescription.preferredHeight);
        itemDescription.rectTransform.sizeDelta = new Vector2(description.sizeDelta.x, itemDescription.preferredHeight);

        height -= itemDescription.preferredHeight * 0.5f;

        description.localPosition = new Vector3(0, height, 0);
    }

    public void ViewSkillSlotUI(SkillSlotUI slotUI, bool detail)
    {
        parent.transform.position = slotUI.transform.position;
        float height = 0;

        overview = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Overview"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        overview.localPosition = Vector3.zero;
        Text itemName = overview.GetChild(0).GetComponent<Text>();
        Text itemRating = overview.GetChild(1).GetComponent<Text>();
        Text itemAmount = overview.GetChild(2).GetComponent<Text>();

        itemName.text = slotUI.AntecedentSkill.Skill.Name;
        itemRating.text = "";

        itemAmount.text = slotUI.AntecedentSkill.Skill.Level + " / " + slotUI.AntecedentSkill.Skill.MaxLevel;

        height -= overview.sizeDelta.y * 0.5f;


        //여기에 액티브 스킬 추가되는 항목

        //패시브 항목

        //스킬을 배우기 위한 조건
        attribute = new RectTransform[slotUI.AntecedentSkill.Antecedents.Count];
        Text attributeText;
        for (int i = 0; i < slotUI.AntecedentSkill.Antecedents.Count; i++)
        {
            attribute[i] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
            attributeText = attribute[i].GetChild(0).GetComponent<Text>();
            attributeText.text = slotUI.AntecedentSkill.Antecedents[i].Skill.Name + " " + slotUI.AntecedentSkill.Antecedents[i].Level + " 필요";
            height -= attribute[i].sizeDelta.y * 0.5f;
            attribute[i].localPosition = new Vector3(0, height, 0);
            height -= attribute[i].sizeDelta.y * 0.5f;
        }

        //설명
        description = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Description"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        Text itemDescription = description.GetChild(0).GetComponent<Text>();

        itemDescription.text = detail ? slotUI.AntecedentSkill.Skill.DetailDescription : slotUI.AntecedentSkill.Skill.Description;
        description.sizeDelta = new Vector2(description.sizeDelta.x, itemDescription.preferredHeight);
        itemDescription.rectTransform.sizeDelta = new Vector2(description.sizeDelta.x, itemDescription.preferredHeight);

        height -= itemDescription.preferredHeight * 0.5f;

        description.localPosition = new Vector3(0, height, 0);
    }

    public void ViewSkillSlotUI(SkillView slotUI, bool detail)
    {
        parent.transform.position = slotUI.transform.position;
        float height = 0;

        overview = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Overview"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        overview.localPosition = Vector3.zero;
        Text itemName = overview.GetChild(0).GetComponent<Text>();
        Text itemRating = overview.GetChild(1).GetComponent<Text>();
        Text itemAmount = overview.GetChild(2).GetComponent<Text>();

        itemName.text = slotUI.AntecedentSkill.Skill.Name;
        itemRating.text = "";

        itemAmount.text = slotUI.AntecedentSkill.Skill.Level + " / " + slotUI.AntecedentSkill.Skill.MaxLevel;

        height -= overview.sizeDelta.y * 0.5f;

        //여기에 액티브 스킬 추가되는 항목

        //패시브 항목

        //스킬을 배우기 위한 조건
        attribute = new RectTransform[slotUI.AntecedentSkill.Antecedents.Count];
        Text attributeText;
        for (int i = 0; i < slotUI.AntecedentSkill.Antecedents.Count; i++)
        {
            attribute[i] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
            attributeText = attribute[i].GetChild(0).GetComponent<Text>();
            attributeText.text = slotUI.AntecedentSkill.Antecedents[i].Skill.Name + " " + slotUI.AntecedentSkill.Antecedents[i].Level + " 필요";
            height -= attribute[i].sizeDelta.y * 0.5f;
            attribute[i].localPosition = new Vector3(0, height, 0);
            height -= attribute[i].sizeDelta.y * 0.5f;
        }

        //설명
        description = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Description"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        Text itemDescription = description.GetChild(0).GetComponent<Text>();

        itemDescription.text = detail ? slotUI.AntecedentSkill.Skill.DetailDescription : slotUI.AntecedentSkill.Skill.Description;
        description.sizeDelta = new Vector2(description.sizeDelta.x, itemDescription.preferredHeight);
        itemDescription.rectTransform.sizeDelta = new Vector2(description.sizeDelta.x, itemDescription.preferredHeight);

        height -= itemDescription.preferredHeight * 0.5f;

        description.localPosition = new Vector3(0, height, 0);
    }

    public void ViewMotionSlotUI(AttackMotionSlot SlotUI)
    {
        parent.transform.position = SlotUI.transform.position;
        float height = 0;

        overview = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Overview"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        overview.localPosition = Vector3.zero;
        Text itemName = overview.GetChild(0).GetComponent<Text>();
        Text itemRating = overview.GetChild(1).GetComponent<Text>();
        Text itemAmount = overview.GetChild(2).GetComponent<Text>();

        itemName.text = SlotUI.AttackMotion.Name;
        itemRating.text = "";

        itemAmount.text = "";

        height -= overview.sizeDelta.y * 0.5f;

        //range delay mutlple
        attribute = new RectTransform[3];
        Text attributeText;

        attribute[0] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        attributeText = attribute[0].GetChild(0).GetComponent<Text>();
        attributeText.text = "범위 : " + SlotUI.AttackMotion.Range;
        height -= attribute[0].sizeDelta.y * 0.5f;
        attribute[0].localPosition = new Vector3(0, height, 0);
        height -= attribute[0].sizeDelta.y * 0.5f;

        attribute[1] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        attributeText = attribute[1].GetChild(0).GetComponent<Text>();
        attributeText.text = "딜레이 : " + SlotUI.AttackMotion.Delay;
        height -= attribute[1].sizeDelta.y * 0.5f;
        attribute[1].localPosition = new Vector3(0, height, 0);
        height -= attribute[1].sizeDelta.y * 0.5f;

        attribute[2] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        attributeText = attribute[2].GetChild(0).GetComponent<Text>();
        attributeText.text = "계수 : " + SlotUI.AttackMotion.Multiple;
        height -= attribute[2].sizeDelta.y * 0.5f;
        attribute[2].localPosition = new Vector3(0, height, 0);
        //height -= attribute[2].sizeDelta.y * 0.5f;

        //description = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Description"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        //Text itemDescription = description.GetChild(0).GetComponent<Text>();

        //itemDescription.text = SlotUI.ItemSlot.Item.Description;
        //description.sizeDelta = new Vector2(description.sizeDelta.x, itemDescription.preferredHeight);
        //itemDescription.rectTransform.sizeDelta = new Vector2(description.sizeDelta.x, itemDescription.preferredHeight);

        //height -= itemDescription.preferredHeight * 0.5f;

        //description.localPosition = new Vector3(0, height, 0);
    }

    public void ViewMotionSlotUI(AttackMotionView SlotUI)
    {
        parent.transform.position = SlotUI.transform.position;
        float height = 0;

        overview = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Overview"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        overview.localPosition = Vector3.zero;
        Text itemName = overview.GetChild(0).GetComponent<Text>();
        Text itemRating = overview.GetChild(1).GetComponent<Text>();
        Text itemAmount = overview.GetChild(2).GetComponent<Text>();

        itemName.text = SlotUI.AttackMotion.Name;
        itemRating.text = "";

        itemAmount.text = "";

        height -= overview.sizeDelta.y * 0.5f;

        //range delay mutlple
        attribute = new RectTransform[3];
        Text attributeText;

        attribute[0] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        attributeText = attribute[0].GetChild(0).GetComponent<Text>();
        attributeText.text = "범위 : " + SlotUI.AttackMotion.Range;
        height -= attribute[0].sizeDelta.y * 0.5f;
        attribute[0].localPosition = new Vector3(0, height, 0);
        height -= attribute[0].sizeDelta.y * 0.5f;

        attribute[1] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        attributeText = attribute[1].GetChild(0).GetComponent<Text>();
        attributeText.text = "딜레이 : " + SlotUI.AttackMotion.Delay;
        height -= attribute[1].sizeDelta.y * 0.5f;
        attribute[1].localPosition = new Vector3(0, height, 0);
        height -= attribute[1].sizeDelta.y * 0.5f;

        attribute[2] = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/UserInterface/Attribute"), parent, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
        attributeText = attribute[2].GetChild(0).GetComponent<Text>();
        attributeText.text = "계수 : " + SlotUI.AttackMotion.Multiple;
        height -= attribute[2].sizeDelta.y * 0.5f;
        attribute[2].localPosition = new Vector3(0, height, 0);
    }
}
