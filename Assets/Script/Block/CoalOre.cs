using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalOre : Block , IMineral
{
    public CoalOre(int id) : base(id, "¼®Åº±¤¼®", HardnessType.PICKAX, 3, false, true, 11, new int[6] { 161, 161, 161, 161, 161, 161 })
    {
        shape = new Vector3Int[2][]
        {
            BlockInfo.plus,
            BlockInfo.chair
        };
    }

    public float Probability => 0.001f;

    public int MinHeight => 5;

    public int MaxHeight => 30;

    public int Depth => 5;

    protected Vector3Int[][] shape;
    public Vector3Int[][] Shape => shape;
}
