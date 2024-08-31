using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//여기서 스킬을 배우면 플레이어에게 스킬을 배우도록 하는거고
//슬롯으로 옮기자면 플레이어가 가지고 있는 스킬을 주도록 하는거지
public class SkillView : MonoBehaviour, ISlotUI, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private SkillInterfaceWindow window;

    private AntecedentSkill  antecedentSkill;
    public AntecedentSkill AntecedentSkill { get { return antecedentSkill; } }
    [SerializeField]
    private Image Icon;
    [SerializeField]
    private Text level;
    [SerializeField]
    private Button plus;
    [SerializeField]
    private Button minus;

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(SkillInterfaceWindow window, AntecedentSkill skill)
    {
        this.window = window;
        this.antecedentSkill = skill;
        //skill.AfterCallback += View;
        View();

        plus.onClick.RemoveAllListeners();
        plus.onClick.AddListener(PlusAddListener);
        minus.onClick.RemoveAllListeners();
        minus.onClick.AddListener(MinusAddListener);
        Deactive();
    }

    public void Active()
    {
        View();
        plus.gameObject.SetActive(true);
        minus.gameObject.SetActive(true);
    }
    public void Deactive()
    {
        antecedentSkill.Skill.Level = 0;
        View();
        plus.gameObject.SetActive(false);
        minus.gameObject.SetActive(false);
    }

    private void View()
    {
        if (antecedentSkill.Skill.Level > 0)
        {
            Icon.sprite = antecedentSkill.Skill.Icon;
            Icon.gameObject.SetActive (true);
        }
        else
        {
            Icon.sprite = null;
            Icon.gameObject.SetActive (false);
        }
        level.text = antecedentSkill.Skill.Level.ToString();
    }

    private void PlusAddListener()
    {
        if(window.System.SkillPoint > 0)
        {
            antecedentSkill.Skill.Level++;
            View();
            window.SufferCondition(antecedentSkill.Skill);
            window.System.SkillPoint--;
        }
    }
    private void MinusAddListener()
    {
        antecedentSkill.Skill.Level--;
        View();
        window.SufferCondition(antecedentSkill.Skill);
        window.System.SkillPoint++;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            if (antecedentSkill != null)
                SlotManager.Instance.SkillCatchState.Explanation.ViewSkillSlotUI(this, false);
            window.AntecedentLineOn(antecedentSkill.Skill);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.SkillCatchState.Explanation.OffExplanSlotUI();
            window.AntecedentLineOff(antecedentSkill.Skill);
        }
    }


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
}