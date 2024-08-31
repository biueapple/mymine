using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Block
{
    public Wood(int id) : base(id, "³ª¹«", HardnessType.AXE, 2, false, true, 8, new int[6] { 451, 451, 452, 452, 451, 451 })
    {
    }
}
