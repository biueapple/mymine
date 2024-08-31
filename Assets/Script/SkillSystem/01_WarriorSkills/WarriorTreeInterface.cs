using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTreeInterface : SkillInterfaceWindow
{
    public override void Init(SkillSystem skillSystem)
    {
        system = skillSystem;
        skillTree = system.WarriorTree;
        CreateView();
    }
}
