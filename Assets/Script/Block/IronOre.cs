using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronOre : Block, IMineral
{
    public IronOre(int id) : base(id, "Ã¶ ±¤¹°", HardnessType.PICKAX, 4, false, true, 14, new int[6] {395, 395, 395, 395, 395, 395 })
    {
        shape = new Vector3Int[2][]
        {
            BlockInfo.plus,
            BlockInfo.chair
        };
    }

    public float Probability => 10;

    public int MinHeight => 5;

    public int MaxHeight => 25;

    public int Depth => 10;

    protected Vector3Int[][] shape;
    public Vector3Int[][] Shape => shape;
}
