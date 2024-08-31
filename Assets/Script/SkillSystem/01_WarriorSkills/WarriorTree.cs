using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTree : SkillTree
{
    public WarriorTree(Player player)
    {
        antecedents = new AntecedentSkill[4];

        antecedents[0] = new AntecedentSkill(new Bash(player.STAT), 1, 0, 0, new());
        antecedents[1] = new AntecedentSkill(new Increase(player.STAT), 1, 1, 0, new());
        antecedents[2] = new AntecedentSkill(new Charge(player.STAT, player.MoveSystem), 1, 2, 0, new());

        List<Antecedent> list = new()
        {
            new Antecedent(antecedents[0].Skill, 1)
        };
        antecedents[3] = new AntecedentSkill(new IceArrow(player), 5, 0, 1, list);
    }
}
