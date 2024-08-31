
using UnityEngine;

public interface ISkill
{
    public string Name { get; }
    public string Description { get; }
    public string DetailDescription { get; }
    public int Level { get; set; }
    public int MaxLevel { get; }
    Stat Stat { get; }
    public Sprite Icon { get; }
}
