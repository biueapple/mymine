using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISlotUI
{
    private AntecedentSkill antecedentSkill;
    public AntecedentSkill AntecedentSkill { get { return antecedentSkill; } set { antecedentSkill = value; View(); } }

    [SerializeField]
    private Image fill;
    [SerializeField]
    private Text level;
    [SerializeField]
    private Image icon;
    
    public void Init()
    {
        View();
    }

    public void View()
    {
        if(antecedentSkill == null)
        {
            fill.fillAmount = 0;
            level.text = "";
            icon.sprite = null;
        }
        else
        {
            level.text = antecedentSkill.Skill.Level.ToString();
            icon.sprite = antecedentSkill.Skill.Icon;
        }
    }

    public void Use()
    {
        if(antecedentSkill.Skill != null && antecedentSkill.Skill is IActiveSkill active)
        {
            if(active.Cooltimer <= 0)
            {
                active.Use();
                StartCoroutine(FillAmount(active));
            }
        }
    }

    private IEnumerator FillAmount(IActiveSkill active)
    {
        while(active.Cooltimer > 0)
        {
            yield return null;
            fill.fillAmount = (active.Cooltimer / active.Cooltime);
        }
    }

    //콜백은 항상 update나 코루틴보다 빨리 호출된다
    public void OnPointerUp(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.SkillCatchState.EventUp(this, eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.SkillCatchState.EventDown(this, eventData);
            SlotManager.Instance.SkillCatchState.Explanation.OffExplanSlotUI();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //SlotManager.Instance.SlotMouseEnter(this, eventData);
        if (SlotManager.Instance.NowState == null)
        {
            if (antecedentSkill != null)
                SlotManager.Instance.SkillCatchState.Explanation.ViewSkillSlotUI(this, false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //SlotManager.Instance.SlotMouseExit(this, eventData);
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.SkillCatchState.Explanation.OffExplanSlotUI();
        }
    }
}
