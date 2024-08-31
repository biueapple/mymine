using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : Block
{
    public Plank(int id) : base(id, "나무 판자", HardnessType.AXE, 2, false, true, 9, new int[6] { 144, 144, 144, 144, 144, 144 })
    {

    }
}
