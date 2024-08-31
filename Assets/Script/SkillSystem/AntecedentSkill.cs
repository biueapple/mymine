using System.Collections.Generic;
using System;

[Serializable]
public class AntecedentSkill
{
    private readonly ISkill criteria;
    public ISkill Skill { get => criteria; }
    private readonly int playerLevel;
    public int Level { get { return playerLevel; } }
    private readonly int x;
    public int X { get { return x; } }
    private readonly int y;
    public int Y { get { return y; } }
    private readonly List<Antecedent> antecedents;
    public List<Antecedent> Antecedents { get { return antecedents; } }
    public AntecedentSkill(ISkill skill, int playerLevel, int x, int y, List<Antecedent> antecedents)
    {
        criteria = skill;
        this.playerLevel = playerLevel;
        this.antecedents = antecedents;
        this.x = x;
        this.y = y;
    }
    public bool Condition(int level)
    {
        if (level < playerLevel)
        {
            return false;
        }
        for (int i = 0; i < antecedents.Count; i++)
        {
            if (antecedents[i].Skill.Level < antecedents[i].Level)
                return false;
        }
        return true;
    }
}