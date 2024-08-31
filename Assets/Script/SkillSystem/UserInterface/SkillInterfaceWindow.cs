using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//���߿� ��ų���� ��ӹ޾Ƽ� ��� (�θ� Ŭ����)
public abstract class SkillInterfaceWindow : MonoBehaviour
{
    protected SkillSystem system;
    public SkillSystem System { get { return system; } }

    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private GameObject line;

    protected SkillTree skillTree;
    public SkillTree Tree { get { return skillTree; } }
    [SerializeField]
    private SkillView skillView;
    protected List<SkillView> list = new ();

    protected int X_START;
    protected int Y_START;
    //ui���� �󸶸�ŭ ������ �ִ��� ���ٿ� ��� ������
    [SerializeField]
    protected int X_SPACE_BETWWEN_ITEM;
    [SerializeField]
    protected int Y_SPACE_BETWWEN_ITEMS;

    protected List<PrecedenceLine<AntecedentSkill>> precedenceLines = new ();
    //���� ũ�� 125 ���԰� ������ �Ÿ� 175

    public abstract void Init(SkillSystem system);

    //������ Ʈ���� ������ŭ ���� ������� ��
    protected void CreateView()
    {
        Debug.Log("������ƮǮ���� ������� �ʴ� ����");
        int x = skillTree.Antecedents.Max(x => x.X);
        int y = skillTree.Antecedents.Max(y => y.Y);

        //����Ʈ�� ũ������
        SetContentSize(x, y + 1);


        for (int i = 0; i < skillTree.Antecedents.Length; i++)
        {
            SkillView view = Instantiate(skillView, content != null ? content : transform);

            view.Init(this, skillTree.Antecedents[i]);

            view.transform.localPosition = GetPosition(skillTree.Antecedents[i].X, skillTree.Antecedents[i].Y);

            list.Add(view);

            Condition(skillTree.Antecedents[i].Skill);
        }

        for (int i = 0; i < skillTree.Antecedents.Length; i++)
        {
            precedenceLines.Add(new PrecedenceLine<AntecedentSkill>(skillTree.Antecedents[i]));
            if (skillTree.Antecedents[i].Antecedents != null)
            {
                for(int j = 0; j < skillTree.Antecedents[i].Antecedents.Count; j++)
                {
                    //�� �����
                    precedenceLines[^1].line.Add(
                        CreateLine(list.Find(x => x.AntecedentSkill.Skill == skillTree.Antecedents[i].Skill).transform.position, list.Find(x => x.AntecedentSkill.Skill == skillTree.Antecedents[i].Antecedents[j].Skill).transform.position)); 
                }
            }
        }
    }

    protected void DeleteView()
    {
        Debug.Log("������ƮǮ���� ������� �ʴ� ����");
        for(int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }
        for(int i = 0; i < precedenceLines.Count; i++)
        {
            for(int j = 0; j < precedenceLines[i].line.Count; j++)
            {
                Destroy(precedenceLines[i].line[j].gameObject);
            }
        }
        list.Clear();
        precedenceLines.Clear();
    }

    //��ų(�ڽ�)�� ��� �� �ִ��� Ȯ��
    protected void Condition(ISkill skill)
    {
        AntecedentSkill antecedent = skillTree.Antecedents.ToList().Find(x => x.Skill == skill);
        if (antecedent.Condition(system.GetComponent<Unit>().Level))
        {
            list.Find(x => x.AntecedentSkill.Skill == antecedent.Skill).Active();
        }
        else
        {
            system.SkillPoint += list.Find(x => x.AntecedentSkill.Skill == antecedent.Skill).AntecedentSkill.Skill.Level;
            list.Find(x => x.AntecedentSkill.Skill == antecedent.Skill).Deactive();
        }
    }

    //���� ������ ��ų���� ��� �� �ִ��� Ȯ��
    public void SufferCondition(ISkill skill)
    {
        for(int i = 0; i < skillTree.Antecedents.Length; i++)
        {
            if(skillTree.Antecedents[i].Antecedents.ToList().Find(x => x.Skill == skill) != null)
            {
                if (skillTree.Antecedents[i].Condition(system.GetComponent<Unit>().Level))
                {
                    list.Find(x => x.AntecedentSkill.Skill == skillTree.Antecedents[i].Skill).Active();
                }
                else
                {
                    system.SkillPoint += list.Find(x => x.AntecedentSkill.Skill == skillTree.Antecedents[i].Skill).AntecedentSkill.Skill.Level;
                    list.Find(x => x.AntecedentSkill.Skill == skillTree.Antecedents[i].Skill).Deactive();
                }
            }
        }
    }

    public void AntecedentLineOn(ISkill skill)
    {
        PrecedenceLine<AntecedentSkill> line = precedenceLines.Find(x => x.antecedent.Skill == skill);
        if(line != null)
        {
            for (int i = 0; i < line.line.Count; i++)
            {
                line.line[i].GetComponent<Image>().color = new Color(1, 0, 0);
            }
        } 
    }
    public void AntecedentLineOff(ISkill skill)
    {
        PrecedenceLine<AntecedentSkill> line = precedenceLines.Find(x => x.antecedent.Skill == skill);
        if (line != null)
        {
            for (int i = 0; i < line.line.Count; i++)
            {
                line.line[i].GetComponent<Image>().color = new Color(0.6f, 0.4f, 0.4f);
            }
        }
    }

    private Transform CreateLine(Vector3 criteria, Vector3 target)
    {
        Transform Line = ObjectPooling.Instance.CreateObject(line, content, (criteria + target) * 0.5f, Quaternion.Euler(new Vector3(0,0, Vector2.Angle(new Vector2(0, 0), target - criteria)))).transform;
        Line.SetAsFirstSibling();
        return Line;
    }

    protected void SetContentSize(int x,int y)
    {
        if (content != null)
        {
            content.sizeDelta = new Vector2(500 - (x * X_SPACE_BETWWEN_ITEM), y * Y_SPACE_BETWWEN_ITEMS);
            X_START = 50;
            Y_START = y * Y_SPACE_BETWWEN_ITEMS + 75;
        }
    }
    //new Vector3(50 + x * 175, y * 175, 0)
    private Vector3 GetPosition(int x, int y)
    {
        return new Vector3(x * X_SPACE_BETWWEN_ITEM + X_START,
            y * Y_SPACE_BETWWEN_ITEMS - Y_START,
            0);
    }
}

public class PrecedenceLine<T>
{
    public T antecedent;
    public List<Transform> line;
    public PrecedenceLine(T antecedent)
    {
        this.antecedent = antecedent;
        line = new ();
    }
}