using System;

[Serializable]
public class Antecedent
{
    private readonly ISkill skill;
    public ISkill Skill { get => skill; }
    private readonly int level;
    public int Level { get { return level; } }
    public Antecedent(ISkill skill, int level)
    {
        this.skill = skill;
        this.level = level;
    }
}